from pathlib import Path
from PIL import Image,ImageDraw
import shutil
root=Path(__file__).resolve().parents[3]
here=Path(__file__).resolve().parent
source=root/'mobile-2d/PlayModeInteractionReports'
out=here/'review';out.mkdir(exist_ok=True)
for name in ['door-open','door-closed','Chair_Left-sitting','Chair_Right-sitting','BunkBed_Left-lying-a','BunkBed_Right-lying-a']:
 shutil.copyfile(source/(name+'.png'),out/(name+'.png'))
panels=[('Door: hallway','door-open'),('Chair: registered seat','Chair_Left-sitting'),('Bed: left','BunkBed_Left-lying-a'),('Bed: right','BunkBed_Right-lying-a')]
canvas=Image.new('RGB',(1280,760),'#eeeeee');draw=ImageDraw.Draw(canvas)
for i,(label,name) in enumerate(panels):
 xy=(i%2*640,i//2*380)
 canvas.paste(Image.open(out/(name+'.png')).resize((640,360)),(xy[0],xy[1]+20))
 draw.text((xy[0]+10,xy[1]+4),label,fill='black')
canvas.save(out/'overview.png')
for side in ['Left','Right']:
 names=[source/f'BunkBed_{side}-enter.png']+sorted(source.glob(f'BunkBed_{side}-enter-frame-*.png'))
 names += [source/f'BunkBed_{side}-lying-a.png',source/f'BunkBed_{side}-lying-b.png',source/f'BunkBed_{side}-sleep.png']
 names += sorted(source.glob(f'BunkBed_{side}-exit-frame-*.png'))
 frames=[Image.open(p).resize((768,432)).convert('P',palette=Image.Palette.ADAPTIVE) for p in names]
 durations=[160 if 'frame' in p.name or '-enter.png' in p.name else 700 for p in names]
 frames[0].save(out/f'bed-{side.lower()}-playmode.gif',save_all=True,append_images=frames[1:],duration=durations,loop=0,disposal=2)
print(out)
