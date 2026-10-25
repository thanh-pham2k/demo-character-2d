#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using ProductionChapters.Years45;

public static class Years45ContractChecks
{
    [MenuItem("Tools/Production Chapters/Years45/Check authored contracts (no Play)")]
    public static void Run()
    {
        var asset=AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/ProductionCampaign/missions.json");
        var definition=ProductionCampaignDefinition.Parse(asset.text);
        int steps=0,checks=0;
        foreach(var mission in definition.missions)
        {
            if(mission.id<24||mission.id>36)continue;
            foreach(var step in mission.steps)
            {
                var a=Years45Activities.Find(mission.id,step.id);
                Require(a!=null,"Missing "+mission.id+":"+step.id);steps++;checks++;
                Require(a.Inputs.Length>0,"Empty input");checks++;
                Require(!a.Accept(-1,a.Inputs[0])&&!a.Accept(a.Inputs.Length,a.Inputs[0]),"Bounds");checks++;
                Require(!a.Accept(0,"invalid"),"Reject invalid");checks++;
                Require(a.Accept(0,a.Inputs[0]),"Accept authored input");checks++;
            }
        }
        int authored=0;foreach(var a in Years45Activities.All)authored++;
        Require(steps==authored,"Extra or duplicate contracts");checks++;
        // Real model replay verifies durable ordering/facts and each independent slot.
        var progress=new ProductionCampaignProgress(definition);
        foreach(var mission in definition.missions)
        {
            if(mission.id>36)break;
            foreach(var step in mission.steps)
            {
                string choice=step.choices.Length==0?"":step.choices[0].id;
                // Apply exact shortfall assistance when this route needs it.
                if(progress.Money<step.cost+(step.choices.Length==0?0:step.choices[0].cost))
                    Require(progress.TryRequestAssistance(choice,out _),"Assistance");
                if(mission.id>=24)
                {
                    Require(Years45Activities.Gate(progress,mission.id,step.id,out _),"Gate "+step.id);checks++;
                    Require(!progress.TryCommit(mission.id,"invalid","",out _),"Reject foreign step");checks++;
                }
                Require(progress.TryCommit(mission.id,step.id,choice,out var error),step.id+": "+error);
                if(mission.id>=29)
                {
                    for(int slot=0;slot<3;slot++)
                    {
                        bool arrived=progress.Has("room:freshman"+(slot+1));
                        Require(Years45Activities.Occupant(progress,slot).StartsWith("Người mới")==arrived,"Independent slot");checks++;
                    }
                }
                if(mission.id>=24&&mission.id<36){Require(!progress.Has("thesis:eligible"),"No early thesis");checks++;}
            }
        }
        Require(progress.Credits==136&&progress.CurrentMission==37,"M36 endpoint");checks++;
        Require(progress.Has("passed:GT2")&&progress.Has("passed:CTDL")&&progress.Has("passed:HDH"),"Course chain");checks++;
        Require(progress.Has("dam:flight_after_visit"),"Flight after supplies");checks++;
        Debug.Log("Years45 contracts: "+steps+" events, "+checks+" checks passed. This does not validate scene/Play.");
    }
    static void Require(bool condition,string message){if(!condition)throw new InvalidOperationException("Years45: "+message);}
}
#endif
