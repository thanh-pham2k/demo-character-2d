"""Reproduce batch slicing; generated alpha is preserved without chroma keying.
All character frames share 0.45 source-pixel scale and a registered hip pivot.
"""
from pathlib import Path
import json
from PIL import Image
ROOT = Path(__file__).resolve().parents[3]
HERE = Path(__file__).resolve().parent
src = Image.open(HERE/'source.png')
alpha = Image.open(HERE/'alpha-sheet.png').convert('RGBA')
out = ROOT/'mobile-2d/Assets/Sprites/MVP/characters/room_visual_v2'
env = ROOT/'mobile-2d/Assets/Sprites/MVP/environment/room_visual_v2'
out.mkdir(parents=True, exist_ok=True); env.mkdir(parents=True, exist_ok=True)
# Fractions handle the odd 1774x887 tool output without losing a boundary pixel.
w,h = alpha.size
names = ['hallway','sitting','enter_01','enter_02','enter_03','lying_a','lying_b','sleeping']
# Authoring anchors in the original sheet, local to each 4x2 cell: seated hip / bed hip.
anchors = [(0,0),(214,326),(216,254),(254,326),(222,252),(235,246),(235,246),(235,246)]
manifest = {'source':'source.png','alpha_source':'alpha-sheet.png','size':[w,h],'grid':[4,2],
 'requested_margin':'8% white; actual bounds reviewed below','ppu':100,'filter':'Point','compression':'Uncompressed','cells':[]}
for i,name in enumerate(names):
 c,r=i%4,i//4
 box=(round(c*w/4),round(r*h/2),round((c+1)*w/4),round((r+1)*h/2))
 cell=alpha.crop(box)
 if i==0:
  crop=(80,11,379,434)
  # Opaque corridor is cropped from the original, not the alpha pass.
  corridor=src.crop(crop).resize((128,230),Image.Resampling.LANCZOS)
  corridor.save(env/'hallway.png')
  manifest['cells'].append({'name':name,'cell':box,'content_crop':crop,'output':[128,230],'pivot':[0,0]})
  continue
 factor=.45
 resized=cell.resize((round(cell.width*factor),round(cell.height*factor)),Image.Resampling.NEAREST)
 anchor=anchors[i]
 xy=(round(128-anchor[0]*factor),round(128-anchor[1]*factor))
 canvas=Image.new('RGBA',(256,256));canvas.alpha_composite(resized,xy)
 bbox=canvas.getbbox()
 assert bbox and min(bbox)>0 and bbox[2]<256 and bbox[3]<256,(name,bbox)
 canvas.save(out/(name+'.png'))
 manifest['cells'].append({'name':name,'cell':box,'hip_source':anchor,'scale':factor,'paste':xy,'alpha_bounds':bbox,'output':[256,256],'pivot':[.5,.5]})
(HERE/'manifest.json').write_text(json.dumps(manifest,indent=2))
# Contact sheet for alpha / registration inspection, not an AI-generated batch.
review=Image.new('RGB',(256*4,256*2),(220,220,220))
for i,name in enumerate(names[1:]):
 im=Image.open(out/(name+'.png')); review.paste(im,(i%4*256,i//4*256),im)
review.save(HERE/'sliced-review.png')
print(json.dumps(manifest,indent=2))
