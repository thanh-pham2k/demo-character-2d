from PIL import Image
from pathlib import Path

# Shapes enclose the visible metal, keeping gaps between rungs transparent.
mask=('M35 13 L58 13 Q67 10 72 18 L72 285 Q72 294 64 294 L56 290 L56 26 L35 26 Z '
      'M39 78 Q47 75 52 82 L54 326 Q48 332 40 326 Z '
      'M25 109 L39 104 L42 112 L25 123 Z '
      'M25 130 L39 119 L42 127 L25 145 Z '
      'M25 156 L39 148 L42 156 L25 170 Z '
      'M25 181 L40 171 L42 180 L25 195 Z '
      'M25 204 L39 197 L42 205 L25 218 Z '
      'M25 231 L40 224 L42 234 L25 252 Z '
      'M25 289 L40 279 L42 289 L25 307 Z')

for side, bbox in [('left', (340, 235, 415, 565)), ('right', (1121, 235, 1196, 565))]:
 im=Image.open('Room404/assets/reference.png').convert('RGB').crop(bbox)
 q=im.quantize(colors=256,method=Image.Quantize.MEDIANCUT,dither=Image.Dither.NONE)
 px=q.load();pal=q.getpalette();paths={}
 for y in range(330):
  x=0
  while x<75:
   c=px[x,y]; start=x; x+=1
   while x<75 and px[x,y]==c: x+=1
   paths.setdefault(c,[]).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
 transform=' transform="translate(75 0) scale(-1 1)"' if side=='right' else ''
 out=Path(f'Room404/assets/reference-draft/ladder_{side}.svg')
 with out.open('w') as f:
  f.write('<svg xmlns="http://www.w3.org/2000/svg" width="75" height="330" viewBox="0 0 75 330" shape-rendering="crispEdges"><defs><clipPath id="ladder"><path d="'+mask+'"'+transform+'/></clipPath></defs><g clip-path="url(#ladder)">')
  for c,runs in paths.items():
   r,g,b=pal[c*3:c*3+3];f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" d="{"".join(runs)}"/>')
  f.write('</g></svg>')
