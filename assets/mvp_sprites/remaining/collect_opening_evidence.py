"""Capture evidence for the current M01-M09 prototype only after a fresh full run.
Run with an explicit visual-review note after inspecting captures. Does not run
Unity or claim that later missions / device / artistic animation QA have passed.
"""
import argparse,hashlib,json,re
from pathlib import Path
from datetime import datetime
BASE=Path(__file__).resolve().parent
ROOT=BASE.parents[2]
parser=argparse.ArgumentParser()
parser.add_argument('--visual-review',required=True)
args=parser.parse_args()
evidence_path=BASE/'review/opening-runtime-evidence.json'
previous=json.loads(evidence_path.read_text())
report_path=BASE/'review/opening-playmode/report.json'
report=json.loads(report_path.read_text())
assert report['success'] and not report['errors'], 'Full Play Mode run did not pass'
assert report['scope'].startswith('M01-M09'), 'A school-only fixture is not a full opening run'
assert sum('Registration opens M10choice' in label for label in report['passed'])==2, 'Both bed routes must complete M09'
paths={f['path'] for f in previous['files']}
paths.update(str(p.relative_to(ROOT)) for p in (ROOT/'mobile-2d/Assets/Scripts').glob('Production*School*.cs'))
paths.update(str(p.relative_to(ROOT)) for p in (ROOT/'mobile-2d/Assets/Editor').glob('ProductionOpeningSchool*.cs'))
paths.update('mobile-2d/Assets/Scripts/'+name+'.cs' for name in ['DormJourneyController','CampusCommuteController','ICommuteProgress','ProductionCommuteProgress','RestroomInteractable','BusStopInteractable','CalculusPuzzleModal','PlayerInteractionStateController','PlayerInteractionDetector'])
paths.update('mobile-2d/Assets/Sprites/MVP/'+name+'.png' for name in ['commute/campus','commute/bus_33','commute/bus_sign','commute/study_0','dorm_journey/hallway','dorm_journey/wc_male','dorm_journey/wc_female','year1/h6_gate_v2','year1/h6_classroom_v2','year1/teacher','remaining/C00/npc_h6_classmate_writing'])
paths.update(['mobile-2d/Assets/Room404Layout/H6_DeskForegroundV2.asset','mobile-2d/Assets/Room404Layout/DoorOpeningSprite.asset'])
paths.update(str(p.relative_to(ROOT)) for p in (BASE/'review/opening-playmode').glob('school-*.png'))
paths.update(str(p.relative_to(ROOT)) for p in (BASE/'review/opening-playmode').glob('year1-*.png'))
paths.update(str(p.relative_to(ROOT)) for p in (ROOT/'mobile-2d/Assets/Scripts').glob('ProductionYearOne*.cs'))
paths.update(['mobile-2d/Assets/Scripts/ProductionMorningClock.cs','mobile-2d/Assets/Scripts/ProductionOpeningYearOne.cs','mobile-2d/Assets/Editor/ProductionOpeningYearOneBuilder.cs','mobile-2d/Assets/Editor/ProductionOpeningYearOnePlayChecks.cs','mobile-2d/Assets/Editor/ProductionYearOneChecks.cs','mobile-2d/Assets/Resources/ProductionCampaign/year-one-courses.json','mobile-2d/Assets/AssetProduction/HotpotCord.mat','assets/mvp_sprites/remaining/review/production-year-one-data.json','assets/mvp_sprites/remaining/review/opening-lifecycle-playmode/report.json','assets/mvp_sprites/remaining/review/year-one-production-playmode/report.json'])
paths.update('mobile-2d/Assets/Sprites/MVP/'+name+'.png' for name in ['year1/pot_0','year1/pot_1','year1/luon_floor','year1/dam_floor','year1/long_floor','year1/to_floor','environment/light_switch_socket'])
paths.update(str(p.relative_to(ROOT)) for p in (BASE/'review/year-one-production-playmode').glob('year1-*.png'))
paths.update(['mobile-2d/Assets/Scripts/ProductionOpeningRegistration.cs','mobile-2d/Assets/Editor/ProductionOpeningRegistrationBuilder.cs','mobile-2d/Assets/Editor/ProductionOpeningRegistrationPlayChecks.cs','assets/mvp_sprites/remaining/review/registration-playmode/report.json'])
paths.update(str(p.relative_to(ROOT)) for p in (BASE/'review/registration-playmode').glob('registration-*.png'))
paths.update(str(p.relative_to(ROOT)) for p in (BASE/'review/opening-playmode').glob('registration-*.png'))
# Include legacy and generated images actually referenced by the saved scene/crops.
scene=ROOT/'mobile-2d/Assets/Scenes/Production_Opening.unity'
references=scene.read_text()+'\n'+'\n'.join((ROOT/name).read_text() for name in paths if name.endswith('.asset'))
guids=set(re.findall(r'guid: ([0-9a-f]{32})',references))
for meta in (ROOT/'mobile-2d/Assets/Sprites/MVP').rglob('*.png.meta'):
    match=re.search(r'^guid: ([0-9a-f]{32})',meta.read_text(),re.M)
    if match and match[1] in guids:paths.add(str(meta.with_suffix('').relative_to(ROOT)))
for name in list(paths):
    meta=ROOT/(name+'.meta')
    if meta.exists():paths.add(str(meta.relative_to(ROOT)))
stamp=re.sub(r'(\.\d{6})\d+',r'\1',report['finishedUtc'].replace('Z','+00:00'))
finished=datetime.fromisoformat(stamp).timestamp()
assert all((ROOT/name).exists() for name in paths), 'Missing source/evidence file'
assert all((ROOT/name).stat().st_mtime<=finished for name in paths if name.endswith(('.cs','.unity','.asset','.png','.meta','.mat','.json')) and name!=str(report_path.relative_to(ROOT))), 'Source/scene/art changed after this report; run the affected checks again'
definition=ROOT/'mobile-2d/Assets/Resources/ProductionCampaign/missions.json'
model=json.loads((BASE/'review/production-campaign-model.json').read_text())
assert model['success'] and model['definitionSha256']==hashlib.sha256(definition.read_bytes()).hexdigest(), 'Model evidence is stale'
result=dict(scope='M01-M09 prototype physical route, both bed choices and reviewed staging captures; M10-M45 runtime and device/audio/detail animation gates remain pending',success=True,finishedUtc=report['finishedUtc'],checks=len(report['passed']),report=str(report_path.relative_to(ROOT)),files=[dict(path=name,sha256=hashlib.sha256((ROOT/name).read_bytes()).hexdigest()) for name in sorted(paths)],visual_review=args.visual_review,missions=list(range(1,10)))
evidence_path.write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n')
print(f"Recorded {result['checks']} checks / {len(paths)} hashed files for M01-M09; full game remains pending")
