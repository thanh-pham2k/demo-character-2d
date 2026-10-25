using System;
using System.Collections.Generic;
using System.Linq;

[Serializable] public sealed class YearOneExamState
{
    public string CourseId; public int[] Answers={-1,-1,-1}; public float Seconds=20;public int Question;
}
[Serializable] public sealed class YearOneCourseResult
{
    public string CourseId,Reason;public bool Passed;public int Correct;
}
[Serializable] public sealed class YearTwoAcademicSnapshot
{
    public int Credits,Target=32;public string[] Passed,Debt,History;public YearOneCourseResult[] Results;
    public int Semester1Credits,Semester2Credits;
}
[Serializable] public sealed class YearOneTransition
{
    public string Source,Destination;public double From,To;public string[] Warnings;public bool ToYearTwo;
}
[Serializable]
public sealed partial class YearOneProgress : UnityEngine.ISerializationCallbackReceiver
{
    public int version=2;
    public string milestoneId="YEAR1_ARRIVAL";
    public double absoluteMinutes=860;
    public int day {get=>Array.IndexOf(YearOneSchedule.Milestones,Milestone)+1;set{var m=YearOneSchedule.ByDay(value);if(m!=null){milestoneId=m.Id;absoluteMinutes=m.CalendarDay*1440d+(value==1?860:440);}}}
    public YearOneSchedule.Milestone Milestone=>YearOneSchedule.ById(milestoneId);
    public int money=35000;
    public int ticketsPurchased;
    public string safeLocation="Room";
    public List<string> completed=new List<string>();
    public List<YearOneCourseResult> results=new List<YearOneCourseResult>();
    public YearOneExamState exam;
    public YearTwoAcademicSnapshot yearTwo;
    public float morningSecondsRemaining
    {
        get=>Milestone.MissableBus?(float)Math.Max(0,Milestone.CalendarDay*1440d+Milestone.DepartureMinute-absoluteMinutes):120;
        set{if(Milestone.MissableBus)AdvanceTo(Math.Max(absoluteMinutes,Milestone.CalendarDay*1440d+Milestone.DepartureMinute-Math.Max(0,value)));}
    }
    public bool backpack,refreshed;
    public bool Has(string id)=>completed!=null && completed.Contains(id);
    private bool Mark(string id){if(Has(id))return false;completed.Add(id);return true;}
    public bool OpeningComplete=>Has("opening:meal");
    public bool CompleteOpening(string id)
    {
        if(id=="claim")return Mark("opening:claim");
        if(id=="clean" && Has("opening:claim"))return Mark("opening:clean");
        if(id=="friends" && Has("opening:clean"))return Mark("opening:friends");
        if(id!="meal" || !Has("opening:friends") || money<25000 || Has("opening:meal"))return false;
        Mark("opening:meal");money=money-25000+120000;safeLocation="Room";return true;
    }
    public YearOneCourseResult Result(string course)=>results.FirstOrDefault(r=>r.CourseId==course);
    public int Absences(YearOneSchedule.Course c)=>new[]{YearOneSchedule.Work.Lesson1,YearOneSchedule.Work.Lesson2}.Count(w=>Missed(c,w));
    public bool Eligible(YearOneSchedule.Course c)=>c!=null && Absences(c)<=1 && Has(c.Activity(YearOneSchedule.Work.Homework));
    public bool RecordAttendance(string sessionId)
    {
        var s=YearOneSchedule.Sessions.FirstOrDefault(x=>x.Id==sessionId && x.Work!=YearOneSchedule.Work.Exam);
        if(s==null || absoluteMinutes<s.Start || absoluteMinutes>=s.End || Has("attendance:"+s.Id) || Has("missed:"+s.Id))return false;
        Mark("attendance:"+s.Id);if(absoluteMinutes>s.Start+YearOneSchedule.LateGraceMinutes)Mark("late:"+s.Id);return true;
    }
    public bool CanComplete(YearOneSchedule.Course c,YearOneSchedule.Work work)
    {
        if(c==null || !OpeningComplete || Has("year:two") || Result(c.Id)!=null)return false;
        if(work==YearOneSchedule.Work.Exam){var e=YearOneSchedule.SessionFor(c,work);return Eligible(c) && exam==null && absoluteMinutes>=e.Start && absoluteMinutes<e.End;}
        if(work==YearOneSchedule.Work.Homework)return absoluteMinutes>=YearOneSchedule.Release(c) && (absoluteMinutes<YearOneSchedule.Deadline(c) || Has(c.Activity(work)));
        if(work==YearOneSchedule.Work.Revision)return absoluteMinutes>=YearOneSchedule.Release(c);
        var s=YearOneSchedule.SessionFor(c,work);return s.MilestoneId==milestoneId && Has("attendance:"+s.Id) && !Has(s.Id);
    }
    public bool Complete(string course,YearOneSchedule.Work work)
    {
        var c=YearOneSchedule.Find(course);if(!CanComplete(c,work) || work==YearOneSchedule.Work.Exam)return false;
        Mark(c.Activity(work));
        if(work==YearOneSchedule.Work.Lesson1 || work==YearOneSchedule.Work.Lesson2)AdvanceTo(Math.Max(absoluteMinutes,YearOneSchedule.SessionFor(c,work).End));
        return true;
    }
    public bool StartExam(string course)
    {
        var c=YearOneSchedule.Find(course);if(!CanComplete(c,YearOneSchedule.Work.Exam))return false;
        exam=new YearOneExamState{CourseId=course};return true;
    }
    public bool AnswerExam(int answer)
    {
        if(exam==null || answer< -1 || answer>2)return false;
        exam.Answers[exam.Question]=answer;exam.Question++;exam.Seconds=20;
        if(exam.Question==3)SubmitExam();return true;
    }
    public bool SubmitExam()
    {
        if(exam==null)return false;var current=exam;var c=YearOneSchedule.Find(current.CourseId);
        int correct=0;for(int i=0;i<3;i++)if(current.Answers[i]==c.ExerciseFor((YearOneSchedule.Work)((int)YearOneSchedule.Work.Homework+i)).Answer)correct++;
        exam=null;if(Result(c.Id)!=null)return false;
        results.Add(new YearOneCourseResult{CourseId=c.Id,Correct=correct,Passed=correct>=2,Reason=correct>=2?"Đạt":"Không đạt bài thi"});
        Mark(c.Activity(YearOneSchedule.Work.Exam));AdvanceTo(Math.Max(absoluteMinutes,YearOneSchedule.SessionFor(c,YearOneSchedule.Work.Exam).End));return true;
    }
    public bool AdvanceTo(double target)
    {
        if(double.IsNaN(target)||double.IsInfinity(target)||target<absoluteMinutes)return false;
        foreach(var s in YearOneSchedule.Sessions.Where(s=>s.End<=target))
        {
            if(s.Work!=YearOneSchedule.Work.Exam){if(!Has("attendance:"+s.Id))Mark("missed:"+s.Id);}
            else if(Result(s.CourseId)==null && (exam==null || exam.CourseId!=s.CourseId))
            {var c=YearOneSchedule.Find(s.CourseId);results.Add(new YearOneCourseResult{CourseId=c.Id,Passed=false,Reason=Eligible(c)?"Bỏ thi":"Không đủ điều kiện"});}
        }
        foreach(var c in YearOneSchedule.Courses)if(target>=YearOneSchedule.Deadline(c) && !Has(c.Activity(YearOneSchedule.Work.Homework)))Mark("overdue:"+c.Id);
        absoluteMinutes=target;
        if(Milestone.MissableBus && morningSecondsRemaining<=0 && !Has("day:4:stop"))MarkMissedBus();
        return true;
    }
    public int Credits=>results.Where(r=>r.Passed).Sum(r=>YearOneSchedule.Find(r.CourseId).Credits);
    public bool TermPassed(int term)=>YearOneSchedule.Courses.Where(c=>c.Semester==term).All(c=>Result(c.Id)?.Passed==true);
    public bool LessonsDoneToday=>YearOneSchedule.Today(milestoneId).Where(s=>s.Work!=YearOneSchedule.Work.Exam).All(s=>Has(s.Id));
    public string TripId(bool returning)=>milestoneId+(returning?":return":":outbound");
    public bool CommitTrip(bool returning,bool nextBus=false)
    {
        if(!OpeningComplete || Has("year:two") || money<7000 || (returning?safeLocation!="Gate":safeLocation=="Gate") || (returning&&!Has(TripId(false))))return false;
        bool missed=!returning && Milestone.MissableBus && Has("day:4:missed");if(nextBus!=missed)return false;
        Mark("ticket:"+milestoneId+":"+(ticketsPurchased++)+(returning?":return":":outbound"));Mark(TripId(returning));if(nextBus)Mark("day:4:next-bus");money-=7000;safeLocation=returning?"Campus":"Gate";return true;
    }
    public bool CompleteClassStory()=>Mark("story:h6-told-dam");
    public bool MarkMissedBus()=>Milestone.MissableBus && morningSecondsRemaining<=0 && !Has("day:4:stop") && !Has(TripId(false)) && Mark("day:4:missed");
    public bool MarkEvening(string activity)=>Milestone.Coop && Mark("day:2:"+activity);
    public bool CanSleep=>!Has("year:two") && exam==null;
    public YearOneTransition PreviewTransition()
    {
        var next=YearOneSchedule.Next(Milestone);bool yearTwo=next==null && Has("year:feast");
        double target=next!=null?next.CalendarDay*1440d+440:yearTwo?(Milestone.CalendarDay+1)*1440d+440:Math.Max(absoluteMinutes,Milestone.CalendarDay*1440d+1380);
        var warnings=new List<string>();
        foreach(var s in YearOneSchedule.Sessions.Where(s=>s.End>absoluteMinutes && s.End<=target))
        {
            if(s.Work==YearOneSchedule.Work.Exam && Result(s.CourseId)==null)warnings.Add("Bỏ kỳ thi: "+YearOneSchedule.Find(s.CourseId).Name);
            else if(s.Work!=YearOneSchedule.Work.Exam && !Has("attendance:"+s.Id))warnings.Add("Vắng: "+YearOneSchedule.Find(s.CourseId).Name);
        }
        foreach(var c in YearOneSchedule.Courses)if(!Has(c.Activity(YearOneSchedule.Work.Homework)) && !Has("overdue:"+c.Id) && YearOneSchedule.Deadline(c)>absoluteMinutes && YearOneSchedule.Deadline(c)<=target)warnings.Add("Bài sẽ quá hạn: "+c.Name);
        return new YearOneTransition{Source=milestoneId,Destination=next?.Id??(yearTwo?"YEAR2_START":milestoneId),From=absoluteMinutes,To=target,Warnings=warnings.ToArray(),ToYearTwo=yearTwo};
    }
    public bool CommitTransition(YearOneTransition preview)
    {
        if(!CanSleep || preview==null || preview.To<=preview.From || preview.Source!=milestoneId || Math.Abs(preview.From-absoluteMinutes)>1e-7)return false;
        var valid=PreviewTransition();if(preview.To!=valid.To || preview.Destination!=valid.Destination)return false;
        if(valid.ToYearTwo && !Has("year:feast"))return false;
        AdvanceTo(valid.To);Mark("sleep:"+milestoneId);
        if(valid.ToYearTwo){yearTwo=BuildYearTwoSnapshot();Mark("year:two");}
        else milestoneId=valid.Destination;
        morningSecondsRemaining=120;refreshed=false;safeLocation="Room";return true;
    }
    public bool FinishDay()=>CommitTransition(PreviewTransition());
    public bool ReadyForFeast=>YearOneSchedule.Next(Milestone)==null && YearOneSchedule.Today(milestoneId).Where(s=>s.Work==YearOneSchedule.Work.Exam).All(s=>Result(s.CourseId)!=null);
    public bool PrepareFeast()=>ReadyForFeast && Mark("year:feast-prepared");
    public bool FinishFeast()=>ReadyForFeast && Has("year:feast-prepared") && Mark("year:feast");
    public bool EnterYearTwo()=>Has("year:feast") && CommitTransition(PreviewTransition());
    public YearTwoAcademicSnapshot BuildYearTwoSnapshot()=>new YearTwoAcademicSnapshot{Credits=Credits,Passed=results.Where(r=>r.Passed).Select(r=>r.CourseId).ToArray(),Debt=results.Where(r=>!r.Passed).Select(r=>r.CourseId).ToArray(),Results=results.Select(r=>new YearOneCourseResult{CourseId=r.CourseId,Reason=r.Reason,Passed=r.Passed,Correct=r.Correct}).ToArray(),History=completed.ToArray(),Semester1Credits=results.Where(r=>r.Passed && YearOneSchedule.Find(r.CourseId).Semester==1).Sum(r=>YearOneSchedule.Find(r.CourseId).Credits),Semester2Credits=results.Where(r=>r.Passed && YearOneSchedule.Find(r.CourseId).Semester==2).Sum(r=>YearOneSchedule.Find(r.CourseId).Credits)};
    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize()
    {
        if(completed==null)completed=new List<string>();
        if(exam!=null && string.IsNullOrEmpty(exam.CourseId))exam=null;
        if(!Has("year:two"))yearTwo=null;
    }
    public bool IsValid()=>version==2 && Milestone!=null && money>=0 && ticketsPurchased>=0 && GamingValid && !double.IsNaN(absoluteMinutes) && !double.IsInfinity(absoluteMinutes) && absoluteMinutes>=Milestone.CalendarDay*1440d && absoluteMinutes<=(Milestone.CalendarDay+1)*1440d+(Has("year:two")?440:0) && completed!=null && results!=null && completed.Count==completed.Distinct().Count() && results.All(r=>r!=null && YearOneSchedule.Find(r.CourseId)!=null && r.Correct>=0 && r.Correct<=3 && r.Passed==(r.Correct>=2)) && results.Select(r=>r.CourseId).Distinct().Count()==results.Count && (exam==null || (YearOneSchedule.Find(exam.CourseId)!=null && Result(exam.CourseId)==null && exam.Answers!=null && exam.Answers.Length==3 && exam.Question>=0 && exam.Question<3 && exam.Seconds>=0 && exam.Seconds<=20 && !float.IsNaN(exam.Seconds) && !float.IsInfinity(exam.Seconds) && exam.Answers.All(a=>a>=-1 && a<=2))) && (!Has("year:two") || yearTwo!=null);
}
