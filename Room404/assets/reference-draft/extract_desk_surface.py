"""Extract desk pixels and approximate areas covered by props."""
from pathlib import Path
from PIL import Image

root = Path(__file__).resolve().parents[3]
folder = Path(__file__).resolve().parent
source = Image.open(root / 'Room404/assets/reference.png').convert('RGB')
for side, bbox in [('left', (90, 425, 342, 495)), ('right', (1194, 425, 1446, 495))]:
    im = source.crop(bbox)
    px = im.load(); paths = {}
    def retained(x, y):
        x = 251-x if side == "right" else x
        return not ((17 <= x < 68 and y < 50) or (82 <= x < 174 and y < 68) or (187 <= x < 239 and y < 25))
    for y in range(70):
        x = 0
        while x < 252:
            if not retained(x,y): x+=1; continue
            color = px[x, y]; start = x; x += 1
            while x < 252 and retained(x,y) and px[x, y] == color: x += 1
            paths.setdefault(color, []).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
    transform = ' transform="translate(252 0) scale(-1 1)"' if side == 'right' else ''
    outline = 'M13 4 H241 L251 47 L246 68 H3 L1 47 Z'
    # These regions contain a lamp, books, laptop, chair and plant in the reference.
    holes = 'M15 0 H70 V52 H15 Z M80 0 H176 V70 H80 Z M185 0 H241 V27 H185 Z'
    with (folder / f'desk_surface_{side}_reconstructed.svg').open('w') as f:
        f.write(f'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 252 70" width="252" height="70" shape-rendering="crispEdges"><defs><clipPath id="surface"><path d="{outline}"{transform}/></clipPath><mask id="visible"><rect width="252" height="70" fill="white"/><path d="{holes}"{transform} fill="black"/></mask><linearGradient id="wood" x2="0" y2="1"><stop stop-color="#e99a39"/><stop offset="0.7" stop-color="#fbb251"/><stop offset="1" stop-color="#b56823"/></linearGradient></defs><g clip-path="url(#surface)"><rect width="252" height="70" fill="url(#wood)"/><path d="M2 49 H250 V68 H2 Z" fill="#ba6b24"/><path d="M4 50 H248" stroke="#f4b251" stroke-width="2"/><g mask="url(#visible)">')
        for (r, g, b), runs in paths.items():
            f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" d="{"".join(runs)}"/>')
        f.write('</g></g></svg>\n')
