from pathlib import Path
from PIL import Image

root=Path(__file__).resolve().parent
im=Image.open(root/'sheet.png').convert('RGBA')
w,h=im.size
names=['sit_idle','study_1','study_2','sit_backpack']
for index,name in enumerate(names):
 row,col=divmod(index,2)
 cell_box = (col*w//2, 0 if row == 0 else 660, (col+1)*w//2, 660 if row == 0 else h)
 cell=im.crop(cell_box)
 cell.save(root/(name+'_cell.png'))
 alpha=cell.getchannel('A')
 box=alpha.point(lambda v:255 if v>=128 else 0).getbbox()
 if not box: raise ValueError('Empty sprite: '+name)
 sprite=cell.crop(box)
 # Align feet and center in a common canvas; retain transparency.
 canvas=Image.new('RGBA',(400,590),(0,0,0,0))
 canvas.alpha_composite(sprite,((400-sprite.width)//2,590-sprite.height))
 canvas.save(root/(name+'.png'))
 # Trace to editable coloured SVG paths; preserve approximate edge alpha.
 img=canvas.resize((200,295),Image.Resampling.LANCZOS)
 a=img.getchannel('A').load(); q=img.convert('RGB').quantize(colors=192,method=Image.Quantize.MEDIANCUT,dither=Image.Dither.NONE)
 px=q.load();palette=q.getpalette();paths={}
 def color(x,y):
  v=a[x,y]
  return (px[x,y],255 if v>=224 else ((v+31)//64)*64) if v>=32 else None
 for y in range(295):
  x=0
  while x<200:
   key=color(x,y);start=x;x+=1
   while x<200 and color(x,y)==key:x+=1
   if key is not None:paths.setdefault(key,[]).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
 with (root/(name+'.svg')).open('w') as f:
  f.write('<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 200 295" shape-rendering="crispEdges">')
  for (index,opacity),runs in paths.items():
   r,g,b=palette[index*3:index*3+3];f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" opacity="{opacity/255:.3f}" d="{"".join(runs)}"/>')
  f.write('</svg>\n')
 print(name, 'cell', cell_box,'visible',box)
