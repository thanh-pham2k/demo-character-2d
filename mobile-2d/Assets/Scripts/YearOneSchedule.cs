using System;

// Authored representative days, independent of scene/UI presentation.
public static partial class YearOneSchedule
{
    public const int LateGraceMinutes = 15;
    public static int LessonStartMinutes(int slot) => (8 + slot * 2) * 60;
    public static int LessonEndMinutes(int slot) => LessonStartMinutes(slot) + 120;
    public enum Work { Lesson1, Lesson2, Homework, Revision, Exam }
    public sealed class Exercise
    {
        public readonly string Question,Hint;
        public readonly string[] Choices;
        public readonly int Answer;
        public Exercise(string question,string[] choices,int answer,string hint){Question=question;Choices=choices;Answer=answer;Hint=hint;}
    }
    [Serializable]
    public sealed class Course
    {
        public readonly string Id, Name;
        public readonly int FirstDay, SecondDay;
        public readonly bool UsesComputer;
        public readonly int Semester,Credits,FirstSlot,SecondSlot;
        public readonly string Question, Hint;
        public readonly string[] Choices;
        public readonly int Answer;
        public Course(string id,string name,int first,int second,bool computer,string question,string[] choices,int answer,string hint,int credits,int firstSlot,int secondSlot,int semester)
        {Id=id;Name=name;FirstDay=first;SecondDay=second;UsesComputer=computer;Question=question;Choices=choices;Answer=answer;Hint=hint;Credits=credits;FirstSlot=firstSlot;SecondSlot=secondSlot;Semester=semester;}
        public Exercise ExerciseFor(Work work)
        {
            int task=(int)work-(int)Work.Homework;
            int course=Array.FindIndex(Courses,c=>ReferenceEquals(c,this));
            return task>=0 && task<3 && course>=0?StudyExercises[course][task]:new Exercise(Question,Choices,Answer,Hint);
        }
        public string RoomName=>UsesComputer?"H6-201 • Phòng máy":Id.StartsWith("physics",StringComparison.Ordinal)?"H6-102 • Lớp Vật Lý":"H6-101 • Lớp Toán";
        public string Activity(Work work)=>Id+":"+work;
    }
    public static readonly Course[] Courses={
        new Course("calculus1","Giải Tích 1",2,3,false,"1 + 2 + 3 + ... + 10 = ?",new[]{"45","55","65"},1,"Ghép 1 + 10, 2 + 9… có 5 cặp bằng 11.",6,0,1,1),
        new Course("physics1","Vật Lý 1",2,4,false,"Đi đều 3 m mỗi giây. Sau 4 giây đi được bao xa?",new[]{"7 m","1 m","12 m"},2,"Quãng đường = vận tốc × thời gian.",5,1,0,1),
        new Course("computing","Nhập môn Tin",3,4,true,"Một nhóm có 4 bạn, mỗi bạn lưu 2 tệp. Tổng cộng bao nhiêu tệp?",new[]{"8","6","4"},0,"Có 4 nhóm nhỏ, mỗi nhóm 2 tệp.",5,0,1,1),
        new Course("calculus2","Giải Tích 2",7,9,false,"Dãy 2, 4, 6, 8… Số tiếp theo là gì?",new[]{"9","12","10"},2,"Mỗi bước tăng thêm 2.",6,0,0,2),
        new Course("physics2","Vật Lý 2",7,8,false,"Hai bóng đèn, mỗi bóng 5 W. Tổng công suất là bao nhiêu?",new[]{"10 W","5 W","25 W"},0,"Cộng công suất của hai bóng.",5,1,1,2),
        new Course("programming","Lập trình",8,9,true,"Vòng lặp in số 1, 2, 3. Có bao nhiêu lần in?",new[]{"2","3","4"},1,"Đếm số giá trị được in ra.",5,0,1,2),
    };
    // Homework, revision and exam use distinct easy examples for every subject.
    private static readonly Exercise[][] StudyExercises={
        new[]{
            new Exercise("1 + 2 + 3 + 4 + 5 = ?",new[]{"12","15","20"},1,"Ghép 1 + 5 và 2 + 4, rồi cộng số 3."),
            new Exercise("6 + 4 + 3 + 7 = ?",new[]{"18","24","20"},2,"Ghép 6 + 4 và 3 + 7 thành hai nhóm 10."),
            new Exercise("Tổng các số từ 1 đến 6 là bao nhiêu?",new[]{"21","18","24"},0,"Có ba cặp: 1 + 6, 2 + 5, 3 + 4."),
        },
        new[]{
            new Exercise("Đi đều 2 m/s trong 3 giây. Đi được bao nhiêu mét?",new[]{"5 m","1 m","6 m"},2,"Quãng đường = 2 × 3."),
            new Exercise("Đi 10 m trong 5 giây. Vận tốc trung bình là bao nhiêu?",new[]{"2 m/s","5 m/s","15 m/s"},0,"Vận tốc = quãng đường chia thời gian."),
            new Exercise("Xe mô hình chạy 4 m/s trong 2 giây. Quãng đường là?",new[]{"6 m","8 m","2 m"},1,"Lấy 4 nhân với 2."),
        },
        new[]{
            new Exercise("Thư mục A có 5 tệp, B có 3 tệp. Tổng cộng bao nhiêu?",new[]{"8 tệp","2 tệp","15 tệp"},0,"Cộng số tệp của hai thư mục."),
            new Exercise("Thiết bị nào dùng để nhập chữ vào máy tính?",new[]{"Màn hình","Bàn phím","Loa"},1,"Đó là thiết bị có các phím chữ cái."),
            new Exercise("Có 3 nhóm, mỗi nhóm lưu 4 tệp. Tổng số tệp là?",new[]{"7","9","12"},2,"Tính 3 × 4."),
        },
        new[]{
            new Exercise("Dãy 3, 6, 9… Số tiếp theo là gì?",new[]{"10","15","12"},2,"Mỗi số tăng thêm 3."),
            new Exercise("Cho f(x) = 2x. Giá trị f(4) bằng bao nhiêu?",new[]{"8","6","4"},0,"Thay x bằng 4 rồi nhân 2."),
            new Exercise("Dãy 5, 10, 15… Số tiếp theo là gì?",new[]{"18","20","25"},1,"Cộng thêm 5 vào số cuối."),
        },
        new[]{
            new Exercise("Hai bóng đèn, mỗi bóng 3 W. Tổng công suất là?",new[]{"3 W","6 W","9 W"},1,"Cộng 3 W với 3 W."),
            new Exercise("U = 9 V, R = 3 Ω. Với I = U/R, I bằng bao nhiêu?",new[]{"6 A","12 A","3 A"},2,"Lấy 9 chia cho 3."),
            new Exercise("Ba bóng đèn, mỗi bóng 4 W. Tổng công suất là?",new[]{"12 W","7 W","4 W"},0,"Tính 3 × 4."),
        },
        new[]{
            new Exercise("n = 2; n = n + 3. Sau đó n bằng bao nhiêu?",new[]{"5","3","2"},0,"Lấy giá trị cũ 2 cộng thêm 3."),
            new Exercise("Vòng lặp in lần lượt 0 và 1. Có bao nhiêu lần in?",new[]{"0","1","2"},2,"Đếm cả hai giá trị 0 và 1."),
            new Exercise("Nếu 5 > 3 thì in Đúng, ngược lại in Sai. Kết quả là?",new[]{"Sai","Đúng","Không in"},1,"So sánh 5 với 3."),
        },
    };
    public static Course Find(string id)=>Array.Find(Courses,c=>c.Id==id);
    public static Course[] Lessons(int day)
    {
        var list=new System.Collections.Generic.List<Course>();
        foreach(var session in Today(ByDay(day)?.Id))if(session.Work!=Work.Exam)list.Add(Find(session.CourseId));
        return list.ToArray();
    }
    public static string DayTitle(int day){var m=ByDay(day);return m==null?"Năm 2":"HK"+m.Semester+" • Tuần "+m.Week+" • "+new[]{"Chủ nhật","Thứ Hai","Thứ Ba","Thứ Tư","Thứ Năm","Thứ Sáu","Thứ Bảy"}[m.CalendarDay%7]+" • "+m.Title;}
}
