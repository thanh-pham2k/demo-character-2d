using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ProductionYearOneChecks
{
    [Serializable] private sealed class Report {public bool success;public int assertions;public string finishedUtc,error,scope="Year1 curriculum data and durable morning countdown; physical route covered separately";}
    private static string Request=>Path.Combine(Application.dataPath,"../Temp/ProductionYearOneChecks.run");
    private static readonly double AllowedAfter;
    private static int count;
    static ProductionYearOneChecks(){AllowedAfter=EditorApplication.timeSinceStartup+3;EditorApplication.update+=Poll;}
    private static void Require(bool value,string label){count++;if(!value)throw new Exception(label);}
    private static void Poll()
    {
        if(!File.Exists(Request)||EditorApplication.isCompiling||EditorApplication.isUpdating||EditorApplication.isPlayingOrWillChangePlaymode||EditorApplication.timeSinceStartup<AllowedAfter)return;
        File.Delete(Request);Run();
    }
    private static void Run()
    {
        var report=new Report();count=0;string directory=Path.Combine(Path.GetTempPath(),"room404-morning-"+Guid.NewGuid().ToString("N"));
        try
        {
            var curriculum=ProductionYearOneCurriculum.Load();int credits=0;
            foreach(var c in curriculum.courses)
            {credits+=c.credits;Require(curriculum.ForStep("study_y1_"+c.id)==c && curriculum.ForStep("exam_y1_"+c.id)==c && c.study.question!=c.exam.question,"Distinct lesson/exam and correct course mapping");}
            Require(credits==32 && curriculum.ForStep("solve_warmup")==null,"Six-course ledger totals32, separate from M06 warmup");
            string json=File.ReadAllText(Path.Combine(Application.dataPath,"Resources/ProductionCampaign/missions.json"));
            var session=new ProductionCampaignSession(directory,json);
            while(session.Progress.CurrentMission<7)
            {
                var p=session.Progress;var step=p.CurrentStep;
                Require(session.Commit(p.CurrentMission,step.id,step.choices.Length>0?step.choices[0].id:"",out _),"Earn clock identity prefix");
            }
            var clock=new ProductionMorningClock(directory,json,session.Progress.ExportEvents(),120);
            Require(clock.Remaining==120 && !clock.Missed,"New morning starts at configured120seconds");
            Require(clock.Tick(10,out _) && clock.Flush(out _) && clock.Remaining==110,"Gameplay time persists");
            var reload=new ProductionMorningClock(directory,json,session.Progress.ExportEvents(),120);
            Require(reload.Remaining==110 && reload.Tick(0,out _) && reload.Remaining==110,"Reload and paused ticks cannot reset the countdown");
            Require(session.Commit(7,"join_wc_queue","",out _),"Wash/queue success can extend model history");
            var afterQueue=new ProductionMorningClock(directory,json,session.Progress.ExportEvents(),120);
            Require(afterQueue.Remaining==110,"Timer identity survives queue event without a fresh120seconds");
            Require(afterQueue.Tick(111,out _) && afterQueue.Missed,"Deadline crossing persists zero");
            Require(new ProductionMorningClock(directory,json,session.Progress.ExportEvents(),120).Missed,"Reload preserves missed departure");
            bool rejected=false;try{new ProductionMorningClock(directory,json+"\n",session.Progress.ExportEvents(),120);}catch(InvalidDataException){rejected=true;}
            Require(rejected,"A different definition cannot silently reinterpret timer data");
            string primary=Path.Combine(directory,"production-morning-v1.json");File.WriteAllText(primary,"{broken");
            Require(new ProductionMorningClock(directory,json,session.Progress.ExportEvents(),120).Remaining==110,"Corrupt primary can recover last validated timer backup");
            File.Delete(primary+".bak");rejected=false;try{new ProductionMorningClock(directory,json,session.Progress.ExportEvents(),120);}catch(InvalidDataException){rejected=true;}
            Require(rejected && File.ReadAllText(primary)=="{broken","Invalid existing clock is preserved, not restarted");
            report.success=true;Debug.Log("[ProductionYearOneChecks] PASS: curriculum and persistent paused countdown.");
        }
        catch(Exception e){report.error=e.ToString();Debug.LogException(e);}
        finally
        {
            report.assertions=count;report.finishedUtc=DateTime.UtcNow.ToString("O");if(Directory.Exists(directory))Directory.Delete(directory,true);
            File.WriteAllText(Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/mvp_sprites/remaining/review/production-year-one-data.json")),JsonUtility.ToJson(report,true));
        }
    }
}
