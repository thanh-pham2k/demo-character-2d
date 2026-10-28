"""Group reference-derived desk parts into standalone SVGs."""
from pathlib import Path
import json
import re

folder = Path(__file__).resolve().parent
manifest = json.loads((folder / 'placements.json').read_text())
entries = {entry['id']: entry for entry in manifest['assets']}
for side in ('left', 'right'):
    origin_x = 90 if side == 'left' else 1194
    origin_y = 425
    names = [f'desk_drawer_outer_{side}', f'desk_drawer_inner_{side}', f'desk_surface_{side}_reconstructed']
    pieces = []
    for name in names:
        item = entries[name]
        source = (folder / item['asset']).read_text().strip()
        body = source.split('>', 1)[1].removesuffix('</svg>')
        for old in re.findall(r'\bid="([^"]+)"', body):
            new = name + '_' + old
            body = body.replace(f'id="{old}"', f'id="{new}"').replace(f'url(#{old})', f'url(#{new})')
        x = round(item['x'] * 1536) - origin_x
        y = round(item['y'] * 1024) - origin_y
        pieces.append(f'<g id="{name}" transform="translate({x} {y})">{body}</g>')
    (folder / f'desk_{side}_composite.svg').write_text(
        '<svg xmlns="http://www.w3.org/2000/svg" width="252" height="170" viewBox="0 0 252 170" shape-rendering="crispEdges">' + ''.join(pieces) + '</svg>\n'
    )
