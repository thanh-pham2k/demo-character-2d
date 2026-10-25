using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Versioned, validated history only. Transient poses/locks/coroutines never enter save data.
public sealed class ProductionCampaignStore
{
    private readonly string path,definitionHash;
    private readonly ProductionCampaignDefinition definition;
    public ProductionCampaignStore(string directory,string definitionJson)
    {
        definition=ProductionCampaignDefinition.Parse(definitionJson);
        using(var sha=SHA256.Create())definitionHash=BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(definitionJson))).Replace("-","").ToLowerInvariant();
        path=Path.Combine(directory,"production-campaign-v1.json");
    }
    public bool Exists=>File.Exists(path)||File.Exists(path+".bak");
    private ProductionCampaignProgress Read(string candidate)
    {
        try
        {
            if(!File.Exists(candidate))return null;
            var save=JsonUtility.FromJson<ProductionCheckpoint>(File.ReadAllText(candidate));
            if(save==null || save.version!=1 || save.events==null)return null;
            if(save.definitionHash!=definitionHash)
            {
                bool compatible=false;
                foreach(var rule in definition.compatibleSaves)
                {
                    if(rule.hash!=save.definitionHash)continue;
                    bool prefix=true;foreach(var record in save.events)if(record==null || record.mission>rule.maxMission)prefix=false;
                    if(prefix)compatible=true;
                }
                if(!compatible)return null;
            }
            return ProductionCampaignProgress.TryRestore(definition,save.events,out var progress,out _)?progress:null;
        }
        catch(Exception e) when(e is IOException || e is ArgumentException || e is UnauthorizedAccessException){return null;}
    }
    public ProductionCampaignProgress Load()=>Read(path)??Read(path+".bak");
    public bool Save(ProductionCampaignProgress progress,out string error)
    {
        error="";
        if(progress==null || !ProductionCampaignProgress.TryRestore(definition,progress.ExportEvents(),out _,out error))return false;
        string temp=path+".tmp";
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(temp,JsonUtility.ToJson(new ProductionCheckpoint{definitionHash=definitionHash,events=progress.ExportEvents()},true));
            if(File.Exists(path))File.Replace(temp,path,Read(path)!=null?path+".bak":null);
            else File.Move(temp,path);
            return true;
        }
        catch(Exception e) when(e is IOException || e is UnauthorizedAccessException)
        {error=e.Message;return false;}
        finally{if(File.Exists(temp))try{File.Delete(temp);}catch(IOException){}}
    }
}

// Commits to disk before publishing state. A failed save leaves the running model intact.
public sealed class ProductionCampaignSession
{
    private readonly ProductionCampaignDefinition definition;
    private readonly ProductionCampaignStore store;
    private ProductionCampaignProgress progress;
    public IProductionCampaignView Progress=>progress;
    public ProductionCampaignSession(string directory,string definitionJson)
    {
        definition=ProductionCampaignDefinition.Parse(definitionJson);store=new ProductionCampaignStore(directory,definitionJson);
        progress=store.Load();
        if(progress==null && store.Exists)throw new InvalidDataException("Existing production checkpoint cannot be validated. Preserve files and recover or migrate explicitly.");
        if(progress==null)progress=new ProductionCampaignProgress(definition);
    }
    public bool Commit(int mission,string step,string choice,out string error)=>Mutate(mission,step,choice,false,out error);
    public bool RequestAssistance(string choice,out string error)=>Mutate(Progress.CurrentMission,Progress.CurrentStep?.id,choice,true,out error);
    private bool Mutate(int mission,string step,string choice,bool assistance,out string error)
    {
        if(!ProductionCampaignProgress.TryRestore(definition,Progress.ExportEvents(),out var candidate,out error))return false;
        bool changed=assistance?candidate.TryRequestAssistance(choice,out error):candidate.TryCommit(mission,step,choice,out error);
        if(!changed || !store.Save(candidate,out error))return false;
        progress=candidate;return true;
    }
}
