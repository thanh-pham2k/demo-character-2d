"""Host data/ownership validation only. Does not compile or launch Unity."""
import hashlib,json,re
from pathlib import Path
root=Path(__file__).resolve().parents[3]
assets=root/'mobile-2d/Assets'
catalog=json.loads((assets/'Resources/ProductionAudio/catalog.json').read_text())
mission_path=assets/'Resources/ProductionCampaign/missions.json'
missions=json.loads(mission_path.read_text())
expected={(m['id'],s['id']) for m in missions['missions'] for s in m['steps']}
cues={c['id']:c for c in catalog['cues']}
assert len(cues)==len(catalog['cues'])
actual={(e['mission'],e['step']) for e in catalog['events']}
assert actual==expected and len(actual)==len(catalog['events'])
for e in catalog['events']: assert e['cue'] in cues and not cues[e['cue']]['loop']
for c in cues.values():
    assert 0<c['duration']<=10 and 0<=c['volume']<=1 and c['rights']
    assert c['status']!='pending_music' or c['source']=='pending'
for s in catalog['scenes']:
    for key in ['ambience','music']: assert not s[key] or (s[key] in cues and cues[s[key]]['loop'])
assert {c['group'] for c in cues.values()}=={'A_AMBIENCE','A_TRAVEL','A_STUDY','A_LIFE','A_STEALTH','A_FRIENDS','A_THESIS_END','A_UI','A_BGM'}
synthesis=(assets/'Scripts/ProductionAudio/ProductionAudioSynthesis.cs').read_text()
for c in cues.values():
    if c['source'].startswith('synth:') and c['source']!='synth:click': assert 'case "'+c['source']+'"' in synthesis
    if c['source'].startswith('DormSound.'):
        assert c['source'].split('.')[-1]+'()' in (assets/'Scripts/DormSound.cs').read_text()
for folder in ['Scripts/ProductionAudio','Editor/ProductionAudio']:
    for p in (assets/folder).glob('*.cs'):
        text=p.read_text()
        assert not any(x in text for x in ['InitializeOnLoad','AssetDatabase.Refresh','EnterPlaymode','EditorApplication.isPlaying =','new ProductionCampaignSession','Session.Commit('])
report=dict(status='passed_host_data_only',missions=len(missions['missions']),events=len(actual),cues=len(cues),existing_procedural=sum(c['status']=='existing_procedural' for c in cues.values()),new_procedural_placeholders=sum(c['status']=='procedural_placeholder' for c in cues.values()),pending_music=sum(c['status']=='pending_music' for c in cues.values()),recorded_audio_assets=[str(p.relative_to(assets)) for p in assets.rglob('*') if p.suffix.lower() in {'.wav','.mp3','.ogg','.aiff','.flac'}],definition_sha256=hashlib.sha256(mission_path.read_bytes()).hexdigest(),unity_compile='not_run_coordinator_queue',play_mode='not_run_coordinator_queue',listening='pending',checks=['212 exact mission/step keys without loop commits','cue IDs/levels/duration/provenance','9 required groups','loop scene references','DormSound factory references','synthesis recipe references','no automatic editor/scene/session mutation markers'])
(root/'review/parallel/audio/host-report.json').write_text(json.dumps(report,indent=2)+'\n')
print(json.dumps(report,indent=2))
