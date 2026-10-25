using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;

public sealed partial class ProductionOpeningController
{
    public float morningDepartureSeconds=120,queueWaitSeconds=8;
    public GameObject[] queueStudents;
    public SpriteRenderer feastPot,storedCooker;
    public Sprite[] feastPotFrames;
    public GameObject feastCord,feastIngredients;
    public GameObject[] feastActors;
    private ProductionYearOneCurriculum yearOneCurriculum;
    private ProductionMorningClock morningClock;
    private bool morningEntered,queueReady,feastHidPlayer,feastPreviousVisible,clockFailed;
    private float queueElapsed;
    private bool clockWasPaused;
    public float MorningRemaining=>morningClock?.Remaining??morningDepartureSeconds;
    public bool MorningMissed=>clockFailed || morningClock!=null && morningClock.Missed;
    public bool QueueReady=>queueReady;
    public ProductionYearOneCurriculum YearOneCurriculum=>yearOneCurriculum;
    private void SetupYearOne()=>yearOneCurriculum=ProductionYearOneCurriculum.Load();
    private void ResetYearOne()
    {
        FlushMorningClock();morningClock=null;morningEntered=false;queueReady=false;queueElapsed=0;clockFailed=false;
        RestoreFeastActor();
    }
    private void EnterSecondMorning()
    {
        if(!Ready || Progress.CurrentMission!=7 || morningEntered)return;
        morningEntered=true;schoolHall=false;schoolLobby=false;schoolReturnStop=false;boundSchoolTrip=null;
        try{morningClock=new ProductionMorningClock(saveDirectory??Path.Combine(Application.persistentDataPath,"production"),definition.text,Progress.ExportEvents(),morningDepartureSeconds);}
        catch(Exception error){clockFailed=true;Notify(error.Message);}
    }
    private void FlushMorningClock()
    {
        if(morningClock!=null && !morningClock.Flush(out string error)){clockFailed=true;Notify("Chưa lưu được đồng hồ buổi sáng: "+error);}
    }
    private int YearOneZone(int fallback)
    {
        EnterSecondMorning();
        if(Progress.CurrentMission==7)
        {
            if(Step=="join_wc_queue")return schoolHall?2:3;
            if(Step=="catch_bus")return schoolLobby?5:2;
            return Step=="attend_class"?6:7;
        }
        return fallback;
    }
    private string YearOneGuide()
    {
        if(Progress.CurrentMission==8)return Step=="get_cooker"?"32 tín chỉ đã được xác nhận. Tới góc để bếp và nồi.":Step=="plug_cooker"?"Đặt nồi xuống sàn, tới đầu dây bên phải và giữ nút để cắm bếp vào ổ tường.":Step=="add_ingredients"?"Tộ và Long góp đồ ăn. Tới nồi để thêm nguyên liệu.":Step=="share_feast"?"Cả nhóm ngồi quanh nồi lẩu. Tới giữa phòng để cùng ăn.":Step=="toast"?"Nâng ly chúc mừng anh em qua năm nhất.":"Tới giường đã chọn, ngủ rồi chuẩn bị đăng ký học kỳ 3.";
        if(Progress.CurrentMission==9)return "Học kỳ 3 • 08:59. Tới laptop trên bàn để mở cổng đăng ký môn.";
        if(Progress.CurrentMission==10)return "Một tối 23:40 • Long rủ chơi thêm. Sáng mai vẫn có lớp 06:45.";
        if(Progress.CurrentMission!=7)return null;
        if(clockFailed)return "Checkpoint đồng hồ cần phục hồi. Mở menu tải lại; file hiện có được giữ nguyên.";
        if(Step=="join_wc_queue")return activeZone==3?"Một buổi sáng khác • 07:20. Ra hành lang, WC đang có người chờ.":queueReady?"Đã tới lượt. Đi vào WC Nam rồi xuống trạm 33.":"Tới cuối hàng bên trái WC Nam. Xe buýt không đợi mãi.";
        if(Step=="catch_bus")return activeZone==2?"Tới cầu thang để xuống trạm 33.":MorningMissed?"Đã lỡ chuyến trước. Đợi chuyến xe 33 tiếp theo; vé vẫn 7.000đ.":"Tới trạm 33 trước khi hết giờ, đợi xe dừng rồi lên xe.";
        if(Step=="attend_class")return "Tới cửa lớp. Thầy sẽ điểm danh sau khi Lươn tới H6.";
        return yearOneCurriculum.ForStep(Step)!=null?"Tới bàn học: hoàn thành bài học hoặc bài thi hiện tại. Có thể thử lại hoặc đóng câu hỏi.":"Tới bàn để xem lịch, sổ vé xe và kết quả các môn năm nhất.";
    }
    private string YearOneDescription()
    {
        if(Progress.CurrentMission==8)
        {
            switch(Step)
            {
                case "get_cooker":return "Tộ: Cả nhóm qua năm nhất rồi, lấy bếp và nồi ra làm bữa lẩu đi!\nLươn lấy đúng bộ bếp/nồi ở góc phòng, đặt quạt sang bên để chừa chỗ ngồi.";
                case "plug_cooker":return "Đặt bếp trên nền phẳng, kiểm tra dây rồi cắm vào ổ điện.\nGiữ nút hoặc Space; thả ra sẽ dừng thao tác.";
                case "add_ingredients":return "Tộ mang thịt trâu khô, Long góp rau và mì. Đạm đem thêm nước uống.\nCho nguyên liệu vào nồi; đồ ăn của bữa này do cả nhóm góp, không trừ tiền ví lần nữa.";
                case "share_feast":return dialogueLine==0?"Tộ: Đồ quê tui đem lên, anh em ăn chung cho vui!":dialogueLine==1?"Long: Mỗi người một trường một lịch, mà về404 là có nhau. Năm nhất qua rồi!":"Đạm: Tui học HCMUS, mấy ông Bách Khoa. Cứ tối về lại kể nhau nghe. Ăn đi, còn nóng!";
                case "toast":return dialogueLine==0?"Lươn: Chúc anh em404 qua năm nhất, năm sau vẫn giúp nhau học nhé!":"Cả nhóm nâng ly nước: Một, hai, ba… dô!\nBữa lẩu kết thúc; sáng mai chuẩn bị đăng ký môn học kỳ 3.";
                case "sleep_new_term":return "Lươn cất bếp và đồ ăn, về đúng giường của mình.\nSau một đêm, điện thoại nhắc: Cổng đăng ký môn học kỳ 3 mở lúc09:00.";
            }
        }
        if(Progress.CurrentMission!=7)return null;
        if(Step=="join_wc_queue")return "WC Nam đang đông. Đứng cuối hàng, chờ từng bạn ra rồi mới vào rửa mặt.\nGiờ hiện tại07:20; còn thời gian đón xe nhưng đừng chần chừ.";
        if(Step=="attend_class")return Progress.Has("travel:missed_bus_recovered")?"Thầy: Đi chuyến sau vẫn tới lớp rồi. Vào chỗ và theo bài nhé!\nBạn cùng lớp: Tui giữ vở giúp ông. Lần sau nhớ canh giờ WC.":"Thầy: Lớp điểm danh nhé. Lươn đến đúng giờ rồi, vào chỗ nào!\nBạn cùng lớp: Ngồi cạnh tui, hôm nay học tiếp.";
        if(Step=="review_commute_ledger")return "SỔ VÉ XE • 6 NGÀY ĐẠI DIỆN\nĐã trả: 3 lượt × 7.000đ = 21.000đ\nTrích quỹ cho 9 lượt còn lại: 63.000đ\nCả năm: 12 lượt = 84.000đ; quỹ này lo phần đi lại của các buổi còn lại.\n\nHai học kỳ tiếp tục qua các buổi học và kỳ thi. Mỗi môn có bài học và bài kiểm tra riêng; mốc thi được tự lưu.";
        var course=yearOneCurriculum.ForStep(Step);
        if(course!=null)
        {
            int index=Array.FindIndex(yearOneCurriculum.courses,c=>c==course);bool exam=Step.StartsWith("exam_y1_",StringComparison.Ordinal);
            return (index==3 && !exam?"Thời gian trôi qua • Học kỳ 2 bắt đầu.\n":"")+"Học kỳ "+(index<3?"1":"2")+" • Chặng "+(index+1)+"/6\n"+(exam?"KỲ THI • ":"BUỔI HỌC • ")+course.name+" • "+course.credits+" tín chỉ.\n"+(exam?"Dựa trên phần đã ôn, trả lời câu hỏi. Sai hoặc hết giờ có thể thử lại; đóng câu hỏi chưa ghi kết quả.":"Buổi học đại diện: ghi bài, làm câu luyện tập rồi chuẩn bị kỳ thi của môn này.");
        }
        if(Step=="finish_year_exams" || Step=="read_year_results")
        {
            var text=new StringBuilder(Step=="finish_year_exams"?"KẾT THÚC CÁC KỲ THI\n":"KẾT QUẢ NĂM 1 • KHÔNG NỢ MÔN\n");
            foreach(var c in yearOneCurriculum.courses)text.Append(c.name).Append(" • ").Append(c.credits).Append(" TC • ").Append(Progress.Has("passed:Y1_"+c.id)?"ĐẠT":"CHƯA ĐẠT").Append('\n');
            text.Append("Tổng: ").Append(Progress.Credits).Append(" / 150 • Vé xe: 12 lượt / 84.000đ đã đối chiếu.");
            if(Step=="read_year_results")text.Append("\nNhóm 404 rủ về nấu lẩu ăn mừng.");
            return text.ToString();
        }
        return null;
    }
    private void OpenYearOne()
    {
        if(Progress.CurrentMission==7 && (Step=="review_commute_ledger" || Step=="finish_year_exams" || Step=="read_year_results"))
        {body.fontSize=20;body.rectTransform.sizeDelta=new Vector2(950,260);body.rectTransform.anchoredPosition=new Vector2(0,35);}
        if(Progress.CurrentMission==8 && (Step=="share_feast" || Step=="toast"))
        {var r=player.CharacterVisual.GetComponent<SpriteRenderer>();feastPreviousVisible=r.enabled;r.enabled=false;feastHidPlayer=true;RefreshFeast();}
    }
    private bool CanCommitYearOne()=>Progress.CurrentMission!=7 || Step!="join_wc_queue" && Step!="attend_class" && Step!="read_year_results" && yearOneCurriculum.ForStep(Step)==null;
    private bool HandleYearOneConfirm()
    {
        if(Progress.CurrentMission==7)
        {
            if(Step=="join_wc_queue"){StartCoroutine(WaitSchoolQueue());return true;}
            if(Step=="attend_class"){StartCoroutine(EnterSecondClass());return true;}
            var course=yearOneCurriculum.ForStep(Step);
            if(course!=null)
            {
                string id=Step;bool exam=id.StartsWith("exam_y1_",StringComparison.Ordinal);
                schoolPuzzle.SetExercise(course.name+(exam?" • KỲ THI":" • BUỔI HỌC"),(exam?course.exam:course.study).ToExercise());
                modal.SetActive(false);schoolPreviousVisible=player.CharacterVisual.GetComponent<SpriteRenderer>().enabled;
                player.CharacterVisual.GetComponent<SpriteRenderer>().enabled=false;schoolHidPlayer=true;schoolStudyPose.enabled=true;
                schoolPuzzle.Open(()=>{bool saved=session.Commit(7,id,"",out string error);Close();RefreshStage(false);if(!saved)Notify(error);});return true;
            }
            if(Step=="read_year_results"){StartCoroutine(YearOneReturnHome());return true;}
        }
        if(Progress.CurrentMission==8)
        {
            if(Step=="share_feast" && dialogueLine++<2 || Step=="toast" && dialogueLine++==0){body.text=YearOneDescription();return true;}
            if(Step=="sleep_new_term"){StartCoroutine(SleepIntoRegistration());return true;}
        }
        return false;
    }
    private IEnumerator WaitSchoolQueue()
    {
        travelling=true;modal.SetActive(false);queueElapsed=0;
        while(queueElapsed<queueWaitSeconds)
        {if(!appPaused)queueElapsed+=Time.deltaTime;Notify("Đang xếp hàng WC • Còn "+Mathf.CeilToInt(Mathf.Max(0,queueWaitSeconds-queueElapsed))+" giây");RefreshQueue();yield return null;}
        queueReady=true;travelling=false;Close();RefreshSchool();RefreshQueue();Notify("Đã tới lượt Lươn. Tới cửa WC Nam để rửa mặt.");
    }
    private IEnumerator EnterSecondClass()
    {
        travelling=true;modal.SetActive(false);yield return FadeTo(1);
        bool saved=session.Commit(7,"attend_class","",out string error);RefreshStage(true);
        yield return FadeTo(0);travelling=false;Close();RefreshSchool();if(!saved)Notify(error);
    }
    private IEnumerator YearOneReturnHome()
    {
        travelling=true;modal.SetActive(false);yield return FadeTo(1);
        Notify("Kỳ thi đã xong • Lươn dùng lượt xe đã dự trù, về KTX cùng nhóm404.");yield return new WaitForSeconds(.8f);
        bool saved=session.Commit(7,"read_year_results","",out string error);RefreshStage(true);
        yield return FadeTo(0);travelling=false;Close();if(!saved)Notify(error);
    }
    private IEnumerator SleepIntoRegistration()
    {
        travelling=true;modal.SetActive(false);yield return FadeTo(1);yield return new WaitForSeconds(.8f);
        bool saved=session.Commit(8,"sleep_new_term","",out string error);RefreshStage(true);
        yield return FadeTo(0);travelling=false;Close();if(!saved)Notify(error);
    }
    private void RefreshQueue()
    {
        if(queueStudents==null)return;
        for(int i=0;i<queueStudents.Length;i++)if(queueStudents[i]!=null)queueStudents[i].SetActive(Ready && Progress.CurrentMission==7 && Step=="join_wc_queue" && activeZone==2 && !queueReady && queueElapsed<(i+1)*queueWaitSeconds/queueStudents.Length);
    }
    private void RestoreFeastActor()
    {
        var visual=player!=null?player.CharacterVisual:null;
        var renderer=visual!=null?visual.GetComponent<SpriteRenderer>():null;
        if(feastHidPlayer && renderer!=null)renderer.enabled=feastPreviousVisible;
        feastHidPlayer=false;
        if(feastActors!=null)foreach(var actor in feastActors)if(actor!=null)actor.SetActive(false);
    }
    private void CloseYearOne()
    {
        RestoreFeastActor();if(body!=null){body.fontSize=24;body.rectTransform.sizeDelta=new Vector2(950,175);body.rectTransform.anchoredPosition=new Vector2(0,60);}
        RefreshFeast();
    }
    private void RefreshFeast()
    {
        if(feastPot==null)return;
        bool feast=Ready && Progress.CurrentMission==8;
        bool got=Ready && Progress.Has("action:8:get_cooker"),plugged=Ready && Progress.Has("action:8:plug_cooker");
        if(fan!=null)fan.transform.localPosition=got?new Vector3(-2.6f,-1.5f,0):new Vector3(0,-1.5f,0);
        if(storedCooker!=null)storedCooker.enabled=feast && !got;feastPot.enabled=feast && got;if(feastCord!=null)feastCord.SetActive(feast && plugged);
        if(feastIngredients!=null)feastIngredients.SetActive(feast && Progress.Has("action:8:add_ingredients"));
        if(feastActors!=null)foreach(var actor in feastActors)if(actor!=null)actor.SetActive(feast && feastHidPlayer);
        if(feastHidPlayer && roomFriends!=null)foreach(var friend in roomFriends)if(friend!=null)friend.SetActive(false);
        if(feast && plugged && feastPotFrames.Length>0)feastPot.sprite=feastPotFrames[(int)(Time.time*3)%feastPotFrames.Length];
    }
    private void UpdateYearOne()
    {
        if(!Ready)return;
        bool morning=Progress.CurrentMission==7 && (Step=="join_wc_queue" || Step=="catch_bus");
        if(morning && morningClock!=null && !clockFailed)
        {
            bool paused=appPaused || startPanel.activeSelf || Step=="catch_bus" && activeZone==5 && schoolJourney.IsBusy;
            if(!paused && !morningClock.Tick(Time.deltaTime,out string error)){clockFailed=true;Notify(error);}
            int minutes=7*60+20+Mathf.FloorToInt((morningClock.Duration-morningClock.Remaining)/morningClock.Duration*20);
            hud.text="M07 • "+$"{minutes/60:00}:{minutes%60:00}"+" • "+(MorningMissed?"Chuyến trước đã đi • Có chuyến sau":"Xe: "+Mathf.CeilToInt(MorningRemaining)+"s")+"\nVí: "+Progress.Money.ToString("N0",System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))+"đ • Tín chỉ: "+Progress.Credits+"/150";
            if(paused && !clockWasPaused)FlushMorningClock();clockWasPaused=paused;
        }
        RefreshQueue();RefreshFeast();
    }
}
