"""Add approximate vector details where reference objects are occluded."""
from pathlib import Path

folder = Path(__file__).resolve().parent
for side in ('left', 'right'):
    source = (folder / f'laptop_{side}_original.svg').read_text()
    transform = ' transform="translate(90 0) scale(-1 1)"' if side == 'right' else ''
    reconstruction = (
        f'<g{transform}>'
        '<path d="M4 68 L85 68 L85 77 Q84 79 81 79 H6 Q3 79 3 77 Z" fill="#47443e" stroke="#24231f" stroke-width="2"/>'
        '<path d="M6 73 H83" fill="none" stroke="#77736c" stroke-width="1"/>'
        '</g>'
    )
    output = source.replace('</defs><g clip-path="url(#outline)">', '</defs>' + reconstruction + '<g clip-path="url(#outline)">', 1)
    (folder / f'laptop_{side}_reconstructed.svg').write_text(output)

for side in ('left', 'right'):
    source = (folder / f'bed_front_post_{side}_original.svg').read_text()
    transform = ' transform="translate(60 0) scale(-1 1)"' if side == 'right' else ''
    reconstruction = (
        f'<g{transform}>'
        '<path d="M10 264 H35 L35 314 Q35 319 29 320 H16 Q10 319 10 314 Z" fill="#253342" stroke="#121b25" stroke-width="2"/>'
        '<path d="M14 266 V313 Q14 317 17 318" fill="none" stroke="#778b99" stroke-width="2"/>'
        '<path d="M30 266 V314" fill="none" stroke="#b58a5b" stroke-width="2"/>'
        '</g>'
    )
    output = source.replace('</defs><g clip-path="url(#outline)">', '</defs>' + reconstruction + '<g clip-path="url(#outline)">', 1)
    (folder / f'bed_front_post_{side}_reconstructed.svg').write_text(output)
