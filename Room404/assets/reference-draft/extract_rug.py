"""Trace the visible rug and approximate the section underneath the table."""
from pathlib import Path
from PIL import Image
root = Path(__file__).resolve().parents[3]
out = Path(__file__).resolve().parent / 'rug_reconstructed.svg'
im = Image.open(root / 'Room404/assets/reference.png').convert('RGB').crop((420, 772, 1096, 956))
pixels = im.load(); paths = {}
def retained(x, y):
    return not (216 <= x < 468 and y >= 45)
for y in range(184):
    x = 0
    while x < 676:
        if not retained(x,y): x+=1; continue
        color = pixels[x, y]; start = x; x += 1
        while x < 676 and retained(x,y) and pixels[x, y] == color: x += 1
        paths.setdefault(color, []).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
outline = 'M34 1 H643 L674 183 H5 Z'
hole = 'M214 43 H470 V184 H214 Z'
with out.open('w') as f:
    f.write(f'<svg xmlns="http://www.w3.org/2000/svg" width="676" height="184" viewBox="0 0 676 184" shape-rendering="crispEdges"><defs><clipPath id="rug"><path d="{outline}"/></clipPath><mask id="visible"><rect width="676" height="184" fill="white"/><path d="{hole}" fill="black"/></mask></defs><g clip-path="url(#rug)"><rect width="676" height="184" fill="#347fab"/><path d="M65 29 H610 L628 182 H46 Z" fill="#d4c6a0"/><path d="M88 40 H586 L601 182 H72 Z" fill="#428aab"/><path d="M85 72 H590 M80 119 H596 M76 166 H601" fill="none" stroke="#d4c6a0" stroke-width="11"/><g mask="url(#visible)">')
    for (r, g, b), runs in paths.items():
        f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" d="{"".join(runs)}"/>')
    f.write('</g></g></svg>\n')
