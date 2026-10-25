using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class ProductionEffect
{
    public int cost,reward;
    public string[] requires=Array.Empty<string>(),grants=Array.Empty<string>(),removes=Array.Empty<string>();
}
[Serializable] public sealed class ProductionChoice : ProductionEffect { public string id,label; }
[Serializable] public sealed class ProductionStep : ProductionEffect
{
    public string id,label,zone;
    public int credits;
    public ProductionChoice[] choices=Array.Empty<ProductionChoice>();
}
[Serializable] public sealed class ProductionMission
{
    public int id;
    public string entry,bridge;
    public ProductionStep[] steps=Array.Empty<ProductionStep>();
}
[Serializable] public sealed class ProductionSaveCompatibility { public string hash;public int maxMission; }
[Serializable] public sealed class ProductionCampaignDefinition
{
    public int version,initialMoney;
    public ProductionSaveCompatibility[] compatibleSaves=Array.Empty<ProductionSaveCompatibility>();
    public string scope;
    public string[] initialFacts=Array.Empty<string>();
    public ProductionMission[] missions=Array.Empty<ProductionMission>();
    public static ProductionCampaignDefinition Parse(string json)
    {
        string error="";
        var definition=JsonUtility.FromJson<ProductionCampaignDefinition>(json);
        if(definition==null || !definition.Validate(out error))throw new ArgumentException("Invalid production definition: "+(definition==null?"empty":error));
        return definition;
    }
    public bool Validate(out string error)
    {
        error="";
        if(compatibleSaves==null || version!=1 || initialMoney<0 || initialFacts==null || missions==null || missions.Length!=45){error="Expected version1 and45missions";return false;}
        foreach(var rule in compatibleSaves)if(rule==null || rule.hash==null || rule.hash.Length!=64 || rule.maxMission<0 || rule.maxMission>45){error="Invalid explicit save compatibility rule";return false;}
        foreach(string fact in initialFacts)if(string.IsNullOrWhiteSpace(fact)){error="Invalid starting inventory";return false;}
        for(int i=0;i<missions.Length;i++)
        {
            var m=missions[i];var ids=new HashSet<string>();
            if(m==null || m.id!=i+1 || m.steps==null || m.steps.Length==0){error="Mission ordering/steps";return false;}
            foreach(var step in m.steps)
            {
                if(step==null || string.IsNullOrWhiteSpace(step.id) || !ids.Add(step.id) || string.IsNullOrWhiteSpace(step.label) || string.IsNullOrWhiteSpace(step.zone) || step.credits<0 || !ValidEffect(step) || step.choices==null){error="Invalid step inM"+m.id;return false;}
                var options=new HashSet<string>();
                foreach(var choice in step.choices)
                    if(choice==null || string.IsNullOrWhiteSpace(choice.id) || !options.Add(choice.id) || !ValidEffect(choice)){error="Invalid choice inM"+m.id;return false;}
            }
        }
        return true;
    }
    private static bool ValidEffect(ProductionEffect e)
    {
        if(e.cost<0 || e.reward<0 || e.requires==null || e.grants==null || e.removes==null)return false;
        foreach(var list in new[]{e.requires,e.grants,e.removes})foreach(var fact in list)if(string.IsNullOrWhiteSpace(fact))return false;
        return true;
    }
}
