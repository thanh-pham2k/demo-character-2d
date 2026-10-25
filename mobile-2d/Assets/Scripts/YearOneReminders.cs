using System.Linq;
public sealed partial class YearOneProgress
{
    public YearOneSchedule.Session NextSession=>YearOneSchedule.Today(milestoneId).FirstOrDefault(s=>s.End>absoluteMinutes && (s.Work==YearOneSchedule.Work.Exam?Result(s.CourseId)==null:!Has(s.Id)));
    public int PendingReminder(YearOneSchedule.Session s)
    {
        if(s==null || absoluteMinutes>=s.Start || Has("attendance:"+s.Id))return -1;
        foreach(int t in new[]{5,15,30})if(s.Start-absoluteMinutes<=t && !Has("reminder:"+s.Id+":"+t))return t;
        return -1;
    }
    public void MarkReminder(YearOneSchedule.Session s,int threshold)
    {foreach(int t in new[]{30,15,5})if(t>=threshold)Mark("reminder:"+s.Id+":"+t);}
}
