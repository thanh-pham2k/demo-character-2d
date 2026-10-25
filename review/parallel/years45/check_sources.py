"""Offline Roslyn syntax/type check only; never starts or refreshes Unity."""
from pathlib import Path
import subprocess,json,re,hashlib
base=Path('/Applications/Unity/Hub/Editor/6000.6.7f1/Unity.app/Contents')
mono=base/'Resources/Scripting/MonoBleedingEdge/bin/mono'
csc=base/'Resources/Scripting/MonoBleedingEdge/lib/mono/msbuild/Current/bin/Roslyn/csc.exe'
refs=list((base/'Resources/Scripting/Managed/UnityEngine').glob('*.dll'))
refs += [base/'Resources/Scripting/MonoBleedingEdge/lib/mono/4.8-api/Facades/netstandard.dll',Path('mobile-2d/Library/ScriptAssemblies/UnityEngine.UI.dll'),Path('mobile-2d/Library/ScriptAssemblies/Assembly-CSharp.dll')]
sources=list(Path('mobile-2d/Assets/Scripts/ProductionChapters/Years45').glob('*.cs'))
# Resolve injection contract from the existing editor assembly, without rebuilding shared sources.
args=[str(mono),str(csc),'-nologo','-target:library','-out:review/parallel/years45/Years45.SourceCheck.dll']+['-r:'+str(p) for p in refs]+[str(s) for s in sources]
r=subprocess.run(args,capture_output=True,text=True)
Path('review/parallel/years45/source-compile.txt').write_text(r.stdout+r.stderr)
print('Runtime Roslyn exit:',r.returncode,r.stdout,r.stderr)
if r.returncode:raise SystemExit(r.returncode)
refs += [p for p in (base/'Resources/Scripting/Managed/UnityEngine').glob('UnityEditor*.dll')]
refs += [Path('review/parallel/years45/Years45.SourceCheck.dll')]
args=[str(mono),str(csc),'-nologo','-define:UNITY_EDITOR','-target:library','-out:review/parallel/years45/Years45.EditorCheck.dll']+['-r:'+str(p) for p in refs]+[str(s) for s in Path('mobile-2d/Assets/Editor/ProductionChapters/Years45').glob('*.cs')]
r=subprocess.run(args,capture_output=True,text=True)
Path('review/parallel/years45/editor-compile.txt').write_text(r.stdout+r.stderr)
print('Editor Roslyn exit:',r.returncode,r.stdout,r.stderr)
if r.returncode:raise SystemExit(r.returncode)
j=json.loads(Path('mobile-2d/Assets/Resources/ProductionCampaign/missions.json').read_text())
s=Path('mobile-2d/Assets/Scripts/ProductionChapters/Years45/Years45Activities.cs').read_text()
a={(int(m),step) for m,step in re.findall(r'Add\((\d+),"([^"]+)"',s)}
a|={(int(m),step) for m,step in re.findall(r'all.Add\("(\d+):([^\"]+)"',s)}
e={(m['id'],st['id']) for m in j['missions'] if 24<=m['id']<=36 for st in m['steps']}
assert a==e,(e-a,a-e)
report={'events':len(e),'authored':len(a),'missing':[],'extra':[],'roslynRuntime':True,'roslynEditor':True,'unityRefresh':False,'unityCompile':False,'playMode':False,'files':{str(p):hashlib.sha256(p.read_bytes()).hexdigest() for p in sources}}
Path('review/parallel/years45/source-report.json').write_text(json.dumps(report,indent=2)+'\n')
print('Coverage:',len(e),'events')
