using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad] public static class YearOneV2Checks
{
    static int count;static void Check(bool ok,string why){count++;if(!ok)throw new Exception(why);}
    static YearOneV2Checks(){EditorApplication.update+=Poll;}
    static void Poll()
    {
        var request=Path.GetFullPath("Temp/YearOneV2Checks.run");
        if(!File.Exists(request)||EditorApplication.isPlayingOrWillChangePlaymode||EditorApplication.isCompiling||EditorApplication.isUpdating)return;
        File.Delete(request);Run();
    }
    public static void Run()
    {
        count=0;string result;
        try
        {
            Check(YearOneSchedule.Milestones.Length==10,"10 milestones");
            Check(YearOneSchedule.Courses.Sum(c=>c.Credits)==32,"32 credits");
            foreach(var s in YearOneSchedule.Sessions.Where(s=>s.Work!=YearOneSchedule.Work.Exam))
            {
                foreach(double offset in new[]{-1d,0,15,15.001,119.999,120})
                {
                    var p=New();p.day=Array.IndexOf(YearOneSchedule.Milestones,YearOneSchedule.ById(s.MilestoneId))+1;p.AdvanceTo(s.Start+offset);
                    bool record=p.RecordAttendance(s.Id);
                    Check(record==(offset>=0 && offset<120),"attendance boundary "+s.Id+" "+offset);
                    if(record)Check(p.Has("late:"+s.Id)==(offset>15),"late boundary");
                }
            }
            foreach(int mode in new[]{0,1,2})
            {
                var p=New();int safeguard=0;
                while(!p.Has("year:two") && safeguard++<11)
                {
                    if(mode!=0)
                    {
                        bool travel=YearOneSchedule.Today(p.milestoneId).Length>0;
                        if(travel)Check(p.CommitTrip(false),"outbound fare");
                        foreach(var s in YearOneSchedule.Today(p.milestoneId))
                        {
                            p.AdvanceTo(s.Start);
                            var c=YearOneSchedule.Find(s.CourseId);
                            if(s.Work!=YearOneSchedule.Work.Exam)
                            {p.RecordAttendance(s.Id);p.Complete(c.Id,s.Work);if(mode==2 || c.Id!="calculus1")p.Complete(c.Id,YearOneSchedule.Work.Homework);}
                            else if(p.StartExam(c.Id))
                            {for(int q=0;q<3;q++)p.AnswerExam(c.ExerciseFor((YearOneSchedule.Work)(2+q)).Answer);Check(!p.StartExam(c.Id),"exam cannot repeat");}
                        }
                        if(travel){Check(p.CommitTrip(true),"return fare");p.safeLocation="Room";}
                    }
                    if(YearOneSchedule.Next(p.Milestone)==null){p.AdvanceTo(YearOneSchedule.At(10,840));Check(p.PrepareFeast()&&p.FinishFeast(),"feast independent of grades");}
                    var preview=p.PreviewTransition();double before=p.absoluteMinutes;p.PreviewTransition();Check(before==p.absoluteMinutes,"preview nonmutating");
                    Check(p.CommitTransition(preview),"transition works for all grades");Check(!p.CommitTransition(preview),"transition idempotent");
                    Check(p.IsValid(),"valid after transition");
                    var directory=Path.Combine(Path.GetTempPath(),"year1v2-"+Guid.NewGuid());var store=new YearOneCheckpointStore(directory);
                    File.WriteAllText(Path.Combine(directory+"-legacy.txt"),"untouched");Check(store.Save(p),"save");p=store.Load();Check(p!=null,"load");
                }
                Check(p.Has("year:two") && p.yearTwo!=null,"handoff exists");
                Check(p.Credits==(mode==0?0:mode==1?26:32),"expected credit path");
                Check(p.yearTwo.Debt.Length==(mode==0?6:mode==1?1:0),"expected debt");
                Check(p.money==(mode==0?130000:18000),"v2 bus budget covers sixteen fares");
            }
            var exam=New();exam.day=6;var course=YearOneSchedule.Courses[0];exam.completed.Add(course.Activity(YearOneSchedule.Work.Homework));exam.AdvanceTo(YearOneSchedule.At(6,480));
            // Attendance must be authored before advancing to the exam; both absences make this ineligible.
            Check(!exam.StartExam(course.Id),"two absences deny exam");
            var overdue=New();overdue.AdvanceTo(YearOneSchedule.Deadline(course));Check(overdue.Has("overdue:"+course.Id),"deadline exact");
            Check(!overdue.Complete(course.Id,YearOneSchedule.Work.Homework),"no late submission");
            var fractional=DeveloperCheckpointCatalog.Build("HK1_FIRST_CLASSES");fractional.AdvanceComputerTime(.31234567f,false);
            var fractionalPreview=fractional.PreviewTransition();var clone=JsonUtility.FromJson<YearOneProgress>(JsonUtility.ToJson(fractional));
            Check(clone.CommitTransition(fractionalPreview),"fractional clock survives transaction clone");
            var stale=DeveloperCheckpointCatalog.Build("HK1_FIRST_CLASSES");var stalePreview=stale.PreviewTransition();stale.AdvanceComputerTime(.01f,false);Check(!stale.CommitTransition(stalePreview),"genuinely stale preview rejected");
            var c0=YearOneSchedule.Courses[0];
            var paused=New();paused.day=2;double clock=paused.absoluteMinutes;paused.AdvanceComputerTime(1,false);Check(Math.Abs(paused.absoluteMinutes-clock-1)<.001,"x1");paused.AdvanceGaming(1);Check(Math.Abs(paused.absoluteMinutes-clock-5)<.001,"x4");
            paused.AdvanceGaming(100000);Check(paused.clockMinutes==1440 && paused.day==2,"midnight waits for explicit sleep");
            var reminder=DeveloperCheckpointCatalog.Build("HK1_FIRST_CLASSES");var session=reminder.NextSession;
            reminder.AdvanceTo(session.Start-4);Check(reminder.PendingReminder(session)==5,"highest-priority crossed reminder");reminder.MarkReminder(session,5);Check(reminder.PendingReminder(session)==-1,"no reminder duplicate");
            var active=DeveloperCheckpointCatalog.Build("HK1_FINAL_EXAMS");active.AdvanceTo(YearOneSchedule.At(6,480));Check(active.StartExam(c0.Id),"eligible starts exam");
            active.AnswerExam(c0.ExerciseFor(YearOneSchedule.Work.Homework).Answer);active.exam.Seconds=11;
            var folder=Path.Combine(Path.GetTempPath(),"year1v2-extra-"+Guid.NewGuid());Directory.CreateDirectory(folder);File.WriteAllText(Path.Combine(folder,"year-one-v1.json"),"legacy");
            var persistence=new YearOneCheckpointStore(folder);Check(persistence.Save(active),"save active exam");active=persistence.Load();Check(active.exam.Question==1 && active.exam.Seconds==11,"resume answers and timer");
            Check(active.SubmitExam() && active.Result(c0.Id).Correct==1 && !active.Result(c0.Id).Passed,"early submission fails unanswered questions");Check(!active.StartExam(c0.Id),"no retry after failed exam");
            Check(persistence.Save(active),"save result");File.WriteAllText(Path.Combine(folder,"year-one-v2.json"),"{broken");Check(persistence.Load()!=null,"corrupt primary falls back");Check(File.ReadAllText(Path.Combine(folder,"year-one-v1.json"))=="legacy","legacy untouched");
            var last=DeveloperCheckpointCatalog.Build("HK2_FINAL_EXAMS",false);Check(last.FinishDay() && last.ReadyForFeast && !last.Has("year:two"),"sleep through final exams still reaches feast without day eleven");
            foreach(var entry in DeveloperCheckpointCatalog.Entries)Check(entry.Progress().IsValid(),"checkpoint valid: "+entry.Id);
            result="PASS: "+count+" model checks; calendar, attendance boundaries, three full-year paths, save/load, preview, idempotency, handoff.\n";
        }
        catch(Exception e){result="FAIL after "+count+": "+e;Debug.LogException(e);}
        Directory.CreateDirectory("PlayModeInteractionReports/year-one-v2");File.WriteAllText("PlayModeInteractionReports/year-one-v2/model.txt",result);
    }
    static YearOneProgress New(){var p=new YearOneProgress();p.CompleteOpening("claim");p.CompleteOpening("clean");p.CompleteOpening("friends");p.CompleteOpening("meal");return p;}
}
