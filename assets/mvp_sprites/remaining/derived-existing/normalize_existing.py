"""Register existing side-view sprites; no image generation or new artwork."""
from pathlib import Path
import json,hashlib,statistics
from PIL import Image
BASE=Path(__file__).resolve().parent
ROOT=BASE.parents[3]
DEST=ROOT/'mobile-2d/Assets/Sprites/MVP/remaining/DerivedExisting';DEST.mkdir(exist_ok=True)
records=[]
for outfit,files in [('casual',['characters/luon_idle_0.png']+[f'characters/luon_walk_{i}.png' for i in range(4)]),('backpack',['dorm_journey/backpack_idle.png']+[f'dorm_journey/backpack_walk_{i}.png' for i in range(3)])]+[(name,[f'characters/{name}_standing.png']) for name in ['long','to','dam']]:
 images=[]
 for file in files:
  path=ROOT/'mobile-2d/Assets/Sprites/MVP'/file
  im=Image.open(path).convert('RGBA');bbox=im.getchannel('A').point(lambda v:255 if v>12 else 0).getbbox()
  if not bbox:raise ValueError(file)
  images.append((path,im,bbox))
 scale=min(240/statistics.median(b[3]-b[1] for _,_,b in images),224/max(b[2]-b[0] for _,_,b in images))
 for i,(path,im,b) in enumerate(images):
  content=im.crop(b);content=content.resize((round(content.width*scale),round(content.height*scale)),Image.Resampling.LANCZOS)
  canvas=Image.new('RGBA',(256,320));canvas.alpha_composite(content,((256-content.width)//2,320-content.height))
  id=outfit+('_right_idle' if i==0 else f'_right_walk_{i-1}')
  output=DEST/(id+'.png');canvas.save(output)
  output.with_suffix('.sprite.json').write_text(json.dumps(dict(pivotX=.5,pivotY=0,ppu=100))+'\n')
  records.append(dict(id=id,production='derived_from_existing_not_generated',source=str(path.relative_to(ROOT)),source_sha256=hashlib.sha256(path.read_bytes()).hexdigest(),source_crop=list(b),common_scale=scale,output=str(output.relative_to(ROOT)),sha256=hashlib.sha256(output.read_bytes()).hexdigest(),canvas=[256,320],pivot=[.5,0],ppu=100))
(BASE/'manifest.json').write_text(json.dumps(dict(scope='Registration of existing side sprites, not generated batch',outputs=records),indent=2)+'\n')
print('Registered',len(records),'existing sprites; no new image generation')
