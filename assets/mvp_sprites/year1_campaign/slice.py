"""Measured batch extraction. Preserve shared pose anchors instead of per-frame scaling."""
from pathlib import Path
import json
from PIL import Image
from collections import deque

def isolate(tile):
    # Alpha-component extraction removes neighboring packed-cell fragments.
    alpha=tile.getchannel('A');w,h=tile.size;pixels=alpha.load();seen=set();largest=[]
    for y in range(h):
        for x in range(w):
            if pixels[x,y]<64 or (x,y) in seen:continue
            queue=deque([(x,y)]);seen.add((x,y));component=[]
            while queue:
                a,b=queue.popleft();component.append((a,b))
                for c,d in [(a-1,b),(a+1,b),(a,b-1),(a,b+1)]:
                    if 0<=c<w and 0<=d<h and pixels[c,d]>=64 and (c,d) not in seen:
                        seen.add((c,d));queue.append((c,d))
            if len(component)>len(largest):largest=component
    mask=Image.new('L',(w,h));out=mask.load()
    for x,y in largest:
        for a in range(max(0,x-1),min(w,x+2)):
            for b in range(max(0,y-1),min(h,y+2)):out[a,b]=pixels[a,b]
    tile.putalpha(mask);return tile

ROOT=Path(__file__).resolve().parent
OUT=ROOT.parents[2]/'mobile-2d/Assets/Sprites/MVP/year1_campaign'
OUT.mkdir(parents=True,exist_ok=True)
records=[]

def extract(sheet,name,rect,canvas=(256,256),scale=None,anchor=None,target=(128,128)):
    source=Image.open(ROOT/sheet).convert('RGBA')
    tile=source.crop(rect)
    if sheet.startswith('npc/'):tile=isolate(tile)
    if scale is not None:
        tile=tile.resize((round(tile.width*scale),round(tile.height*scale)),Image.Resampling.NEAREST)
        result=Image.new('RGBA',canvas)
        position=(round(target[0]-(anchor[0]-rect[0])*scale),round(target[1]-(anchor[1]-rect[1])*scale))
        result.alpha_composite(tile,position)
    else:
        box=tile.getchannel('A').point(lambda p:255 if p>64 else 0).getbbox()
        assert box,name
        tile=tile.crop(box)
        tile.thumbnail((canvas[0]-24,canvas[1]-24),Image.Resampling.NEAREST)
        result=Image.new('RGBA',canvas);result.alpha_composite(tile,((canvas[0]-tile.width)//2,canvas[1]-12-tile.height))
    result.save(OUT/(name+'.png'))
    records.append({'name':name,'sheet':sheet,'crop':rect,'canvas':canvas,'scale':scale,'source_anchor':anchor,'target_anchor':target})

# The generator produced a measured 6x2 arrangement rather than requested 4x3.
desk_rows=[(45,490,[(18,258),(260,491),(491,724),(724,976),(976,1212),(1212,1448)]),
           (530,995,[(18,258),(260,494),(494,728),(728,978),(978,1212),(1212,1448)])]
desk=Image.open(ROOT/'desk/source.png').convert('RGBA')
for row,(top,bottom,columns) in enumerate(desk_rows):
    for i,(left,right) in enumerate(columns):
        alpha=desk.crop((left,top,right,top+145)).getchannel('A')
        head=alpha.point(lambda p:255 if p>100 else 0).getbbox()
        anchor=(left+(head[0]+head[2])/2,308 if row==0 else 820)
        extract('desk/source.png',('luon' if row==0 else 'long')+'_typing_'+str(i),(left,top,right,bottom),scale=.42,anchor=anchor)

# NPC batch is packed in five measured rows. Rectangles isolate all 16 poses.
rects=[(50,0,320,330),(350,0,700,330),(700,0,1200,330),
       (0,330,425,495),(430,330,830,495),(830,330,1254,495),
       (0,505,455,770),(455,500,765,790),(770,500,1000,790),
       (40,775,295,1013),(350,775,610,1013),(655,775,930,1013),(980,775,1240,1013),
       (70,1015,280,1254),(380,1015,585,1254),(690,1015,940,1254)]
names=['long_enter_'+str(i) for i in range(3)]+['long_sleep_'+str(i) for i in range(3)]+['long_exit_'+str(i) for i in range(3)]+['dam_write_'+str(i) for i in range(3)]+['dam_surprise']+['teacher_'+str(i) for i in range(3)]
for name,rect in zip(names,rects):extract('npc/source.png',name,rect,canvas=(384,192) if 'sleep' in name else (256,256))

environment=Image.open(ROOT/'environments/source.png').convert('RGBA')
for name,rect in [('classroom',(0,0,966,543)),('lab',(0,543,966,1086)),('canteen',(0,1086,966,1629))]:
    environment.crop(rect).save(OUT/(name+'.png'))
    records.append({'name':name,'sheet':'environments/source.png','crop':rect,'pivot':[.5,.5]})
motion=Image.open(ROOT/'long_motion/source.png').convert('RGBA')
for i,name in enumerate(['long_walk_0','long_walk_1','long_walk_2','long_walk_3','long_rise_0','long_rise_1']):
    x,y=(i%3)*512,(i//3)*512
    head=motion.crop((x,y,x+512,y+180)).getchannel('A').point(lambda p:255 if p>100 else 0).getbbox()
    extract('long_motion/source.png',name,(x,y,x+512,y+512),scale=.48,anchor=(x+(head[0]+head[2])/2,y+500),target=(128,244))
# Targeted Long bed revision: same overhead/front perspective as approved Luon.
# Equal 418px cells; common scale, head anchor registered across the sequence.
bed=Image.open(ROOT/'long_bed_v2/source.png').convert('RGBA')
for i,name in enumerate(['long_enter_'+str(n) for n in range(3)]+['long_sleep_'+str(n) for n in range(3)]+['long_exit_'+str(n) for n in range(3)]):
    x,y=(i%3)*418,(i//3)*418
    head=bed.crop((x,y,x+418,y+150)).getchannel('A').point(lambda p:255 if p>100 else 0).getbbox()
    extract('long_bed_v2/source.png',name,(x,y,x+418,y+418),canvas=(384,384),scale=.9,anchor=(x+(head[0]+head[2])/2,y+head[1]),target=(192,24))
(ROOT/'manifest.json').write_text(json.dumps({'ppu':100,'filter':'Point','compression':'Uncompressed','outputs':list({r['name']:r for r in records}.values()),'superseded_bed_crops':[r for r in records if r['sheet']=='npc/source.png' and r['name'].startswith('long_')]},indent=2)+'\n')
print('Exported',len({r['name'] for r in records}),'sprites')
