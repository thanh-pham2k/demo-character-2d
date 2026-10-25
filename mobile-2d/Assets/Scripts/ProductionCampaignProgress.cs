using System;
using System.Collections.Generic;

// Authored success events, not a CompleteMission shortcut. Scene adapters own
// physical/minigame validation. This model owns ordering and durable effects.
[Serializable] public sealed class ProductionCommit
{
    public int mission;
    public string step,choice;
    public bool assistance;
}
[Serializable] public sealed class ProductionCheckpoint
{
    public int version=1;
    public string definitionHash;
    public ProductionCommit[] events=Array.Empty<ProductionCommit>();
}
public interface IProductionCampaignView
{
    int Money {get;} int Credits {get;} int CurrentMission {get;} int CompletedMissions {get;}
    bool Finished {get;} string SafeZone {get;} ProductionStep CurrentStep {get;}
    bool Has(string fact); ProductionCommit[] ExportEvents();
}
public sealed class ProductionCampaignProgress : IProductionCampaignView
{
    private readonly ProductionCampaignDefinition definition;
    private readonly List<ProductionCommit> history=new List<ProductionCommit>();
    private readonly HashSet<string> facts=new HashSet<string>();
    private int missionIndex,stepIndex;
    public int Money {get;private set;}
    public int Credits {get;private set;}
    public string SafeZone {get;private set;}="Station";
    public int CurrentMission=>missionIndex+1;
    public int CompletedMissions=>missionIndex;
    public bool Finished=>missionIndex==definition.missions.Length && Has("campaign:finished");
    public ProductionStep CurrentStep=>missionIndex<definition.missions.Length?definition.missions[missionIndex].steps[stepIndex]:null;
    public bool Has(string fact)=>facts.Contains(fact);
    public ProductionCampaignProgress(ProductionCampaignDefinition source)
    {
        if(source==null || !source.Validate(out var error))throw new ArgumentException("Invalid campaign definition");
        definition=source;Money=source.initialMoney;
        foreach(string fact in source.initialFacts)facts.Add(fact);
    }
    public ProductionCommit[] ExportEvents()
    {
        var copy=new ProductionCommit[history.Count];
        for(int i=0;i<copy.Length;i++){var e=history[i];copy[i]=new ProductionCommit{mission=e.mission,step=e.step,choice=e.choice,assistance=e.assistance};}
        return copy;
    }
    public static bool TryRestore(ProductionCampaignDefinition definition,ProductionCommit[] events,out ProductionCampaignProgress progress,out string error)
    {
        progress=null;error="";
        if(events==null || events.Length>1000){error="Invalid event history";return false;}
        var candidate=new ProductionCampaignProgress(definition);
        foreach(var e in events)
            if(e==null || !candidate.Apply(e,out error))return false;
        progress=candidate;return true;
    }
    public bool TryCommit(int mission,string step,string choice,out string error)=>Apply(new ProductionCommit{mission=mission,step=step,choice=choice??""},out error);
    public bool TryRequestAssistance(string choice,out string error)
    {
        if(CurrentStep==null){error="Campaign already finished";return false;}
        return Apply(new ProductionCommit{mission=CurrentMission,step=CurrentStep.id,choice=choice??"",assistance=true},out error);
    }
    private bool Requirements(ProductionEffect effect,out string error)
    {
        foreach(string requirement in effect.requires)
        {
            if(requirement.StartsWith("credits:"))
            {
                if(!int.TryParse(requirement.Substring(8),out int threshold) || Credits<threshold){error="Requires "+requirement;return false;}
            }
            else if(!facts.Contains(requirement)){error="Requires "+requirement;return false;}
        }
        error="";return true;
    }
    private bool Apply(ProductionCommit commit,out string error)
    {
        error="";var step=CurrentStep;
        if(step==null || commit.mission!=CurrentMission || commit.step!=step.id){error="Event does not match current mission/step";return false;}
        ProductionChoice choice=null;
        foreach(var option in step.choices)if(option.id==commit.choice){choice=option;break;}
        if((step.choices.Length>0 && choice==null) || (step.choices.Length==0 && !string.IsNullOrEmpty(commit.choice))){error="Invalid choice";return false;}
        if(!Requirements(step,out error) || choice!=null && !Requirements(choice,out error))return false;
        long cost=(long)step.cost+(choice?.cost??0),reward=(long)step.reward+(choice?.reward??0);
        string actionFact="action:"+CurrentMission+":"+step.id;
        if(commit.assistance)
        {
            string aid="assistance:"+CurrentMission+":"+step.id;
            if(Money>=cost || facts.Contains(aid) || cost>int.MaxValue){error="Assistance only covers a current unaffordable cost once";return false;}
            // Covers exactly the shortfall, so it cannot be farmed for surplus cash.
            Money=(int)cost;facts.Add(aid);
        }
        else
        {
            long balance=(long)Money-cost+reward;
            if(Money<cost || balance<0 || balance>int.MaxValue){error="Insufficient funds or balance overflow";return false;}
            if(step.credits>0 && step.credits<Credits){error="Credits cannot decrease";return false;}
            Money=(int)balance;
            ApplyFacts(step);if(choice!=null)ApplyFacts(choice);
            if(step.credits>0)Credits=step.credits;
            facts.Add(actionFact);SafeZone=step.zone;
            stepIndex++;
            if(stepIndex==definition.missions[missionIndex].steps.Length){missionIndex++;stepIndex=0;}
        }
        history.Add(new ProductionCommit{mission=commit.mission,step=commit.step,choice=commit.choice??"",assistance=commit.assistance});
        return true;
    }
    private void ApplyFacts(ProductionEffect effect)
    {
        foreach(string fact in effect.removes)facts.Remove(fact);
        foreach(string fact in effect.grants)facts.Add(fact);
    }
}
