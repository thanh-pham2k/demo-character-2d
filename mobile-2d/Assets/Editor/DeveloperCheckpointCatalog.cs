using System;
using System.Collections.Generic;
using System.Linq;

// Human-selectable v2 fixtures. All academic history is authored through the real model.
public static class DeveloperCheckpointCatalog
{
    public enum Presentation { Room,SeatLeft,SeatRight,Desktop,Schedule,Gaming,Makeup,Study,BedLeft,BedRight }
    public sealed class Entry
    {
        public readonly string Id,Group,Title,Description;public readonly Func<YearOneProgress> Progress;public readonly Presentation View;
        public Entry(string id,string group,string title,string description,Func<YearOneProgress> progress,Presentation view=Presentation.Room)
        {Id=id;Group=group;Title=title;Description=description;Progress=progress;View=view;}
    }
    public static readonly Entry[] Entries=Create();
    static Entry[] Create()
    {
        var list=new List<Entry>();
        foreach(var m in YearOneSchedule.Milestones)
        {
            string id=m.Id;int ordinal=Array.IndexOf(YearOneSchedule.Milestones,m)+1;
            list.Add(new Entry(id,"10 milestone",ordinal+". "+m.Title,"HK"+m.Semester+" • Tuần "+m.Week+" • Ngày lịch "+m.CalendarDay,()=>Build(id)));
            if(ordinal>1)list.Add(new Entry(id+"-skip","Chuyển ngày / skip","Trước chuyển khỏi "+m.Title,"Nằm nghỉ; chọn Ngủ để xem preview, có thể Quay lại.",()=>Build(id,false),Presentation.BedLeft));
        }
        list.Add(new Entry("arrival","Phòng 404","Nhận phòng","Chưa nhận giường; phòng sạch.",()=>new YearOneProgress()));
        foreach(var view in new[]{Presentation.Room,Presentation.SeatLeft,Presentation.SeatRight,Presentation.BedLeft,Presentation.BedRight,Presentation.Desktop,Presentation.Schedule,Presentation.Study})
        {var value=view;list.Add(new Entry("room-"+view,"Phòng / Windows",view.ToString(),"Sáng ngày học đầu tiên; chưa có kết quả môn.",()=>Build("HK1_FIRST_CLASSES"),value));}
        foreach(int minutes in new[]{30,15,5,0,-16})
        {
            int value=minutes;list.Add(new Entry("reminder-"+value,"Đồng hồ / cảnh báo","Trước giờ học "+value+" phút","Đang chơi LOL; HUD nhắc và cập nhật giờ.",()=>At(480-value),Presentation.Gaming));
        }
        list.Add(new Entry("absent","Điểm danh / bài tập","Vắng cả sáng","Vắng 2 buổi, chưa phải môn nợ; lịch sử được giữ.",()=>At(720),Presentation.Schedule));
        list.Add(new Entry("makeup","Điểm danh / bài tập","Đang học bù","Học bù không xóa vắng và không đổi kết quả FAIL.",()=>At(720),Presentation.Makeup));
        list.Add(new Entry("deadline-before","Điểm danh / bài tập","Trước deadline HK1","21:59 ngày chuẩn bị thi; bài chưa nộp.",()=>{var p=Build("HK1_PREPARATION",false);p.AdvanceTo(YearOneSchedule.Deadline(YearOneSchedule.Courses[0])-1);return p;},Presentation.Schedule));
        list.Add(new Entry("deadline-after","Điểm danh / bài tập","Vừa quá hạn HK1","22:00; bài quá hạn, chưa thi nên chưa có FAIL mới.",()=>{var p=Build("HK1_PREPARATION",false);p.AdvanceTo(YearOneSchedule.Deadline(YearOneSchedule.Courses[0]));return p;},Presentation.Schedule));
        list.Add(new Entry("exam-active","Thi / kết quả","Giữa bài thi HK1","Đã trả lời câu 1; load giữ lại câu và timer.",()=>{var p=Build("HK1_FINAL_EXAMS");p.AdvanceTo(YearOneSchedule.At(6,480));p.StartExam("calculus1");p.AnswerExam(YearOneSchedule.Courses[0].ExerciseFor(YearOneSchedule.Work.Homework).Answer);p.exam.Seconds=11;return p;}));
        list.Add(new Entry("exam-ineligible","Thi / kết quả","Không đủ điều kiện HK1","Bỏ học, thiếu bài; chờ hết giờ mới kết luận FAIL.",()=>Build("HK1_FINAL_EXAMS",false)));
        foreach(int mode in new[]{0,1,2})
        {
            int value=mode;string label=mode==0?"0/32":mode==1?"26/32":"32/32";
            list.Add(new Entry("final-"+mode,"Tổng kết / Năm 2","Cuối năm "+label,"Thi xong; tổng kết thật, vẫn được ăn lẩu.",()=>Final(value)));
            list.Add(new Entry("year2-"+mode,"Tổng kết / Năm 2","Năm 2 mang theo "+label,"Snapshot tín chỉ và môn nợ đã được lưu.",()=>{var p=Final(value);p.PrepareFeast();p.FinishFeast();p.EnterYearTwo();return p;}));
        }
        list.Add(new Entry("feast","Tổng kết / Năm 2","Trước ăn lẩu, có môn nợ","Kết năm không cần 32 TC.",()=>Final(1)));
        list.Add(new Entry("after-feast","Tổng kết / Năm 2","Ăn lẩu xong, chuẩn bị sang năm","Đã ăn; chọn ngủ để kiểm tra bàn giao Năm 2.",()=>{var p=Final(1);p.PrepareFeast();p.FinishFeast();return p;},Presentation.BedLeft));
        list.Add(new Entry("coop","KTX / bus","Ngày 8: chơi cùng Long","Chọn game; sau 20 giây không bị ép thoát.",()=>Build("HK2_DORM_LIFE"),Presentation.Desktop));
        list.Add(new Entry("lights","KTX / bus","Ngày 8: tắt đèn","Đã chơi cùng Long; có thể tắt đèn hoặc ngủ trực tiếp.",()=>{var p=Build("HK2_DORM_LIFE");p.MarkEvening("game");return p;}));
        list.Add(new Entry("missed-bus","KTX / bus","Ngày 7: lỡ xe","Đã lỡ chuyến đầu; vé chưa bị trừ.",()=>{var p=Build("HK2_START");p.morningSecondsRemaining=0;p.MarkMissedBus();return p;}));
        list.Add(new Entry("bus-before","KTX / bus","Ngày 7: còn 10 phút game tới hạn xe","WC và balo đã sẵn sàng.",()=>{var p=Build("HK2_START");p.morningSecondsRemaining=10;return p;}));
        list.Add(new Entry("gate","KTX / bus","Cổng H6 trước tiết đầu","Đã trả vé đi, đứng ở cổng.",()=>{var p=Build("HK1_FIRST_CLASSES");p.CommitTrip(false);return p;}));
        return list.ToArray();
    }
    public static YearOneProgress Build(string destination,bool good=true)
    {
        var p=new YearOneProgress();if(destination==p.milestoneId)return p;
        p.CompleteOpening("claim");p.CompleteOpening("clean");p.CompleteOpening("friends");p.CompleteOpening("meal");
        while(p.milestoneId!=destination)
        {if(good)CompleteToday(p,2);if(!p.FinishDay())throw new InvalidOperationException("Invalid fixture destination "+destination);}
        p.backpack=true;p.refreshed=true;p.safeLocation="Room";return p;
    }
    public static void CompleteToday(YearOneProgress p,int mode)
    {
        foreach(var session in YearOneSchedule.Today(p.milestoneId))
        {
            p.AdvanceTo(session.Start);var c=YearOneSchedule.Find(session.CourseId);
            if(session.Work!=YearOneSchedule.Work.Exam)
            {p.RecordAttendance(session.Id);p.Complete(c.Id,session.Work);if(mode==2 || (mode==1 && c.Id!="calculus1"))p.Complete(c.Id,YearOneSchedule.Work.Homework);}
            else if(p.StartExam(c.Id))for(int q=0;q<3;q++)p.AnswerExam(c.ExerciseFor((YearOneSchedule.Work)(2+q)).Answer);
        }
    }
    public static YearOneProgress Final(int mode)
    {
        var p=new YearOneProgress();p.CompleteOpening("claim");p.CompleteOpening("clean");p.CompleteOpening("friends");p.CompleteOpening("meal");
        while(YearOneSchedule.Next(p.Milestone)!=null){if(mode!=0)CompleteToday(p,mode);p.FinishDay();}
        if(mode!=0)CompleteToday(p,mode);p.AdvanceTo(YearOneSchedule.At(10,840));return p;
    }
    static YearOneProgress At(int minutes){var p=Build("HK1_FIRST_CLASSES");p.AdvanceTo(YearOneSchedule.At(2,minutes));return p;}
}
