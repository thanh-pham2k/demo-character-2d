"""Create inspectable motion previews and registration metrics; never auto-approve gait."""
from pathlib import Path
from PIL import Image,ImageDraw
import json,hashlib
BASE=Path(__file__).resolve().parent;ROOT=BASE.parents[2];plan=json.loads((ROOT/'story/plans/ASSET_BATCH_PLAN.json').read_text())
assets={a['id']:a for a in plan['new_assets']};reports=[]
for batch in plan['batches']:
 if batch['kind']!='animation':continue
 groups=[]
 for row in range(batch['rows']):
  ids=batch['asset_ids'][row*batch['columns']:(row+1)*batch['columns']]
  if batch['columns']==5:ids=ids[1:]
  frames=[Image.open(ROOT/assets[aid]['output_file']).convert('RGBA') for aid in ids];hashes=[hashlib.sha256((ROOT/assets[aid]['output_file']).read_bytes()).hexdigest() for aid in ids]
  heights=[];feet=[];legWidths=[]
  for im in frames:
   alpha=im.getchannel('A').point(lambda a:255 if a>12 else 0);box=alpha.getbbox();heights.append(box[3]-box[1]);feet.append(box[3]);legs=alpha.crop((0,240,im.width,im.height)).getbbox();legWidths.append(legs[2]-legs[0] if legs else 0)
  warnings=[]
  if len(set(hashes))<4:warnings.append('duplicate_frame_pixels')
  if max(heights)-min(heights)>7:warnings.append('body_height_bob_exceeds7px_review')
  if max(feet)-min(feet)>2:warnings.append('feet_baseline_drift_review')
  if 'right_walk' in ids[0]:
   ratio=((legWidths[0]+legWidths[2])/2)/max(1,(legWidths[1]+legWidths[3])/2)
   if ratio<1.15:warnings.append('side_contact_and_passing_silhouettes_too_similar_review')
  rec=dict(batch=batch['id'],ids=ids,unique_pixel_frames=len(set(hashes)),heights=heights,feet_bottom_y=feet,lower_leg_widths=legWidths,warnings=warnings,motion_approval='pending_manual_loop_review_and_gameplay_use')
  reports.append(rec);groups.append((ids,frames))
 previews=[]
 for i in range(4):
  board=Image.new('RGB',(300*len(groups),390),(45,54,63));d=ImageDraw.Draw(board)
  for j,(ids,frames) in enumerate(groups):
   im=frames[i];board.paste(im,(j*300+(300-im.width)//2,15),im);d.text((j*300+5,345),ids[i],fill='white')
  previews.append(board)
 previews[0].save(BASE/batch['id']/'cycles.gif',save_all=True,append_images=previews[1:],duration=125,loop=0,disposal=2)
 (BASE/batch['id']/'motion-review.json').write_text(json.dumps([r for r in reports if r['batch']==batch['id']],indent=2)+'\n')
 mpath=BASE/batch['id']/'manifest.json';m=json.loads(mpath.read_text());m['qa']['motion']='pending_manual_loop_review';m['qa']['registration']='metrics_recorded_in_motion-review.json';mpath.write_text(json.dumps(m,ensure_ascii=False,indent=2)+'\n')
(BASE/'motion-review.json').write_text(json.dumps(dict(scope='Pixel registration and phase proxy only; no automatic artistic approval',cycles=len(reports),reports=reports),indent=2)+'\n')
print('Created GIF previews for',len(reports),'cycles')
for r in reports:
 if r['warnings']:print(r['ids'][0],':',','.join(r['warnings']))
