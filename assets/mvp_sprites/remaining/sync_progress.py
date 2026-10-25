"""Synchronize plan with actual generated/sliced artifacts, not intentions.
--visual-passed is explicit human/model visual inspection attestation for named batches.
Unity import is accepted only against current PNG hashes in the Unity report.
"""
from pathlib import Path
import argparse,json,hashlib,re
from datetime import datetime,timezone
BASE=Path(__file__).resolve().parent;ROOT=BASE.parents[2]
p=argparse.ArgumentParser();p.add_argument('--visual-passed',nargs='*',default=[]);args=p.parse_args()
planPath=ROOT/'story/plans/ASSET_BATCH_PLAN.json';plan=json.loads(planPath.read_text())
reportPath=BASE/'review/report.json';report=json.loads(reportPath.read_text()) if reportPath.exists() else {}
importEvidence={x['id']:x['sha256'] for x in report.get('evidence',[])} if report.get('success') else {}
generated=[];visual=[];imported=[];issues=[]
for batch in plan['batches']:
    directory=BASE/batch['id'];mp=directory/'manifest.json'
    if not mp.exists():continue
    manifest=json.loads(mp.read_text());assert (ROOT/manifest['source_file']).exists()
    actual={a['id']:a for a in manifest['outputs']}
    if args.visual_passed and batch['id'] in args.visual_passed:
        manifest['qa']['visual']='pass_source_and_sliced_preview_review'
        for a in manifest['outputs']:a['visual_qa']='pass_source_and_sliced_preview_review'
    generated.append(batch['id']);batch['status']='generated_sliced';batch['source_file']=manifest['source_file']
    for asset in plan['new_assets']:
        if asset['batch']!=batch['id']:continue
        rec=actual[asset['id']];path=ROOT/asset['output_file'];digest=hashlib.sha256(path.read_bytes()).hexdigest()
        assert digest==rec['sha256'],asset['id']+' output hash mismatch'
        asset.update(source_file=manifest['source_file'],actual_crop_xywh=rec['source_crop_xywh'],pivot=rec['pivot'])
        hasVisual=manifest['qa']['visual'].startswith('pass')
        if hasVisual:visual.append(asset['id'])
        hasImport=importEvidence.get(asset['id'])==digest
        if hasImport:imported.append(asset['id'])
        asset['qa_status']='visual_pass_and_unity_import_verified' if hasImport and hasVisual else 'visual_pass_import_pending' if hasVisual else 'sliced_visual_QA_pending'
    manifest['qa']['unity_import']='pass_current_hashes' if all(a['id'] in imported for a in manifest['outputs']) else 'pending_current_import_report'
    mp.write_text(json.dumps(manifest,ensure_ascii=False,indent=2)+'\n')
    pp=directory/'manifest.planned.json';planned=json.loads(pp.read_text());planned['gate'].update(generation='generated_composite_batch',slicing='actual_crops_and_hashes_verified',visual_qa=manifest['qa']['visual'],unity_import=manifest['qa']['unity_import'])
    pp.write_text(json.dumps(planned,ensure_ascii=False,indent=2)+'\n')
