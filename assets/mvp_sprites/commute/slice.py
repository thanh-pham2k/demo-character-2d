from pathlib import Path
from PIL import Image
import json,hashlib
here=Path(__file__).resolve().parent; root=here.parents[2]
out=root/'mobile-2d/Assets/Sprites/MVP/commute';out.mkdir(parents=True,exist_ok=True)
desk=Image.open(here/'desk/final-sheet.png').convert('RGBA')
exterior=Image.open(here/'outdoor/alpha-sheet.png').convert('RGBA')
original=Image.open(here/'outdoor/source.png')
manifest={'desk_canvas':desk.size,'desk_grid':[4,2],'outdoor_canvas':exterior.size,'outdoor_grid':[3,2],
 'requested_margin':'10%, refined character margins after inspection','ppu':100,'filter':'Point','compression':'Uncompressed','outputs':[]}
w,h=desk.size
entries=[('hook_full',0,(235,65),.33,(128,192),(64,20)),('hook_empty',1,(199,80),.33,(128,192),(64,20)),
 ('study_0',2,(153,350),.45,(256,256),(128,128)),('study_1',3,(150,350),.45,(256,256),(128,128)),
 ('study_2',4,(200,327),.45,(256,256),(128,128)),('typing_0',5,(197,332),.45,(256,256),(128,128)),
 ('typing_1',6,(198,332),.45,(256,256),(128,128))]
for name,index,anchor,scale,size,target in entries:
 c,r=index%4,index//4;box=(round(c*w/4),round(r*h/2),round((c+1)*w/4),round((r+1)*h/2))
 cell=desk.crop(box);resized=cell.resize((round(cell.width*scale),round(cell.height*scale)),Image.Resampling.NEAREST)
 xy=(round(target[0]-anchor[0]*scale),round(target[1]-anchor[1]*scale))
 result=Image.new('RGBA',size);result.alpha_composite(resized,xy)
 visible=result.getchannel('A').point(lambda x:255 if x>128 else 0).getbbox()
 assert visible and min(visible)>0 and visible[2]<size[0] and visible[3]<size[1],(name,visible)
 result.save(out/(name+'.png'))
 manifest['outputs'].append({'name':name,'cell':box,'anchor':anchor,'scale':scale,'paste':xy,'canvas':size,'pivot':[.5,.5],'visible_bounds':visible})
box=(1330,443,1774,887)
book=desk.crop(box);book.thumbnail((160,120),Image.Resampling.NEAREST);book.save(out/'notebook.png')
manifest['outputs'].append({'name':'notebook','cell':box,'output':book.size,'pivot':[.5,.5]})
box=(0,0,1536,496);original.crop(box).convert('RGB').save(out/'campus.png');manifest['outputs'].append({'name':'campus','crop':box,'pivot':[.5,.5]})
for name,box,width,height in [('bus_33',(31,527,1299,961),750,None),('bus_sign',(1325,553,1484,961),None,260)]:
 im=exterior.crop(box)
 if width is None:width=round(im.width*height/im.height)
 if height is None:height=round(im.height*width/im.width)
 im.resize((width,height),Image.Resampling.NEAREST).save(out/(name+'.png'))
 manifest['outputs'].append({'name':name,'crop':box,'output':[width,height],'pivot':[.5,0]})
files=list(out.glob('*.png'));hashes=[hashlib.sha256(p.read_bytes()).hexdigest() for p in files]
assert len(files)==11 and len(hashes)==len(set(hashes))
(here/'manifest.json').write_text(json.dumps(manifest,indent=2))
print(out)
