"""Slice sleep and slippers frames."""
from pathlib import Path
from PIL import Image
root=Path(__file__).resolve().parent
sheet=Image.open(root/'sheet.png').convert('RGBA')
boxes=[(0,0,782,700),(782,0,1536,700),(0,700,768,1024),(768,700,1536,1024)]
names=['sleep_0','sleep_1','slippers_0','slippers_1']
frames=[(sheet,box,name) for box,name in zip(boxes,names)]
for source,box,name in frames:
  cell=source.crop(box)
  cell.save(root/f'{name}_cell.png')
  alpha=cell.getchannel('A')
  bounds=alpha.point(lambda a:255 if a>=128 else 0).getbbox()
  if bounds is None:raise ValueError(f'Empty frame {name}')
  x0,y0,x1,y1=bounds
  if x0==0 or y0==0 or x1==cell.width or y1==cell.height:raise ValueError(f'Frame clipped: {name} {bounds}')
  sprite=cell.crop(bounds)
  # Discard residual semi-transparent generated haze; retain original colours.
  sprite.putalpha(sprite.getchannel('A').point(lambda a:0 if a<112 else a))
  sprite.thumbnail((280,440),Image.Resampling.LANCZOS)
  canvas=Image.new('RGBA',(300,460),(0,0,0,0))
  canvas.alpha_composite(sprite,((300-sprite.width)//2,460-sprite.height))
  canvas.save(root/f'{name}.png')
  img=canvas.resize((150,230),Image.Resampling.LANCZOS)
  a=img.getchannel('A').load(); q=img.convert('RGB').quantize(colors=192,method=Image.Quantize.MEDIANCUT,dither=Image.Dither.NONE)
  pixels=q.load();palette=q.getpalette();paths={}
  def color(x,y):
   v=a[x,y];return (pixels[x,y],255 if v>=224 else ((v+31)//64)*64) if v>=32 else None
  for y in range(230):
   x=0
   while x<150:
    key=color(x,y);start=x;x+=1
    while x<150 and color(x,y)==key:x+=1
    if key is not None:paths.setdefault(key,[]).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
  with (root/f'{name}.svg').open('w') as f:
   f.write('<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 150 230" shape-rendering="crispEdges">')
   for (ci,opacity),runs in paths.items():
    r,g,b=palette[ci*3:ci*3+3];f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" opacity="{opacity/255:.3f}" d="{"".join(runs)}"/>')
   f.write('</svg>\n')
  print(name,box,bounds)

# Tight view boxes let horizontal poses and slippers fill their display boxes.
for name, box in [('sleep_0','0 143 150 87'), ('sleep_1','0 143 150 87'), ('slippers_0','0 174 150 56'), ('slippers_1','0 174 150 56')]:
  source=(root/f'{name}.svg').read_text()
  (root/f'{name}_tight.svg').write_text(source.replace('viewBox="0 0 150 230"', f'viewBox="{box}"'))

# Keep only the sleeping head; the original room supplies pillow and blanket.
for index in range(2):
  source=(root/f'sleep_{index}.svg').read_text()
  source=source.replace('viewBox="0 0 150 230"', 'viewBox="5 143 68 66"')
  source=source.replace('shape-rendering="crispEdges">', 'shape-rendering="crispEdges"><defs><clipPath id="head"><path d="M6 181 Q8 156 24 148 Q42 140 59 151 Q70 157 71 177 L68 192 Q64 201 54 207 Q43 210 30 201 L18 195 Q12 192 10 189 Z"/></clipPath></defs><g clip-path="url(#head)">', 1)
  (root/f'head_{index}.svg').write_text(source.replace('</svg>', '</g></svg>'))
