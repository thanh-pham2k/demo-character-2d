"""Reproduce Year 1 batches; coordinates are measured, not assumed grid edges."""
from pathlib import Path
import json
from PIL import Image

ROOT = Path(__file__).resolve().parent
OUT = ROOT.parents[2] / 'mobile-2d/Assets/Sprites/MVP/year1'
OUT.mkdir(parents=True, exist_ok=True)
manifest = {'ppu': 100, 'filter': 'Point', 'compression': 'Uncompressed', 'outputs': []}

def save(sheet, name, rect, size=None):
    image = Image.open(ROOT / sheet).convert('RGBA').crop(rect)
    if size:
        box = image.getchannel('A').point(lambda p: 255 if p > 32 else 0).getbbox()
        assert box, name
        image = image.crop(box)
        image.thumbnail(size, Image.Resampling.NEAREST)
        canvas = Image.new('RGBA', size)
        canvas.alpha_composite(image, ((size[0]-image.width)//2, size[1]-image.height))
        image = canvas
    image.save(OUT / (name+'.png'))
    manifest['outputs'].append({'name':name,'sheet':sheet,'crop':rect,'size':image.size,'pivot':[.5,0] if size else [.5,.5]})

save('h6/source.png','h6_gate',(0,0,765,465))
save('h6/source.png','h6_classroom',(771,0,1536,465))
save('h6/source.png','dam_writing',(8,475,760,1024),(256,256))
save('h6/source.png','teacher',(778,475,1536,1024),(256,320))
for i,name in enumerate(['pot_0','pot_1','luon_floor','dam_floor','long_floor','to_floor']):
    x,y=(i%3)*512,(i//3)*512
    save('hotpot/final-sheet.png',name,(x,y,x+512,y+512),(384,384) if i<2 else (256,256))
(ROOT/'manifest.json').write_text(json.dumps(manifest,indent=2)+'\n')
print('Sliced',len(manifest['outputs']),'Year 1 sprites to',OUT)
