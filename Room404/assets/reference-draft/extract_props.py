"""Trace visible reference pixels inside hand-drawn object outlines."""
from pathlib import Path
import json
import re
from PIL import Image, ImageDraw, ImageFilter

ROOT = Path(__file__).resolve().parents[3]
OUT = Path(__file__).resolve().parent
SOURCE = ROOT / 'Room404/assets/reference.png'
# Rectangles and polygons are in the 1536 × 1024 reference coordinate space.
OBJECTS = {
    'plant_shelf_left': ((0, 510, 80, 625), 'M39 11 Q44 11 46 23 L58 18 L61 22 L57 30 Q67 27 71 34 L66 38 Q73 41 75 48 L70 51 L74 60 L66 60 L61 56 L59 62 L63 66 L63 75 L61 79 L60 101 L55 109 H27 L22 105 L20 80 L18 76 L18 67 L21 62 L17 57 L10 61 L8 58 L13 50 L7 47 L10 42 L17 39 L12 34 L15 31 L25 33 L19 25 L19 22 L27 23 L32 27 L34 17 Z'),
    'plant_center': ((678, 810, 747, 899), 'M33 0 Q38 3 40 15 L50 8 Q54 8 54 19 L48 28 Q58 23 65 28 L59 33 Q66 38 67 43 L57 45 L52 43 L49 47 L53 50 L53 59 L50 63 L48 82 L44 88 H17 L13 83 L11 63 L8 59 L8 50 L12 47 L7 43 L2 44 L0 41 L9 32 L2 29 Q7 24 16 29 L10 19 L10 9 Q14 7 25 16 L29 4 Z'),
    'tablet_center': ((773, 835, 849, 911), 'M7 3 H69 Q73 3 73 7 V69 Q73 73 69 73 H7 Q3 73 3 69 V7 Q3 3 7 3 Z'),
    'window_main': ((479, 126, 797, 336), 'M1 1 H316 V209 H1 Z'),
    'door_main': ((871, 128, 1072, 388), 'M1 2 H199 V258 H1 Z'),
    'light_switch': ((819, 224, 853, 267), 'M1 1 H32 V41 H1 Z'),
    'desk_drawer_outer_left': ((100, 493, 166, 595), 'M1 1 H64 V84 L61 88 V100 H49 L48 89 H15 L14 100 H3 V88 L1 84 Z'),
    'desk_drawer_inner_left': ((270, 493, 339, 595), 'M2 1 H67 L66 84 L64 88 L63 100 H51 L50 89 H15 L14 100 H3 L3 88 L2 84 Z'),
    'bed_inner_side_rail_left': ((345, 130, 390, 315), 'M18 16 Q18 5 29 5 Q40 5 40 16 L40 128 L34 128 L34 35 Q32 33 30 34 L19 164 L10 164 L8 159 L22 41 Q23 35 27 33 L18 33 Z'),
    'bed_side_rail_left': ((70, 130, 133, 315), 'M29 16 Q29 5 40 5 Q52 5 52 16 L52 34 L47 35 Q48 39 47 44 L14 175 L4 181 L2 170 L29 43 Q30 38 34 34 L29 34 Z'),
    'bed_back_rail_left': ((108, 148, 375, 168), 'M2 1 H265 V19 H2 Z'),
    'bed_front_rail_left': ((90, 302, 345, 368), 'M1 7 H254 V62 H1 Z'),
    'bed_inner_post_left': ((335, 280, 375, 605), 'M7 19 Q7 8 18 8 Q29 8 30 19 L31 314 Q30 320 19 321 Q8 320 7 314 Z'),
    'bed_front_post_left': ((60, 280, 120, 610), 'M8 18 Q8 8 17 8 Q28 8 34 17 L36 268 L34 273 L10 273 L9 268 Z'),
    'pillow_left': ((115, 155, 260, 235), 'M21 14 Q23 12 29 12 H106 Q119 11 135 13 L139 17 L136 24 L132 29 L130 40 L128 57 Q112 55 91 56 Q68 59 48 67 H15 L17 57 L19 44 L20 24 Z'),
    'bed_bedding_left': ((92, 158, 372, 310), 'M20 8 Q25 2 40 2 H245 Q266 2 272 17 L267 145 H5 L10 23 Z'),
    'chair_left': ((160, 465, 265, 620), 'M10 13 Q10 6 16 5 H87 Q94 5 95 12 V63 Q95 68 89 68 H16 Q10 68 10 62 Z M11 62 L98 62 Q104 62 104 70 L103 91 L97 96 L17 96 L10 89 Z M14 88 L28 89 L26 123 L20 123 L20 145 Q20 149 16 149 H10 Q7 149 9 142 Z M84 88 L96 88 L104 144 Q104 149 99 149 H94 Q90 149 90 144 Z'),
    'backpack_left': ((90, 645, 175, 770), 'M8 42 Q9 33 14 31 L18 29 L19 18 Q20 8 27 7 Q35 5 40 13 Q54 12 63 23 Q72 33 72 49 Q80 53 79 64 L79 85 Q75 98 65 103 L58 112 L45 113 Q43 119 35 119 L29 117 L27 112 L15 110 Q8 107 6 100 L5 54 Z'),
    'lamp_left': ((105, 370, 173, 458), 'M5 26 L16 15 L20 15 Q20 3 27 1 Q36 0 40 13 Q51 15 60 27 Q64 32 62 38 Q57 47 40 50 Q30 51 24 48 L20 42 L20 23 L14 21 L10 27 L10 64 L15 67 L15 75 L10 78 L4 78 L2 73 L3 66 L4 60 L4 29 Z'),
    'laptop_left': ((174, 397, 264, 478), 'M8 1 Q8 0 12 0 H83 Q87 0 87 5 V50 L87 53 L85 70 H4 L7 52 L8 50 Z'),
    'books_left': ((111, 432, 157, 479), 'M9 2 L41 2 L43 9 L45 18 L40 37 L37 44 L2 44 L1 34 L1 23 Z'),
    'plant_left': ((279, 384, 329, 453), 'M25 1 L29 11 L36 5 L36 15 L44 10 L43 19 L49 23 L44 29 L41 34 L43 42 L40 46 L39 61 L35 67 H13 L10 62 L9 46 L6 42 L8 33 L3 28 L1 23 L9 21 L7 14 L16 16 L17 8 L22 12 Z'),
}
for left_name, (bbox, outline) in list(OBJECTS.items()):
    if not left_name.endswith('_left'):
        continue
    x0, y0, x1, y1 = bbox
    OBJECTS[left_name.replace('_left', '_right')] = ((1536 - x1, y0, 1536 - x0, y1), outline)

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

