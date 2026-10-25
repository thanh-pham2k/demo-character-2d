using System;
namespace ProductionChapters.Year2
{
    // Transient attempt state. Durable ordering remains in the injected campaign.
    public sealed class Year2Challenge
    {
        public float StealthRemaining { get; private set; } = 10;
        public bool Caught { get; private set; }
        public int HideOrder { get; private set; }
        public float AssignmentSeconds { get; private set; }
        public bool DeadlineMissed => AssignmentSeconds >= 1199; // 23:40:00 -> 23:59:59
        public int ExamAnswered { get; private set; }
        public int ExamCorrect { get; private set; }
        public int ExamPercent => ExamAnswered == 0 ? 0 : ExamCorrect * 100 / ExamAnswered;
        public static readonly string[] Questions = { "Tổng 1 + 2 + ... + 10?", "Mảng int a[3]: chỉ số cuối hợp lệ?", "Vòng for(i=0;i<3;i++) chạy mấy lần?", "10 / 2 + 3 bằng?", "Điều kiện dừng: i < n hay i <= n khi mảng có n phần tử?" };
        public static readonly string[][] Answers = { new[]{"55","45"}, new[]{"2","3"}, new[]{"3","4"}, new[]{"8","5"}, new[]{"i < n","i <= n"} };
        public void Tick(float seconds, bool stealth, bool assignment)
        {
            if(seconds < 0 || float.IsNaN(seconds) || float.IsInfinity(seconds))return;
            if(stealth && !Caught) { StealthRemaining=Math.Max(0,StealthRemaining-seconds); if(StealthRemaining<=0)Caught=true; }
            if(assignment)AssignmentSeconds+=seconds;
        }
        public bool Hide(int order) { if(Caught || order!=HideOrder)return false; HideOrder++;return true; }
        public void RestoreHideOrder(int order) { HideOrder=order; }
        public void RetryStealth(){Caught=false;StealthRemaining=10;}
        public void RetryAssignment(){AssignmentSeconds=0;}
        public bool Answer(int answer)
        {
            if(ExamAnswered>=Questions.Length || answer<0 || answer>1)return false;
            if(answer==0)ExamCorrect++; ExamAnswered++; return true;
        }
        public void ResetExam(){ExamAnswered=0;ExamCorrect=0;}
    }
}
