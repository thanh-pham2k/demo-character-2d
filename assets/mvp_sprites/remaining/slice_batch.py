"""Slice an explicitly measured generated batch; preserve source provenance/alpha.
Usage: python slice_batch.py C00 --source C00/source.png [--crops C00/crops.json]
Relative source/crop paths are resolved from this directory. Grid is proportional
only when --crops is omitted; inspect preview and record actual crop before QA.
"""
import argparse,json,hashlib,statistics
from pathlib import Path
from PIL import Image,ImageDraw
BASE=Path(__file__).resolve().parent
ROOT=BASE.parents[2]

def run():
    ap=argparse.ArgumentParser();ap.add_argument('batch');ap.add_argument('--source',required=True);ap.add_argument('--crops');args=ap.parse_args()
    directory=BASE/args.batch
    planned=json.loads((directory/'manifest.planned.json').read_text())
    source=Path(args.source);source=source if source.is_absolute() else BASE/source
    source=source.resolve();sheet=Image.open(source).convert('RGBA');w,h=sheet.size
    b=planned['batch'];cols,rows=b['columns'],b['rows']
    if args.crops:
        cp=Path(args.crops);cp=cp if cp.is_absolute() else BASE/cp
        crops=json.loads(cp.read_text())
    else:
        crops={a['id']:[round(a['column']*w/cols),round(a['row']*h/rows),round((a['column']+1)*w/cols)-round(a['column']*w/cols),round((a['row']+1)*h/rows)-round(a['row']*h/rows)] for a in planned['assets']}
    report=[];thumbs=[];alpha=sheet.getchannel('A')
    if b['kind']!='environment' and alpha.getextrema()==(255,255):
        raise ValueError('Sprite sheet has no transparency. Refine whole sheet or explicitly matte/extract, review edges, then supply extracted sheet. No silent white-background import.')
    records=[]
    animation_scale=None
    if b['kind']=='animation':
        heights=[];widths=[]
        for item in planned['assets']:
            xx,yy,ww,hh=crops[item['id']];frame=sheet.crop((xx,yy,xx+ww,yy+hh))
            bounds=frame.getchannel('A').point(lambda v:255 if v>12 else 0).getbbox()
            if not bounds:raise ValueError(item['id']+' empty animation cell')
            heights.append(bounds[3]-bounds[1]);widths.append(bounds[2]-bounds[0])
        animation_scale=min((270 if args.batch=="AN03" else 240)/statistics.median(heights),224/max(widths))
    for a in planned['assets']:
        aid=a['id'];x,y,cw,ch=crops[aid]
        if not(0<=x<x+cw<=w and 0<=y<y+ch<=h):raise ValueError((aid,'crop bounds'))
        cell=sheet.crop((x,y,x+cw,y+ch));bbox=cell.getchannel('A').point(lambda v:255 if v>12 else 0).getbbox()
        if not bbox:raise ValueError((aid,'empty cell'))
        if b['kind']=='character':
            content=cell.crop(bbox)
            target_h=270 if aid.endswith("_graduate") else 190 if aid.startswith('committee_') or any(word in aid for word in ('writing','sitting','_sit','exam_','packing','repair_laptop','gaming')) else 240
            canvas_w=384 if '_reunion_arm' in aid else 256
            scale=min(target_h/content.height,(canvas_w-32)/content.width)
            content=content.resize((max(1,round(content.width*scale)),max(1,round(content.height*scale))),Image.Resampling.LANCZOS)
            result=Image.new('RGBA',(canvas_w,320));paste=((canvas_w-content.width)//2,320-content.height)
            result.alpha_composite(content,paste)
            normalization={'source_bbox':list(bbox),'scale':scale,'paste_xy':list(paste),'canvas':[canvas_w,320],'feet_baseline':320}
        elif b['kind']=='animation':
            content=cell.crop(bbox);alpha_pixels=content.getchannel('A').load()
            head_x=[xx for yy in range(max(1,round(content.height*.33))) for xx in range(content.width) if alpha_pixels[xx,yy]>128]
            head_center=statistics.median(head_x) if head_x else content.width/2
            new_size=(max(1,round(content.width*animation_scale)),max(1,round(content.height*animation_scale)))
            content=content.resize(new_size,Image.Resampling.LANCZOS)
            result=Image.new('RGBA',(256,320));paste=(round(128-head_center*animation_scale),320-content.height)
            result.alpha_composite(content,paste)
            normalization={'mode':'shared_batch_scale_head_center_feet_baseline','source_bbox':list(bbox),'scale':animation_scale,'source_head_center_x':head_center,'paste_xy':list(paste),'canvas':[256,320],'feet_baseline':320}
        elif b['kind']=='prop' and aid.startswith('wardrobe_'):
            result=cell
            # Fixed equal-sized registration canvases, cabinet carcass centered at x222.
            # Front feet at source y438 for both closed/open variants.
            a['pivot']=[.5,(ch-438)/ch]
            normalization={'mode':'cabinet_registered_pair','canvas':list(cell.size),'carcass_floor_xy':[222,438]}
        elif b['kind']=='prop':
            result=cell.crop(bbox)
            normalization={'mode':'tight_alpha_preserve_source_scale','source_bbox':list(bbox),'canvas':list(result.size),'floor_at_output_bottom':True}
        else:
            # Animation cells remain identical dimensions and shared registration.
            result=cell;normalization={'mode':'untrimmed_cell','canvas':list(cell.size)}
        target=ROOT/a['output_file'];target.parent.mkdir(parents=True,exist_ok=True);result.save(target)
        target.with_suffix('.sprite.json').write_text(json.dumps(dict(pivotX=a['pivot'][0],pivotY=a['pivot'][1],ppu=a['ppu']))+'\n')
        edge=bbox[0]==0 or bbox[1]==0 or bbox[2]==cw or bbox[3]==ch
        records.append(dict(id=aid,missions=a['missions'],source_crop_xywh=[x,y,cw,ch],normalization=normalization,pivot=a['pivot'],ppu=a['ppu'],output_file=a['output_file'],alpha_bounds=list(bbox),touches_cell_edge=edge,sha256=hashlib.sha256(target.read_bytes()).hexdigest(),visual_qa='pending'))
        tile=Image.new('RGBA',(270,350),(52,58,68,255));thumb=result.copy();thumb.thumbnail((250,300));tile.alpha_composite(thumb,((270-thumb.width)//2,0));d=ImageDraw.Draw(tile);d.text((5,305),aid,fill='white');d.text((5,323),str(result.size),fill='white');thumbs.append(tile)
    preview=Image.new('RGBA',(270*cols,350*rows),(52,58,68,255))
    for i,tile in enumerate(thumbs):preview.alpha_composite(tile,((i%cols)*270,(i//cols)*350))
    preview.convert('RGB').save(directory/'slice-preview.jpg',quality=94)
    actual=dict(batch=args.batch,provenance='generated_composite_batch',source_file=str(source.relative_to(ROOT)),source_sha256=hashlib.sha256(source.read_bytes()).hexdigest(),source_dimensions=[w,h],grid=[cols,rows],crop_format='x_y_width_height_top_left',tool='built_in_imagegen',outputs=records,qa={'slice_bounds':'pass','visual':'pending','unity_import':'pending','integration':'pending'})
    (directory/'manifest.json').write_text(json.dumps(actual,ensure_ascii=False,indent=2)+'\n')
    print(json.dumps({'batch':args.batch,'outputs':len(records),'touching_edge':[r['id'] for r in records if r['touches_cell_edge']],'preview':str(directory/'slice-preview.jpg')},ensure_ascii=False))
if __name__=='__main__':run()
