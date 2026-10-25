"""Extract the explicitly generated flat-white batch without keying white shirts.
Only edge-connected near-white areas are background. Interior white regions are
preserved. A one-pixel boundary is unmatted against white for clean dark previews.
"""
from pathlib import Path
import json
from PIL import Image,ImageDraw,ImageFilter
BASE=Path(__file__).resolve().parent
im=Image.open(BASE/'source-white.png').convert('RGB');w,h=im.size;rgb=im.load()
mask=Image.new('L',im.size);mp=mask.load()
for y in range(h):
 for x in range(w):
  r,g,b=rgb[x,y]
  if min(r,g,b)>=240 and max(r,g,b)-min(r,g,b)<=14:mp[x,y]=255
for x,y in [(x,0) for x in range(w)]+[(x,h-1) for x in range(w)]+[(0,y) for y in range(h)]+[(w-1,y) for y in range(h)]:
 if mask.getpixel((x,y))==255:ImageDraw.floodfill(mask,(x,y),128,thresh=0)
bg=mask.point(lambda p:255 if p==128 else 0);edge=bg.filter(ImageFilter.MaxFilter(3));bp=bg.load();ep=edge.load()
out=im.convert('RGBA');op=out.load();removed=0
for y in range(h):
 for x in range(w):
  r,g,b=rgb[x,y]
  if bp[x,y]:op[x,y]=(r,g,b,0);removed+=1
  elif ep[x,y] and min(r,g,b)>40:
   alpha=max(.05,min(1,(255-min(r,g,b))/235))
   clean=tuple(round(max(0,min(255,(v-255*(1-alpha))/alpha))) for v in (r,g,b))
   op[x,y]=(*clean,round(255*alpha))
out.save(BASE/'source.png')
(BASE/'extraction.json').write_text(json.dumps(dict(raw_generated_sheet='source-white.png',transparent_sheet='source.png',method='edge_connected_white_flood_fill_then_one_pixel_white_unmatte',threshold=240,channel_range=14,removed_background_pixels=removed,preserved_interior_white=True),indent=2)+'\n')
print('Removed outside white pixels:',removed,'of',w*h)
