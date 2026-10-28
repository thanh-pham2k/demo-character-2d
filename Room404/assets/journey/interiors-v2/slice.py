from pathlib import Path
from PIL import Image
root = Path(__file__).resolve().parent
sheet = Image.open(root / 'sheet.png')
for name, box in [('wc-day', (7, 7, 835, 462)), ('wc-night', (848, 7, 1676, 462)), ('stairs-down', (7, 471, 835, 926)), ('stairs-up', (848, 471, 1676, 926))]:
    sheet.crop(box).save(root / f'{name}.png')
