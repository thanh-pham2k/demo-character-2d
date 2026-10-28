"""Keep visible table pixels and approximate the areas hidden by props."""
from pathlib import Path
from PIL import Image

root = Path(__file__).resolve().parents[3]
out = Path(__file__).resolve().parent / 'center_table_reconstructed.svg'
im = Image.open(root / 'Room404/assets/reference.png').convert('RGB').crop((636, 818, 890, 963))
pixels = im.load(); paths = {}
def retained(x, y):
    return not ((42 <= x < 112 and y < 82) or (139 <= x < 213 and 17 <= y < 92))
for y in range(145):
    x = 0
    while x < 254:
        if not retained(x,y): x+=1; continue
        color = pixels[x, y]; start = x; x += 1
        while x < 254 and retained(x,y) and pixels[x, y] == color: x += 1
        paths.setdefault(color, []).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
outline = 'M9 2 H244 Q249 2 249 8 L253 103 Q253 115 246 116 H243 V141 H226 V116 H29 V141 H12 V116 H8 Q2 116 2 108 L5 9 Q5 2 9 2 Z'
holes = 'M40 0 H114 V84 H40 Z M137 15 H215 V94 H137 Z'
with out.open('w') as f:
    f.write(f'<svg xmlns="http://www.w3.org/2000/svg" width="254" height="145" viewBox="0 0 254 145" shape-rendering="crispEdges"><defs><clipPath id="table"><path d="{outline}"/></clipPath><mask id="visible"><rect width="254" height="145" fill="white"/><path d="{holes}" fill="black"/></mask><linearGradient id="wood" x2="0" y2="1"><stop stop-color="#df8c35"/><stop offset="1" stop-color="#ce7523"/></linearGradient></defs><g clip-path="url(#table)"><rect width="254" height="145" fill="url(#wood)"/><g mask="url(#visible)">')
    for (r, g, b), runs in paths.items():
        f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" d="{"".join(runs)}"/>')
    f.write('</g></g></svg>\n')
