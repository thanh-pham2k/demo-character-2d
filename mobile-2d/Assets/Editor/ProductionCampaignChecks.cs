using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ProductionCampaignChecks
{
    [Serializable] private sealed class Report
    {
        public string scope="45mission event model, economy, academic prerequisites and checkpoint persistence; scene/minigame integration is not covered";
        public string finishedUtc,definitionSha256;
        public bool success;
        public int assertions,routes,eventsPerRoute;
        public string[] checks,errors;
    }
    private static string Request=>Path.Combine(Application.dataPath,"../Temp/ProductionCampaignChecks.run");
    private static double allowedAfter;
    private static int count;
    static ProductionCampaignChecks(){allowedAfter=EditorApplication.timeSinceStartup+3;EditorApplication.update+=Poll;}
    private static void Require(bool value,string label){count++;if(!value)throw new Exception(label);}
    private static void Poll()
    {
        if(!File.Exists(Request)||EditorApplication.timeSinceStartup<allowedAfter||EditorApplication.isCompiling||EditorApplication.isUpdating||EditorApplication.isPlayingOrWillChangePlaymode)return;
        File.Delete(Request);Run();
    }
    private static void Run()
    {
        string definitionSha="";
        count=0;var checks=new List<string>();var errors=new List<string>();int routes=0,totalEvents=0;
        string sandbox=Path.Combine(Path.GetTempPath(),"room404-production-"+Guid.NewGuid().ToString("N"));
        try
        {
            string json=File.ReadAllText(Path.Combine(Application.dataPath,"Resources/ProductionCampaign/missions.json"));
            using(var sha=SHA256.Create())definitionSha=BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(json))).Replace("-","").ToLowerInvariant();
            var definition=ProductionCampaignDefinition.Parse(json);
            foreach(var m in definition.missions)totalEvents+=m.steps.Length;
            Require(totalEvents==212,"Expected authored212success events");
            int combinations=1;foreach(var m in definition.missions)foreach(var s in m.steps)if(s.choices.Length>0)combinations*=s.choices.Length;
            for(int route=0;route<combinations;route++)
            {
                var state=new ProductionCampaignProgress(definition);int routeCode=route,mealCost=20000;
                foreach(var mission in definition.missions)
                {
                    foreach(var step in mission.steps)
                    {
                        int money=state.Money,credits=state.Credits,history=state.ExportEvents().Length;
                        Require(!state.TryCommit(mission.id+1,step.id,"",out _),"Reject next mission event");
                        Require(!state.TryCommit(mission.id,"not_an_action","",out _),"Reject unknown action");
                        Require(!state.TryCommit(mission.id,step.id,"invalid_choice",out _),"Reject invalid choice");
                        Require(state.Money==money && state.Credits==credits && state.ExportEvents().Length==history,"Rejected event has no effects");
                        string choice="";
                        if(step.choices.Length>0){var selected=step.choices[routeCode%step.choices.Length];routeCode/=step.choices.Length;choice=selected.id;if(mission.id==5 && step.id=="choose_meal")mealCost=selected.cost;}
                        Require(state.TryCommit(mission.id,step.id,choice,out var error),"Route event "+mission.id+":"+step.id+" "+error);
                        int committedMoney=state.Money;
                        Require(!state.TryCommit(mission.id,step.id,choice,out _),"Duplicate event rejected");
                        Require(state.Money==committedMoney,"No duplicate fare/reward");
                        Require(ProductionCampaignProgress.TryRestore(definition,state.ExportEvents(),out var restored,out error),"Replay checkpoint "+error);
                        Require(restored.CurrentMission==state.CurrentMission && restored.Money==state.Money && restored.Credits==state.Credits && restored.SafeZone==state.SafeZone,"Replay reproduces state");
                    }
                    if(mission.id==8)Require(state.Credits==32 && state.CurrentMission==9,"Feast leads to registrationM09");
                    if(mission.id==10)Require(state.Has("choice:gaming_no")!=state.Has("choice:gaming_yes"),"YES/NO preserves different flags");
                    if(mission.id==11)Require(state.Has("choice:wake")!=state.Has("choice:snooze"),"Wake/snooze preserves different flags");
                    if(mission.id==15)Require(state.Credits==50 && state.Has("failed:KTLT") && !state.Has("passed:KTLT"),"Year2 failure state");
                    if(mission.id==17)Require(state.Money==89000-mealCost,"First job adds exactly50k to shared wallet");
                    if(mission.id==19)Require(state.Money==489000-mealCost,"Representative jobs, weekly ledger and tuition reconcile");
                    if(mission.id==19)Require(state.Has("passed:KTLT") && state.Has("enrolled:CTDL") && !state.Has("failed:KTLT"),"KTLT unlocksCTDL only after pass");
                    if(mission.id==23)Require(state.Credits==78 && state.Has("passed:GT2") && !state.Has("failed:GT2") && state.Has("failed:CTDL"),"GT2 stays passed; CTDL debt is distinct");
                    if(mission.id==27)Require(state.Credits==118 && !state.Has("thesis:eligible"),"118credits cannot unlock thesis");
                    if(mission.id==31)Require(state.Has("room:freshman1") && state.Has("room:freshman2") && state.Has("room:freshman3") && !state.Has("dam:flight_after_visit"),"Three replacements; Dam not flown before visit");
                    if(mission.id==36)Require(state.Credits==136 && state.Has("passed:CTDL") && state.Has("passed:HDH") && state.Has("thesis:eligible"),"136and prerequisite chain unlock thesis");
                    if(mission.id==43)Require(state.Credits==150 && state.Has("thesis:defense_pass") && state.Has("thesis:grade_a") && !state.Finished,"A follows defense, not final completion");
                    if(mission.id==44)Require(!state.Finished && state.Has("room:key_returned") && !state.Has("item:room_key") && state.Has("room:freshman1") && state.Has("room:freshman2") && state.Has("room:freshman3"),"Lights/key preserve freshmen and do not finish game");
                }
                Require(state.Finished && state.CompletedMissions==45 && state.Has("item:degree") && state.SafeZone=="BinhDong","Only full ending/credits finishes");
                var events=state.ExportEvents();events[0].step="forged";
                Require(!ProductionCampaignProgress.TryRestore(definition,events,out _,out _),"Tampered history rejected");
                routes++;
            }
            checks.Add("All192 combinations of bed/meal/gaming/alarm/mother/travel choices complete212ordered events without skipping missions");
            checks.Add("Repeated/reordered/unknown events cannot duplicate rewards, fares, credits or ending flags");
            checks.Add("KTLT→CTDL→HĐH, GT2pass,32/50/78/118/136/150, thesis eligibility and defense/ending gates checked at milestones");
            checks.Add("Three freshmen remain through packing; keys returned; physical degree and final credits required");
            var poorDefinition=ProductionCampaignDefinition.Parse(json);poorDefinition.initialMoney=0;
            var poor=new ProductionCampaignProgress(poorDefinition);
            Require(poor.TryCommit(1,"decline_tout","",out _),"Poor route opens");
            Require(!poor.TryCommit(1,"buy_ticket","",out _) && poor.Money==0,"Unaffordable ticket rejected");
            Require(poor.TryRequestAssistance("",out _) && poor.Money==7000,"Assistance exactly covers shortfall");
            Require(!poor.TryRequestAssistance("",out _) && poor.Money==7000,"No aid farming");
            Require(poor.TryCommit(1,"buy_ticket","",out _) && poor.Money==0,"Assistance recovers route");
            checks.Add("Zero-money fare has explicit one-time exact-shortfall assistance; no surplus farming");
            var session=new ProductionCampaignSession(sandbox,json);
            Require(session.Commit(1,"decline_tout","",out _),"Durable first event");
            Require(session.Commit(1,"buy_ticket","",out _),"Durable ticket");
            var resume=new ProductionCampaignSession(sandbox,json);
            Require(resume.Progress.Money==28000 && resume.Progress.CurrentStep.id=="arrive_dorm","Resume does not charge twice");
            string save=Path.Combine(sandbox,"production-campaign-v1.json");
            File.WriteAllText(save,"{broken");
            resume=new ProductionCampaignSession(sandbox,json);
            Require(resume.Progress.CurrentStep.id=="buy_ticket" && resume.Progress.Money==35000,"Corruption falls back to previous complete checkpoint");
            Require(resume.Commit(1,"buy_ticket","",out _),"Recovered backup can commit");
            File.WriteAllText(save,"{broken_again");
            Require(new ProductionCampaignSession(sandbox,json).Progress.CurrentStep.id=="buy_ticket","Saving after corruption preserves valid backup");
            var mismatchStore=new ProductionCampaignStore(sandbox,json+"\n");
            Require(mismatchStore.Load()==null,"Changed definition requires explicit migration, not silent reinterpretation");
            bool refused=false;
            try{new ProductionCampaignSession(sandbox,json+"\n");}catch(InvalidDataException){refused=true;}
            Require(refused,"Invalid existing save cannot silently start over or overwrite progress");
            checks.Add("Atomic save/load, corrupt-primary fallback, valid-backup preservation and definition hash mismatch checked; invalid existing saves are preserved");
            foreach(var rule in definition.compatibleSaves)
            {
                string oldJson=File.ReadAllText(Path.GetFullPath(Path.Combine(Application.dataPath,"../../story/plans/production-definitions/"+rule.hash+".json")));
                string compatibleDirectory=Path.Combine(sandbox,"compatible-"+rule.hash);
                var legacy=new ProductionCampaignSession(compatibleDirectory,oldJson);
                while(legacy.Progress.CurrentMission<=rule.maxMission)
                {
                    var active=legacy.Progress.CurrentStep;string selected=active.choices.Length>0?active.choices[0].id:"";
                    Require(legacy.Commit(legacy.Progress.CurrentMission,active.id,selected,out _),"Build actual old checkpoint prefix");
                }
                var migrated=new ProductionCampaignSession(compatibleDirectory,json);
                Require(migrated.Progress.CurrentMission==rule.maxMission+1 && migrated.Progress.Money==legacy.Progress.Money && migrated.Progress.SafeZone==legacy.Progress.SafeZone,"Explicit unchanged prefix loads with same wallet and safe destination");
                var next=migrated.Progress.CurrentStep;string choice=next.choices.Length>0?next.choices[0].id:"";
                Require(migrated.Commit(migrated.Progress.CurrentMission,next.id,choice,out _) && migrated.Progress.SafeZone==next.zone,"First new event commits current definition destination after migration");
                var oldNext=legacy.Progress.CurrentStep;string oldChoice=oldNext.choices.Length>0?oldNext.choices[0].id:"";
                Require(legacy.Commit(legacy.Progress.CurrentMission,oldNext.id,oldChoice,out _),"Create old past-boundary save");
                string oldSave=Path.Combine(compatibleDirectory,"production-campaign-v1.json");string unchanged=File.ReadAllText(oldSave);
                bool blocked=false;try{new ProductionCampaignSession(compatibleDirectory,json);}catch(InvalidDataException){blocked=true;}
                if(!blocked)Require(new ProductionCampaignSession(compatibleDirectory,json).Progress.CurrentMission==rule.maxMission+1,"Only compatible prefix backup is used");
                Require(File.ReadAllText(oldSave)==unchanged,"Loading never rewrites an unsupported old primary");
                checks.Add("Explicit prefix throughM"+rule.maxMission+" preserves wallet/destination; next event adopts current data; unsupported primary is preserved");
            }
            string blocker=Path.Combine(sandbox,"file-instead-of-directory");File.WriteAllText(blocker,"test");
            var diskFailure=new ProductionCampaignSession(blocker,json);
            Require(!diskFailure.Commit(1,"decline_tout","",out _) && diskFailure.Progress.ExportEvents().Length==0,"Failed disk commit leaves live state untouched");
            checks.Add("Disk failure publishes no in-memory progression or reward");
        }
        catch(Exception ex){errors.Add(ex.ToString());Debug.LogException(ex);}
        finally{if(Directory.Exists(sandbox))Directory.Delete(sandbox,true);}
        string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/mvp_sprites/remaining/review/production-campaign-model.json"));
        File.WriteAllText(output,JsonUtility.ToJson(new Report{definitionSha256=definitionSha,finishedUtc=DateTime.UtcNow.ToString("O"),success=errors.Count==0,assertions=count,routes=routes,eventsPerRoute=totalEvents,checks=checks.ToArray(),errors=errors.ToArray()},true));
    }
}
