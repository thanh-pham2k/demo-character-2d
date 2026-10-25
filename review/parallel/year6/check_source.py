"""Read-only source contract checks; does not compile C# or start Unity."""
from pathlib import Path
import hashlib,json,re
root=Path(__file__).resolve().parents[3]
runtime=root/'mobile-2d/Assets/Scripts/ProductionChapters/Year6'
editor=root/'mobile-2d/Assets/Editor/ProductionChapters/Year6'
data=json.loads((root/'mobile-2d/Assets/Resources/ProductionCampaign/missions.json').read_text())
expected=[s['id'] for m in data['missions'] if 37<=m['id']<=45 for s in m['steps']]
activities=(runtime/'Year6Activities.cs').read_text()
actual=re.findall(r'A\("([^"]+)"',activities)
checks=[]
def check(ok,label):
    checks.append({'check':label,'passed':bool(ok)})
    assert ok,label
check(actual==expected,'47 activity IDs match current authored event order')
check(len(actual)==len(set(actual)),'No duplicate activity IDs')
module=(runtime/'Year6ChapterModule.cs').read_text()
check('IProductionChapterModule,IInteractable' in module,'Injected chapter interface and interaction owner')
check('new ProductionCampaignSession' not in module,'No second gameplay session')
check('context.Commit(mission,step,"",out var failure)' in module,'Commit current event only')
check('IsInteractingWith(this)' in module,'Release only owned player lock')
check('rendererEnabled[i]' in module,'Restore captured player renderer state')
check('Time.deltaTime' in module and 'creditsElapsed>=activity.Seconds' in module,'Credits viewing duration gate')
check('Year6Activities.SubmissionSeconds' in module,'Submission clock derived from durable event prefix')
for p in [*runtime.glob('*.cs'),*editor.glob('*.cs')]:
    text=p.read_text()
    for forbidden in ['InitializeOnLoad','AssetDatabase.Refresh(', 'EditorApplication.isPlaying=', 'Application.OpenURL(', 'CompleteMission(']:
        # Builder comment documents that import hooks are absent.
        code=re.sub(r'//[^\n]*','',text)
        check(forbidden not in code,p.name+': no '+forbidden)
builder=(editor/'Year6ModuleBuilder.cs').read_text()
for path in re.findall(r'"(Assets/[^\"]+\.png)"',builder):
    check((root/'mobile-2d'/path).is_file(),'Existing native asset '+path)
# Compare stage asset IDs against files in declared reuse folders and explicit aliases.
ids=re.findall(r'Place\("([^\"]+)"',module)
for asset in set(ids+['teacher','committee_chair','committee_examiner_a','committee_examiner_b','committee_examiner_c','luon_reunion_arm','dam_reunion_arm','long_reunion_arm','to_reunion_arm']):
    check(any((root/'mobile-2d/Assets/Sprites/MVP').rglob(asset+'.png')),'Stage art available '+asset)
report={'status':'source-contract-passed','checks':checks,'count':len(checks),'events':len(expected),
        'unityCompile':'pending coordinator','playMode':'pending coordinator','runtimeValidation':'pending coordinator',
        'sha256':{str(p.relative_to(root)):hashlib.sha256(p.read_bytes()).hexdigest() for p in [*runtime.glob('*.cs'),*editor.glob('*.cs')]}}
(root/'review/parallel/year6/source-contract.json').write_text(json.dumps(report,indent=2,ensure_ascii=False)+'\n')
print(f'{len(checks)} source checks passed; {len(expected)} events. C# compile and Play not run.')
