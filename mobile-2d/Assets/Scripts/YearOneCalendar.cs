using System;
using System.Linq;

public static partial class YearOneSchedule
{
    [Serializable] public sealed class Milestone
    {
        public string Id,Title; public int CalendarDay,Semester,Week,DepartureMinute; public bool Exams,MissableBus,Coop;
        public Milestone(string id,string title,int date,int term,int week,bool exams=false,bool bus=false,bool coop=false,int departure=-1)
        {Id=id;Title=title;CalendarDay=date;Semester=term;Week=week;Exams=exams;MissableBus=bus;Coop=coop;DepartureMinute=departure;}
    }
    public static readonly Milestone[] Milestones={
        new Milestone("YEAR1_ARRIVAL","Nhận phòng 404",0,1,1),
        new Milestone("HK1_FIRST_CLASSES","Ngày học đầu tiên",1,1,1),
        new Milestone("HK1_GAMING_CHOICE","Học hay chơi",2,1,1),
        new Milestone("HK1_MIDTERM","Giữa học kỳ 1",29,1,5),
        new Milestone("HK1_PREPARATION","Chuẩn bị thi HK1",92,1,14),
        new Milestone("HK1_FINAL_EXAMS","Thi học kỳ 1",99,1,15,true),
        new Milestone("HK2_START","Trở lại trường",113,2,1,false,true,false,460),
        new Milestone("HK2_DORM_LIFE","Đời sống KTX",155,2,7,false,false,true),
        new Milestone("HK2_PREPARATION","Chuẩn bị thi HK2",204,2,14),
        new Milestone("HK2_FINAL_EXAMS","Thi cuối năm",211,2,15,true)
    };
    public static Milestone ById(string id)=>Array.Find(Milestones,m=>m.Id==id);
    public static Milestone ByDay(int ordinal)=>ordinal>=1 && ordinal<=Milestones.Length?Milestones[ordinal-1]:null;
    public static Milestone Next(Milestone m){int i=Array.IndexOf(Milestones,m);return i>=0 && i+1<Milestones.Length?Milestones[i+1]:null;}
    public static double At(int ordinal,int minutes)=>ByDay(ordinal).CalendarDay*1440d+minutes;
    [Serializable] public sealed class Session
    {
        public string Id,CourseId,MilestoneId;public Work Work;public double Start,End;
        public Session(Course c,int ordinal,int start,int end,Work work)
        {Id=c.Activity(work);CourseId=c.Id;MilestoneId=ByDay(ordinal).Id;Start=At(ordinal,start);End=At(ordinal,end);Work=work;}
    }
    private static Session[] sessions;
    public static Session[] Sessions=>sessions??(sessions=Courses.SelectMany((c,i)=>new[]{
        new Session(c,c.FirstDay,c.FirstSlot*120+480,c.FirstSlot*120+600,Work.Lesson1),
        new Session(c,c.SecondDay,c.SecondSlot*120+480,c.SecondSlot*120+600,Work.Lesson2),
        new Session(c,Array.IndexOf(Milestones,ById(c.Semester==1?"HK1_FINAL_EXAMS":"HK2_FINAL_EXAMS"))+1,new[]{480,600,780}[i%3],new[]{540,660,840}[i%3],Work.Exam)}).ToArray());
    public static Session SessionFor(Course c,Work work)=>Sessions.FirstOrDefault(s=>s.CourseId==c.Id && s.Work==work);
    public static Session[] Today(string milestone)=>Sessions.Where(s=>s.MilestoneId==milestone).OrderBy(s=>s.Start).ToArray();
    public static double Deadline(Course c)=>ById(c.Semester==1?"HK1_PREPARATION":"HK2_PREPARATION").CalendarDay*1440d+1320;
    public static double Release(Course c)=>SessionFor(c,Work.Lesson1).Start;
}