plan.update(status='in_progress',generated=bool(generated));planPath.write_text(json.dumps(plan,ensure_ascii=False,indent=2)+'\n')
progress=dict(updated_utc=datetime.now(timezone.utc).isoformat(),objective='Execute asset production plan: blockout, batches, slice, visual QA, import/catalog and gameplay integration',status='in_progress',planned_batches=len(plan['batches']),planned_outputs=len(plan['new_assets']),prepared_batches=len(plan['batches']),generated_batches=generated,generated_outputs=sum(len(b['asset_ids']) for b in plan['batches'] if b['id'] in generated),visually_reviewed_outputs=visual,unity_import_verified_outputs=imported,gameplay_integration='pending; import/catalog does not prove full45mission flow',remaining_batches=[b['id'] for b in plan['batches'] if b['id'] not in generated])
motion_path=BASE/'motion-review.json'
motion=json.loads(motion_path.read_text()) if motion_path.exists() else {}
progress['visual_review_scope']='Static source and sliced preview only; not motion/gameplay approval'
progress['motion_review']={
    'cycles':motion.get('cycles',0),
    'approved_cycles':sum(r.get('motion_approval','').startswith('pass') for r in motion.get('reports',[])),
    'pending_cycles':[r['ids'][0].removesuffix('_0') for r in motion.get('reports',[]) if not r.get('motion_approval','').startswith('pass')],
    'warnings':[{'cycle':r['ids'][0].removesuffix('_0'),'warnings':r['warnings']} for r in motion.get('reports',[]) if r.get('warnings')],
    'native_clip_sample_checks':sum('native clip samples' in c for c in report.get('checks',[])) if report.get('success') else 0,
    'scope':'Native sample checks prove frame binding/timing only; gait and gameplay remain pending'
}
campaign_report_path=BASE/'review/production-campaign-model.json'
campaign_definition=ROOT/'mobile-2d/Assets/Resources/ProductionCampaign/missions.json'
if campaign_report_path.exists() and campaign_definition.exists():
    campaign_report=json.loads(campaign_report_path.read_text())
    definition_hash=hashlib.sha256(campaign_definition.read_bytes()).hexdigest()
    progress['production_campaign_model']={
        'status':'verified_current_definition' if campaign_report.get('success') and campaign_report.get('definitionSha256')==definition_hash else 'pending_current_definition_check',
        'report':str(campaign_report_path.relative_to(ROOT)),
        'assertions':campaign_report.get('assertions',0),
        'scope':campaign_report.get('scope',''),
        'runtime_scene_integration':'pending; passing the event model does not prove playable missions'
    }
opening_evidence_path=BASE/'review/opening-runtime-evidence.json'
if opening_evidence_path.exists():
    opening=json.loads(opening_evidence_path.read_text())
    current=opening.get('success',False) and all((ROOT/f['path']).exists() and hashlib.sha256((ROOT/f['path']).read_bytes()).hexdigest()==f['sha256'] for f in opening.get('files',[]))
    progress['production_opening']={
        'status':'prototype_verified_current_hashes' if current else 'pending_current_source_verification',
        'missions':opening.get('missions',[1,2,3]), 'playmode_checks':opening.get('checks',0),
        'scope':opening.get('scope',''), 'evidence':str(opening_evidence_path.relative_to(ROOT))
    }
(BASE/'progress.json').write_text(json.dumps(progress,ensure_ascii=False,indent=2)+'\n')
# Keep the user-facing mission map aligned with the machine-readable plan.
mdpath=ROOT/'story/REMAINING_ASSET_INVENTORY.md';md=mdpath.read_text()
md=re.sub(r'Ngày cập nhật: 25/10/2026\. \*\*Đang thực thi:[^\n]+',f"Ngày cập nhật: 25/10/2026. **Đang thực thi: {len(generated)}/{len(plan['batches'])} sheet đã generate/slice, {progress['generated_outputs']}/{len(plan['new_assets'])} output.**",md,count=1)
for mission in plan['missions']:
    if mission['id']==5:mission['data_note']='Pose ngồi cũ là ngồi sàn; dùng chung bốn pose ghế C02 với M22, không thêm sheet.'
    row=f"| M{mission['id']:02} | {', '.join(mission['reuse'])} | {', '.join(mission['new_assets']) or 'Không'} | {', '.join(mission['animation_batches']) or 'Không'} | {', '.join(mission['unity'])} | {mission['data_note']} |"
    # Only the six-column mission map, not state tables.
    md=re.sub(r'^\| M'+f"{mission['id']:02}"+r' \| [^\n]+\| [^\n]+\| [^\n]+\| [^\n]+\| [^\n]+\|$',lambda _:row,md,count=1,flags=re.M)
for asset in plan['new_assets']:
    if asset['kind']=='animation':continue
    aid=asset['id'];pattern=r'^(\| \d+ \| `'+re.escape(aid)+r'` \| )[^|]+( \| [^\n]+)$'
    md=re.sub(pattern,lambda m:m[1]+', '.join(f'M{x:02}' for x in asset['missions'])+m[2],md,flags=re.M)
mdpath.write_text(md)
planPath.write_text(json.dumps(plan,ensure_ascii=False,indent=2)+'\n')
print(json.dumps({k:progress[k] for k in ['generated_batches','generated_outputs','remaining_batches']},ensure_ascii=False));print('Visual:',len(visual),'Unity import current hashes:',len(imported))
