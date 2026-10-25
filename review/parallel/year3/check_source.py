import json,pathlib,re,hashlib
root=pathlib.Path(__file__).resolve().parents[3]
definition=json.loads((root/'mobile-2d/Assets/Resources/ProductionCampaign/missions.json').read_text())
source=root/'mobile-2d/Assets/Scripts/ProductionChapters/Year3/Year3Activities.cs'
ids=re.findall(r'\{"([^"]+)",new Year3Activity',source.read_text())
expected=[s['id'] for m in definition['missions'][15:23] for s in m['steps']]
assert set(ids)|{'answer_mother'}==set(expected)
assert len(ids)==len(set(ids))
# Definition projection only: does not substitute for executing C# or Unity.
money=definition['initialMoney'];credits=0;facts=set(definition['initialFacts'])
for mission in definition['missions'][:23]:
 for step in mission['steps']:
  choice=step['choices'][0] if step['choices'] else {}
  for effect in [step,choice]:
   for req in effect.get('requires',[]):
    assert (credits>=int(req[8:]) if req.startswith('credits:') else req in facts),(mission['id'],step['id'],req)
  cost=step['cost']+choice.get('cost',0)
  # Assistance is authored model recovery, not module-created money.
  if money<cost:money=cost
  money+=step['reward']+choice.get('reward',0)-cost
  for effect in [step,choice]:
   facts.difference_update(effect.get('removes',[]));facts.update(effect.get('grants',[]))
  facts.add('action:'+str(mission['id'])+':'+step['id'])
  if step['credits']:credits=step['credits']
assert credits==78
assert {'passed:KTLT','passed:VL1','passed:GT2','failed:CTDL','enrolled:CTDL'}<=facts
assert 'item:borrowed_jacket' not in facts
result={'coverage':len(expected),'activitySequences':len(ids),'definitionProjection':{'credits':credits,'money':money,'GT2':'pass','CTDL':'fail','jacketReturned':True},'unityCompile':False,'unityPlay':False,'sources':{}}
for folder in ['mobile-2d/Assets/Scripts/ProductionChapters/Year3','mobile-2d/Assets/Editor/ProductionChapters/Year3']:
 for p in sorted((root/folder).glob('*.cs')):result['sources'][str(p.relative_to(root))]=hashlib.sha256(p.read_bytes()).hexdigest()
(root/'review/parallel/year3/source-check.json').write_text(json.dumps(result,indent=2)+'\n')
print(json.dumps(result,indent=2))
