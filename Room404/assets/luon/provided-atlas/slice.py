"""Extract selected user-provided atlas frames into transparent SVG paths."""
from pathlib import Path
import re
from PIL import Image, ImageDraw, ImageFilter
root=Path(__file__).resolve().parent
atlas=Image.open(root/'source.png').convert('RGBA')
frames={
 'backpack_idle': ((36,35,150,230), None),
 'backpack_walk_1': ((400,35,515,230), None),
 'backpack_walk_2': ((515,35,630,230), None),
 'backpack_walk_3': ((630,35,745,230), None),
 # Atlas includes desk and chair. Restrict these to visible head and shoulders.
 'sit': ((335,250,455,430), 'M7 22 Q14 2 49 1 Q81 2 88 22 L86 58 Q84 73 70 82 L79 84 Q92 86 99 100 L104 121 L96 128 L87 103 L40 103 L29 133 L20 132 L24 101 L35 84 L43 81 Q19 73 11 58 Z'),
 'study': ((790,250,910,430), 'M5 26 Q12 3 45 4 Q77 3 83 27 L81 59 Q80 74 69 82 L76 84 Q91 86 97 99 L97 103 L53 104 L48 129 L33 131 L19 124 L16 113 L20 102 L31 88 L40 82 Q16 72 9 57 Z'),
 # The supplied tablet/study pose contains a laptop; retain head and upper device.
 'tablet': ((45,500,165,650), 'M8 18 Q12 2 43 1 Q76 2 84 24 L83 56 Q81 66 73 72 L96 83 L100 114 L4 114 L4 83 L19 72 Q7 62 8 48 Z'),
}
def outline_mask(outline, width, height, mirrored):
    # Rasterize the simple M/L/H/V/Q/Z outlines to discard source pixels that
    # are hidden by the SVG clip. One-pixel dilation preserves edge coverage.
    tokens = re.findall(r'[MLHVQZ]|-?\d+(?:\.\d+)?', outline)
    mask = Image.new('L', (width, height), 0)
    draw = ImageDraw.Draw(mask)
    points = []; x = y = 0.0; index = 0; command = ''
    while index < len(tokens):
        if tokens[index].isalpha():
            command = tokens[index]; index += 1
            if command == 'Z':
                if points: draw.polygon(points, fill=255)
                points = []; continue
        if command in ('M', 'L'):
            x, y = float(tokens[index]), float(tokens[index+1]); index += 2
            points.append((x, y))
            if command == 'M': command = 'L'
        elif command == 'H':
            x = float(tokens[index]); index += 1; points.append((x, y))
        elif command == 'V':
            y = float(tokens[index]); index += 1; points.append((x, y))
        elif command == 'Q':
            cx, cy, nx, ny = map(float, tokens[index:index+4]); index += 4
            for step in range(1, 9):
                t = step / 8
                points.append(((1-t)**2*x + 2*(1-t)*t*cx + t*t*nx, (1-t)**2*y + 2*(1-t)*t*cy + t*t*ny))
            x, y = nx, ny
        else:
            raise ValueError('Unsupported outline command: ' + command)
    if points: draw.polygon(points, fill=255)
    if mirrored: mask = mask.transpose(Image.Transpose.FLIP_LEFT_RIGHT)
    return mask.filter(ImageFilter.MaxFilter(3)).load()

for name,(box,mask) in frames.items():
 im=atlas.crop(box);w,h=im.size;im.save(root/(name+'.png'))
 included=outline_mask(mask,w,h,False) if mask else None
 alpha=im.getchannel('A').load(); q=im.convert('RGB').quantize(colors=192,method=Image.Quantize.MEDIANCUT,dither=Image.Dither.NONE)
 pix=q.load();palette=q.getpalette();paths={}
 def val(x,y):
  a=alpha[x,y];r,g,b=im.getpixel((x,y))[:3]
  # The supplied sheet contains isolated red/yellow matte spill outside sprites.
  if (included is not None and not included[x,y]) or a<48 or (r>220 and g<50 and b<50) or (r>240 and g>220 and b<40):return None
  return pix[x,y],255 if a>=224 else ((a+31)//64)*64
 for y in range(h):
  x=0
  while x<w:
   key=val(x,y);start=x;x+=1
   while x<w and val(x,y)==key:x+=1
   if key is not None:paths.setdefault(key,[]).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
 with (root/(name+'.svg')).open('w') as f:
  f.write(f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 {w} {h}" shape-rendering="crispEdges">')
  if mask:f.write(f'<defs><clipPath id="subject"><path d="{mask}"/></clipPath></defs><g clip-path="url(#subject)">')
  for (index,opacity),runs in paths.items():
   r,g,b=palette[index*3:index*3+3];f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" opacity="{opacity/255:.3f}" d="{"".join(runs)}"/>')
  if mask:f.write('</g>')
  f.write('</svg>\n')
