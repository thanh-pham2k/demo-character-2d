"""Fill furniture areas with approximate wall/floor under a traced reference."""
from pathlib import Path

root = Path(__file__).resolve().parents[3]
source = (root / 'Room404/assets/room-vector.svg').read_text().strip()
head, paths = source.split('>', 1)
paths = paths.removesuffix('</svg>')
# These are intentionally broad: the source is a flattened image, so obscured
# surfaces cannot be recovered. The underlay is an editable approximation.
regions = [(32, 65, 176, 244), (560, 65, 176, 244),
           (0, 254, 52, 181), (716, 254, 52, 181),
           (45, 318, 43, 70), (680, 318, 43, 70),
           (210, 383, 340, 97), (237, 62, 162, 105),
           (435, 63, 101, 132), (408, 111, 19, 22)]
mask = '<filter id="feather" x="-30%" y="-30%" width="160%" height="160%"><feGaussianBlur stdDeviation="3"/></filter><mask id="unobstructed"><rect width="768" height="512" fill="white"/>' + ''.join(
    f'<rect x="{x}" y="{y}" width="{w}" height="{h}" fill="black" filter="url(#feather)"/>' for x, y, w, h in regions
) + '</mask>'
under = '''<linearGradient id="wall" x1="0" y1="40" x2="0" y2="181" gradientUnits="userSpaceOnUse"><stop offset="0" stop-color="#e6a14a"/><stop offset=".35" stop-color="#f0ab4e"/><stop offset=".7" stop-color="#f2af4f"/><stop offset="1" stop-color="#eaa34a"/></linearGradient><linearGradient id="floor" x1="0" y1="190" x2="0" y2="471" gradientUnits="userSpaceOnUse"><stop offset="0" stop-color="#ae683b"/><stop offset=".12" stop-color="#b46a3a"/><stop offset=".25" stop-color="#c7783c"/><stop offset=".43" stop-color="#de8d45"/><stop offset=".60" stop-color="#ed9d4a"/><stop offset=".78" stop-color="#d88e4d"/><stop offset="1" stop-color="#e89c4e"/></linearGradient>'''
ground = '''<rect width="768" height="512" fill="#161c29"/><path d="M40 40 H728 V181 H40 Z" fill="url(#wall)"/><path d="M35 178 H733 V190 H35 Z" fill="#30323b"/><path d="M35 190 H733 L754 471 H14 Z" fill="url(#floor)"/><g fill="none" stroke="#a96536" stroke-opacity=".35" stroke-width=".6"><path d="M20 208 H748 M20 226 H748 M20 244 H748 M20 262 H748 M20 280 H748 M20 298 H748 M20 316 H748 M20 334 H748 M20 352 H748 M20 370 H748 M20 388 H748 M20 406 H748 M20 424 H748 M20 442 H748 M20 460 H748"/><path d="M85 190 L70 470 M175 190 L165 470 M265 190 L260 470 M355 190 V470 M445 190 L450 470 M535 190 L545 470 M625 190 L640 470"/></g>'''
output = head + '><defs>' + under + mask + '</defs>' + ground + '<g mask="url(#unobstructed)">' + paths + '</g></svg>\n'
(Path(__file__).resolve().parent / 'background_reconstructed.svg').write_text(output)