def extract(name, bbox, outline):
    image = Image.open(SOURCE).convert('RGB').crop(bbox)
    w, h = image.size; pixels = image.load(); paths = {}
    included = outline_mask(outline, w, h, name.endswith('_right'))
    for y in range(h):
        x = 0
        while x < w:
            if not included[x, y]:
                x += 1; continue
            color = pixels[x, y]; start = x; x += 1
            while x < w and included[x, y] and pixels[x, y] == color: x += 1
            paths.setdefault(color, []).append(f'M{start} {y}h{x-start}v1h-{x-start}z')
    mirror = f' transform="translate({w} 0) scale(-1 1)"' if name.endswith('_right') else ''
    with (OUT / f'{name}_original.svg').open('w') as f:
        f.write(f'<svg xmlns="http://www.w3.org/2000/svg" width="{w}" height="{h}" viewBox="0 0 {w} {h}" shape-rendering="crispEdges"><defs><clipPath id="outline"><path d="{outline}"{mirror}/></clipPath></defs><g clip-path="url(#outline)">')
        for (r, g, b), runs in paths.items():
            f.write(f'<path fill="#{r:02x}{g:02x}{b:02x}" d="{"".join(runs)}"/>')
        f.write('</g></svg>\n')

for name, (bbox, outline) in OBJECTS.items():
    extract(name, bbox, outline)

# Positions are normalized to the original reference; the SVG viewBox retains
# the crop dimensions so callers can place layers without guessing.
manifest = {"source": "../reference.png", "reference_width": 1536, "reference_height": 1024, "assets": []}
for name, (bbox, _) in OBJECTS.items():
    x0, y0, x1, y1 = bbox
    manifest["assets"].append({"id": name, "asset": name + "_original.svg", "x": x0 / 1536, "y": y0 / 1024, "width": (x1-x0) / 1536, "height": (y1-y0) / 1024, "status": "draft"})
for name, bbox in {
    "ladder_left_reconstructed": (340, 235, 415, 565),
    "ladder_right_reconstructed": (1121, 235, 1196, 565),
    "desk_surface_left_reconstructed": (90, 425, 342, 495),
    "desk_surface_right_reconstructed": (1194, 425, 1446, 495),
    "center_table_reconstructed": (636, 818, 890, 963),
    "rug_reconstructed": (420, 772, 1096, 956),
    "bookshelf_left_reconstructed": (0, 540, 100, 875),
    "bookshelf_right_reconstructed": (1436, 540, 1536, 875),
}.items():
    x0, y0, x1, y1 = bbox
    manifest["assets"].append({"id": name, "asset": name + ".svg", "x": x0 / 1536, "y": y0 / 1024, "width": (x1-x0) / 1536, "height": (y1-y0) / 1024, "status": "reconstructed_draft"})
(OUT / 'placements.json').write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + '\n')
