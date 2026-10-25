from pathlib import Path
from PIL import Image,ImageDraw,ImageOps
import shutil,json
here=Path(__file__).resolve().parent
root=here.parents[2]
source=root/'mobile-2d/PlayModeInteractionReports'
out=here/'review';out.mkdir(exist_ok=True)
panels=[('Desk: study menu','Chair_Left-study-ui'),('Floor 4: arrival','journey-hallway-entry'),('WC Nu: entry refused','journey-wc-female-blocked'),('Stairs: ready for Bus 33 / H6','journey-stairs-ready')]
canvas=Image.new('RGB',(1280,680),'#10212a');draw=ImageDraw.Draw(canvas)
for i,(label,name) in enumerate(panels):
 im=Image.open(source/(name+'.png')).convert('RGB')
 shutil.copyfile(source/(name+'.png'),out/(name+'.png'))
 thumb=ImageOps.contain(im,(640,310))
 x,y=i%2*640,i//2*340
 canvas.paste(thumb,(x+(640-thumb.width)//2,y+25))
 draw.text((x+12,y+6),label,fill='white')
canvas.save(out/'overview.png')
for name in ['journey-refreshed','journey-wc-washing','journey-room-return','Chair_Left-computer-ui']:
 shutil.copyfile(source/(name+'.png'),out/(name+'.png'))
for name in ['report.json','story-report.json']:
 shutil.copyfile(source/name,out/name)
print(out)
