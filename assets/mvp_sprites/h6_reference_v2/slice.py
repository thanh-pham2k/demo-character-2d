from pathlib import Path
import json
from PIL import Image

ROOT = Path(__file__).resolve().parent
OUT = ROOT.parents[2] / 'mobile-2d/Assets/Sprites/MVP/year1'
im = Image.open(ROOT/'final-sheet.png').convert('RGBA')
# Measured panel split; generation does not guarantee requested canvas dimensions.
w, h = im.size
split = 717
outputs = []
for name, rect in [('h6_gate_v2',(0,0,w,split)),('h6_classroom_v2',(0,split,w,h))]:
    crop=im.crop(rect)
    crop.save(OUT/(name+'.png'))
    outputs.append({'file':name+'.png','crop':rect,'size':crop.size,'pivot':[.5,.5]})
(ROOT/'manifest.json').write_text(json.dumps({'canvas':im.size,'grid':[1,2],
    'measured_split_y':split,'ppu':100,'filter':'Point','compression':'Uncompressed','outputs':outputs},indent=2)+'\n')
print(outputs)
