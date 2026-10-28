"""Slice backpack desk and tablet poses."""
from pathlib import Path
from PIL import Image
root=Path(__file__).resolve().parent
sheet=Image.open(root/'sheet.png').convert('RGBA')
frames=[(sheet,(col*512,0 if row==0 else 745,(col+1)*512,745 if row==0 else 1536),f'{name}_{col}') for row,name in enumerate(('study','tablet')) for col in range(2)]
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
