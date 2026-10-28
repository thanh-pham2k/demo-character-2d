"""Group editable reference-derived bed layers into standalone SVGs."""
from pathlib import Path
import json
import re

folder = Path(__file__).resolve().parent
manifest = json.loads((folder / 'placements.json').read_text())
entries = {entry['id']: entry for entry in manifest['assets']}
for side in ('left', 'right'):
    origin_x = 60 if side == 'left' else 1121
    origin_y = 130
    names = [f'bed_back_rail_{side}', f'bed_side_rail_{side}', f'bed_inner_side_rail_{side}',
             f'bed_bedding_{side}', f'ladder_{side}_reconstructed', f'bed_front_rail_{side}',
             f'bed_front_post_{side}', f'bed_inner_post_{side}']
    pieces = []
    for name in names:
        item = entries[name]
        asset = item['asset']
        if name.startswith('bed_front_post_'):
            asset = name + '_reconstructed.svg'
        source = (folder / asset).read_text().strip()
        body = source.split('>', 1)[1].removesuffix('</svg>')
        # Each clip, gradient and mask must have a unique document-wide ID.
        ids = re.findall(r'\bid="([^"]+)"', body)
        for old in ids:
            new = name + '_' + old
            body = body.replace(f'id="{old}"', f'id="{new}"').replace(f'url(#{old})', f'url(#{new})')
        x = round(item['x'] * 1536) - origin_x
        y = round(item['y'] * 1024) - origin_y
        pieces.append(f'<g id="{name}" transform="translate({x} {y})">{body}</g>')
    (folder / f'bed_{side}_composite.svg').write_text(
        '<svg xmlns="http://www.w3.org/2000/svg" width="355" height="480" viewBox="0 0 355 480" shape-rendering="crispEdges">' + ''.join(pieces) + '</svg>\n'
    )
