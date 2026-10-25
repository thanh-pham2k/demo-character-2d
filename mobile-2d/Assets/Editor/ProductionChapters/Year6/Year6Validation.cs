using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ProductionChapters.Year6;

namespace ProductionChapters.Year6.Editor
{
    public static class Year6Validation
    {
        static int checks;
        static void Check(bool condition,string label){checks++;if(!condition)throw new InvalidOperationException("Year6 check failed: "+label);}
        [MenuItem("Production/Chapters/Year6/Validate activities and workbench")]
        public static void Run()
        {
            checks=0;
            var definition=JsonUtility.FromJson<ProductionCampaignDefinition>(Resources.Load<TextAsset>("ProductionCampaign/missions").text);
            var ids=new HashSet<string>();var bench=new Year6Workbench();
            Check(!bench.ValidateGraph(),"empty graph rejected");Check(!bench.ProjectTests(),"unimplemented modules fail");
            Check(!bench.SecurityTests(),"unprotected API fails security suite");
            Check(!bench.FinalTests(),"crashing release fails");Check(!bench.ReportValid(),"empty report fails");
            foreach(var mission in definition.missions)
            {
                if(mission.id<37)continue;
                foreach(var step in mission.steps)
                {
                    var a=Year6Activities.Find(step.id);Check(a!=null,"coverage "+step.id);Check(ids.Add(step.id),"unique step "+step.id);
                    Check(a.Required.Length>0,"nonempty task "+step.id);var attempt=new Year6Attempt();
                    Check(!attempt.Choose(a,"injected-invalid-operation"),"unknown operation rejected");Check(attempt.Position==0,"unknown operation does not progress");
                    foreach(string op in a.Required)
                    {
                        Check(Array.IndexOf(a.Operations,op)>=0,"required operation offered "+step.id);
                        bool passed=attempt.Choose(a,op);bench.Apply(op);
                        Check(passed==(attempt.Position==a.Required.Length),"pass only at final operation "+step.id);
                    }
                    Check(attempt.Passed,"activity completes "+step.id);
                    Check(bench.Validate(step.id,out var error),step.id+" executable result "+error);
                    Check(!attempt.Choose(a,a.Required[0]),"no repeated completion "+step.id);
                    if(a.Required.Length>1)
                    {
                        attempt.Reset();for(int i=0;i<3;i++)attempt.Choose(a,a.Required[1]);
                        Check(attempt.Confidence==0,"three mistakes stop activity "+step.id);
                        Check(!attempt.Choose(a,a.Required[0]) && attempt.Position==0,"confidence gate "+step.id);
                        attempt.Reset();Check(attempt.Confidence==3,"checkpoint rehearsal restores confidence");
                    }
                }
            }
            Check(ids.Count==Year6Activities.All.Length,"no orphan activity");
            Check(bench.Request(false,true,true)==401,"invalid token 401");Check(bench.Request(true,false,true)==403,"permission 403");
            Check(bench.Request(true,true,false)==400,"payload 400");Check(bench.Request(true,true,true)==200,"valid request 200");
            Check(bench.ReadSnapshot(false)=="disconnected","disconnect repaired");Check(bench.ReadSnapshot(true)=="telemetry","reconnect works");
            Check(bench.Pages==120,"report has 120 pages");
            Debug.Log("Year6 activity/workbench validation passed: "+checks+" checks. This does not validate Unity Play Mode, UI, art contact or full campaign integration.");
        }
    }
}
