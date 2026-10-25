using System;
using System.Linq;
public sealed partial class YearOneProgress
{
    public const float GameMinutesPerSecond=4f;
    public float gamingSeconds;
    // Compatibility accessors; absoluteMinutes is the only persisted clock.
    public int clockDay {get=>day;set{}}
    public float clockMinutes {get=>(float)(absoluteMinutes-Milestone.CalendarDay*1440d);set=>AdvanceTo(Math.Max(absoluteMinutes,Milestone.CalendarDay*1440d+value));}
    public void EnsureClock(){}
    public string Clock {get{int m=(int)clockMinutes;return $"{m/60:00}:{m%60:00}";}}
    public string TimeOfDay=>clockMinutes<720?"Sáng":clockMinutes<840?"Trưa":clockMinutes<1080?"Chiều":clockMinutes<1320?"Tối":"Đêm";
    public static YearOneSchedule.Work LessonWork(YearOneSchedule.Course c,int onDay)=>c.FirstDay==onDay?YearOneSchedule.Work.Lesson1:YearOneSchedule.Work.Lesson2;
    public bool Missed(YearOneSchedule.Course c,YearOneSchedule.Work work)=>Has("missed:"+c.Activity(work));
    public bool NeedsMakeup(YearOneSchedule.Course c,YearOneSchedule.Work work)=>Missed(c,work) && !Has("makeup:"+c.Activity(work)) && Result(c.Id)==null;
    public int OwedCourses=>results.Count(r=>!r.Passed);
    public int RemainingCourses=>YearOneSchedule.Courses.Count(c=>Result(c.Id)?.Passed!=true);
    public bool LessonResolved(YearOneSchedule.Course c,YearOneSchedule.Work work)=>Has("attendance:"+c.Activity(work)) || Missed(c,work);
    public bool DayClassesResolved=>YearOneSchedule.Today(milestoneId).All(s=>s.Work==YearOneSchedule.Work.Exam?Result(s.CourseId)!=null:Has("attendance:"+s.Id)||Has("missed:"+s.Id));
    public bool AdvanceGaming(float seconds)=>AdvanceComputerTime(seconds,true);
    public bool AdvanceComputerTime(float seconds,bool gaming)
    {
        if(seconds<=0 || float.IsNaN(seconds)||float.IsInfinity(seconds)||Has("year:two"))return false;
        if(gaming)gamingSeconds+=seconds;
        return AdvanceTo(Math.Min((Milestone.CalendarDay+1)*1440d,absoluteMinutes+seconds*(gaming?4:1)));
    }
    public bool CompleteMakeup(string course,YearOneSchedule.Work work)
    {var c=YearOneSchedule.Find(course);return c!=null && safeLocation=="Room" && NeedsMakeup(c,work) && Mark("makeup:"+c.Activity(work));}
    public bool GamingValid=>!float.IsNaN(gamingSeconds) && !float.IsInfinity(gamingSeconds) && gamingSeconds>=0;
}
