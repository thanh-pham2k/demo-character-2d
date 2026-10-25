using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ProductionCommuteChecks
{
    [Serializable] private sealed class Report
    {
        public bool success;
        public int assertions;
        public string finishedUtc,error,scope="Production bus host/session fare, direction, choice and save failure; physical production scene integration pending";
    }
    private static string Request=>Path.Combine(Application.dataPath,"../Temp/ProductionCommuteChecks.run");
    private static readonly double AllowedAfter;
    private static int assertions;
    static ProductionCommuteChecks(){AllowedAfter=EditorApplication.timeSinceStartup+3;EditorApplication.update+=Poll;}
    private static void Require(bool value,string message){assertions++;if(!value)throw new Exception(message);}
    private static void Poll()
    {
        if(!File.Exists(Request) || EditorApplication.isCompiling || EditorApplication.isUpdating || EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.timeSinceStartup<AllowedAfter)return;
        File.Delete(Request);Run();
    }
    private static void AdvanceTo(ProductionCampaignSession session,int mission,string step)
    {
        for(int i=0;i<199;i++)
        {
            var p=session.Progress;
            if(p.CurrentMission==mission && p.CurrentStep.id==step)return;
            var current=p.CurrentStep;
            string choice=current.choices.Length>0?current.choices[0].id:"";
            Require(session.Commit(p.CurrentMission,current.id,choice,out var error),"Earn fixture prefix: "+error);
        }
        throw new Exception("Target step not reached");
    }
    private static void Run()
    {
        var report=new Report();assertions=0;
        string directory=Path.Combine(Path.GetTempPath(),"room404-commute-"+Guid.NewGuid().ToString("N"));
        try
        {
            string json=File.ReadAllText(Path.Combine(Application.dataPath,"Resources/ProductionCampaign/missions.json"));
            foreach(string choice in new[]{"on_time","next_bus"})
            {
                string route=Path.Combine(directory,choice);
                var session=new ProductionCampaignSession(route,json);
                AdvanceTo(session,6,"take_bus");
                int wallet=session.Progress.Money;
                var outbound=new ProductionCommuteProgress(session,6,"take_bus");
                Require(outbound.Money==wallet && outbound.CanBoard(false,out _) && !outbound.CanBoard(true,out _),"Shared wallet and outbound direction");
                Require(!outbound.CommitArrival(true,out _) && session.Progress.Money==wallet,"Wrong direction cannot charge");
                string obstruction=Path.Combine(route,"production-campaign-v1.json.tmp");
                Directory.CreateDirectory(obstruction);
                Require(!outbound.CommitArrival(false,out _) && session.Progress.Money==wallet && session.Progress.CurrentStep.id=="take_bus","Failed save preserves trip and wallet");
                Directory.Delete(obstruction);
                Require(outbound.CommitArrival(false,out _) && session.Progress.Money==wallet-7000,"Successful destination charges shared wallet once");
                Require(!outbound.CommitArrival(false,out _) && outbound.Money==wallet-7000,"Stale host cannot double charge");
                session=new ProductionCampaignSession(route,json);
                Require(session.Progress.CurrentStep.id=="enter_class" && session.Progress.Money==wallet-7000 && session.Progress.Credits==0,"Reload restores paid trip with no premature credits");
                AdvanceTo(session,6,"return_room");
                var home=new ProductionCommuteProgress(session,6,"return_room");
                Require(!home.CanBoard(false,out _) && home.CommitArrival(true,out _) && session.Progress.Money==wallet-14000 && session.Progress.SafeZone=="Room","Separate return fare and durable room destination");
                AdvanceTo(session,7,"catch_bus");
                var recovery=new ProductionCommuteProgress(session,7,"catch_bus",choice);
                Require(recovery.MissedBus==(choice=="next_bus") && recovery.CanBoard(false,out _),"Departure choice controls recovery presentation");
                Require(recovery.CommitArrival(false,out _) && session.Progress.Money==wallet-21000,"M07 fare uses same wallet");
                Require(session.Progress.Has(choice=="next_bus"?"travel:missed_bus_recovered":"travel:on_time") && session.Progress.Credits==0,"Choice persists without awarding year credits");
            }
            var poorDefinition=ProductionCampaignDefinition.Parse(json);poorDefinition.initialMoney=23000;
            foreach(var mission in poorDefinition.missions)foreach(var step in mission.steps)if(mission.id<=5)step.reward=0;
            var poor=new ProductionCampaignSession(Path.Combine(directory,"poor"),JsonUtility.ToJson(poorDefinition));
            // Check an early stale binding cannot manufacture a trip or an aid reward.
            var stale=new ProductionCommuteProgress(poor,6,"take_bus");
            Require(!stale.CanBoard(false,out _) && !stale.CommitArrival(false,out _) && stale.Money==23000,"Unreached trip cannot bypass quest gates");
            AdvanceTo(poor,6,"take_bus");
            Require(poor.Progress.Money==1000 && !stale.CanBoard(false,out _) && !stale.CommitArrival(false,out _) && poor.Progress.CurrentStep.id=="take_bus","Actual reached trip rejects insufficient fare without recording arrival");
            Require(poor.RequestAssistance("",out _) && poor.Progress.Money==7000 && stale.CommitArrival(false,out _) && poor.Progress.Money==0,"Existing exact-shortfall assistance recovers a poor production bus trip");
            bool rejected=false;try{new ProductionCommuteProgress(poor,1,"buy_ticket");}catch(ArgumentException){rejected=true;}
            Require(rejected,"Opening fare cannot bind a school bus host");
            report.success=true;
            Debug.Log("[ProductionCommuteChecks] PASS: shared wallet, paid round trip, recovery choices, save failure and stale-host gates.");
        }
        catch(Exception e){report.error=e.ToString();Debug.LogError("[ProductionCommuteChecks] FAIL: "+e);}
        finally
        {
            report.assertions=assertions;report.finishedUtc=DateTime.UtcNow.ToString("O");
            if(Directory.Exists(directory))Directory.Delete(directory,true);
            File.WriteAllText(Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/mvp_sprites/remaining/review/production-commute-host.json")),JsonUtility.ToJson(report,true));
        }
    }
}
