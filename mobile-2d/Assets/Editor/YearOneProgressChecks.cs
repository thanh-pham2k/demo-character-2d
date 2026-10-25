using System;
using System.IO;
using UnityEditor;
using UnityEngine;

// Isolated data/checkpoint checks; never touches the user's save or scene.
[InitializeOnLoad]
public static class YearOneProgressChecks
{
    [Serializable] private sealed class Report { public bool success;public int assertions;public string finishedUtc,error,scope="Existing Year1 data/checkpoint budget and next-bus recovery; not full45mission integration"; }
    private static int assertions;
    private static double allowedAfter;
    private static string Request=>Path.Combine(Application.dataPath,"../Temp/YearOneProgressChecks.run");
    static YearOneProgressChecks(){allowedAfter=EditorApplication.timeSinceStartup+2;EditorApplication.update+=Poll;}
    private static void Poll()
    {
        if(EditorApplication.isCompiling || EditorApplication.isUpdating || EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.timeSinceStartup<allowedAfter)return;
        if(File.Exists(Request))RunRequested();
    }
    private static void Require(bool condition,string description){assertions++;if(!condition)throw new Exception(description);}
    private static void RunRequested()
    {
        string request=Request;
        if(!File.Exists(request))return;
        File.Delete(request);
        string sandbox=Path.Combine(Path.GetTempPath(),"room404-progress-"+Guid.NewGuid().ToString("N"));
        assertions=0;bool success=false;string error="";
        try
        {
            foreach(var course in YearOneSchedule.Courses)
            {
                var questions=new System.Collections.Generic.HashSet<string>{course.Question};
                foreach(var task in new[]{YearOneSchedule.Work.Homework,YearOneSchedule.Work.Revision,YearOneSchedule.Work.Exam})
                {
                    var exercise=course.ExerciseFor(task);
                    Require(questions.Add(exercise.Question) && exercise.Answer>=0 && exercise.Answer<exercise.Choices.Length && !string.IsNullOrEmpty(exercise.Hint),"Distinct valid task questions and hints: "+course.Name);
                }
            }
            var p=new YearOneProgress();
            Require(!p.CompleteClassStory(),"Cannot tell H6 story before attending class");
            Require(!p.CompleteOpening("meal") && !p.ReadyForFeast,"Cannot skip opening to feast");
            Require(p.CompleteOpening("claim") && p.CompleteOpening("clean") && p.CompleteOpening("friends"),"Opening order");
            Require(p.CompleteOpening("meal") && p.money==110000 && !p.CompleteOpening("meal"),"Meal cost and one-time grant");
            var store=new YearOneCheckpointStore(sandbox);
            for(int day=1;day<=6;day++)
            {
                Require(p.day==day && !p.CanSleep,"Day cannot finish before activities");
                Require(p.CommitTrip(false) && !p.CommitTrip(false),"One outbound fare per day");
                foreach(var c in YearOneSchedule.Lessons(day))
                {
                    var lesson=c.FirstDay==day?YearOneSchedule.Work.Lesson1:YearOneSchedule.Work.Lesson2;
                    Require(p.Complete(c.Id,lesson) && !p.Complete(c.Id,lesson),"Lesson records exactly once");
                    if(!p.Has(c.Activity(YearOneSchedule.Work.Homework)))Require(p.Complete(c.Id,YearOneSchedule.Work.Homework),"Homework after first lesson");
                }
                Require(p.CommitTrip(true) && !p.CommitTrip(true),"One return fare per day");
                Require(!p.CompleteClassStory(),"Cannot tell dorm story from campus after bus arrival");
                p.safeLocation="Room";
                if(day==1)Require(p.CompleteClassStory() && !p.CompleteClassStory(),"H6 reaction commits once at home");
                else Require(!p.CompleteClassStory() && p.Has("story:h6-told-dam"),"H6 story survives checkpoint without replay");
                if(day==2){Require(p.MarkEvening("game") && p.MarkEvening("lights"),"Night game then switch");}
                if(day==3 || day==6)foreach(var c in YearOneSchedule.Courses)
                {
                    if((c.FirstDay<=3)!=(day==3))continue;
                    Require(!p.Complete(c.Id,YearOneSchedule.Work.Exam),"Exam requires revision");
                    Require(p.Complete(c.Id,YearOneSchedule.Work.Revision) && p.Complete(c.Id,YearOneSchedule.Work.Exam),"Revision then exam");
                }
                Require(p.CanSleep && p.FinishDay(),"Sleep advances representative day");
                Require(store.Save(p),"Atomic checkpoint save");
                p=store.Load();Require(p!=null && p.day==day+1,"Checkpoint restores day");
            }
            Require(p.money==26000 && p.Credits==32 && p.ReadyForFeast,"12 paid trips and 32 credits after six days");
            Require(!p.EnterYearTwo() && p.PrepareFeast() && p.FinishFeast() && p.EnterYearTwo() && !p.EnterYearTwo(),"Feast gate and single year transition");
            // Current file corruption falls back to the last complete atomic checkpoint.
            Require(store.Save(p) && store.Save(p),"Final checkpoint and backup exist");
            File.WriteAllText(Path.Combine(sandbox,"year-one-v1.json"),"{broken checkpoint");
            var recovered=store.Load();Require(recovered!=null && recovered.Has("year:two") && recovered.money==26000,"Corrupt primary uses valid backup without duplicate rewards");
            File.WriteAllText(Path.Combine(sandbox,"year-one-v1.json"),"{\"version\":999}");
            Require(store.Load()!=null,"Unsupported primary version falls back to backup");
            File.Delete(Path.Combine(sandbox,"year-one-v1.json"));
            Require(store.Exists && store.Load()!=null,"Continue remains available when only backup survives");
            var late=new YearOneProgress();late.completed.AddRange(new[]{"opening:claim","opening:clean","opening:friends","opening:meal"});late.day=4;
            Require(!late.CommitTrip(false,true),"Next bus recovery requires an actual missed trip");
            Require(YearOneTravelPresentation.Countdown(late).Contains("Xe:"),"Before departure shows countdown");
            Require(late.MarkMissedBus() && !late.CommitTrip(false),"Missed departure must use next-bus event");
            Require(YearOneTravelPresentation.Countdown(late)=="" && YearOneTravelPresentation.Objective(late,false).Contains("chờ chuyến sau"),"Missed departure shows recovery instead of expired clock");
            Require(YearOneTravelPresentation.Objective(late,true).Contains("Lên chuyến sau"),"Parked recovery bus shows boarding objective");
            Require(late.CommitTrip(false,true) && late.money==28000 && late.Has("day:4:next-bus"),"Next bus charges one7000fare and records recovery");
            Require(!late.CommitTrip(false,true) && late.money==28000,"Repeated recovery cannot charge twice");
            Require(store.Save(late) && store.Load().Has("day:4:next-bus") && store.Load().money==28000,"Next-bus recovery survives checkpoint");
            var poorLate=new YearOneProgress{day=4,money=6999};poorLate.completed.AddRange(new[]{"opening:claim","opening:clean","opening:friends","opening:meal","day:4:missed"});
            Require(!poorLate.CommitTrip(false,true) && poorLate.money==6999 && !poorLate.Has("day:4:outbound"),"Insufficient fare cannot record recovered trip");
            var historical=new YearOneProgress{day=4,money=33000};historical.completed.AddRange(new[]{"opening:claim","opening:clean","opening:friends","opening:meal","day:4:missed","day:4:outbound"});
            Require(historical.IsValid() && !historical.CommitTrip(false,true) && historical.money==33000,"Already-completed old trips keep wallet and are never retroactively charged");
            success=true;
            Debug.Log("[YearOneProgressChecks] PASS: six-day gates, 12 lessons, assignments, exams, budget, checkpoints, paid next-bus recovery, old completed-trip preservation and transition idempotency.");
        }
        catch(Exception e){error=e.ToString();Debug.LogError("[YearOneProgressChecks] FAIL: "+e);}
        finally
        {
            if(Directory.Exists(sandbox))Directory.Delete(sandbox,true);
            string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/mvp_sprites/remaining/review/year-one-progress-next-bus.json"));
            File.WriteAllText(output,JsonUtility.ToJson(new Report{success=success,assertions=assertions,finishedUtc=DateTime.UtcNow.ToString("O"),error=error},true));
        }
    }
}
