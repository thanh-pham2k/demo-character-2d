"""Run real model/Years45 contracts under Mono; JSON initialization avoids Unity native APIs."""
from pathlib import Path
import json,subprocess
j=json.loads(Path('mobile-2d/Assets/Resources/ProductionCampaign/missions.json').read_text())
def q(s):return json.dumps(s,ensure_ascii=False)
def strs(a):return 'new string[]{'+','.join(q(x) for x in a)+'}'
def effect(d):return ','.join(k+'='+str(d.get(k,0)) for k in ['cost','reward'])+','+','.join(k+'='+strs(d.get(k,[])) for k in ['requires','grants','removes'])
def choice(c):return 'new ProductionChoice{'+effect(c)+',id='+q(c['id'])+',label='+q(c['label'])+'}'
def step(s):return 'new ProductionStep{'+effect(s)+',id='+q(s['id'])+',label='+q(s['label'])+',zone='+q(s['zone'])+',credits='+str(s['credits'])+',choices=new ProductionChoice[]{'+','.join(choice(c) for c in s['choices'])+'}}'
def mission(m):return 'new ProductionMission{id='+str(m['id'])+',entry='+q(m['entry'])+',bridge='+q(m['bridge'])+',steps=new ProductionStep[]{'+','.join(step(s) for s in m['steps'])+'}}'
init='new ProductionCampaignDefinition{version=1,initialMoney='+str(j['initialMoney'])+',initialFacts='+strs(j['initialFacts'])+',missions=new ProductionMission[]{'+','.join(mission(m) for m in j['missions'])+'}}'
code='''using System;
using ProductionChapters.Years45;
namespace UnityEngine { public static class JsonUtility { public static T FromJson<T>(string s){throw new NotSupportedException();} } }
public static class Years45Host {
static int checks;
static void Check(bool b,string msg){checks++;if(!b)throw new Exception(msg);}
public static void Main(){var d=INIT;
Check(d.Validate(out var error),error);var p=new ProductionCampaignProgress(d);
foreach(var m in d.missions){if(m.id>36)break;foreach(var s in m.steps){
string choice=s.choices.Length==0?"":s.choices[0].id;
int cost=s.cost+(s.choices.Length==0?0:s.choices[0].cost);
if(p.Money<cost)Check(p.TryRequestAssistance(choice,out error),error);
if(m.id>=24){
Check(Years45Activities.Gate(p,m.id,s.id,out error),error);
Check(!p.TryCommit(m.id,"not-current","",out _),"wrong step accepted");
Check(!Years45Activities.Gate(p,m.id+1,s.id,out _),"future mission accepted");
var a=Years45Activities.Find(m.id,s.id);Check(a!=null,"missing activity");
Check(!a.Accept(-1,a.Inputs[0])&&!a.Accept(a.Inputs.Length,a.Inputs[0]),"input bounds");
Check(!a.Accept(0,"invalid"),"wrong action accepted");
}
Check(p.TryCommit(m.id,s.id,choice,out error),m.id+":"+s.id+":"+error);
if(m.id>=24){
Check(!p.TryCommit(m.id,s.id,choice,out _),"duplicate accepted");
Check(ProductionCampaignProgress.TryRestore(d,p.ExportEvents(),out var restored,out error),error);
Check(restored.CurrentMission==p.CurrentMission&&restored.Credits==p.Credits&&restored.Money==p.Money,"restore mismatch");
for(int slot=0;slot<3;slot++){string name=Years45Activities.Occupant(p,slot);
Check(name==Years45Activities.Occupant(restored,slot),"slot restore mismatch");
Check(name.StartsWith("Người mới")==p.Has("room:freshman"+(slot+1)),"freshman slot mismatch");}
if(m.id<36)Check(!p.Has("thesis:eligible"),"early thesis");
if(m.id<35)Check(p.Has("passed:GT2"),"GT2 lost");
if(m.id<34||m.id==34&&s.id!="read_flight_message")Check(!p.Has("dam:flight_after_visit"),"early flight");
if(m.id==35&&s.id=="pass_data_structures")Check(!p.Has("passed:HDH")&&p.Has("passed:CTDL"),"course order");
if(m.id==29&&s.id=="long_leaves")Check(Years45Activities.Occupant(p,0)=="Trống"&&Years45Activities.Occupant(p,1)=="Tộ"&&Years45Activities.Occupant(p,2)=="Đạm","Long slot isolation");
}
}}
Check(p.CurrentMission==37&&p.Credits==136&&!p.Finished,"endpoint");
Check(p.Has("thesis:eligible")&&p.Has("passed:GT2")&&p.Has("passed:CTDL")&&p.Has("passed:HDH"),"eligibility");
Console.WriteLine("Years45 host: "+checks+" assertions passed; M37 entry, 136 credits. No Unity scene validation.");}}
'''.replace('INIT',init)
out=Path('review/parallel/years45');(out/'Years45Host.cs').write_text(code)
base=Path('/Applications/Unity/Hub/Editor/6000.6.7f1/Unity.app/Contents/Resources/Scripting/MonoBleedingEdge')
args=[str(base/'bin/mono'),str(base/'lib/mono/msbuild/Current/bin/Roslyn/csc.exe'),'-nologo','-out:'+str(out/'Years45Host.exe'),str(out/'Years45Host.cs'),'mobile-2d/Assets/Scripts/ProductionCampaignDefinition.cs','mobile-2d/Assets/Scripts/ProductionCampaignProgress.cs','mobile-2d/Assets/Scripts/ProductionChapters/Years45/Years45Activities.cs']
r=subprocess.run(args,text=True,capture_output=True);print(r.stdout+r.stderr)
if r.returncode:raise SystemExit(r.returncode)
r=subprocess.run([str(base/'bin/mono'),str(out/'Years45Host.exe')],capture_output=True,text=True)
(out/'model-checks.txt').write_text(r.stdout+r.stderr);print(r.stdout+r.stderr)
raise SystemExit(r.returncode)
