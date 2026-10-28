from pathlib import Path
from PIL import Image
root = Path(__file__).resolve().parent
sheet = Image.open(root / 'sheet.png')
for name, box in [('day', (0, 0, 1942, 394)), ('night', (0, 411, 1942, 809))]:
    sheet.crop(box).save(root / f'{name}.png')
