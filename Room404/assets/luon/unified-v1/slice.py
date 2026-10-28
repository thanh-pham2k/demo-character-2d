"""Slice generated sheets into consistently aligned transparent SVG frames."""
from pathlib import Path
from PIL import Image

root=Path(__file__).resolve().parent
without=Image.open(root/'no-backpack-sheet.png').convert('RGBA')
with_bag=Image.open(root/'with-backpack-sheet.png').convert('RGBA')
boxes=[(0,0,768,533),(768,0,1536,533),(0,533,768,1024),(768,533,1536,1024)]
seated=Image.open(root/'seated-sheet.png').convert('RGBA')
frames=[]
for row in ('plain','bag'):
 for index in range(4):
  box=boxes[index] if row=='plain' else (index*384,512,(index+1)*384,1024)
  frames.append((without if row=='plain' else with_bag, box, f'{row}_{index}'))
for row, prefix in enumerate(('seat_plain','seat_bag')):
 for index in range(4):
  frames.append((seated, (round(index*1774/4),row*443,round((index+1)*1774/4),443 if row==0 else 887), f'{prefix}_{index}'))
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
