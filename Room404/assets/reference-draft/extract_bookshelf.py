"""Extract visible shelves; approximate their tops under the plants."""
from pathlib import Path
from PIL import Image

root = Path(__file__).resolve().parents[3]
folder = Path(__file__).resolve().parent
source = Image.open(root / 'Room404/assets/reference.png').convert('RGB')
for side, bbox in [('left', (0, 540, 100, 875)), ('right', (1436, 540, 1536, 875))]:
    im = source.crop(bbox); pixels = im.load(); paths = {}
    def retained(x, y):
        x = 99-x if side == "right" else x
        return not (12 <= x < 71 and y < 80)
    for y in range(335):
        x = 0
        while x < 100:
            if not retained(x,y): x+=1; continue
            color = pixels[x, y]; start = x; x += 1
            while x < 100 and retained(x,y) and pixels[x, y] == color: x += 1
            paths.setdefault(color, []).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
    flip = ' transform="translate(100 0) scale(-1 1)"' if side == 'right' else ''
    outline = 'M31 10 H92 Q95 10 95 16 L95 219 L69 318 L69 326 H55 V315 H0 V139 Z'
    hole = 'M10 0 H73 V82 H10 Z'
    with (folder / f'bookshelf_{side}_reconstructed.svg').open('w') as f:
        f.write(f'<svg xmlns="http://www.w3.org/2000/svg" width="100" height="335" viewBox="0 0 100 335" shape-rendering="crispEdges"><defs><clipPath id="shelf"><path d="{outline}"{flip}/></clipPath><mask id="visible"><rect width="100" height="335" fill="white"/><path d="{hole}"{flip} fill="black"/></mask><linearGradient id="wood" x2="0" y2="1"><stop stop-color="#e89a38"/><stop offset="1" stop-color="#aa551b"/></linearGradient></defs><g clip-path="url(#shelf)"><rect width="100" height="335" fill="url(#wood)"/><g mask="url(#visible)">')
        for (r, g, b), runs in paths.items():
            f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" d="{"".join(runs)}"/>')
        f.write('</g></g></svg>\n')
