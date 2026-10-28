"""Trace existing Lươn PNG frames as editable vector paths."""
from pathlib import Path
from PIL import Image

folder = Path(__file__).resolve().parent
for name in ('luon_idle', 'luon_walk_1', 'luon_walk_2', 'luon_walk_3'):
    im = Image.open(folder / f'{name}.png').convert('RGBA').resize((222, 222), Image.Resampling.LANCZOS)
    alpha = im.getchannel('A').load()
    colors = im.convert('RGB').quantize(colors=128, method=Image.Quantize.MEDIANCUT, dither=Image.Dither.NONE)
    pixels = colors.load(); palette = colors.getpalette(); paths = {}
    def value(x,y):
        a=alpha[x,y]
        return (pixels[x,y], 255 if a >= 224 else ((a + 31)//64)*64) if a >= 32 else None
    for y in range(222):
        x = 0
        while x < 222:
            key=value(x,y); start=x; x+=1
            while x < 222 and value(x,y)==key: x+=1
            if key is not None: paths.setdefault(key,[]).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
    with (folder / f'{name}.svg').open('w') as f:
        f.write('<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 222 222" shape-rendering="crispEdges">')
        for (index,opacity), runs in paths.items():
            r,g,b=palette[index*3:index*3+3]
            f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" opacity="{opacity/255:.3f}" d="{"".join(runs)}"/>')
        f.write('</svg>\n')
