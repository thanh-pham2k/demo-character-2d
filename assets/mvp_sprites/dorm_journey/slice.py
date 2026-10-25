from pathlib import Path
from PIL import Image
import json,hashlib
HERE=Path(__file__).resolve().parent
ROOT=HERE.parents[2]
src=Image.open(HERE/'source.png').convert('RGBA')
alpha=Image.open(HERE/'alpha-sheet.png').convert('RGBA')
out=ROOT/'mobile-2d/Assets/Sprites/MVP/dorm_journey';out.mkdir(parents=True,exist_ok=True)
manifest={'tool':'built-in ImageGen + one whole-sheet alpha/walk-cycle edit','canvas':src.size,'requested_grid':[3,3],
 'actual_rows':[0,435,839,1254],'margin':'8% requested; actual rectangles below','ppu':100,'filter':'Point','compression':'Uncompressed','outputs':[]}
box=(4,4,1250,433)
src.crop(box).convert('RGB').save(out/'hallway.png')
manifest['outputs'].append({'name':'hallway','source':box,'pivot':[.5,.5]})
for name,box in [('wc_male',(124,450,320,827)),('wc_female',(531,450,727,827))]:
 im=alpha.crop(box).resize((128,250),Image.Resampling.NEAREST);im.save(out/(name+'.png'))
 manifest['outputs'].append({'name':name,'source':box,'output':[128,250],'pivot':[.5,0]})
for name,box,anchor in [('backpack_idle',(837,435,1254,839),(190,395)),('backpack_walk_0',(0,839,418,1254),(211,394)),('backpack_walk_1',(418,839,836,1254),(207,394)),('backpack_walk_2',(836,839,1254,1254),(209,394))]:
 im=alpha.crop(box);factor=.62
 resized=im.resize((round(im.width*factor),round(im.height*factor)),Image.Resampling.NEAREST)
 canvas=Image.new('RGBA',(180,260));xy=(round(90-anchor[0]*factor),round(246-anchor[1]*factor));canvas.alpha_composite(resized,xy)
 visible=canvas.getchannel('A').point(lambda x:255 if x>128 else 0).getbbox()
 assert visible and visible[1]>0 and visible[3]<260,(name,visible)
 canvas.save(out/(name+'.png'))
 manifest['outputs'].append({'name':name,'source':box,'foot_anchor':anchor,'scale':factor,'paste':xy,'visible_bounds':visible,'output':[180,260],'pivot':[.5,.5]})
hashes=[hashlib.sha256(p.read_bytes()).hexdigest() for p in out.glob('*.png')]
assert len(hashes)==len(set(hashes)),'Duplicate output pixels'
(HERE/'manifest.json').write_text(json.dumps(manifest,indent=2))
print(out)
