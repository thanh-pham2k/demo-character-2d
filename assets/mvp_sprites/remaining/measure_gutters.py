"""Find shared transparent gutters in a generated grid before slicing.
Only sprite/prop sheets with empty gutters; backgrounds require explicit panel bounds.
This records measured crop geometry, it never edits the source sheet.
"""
import argparse,json
from pathlib import Path
from PIL import Image
BASE=Path(__file__).resolve().parent
p=argparse.ArgumentParser();p.add_argument('batch');p.add_argument('--source',default='source.png');args=p.parse_args()
directory=BASE/args.batch;planned=json.loads((directory/'manifest.planned.json').read_text());b=planned['batch']
if b['kind']=='environment':raise ValueError('Opaque backgrounds need measured panel bounds, not alpha gutters')
im=Image.open(directory/args.source).convert('RGBA');w,h=im.size;mask=im.getchannel('A').point(lambda v:255 if v>12 else 0)
def separator(axis,center,radius,restriction=None):
 length=w if axis==0 else h;runs=[]
 for x in range(max(1,center-radius),min(length-1,center+radius)):
  strip=mask.crop((x,restriction[0] if restriction else 0,x+1,restriction[1] if restriction else h) if axis==0 else (restriction[0] if restriction else 0,x,restriction[1] if restriction else w,x+1))
  if strip.getbbox() is None:
   if not runs or x!=runs[-1][-1]+1:runs.append([x])
   else:runs[-1].append(x)
 if not runs:raise ValueError(('No full transparent separator',axis,center,'inspect source/refine whole sheet'))
 best=max(runs,key=lambda r:(len(r),-abs(sum(r)/len(r)-center)))
 return (best[0]+best[-1])//2
cols,rows=b['columns'],b['rows']
def boundaries(axis,count,restriction=None):
 length=w if axis==0 else h
 return [0]+[separator(axis,round(i*length/count),round(length/count*.35),restriction) for i in range(1,count)]+[length]
try:xs=boundaries(0,cols)
except ValueError:xs=None
try:ys=boundaries(1,rows)
except ValueError:ys=None
if xs is None and ys is None:raise ValueError('No shared row or column gutter; requires explicit visually measured crops/refinement')
crops={};rowX={};colY={}
for a in planned['assets']:
 c,r=a['column'],a['row']
 if xs is None:
  if r not in rowX:rowX[r]=boundaries(0,cols,(ys[r],ys[r+1]))
  xx=rowX[r];yy=ys
 elif ys is None:
  if c not in colY:colY[c]=boundaries(1,rows,(xs[c],xs[c+1]))
  xx=xs;yy=colY[c]
 else:xx,yy=xs,ys
 crops[a['id']]=[xx[c],yy[r],xx[c+1]-xx[c],yy[r+1]-yy[r]]
(directory/'crops.json').write_text(json.dumps(crops,indent=2)+'\n')
measurement=dict(source=args.source,size=[w,h],alpha_threshold=12,x_boundaries=xs,y_boundaries=ys,per_row_x=rowX,per_column_y=colY,method='transparent_gutter_midpoints; row_or_column_specific_when_needed; visual_review_required')
(directory/'measurement.json').write_text(json.dumps(measurement,indent=2)+'\n')
print(json.dumps(dict(batch=args.batch,**measurement)))
