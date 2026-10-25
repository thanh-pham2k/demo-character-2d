using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Shared in-world HUD, transition confirmation and persistent three-question exam.
public sealed class YearOneRuntimeUI : MonoBehaviour
{
    public static YearOneRuntimeUI Active {get;private set;}
    public static bool CapturesMovement=>Active!=null && (Active.BlocksInput || Active.year.DialogueVisible || Active.academic.StartupVisible || Active.academic.StudyVisible || (Active.desk!=null && Active.desk.ComputerVisible));
    YearOneCampaignController campaign;YearOneCampaignUI academic;DeskActionModal desk;YearOneMilestoneController year;
    GameObject root,modal,examPanel,gameBar,bedBar,noticePanel;TMP_Text hud,body,title,examText,notice,saveError;
    Button yes,no;Button[] answers;int displayedQuestion=-1;Action confirm,cancel;float noticeUntil;string lastExam;YearOneSchedule.Session reminderSession;
    public bool ModalVisible=>modal!=null && modal.activeSelf;
    public bool ExamVisible=>examPanel!=null && examPanel.activeSelf;
    public bool BlocksInput=>ModalVisible || ExamVisible;
    public string LastNotification {get;private set;}="";
    public Button SleepButton {get;private set;}
    public Button GameStopButton {get;private set;}
    public Button ConfirmButton=>yes;
    public Button CancelButton=>no;
    public Button[] ExamAnswers=>answers;
    public Action<YearOneTransition> SleepRequested;
    public Action ExamFinished;
    public void Configure(YearOneCampaignController c,YearOneCampaignUI ui,TMP_FontAsset font)
    {
        Active=this;campaign=c;academic=ui;desk=FindAnyObjectByType<DeskActionModal>();year=FindAnyObjectByType<YearOneMilestoneController>();
        var canvas=ui.GetComponentInParent<Canvas>();var overlay=Box(canvas.transform,"YearOneV2",Vector2.zero,Vector2.one,Color.clear);root=overlay.gameObject;overlay.GetComponent<Image>().raycastTarget=false;
        var nested=root.AddComponent<Canvas>();nested.overrideSorting=true;nested.sortingLayerID=canvas.sortingLayerID;nested.sortingOrder=1500;root.AddComponent<GraphicRaycaster>();
        Font=font;
        var top=Box(overlay,"WorldClock",new Vector2(.025f,.84f),new Vector2(.975f,.99f),new Color(.02f,.06f,.1f,.9f));
        hud=Text(top,"",Vector2.zero,Vector2.one,19);
        var err=Box(overlay,"SaveError",new Vector2(.02f,.78f),new Vector2(.98f,.84f),Color.clear);err.GetComponent<Image>().raycastTarget=false;saveError=Text(err,"",Vector2.zero,Vector2.one,17);
        gameBar=Box(overlay,"GameControls",new Vector2(.24f,.02f),new Vector2(.76f,.1f),new Color(.02f,.08f,.12f,.9f)).gameObject;
        GameStopButton=Button(gameBar.transform,"Thoát hoạt động",new Vector2(0,0),new Vector2(.49f,1),()=>campaign.CancelComputerActivity());
        Button(gameBar.transform,"Mở Windows",new Vector2(.51f,0),Vector2.one,()=>desk.UseComputer());
        bedBar=Box(overlay,"RestControls",new Vector2(.2f,.02f),new Vector2(.8f,.11f),new Color(.02f,.08f,.12f,.95f)).gameObject;
        SleepButton=Button(bedBar.transform,"Ngủ / Kết thúc ngày",Vector2.zero,new Vector2(.65f,1),RequestSleep);
        Button(bedBar.transform,"Rời giường [E]",new Vector2(.67f,0),Vector2.one,()=>{var bed=year.Player.ActiveInteractable as BedInteractable;if(bed!=null)bed.Interact(year.Player);});
        noticePanel=Box(overlay,"SchoolReminder",new Vector2(.15f,.61f),new Vector2(.85f,.81f),new Color(.25f,.13f,.025f,.98f)).gameObject;
        notice=Text(noticePanel.transform,"",new Vector2(.02f,.4f),new Vector2(.98f,.98f),22);
        Button(noticePanel.transform,"Thoát game",new Vector2(.03f,.03f),new Vector2(.48f,.37f),()=>campaign.StopComputerGame());
        Button(noticePanel.transform,"Chơi tiếp / Đã hiểu",new Vector2(.52f,.03f),new Vector2(.97f,.37f),()=>noticePanel.SetActive(false));
        modal=Box(overlay,"Confirmation",Vector2.zero,Vector2.one,new Color(.005f,.02f,.04f,.99f)).gameObject;
        title=Text(modal.transform,"",new Vector2(.08f,.8f),new Vector2(.92f,.96f),29);
        body=Text(modal.transform,"",new Vector2(.08f,.23f),new Vector2(.92f,.79f),21);
        yes=Button(modal.transform,"Tiếp tục",new Vector2(.52f,.08f),new Vector2(.9f,.19f),()=>{var action=confirm;modal.SetActive(false);confirm=null;action?.Invoke();});
        no=Button(modal.transform,"Quay lại",new Vector2(.1f,.08f),new Vector2(.48f,.19f),()=>{var action=cancel;modal.SetActive(false);cancel=null;action?.Invoke();});
        examPanel=Box(overlay,"Exam",Vector2.zero,Vector2.one,new Color(.015f,.04f,.08f,1)).gameObject;
        examText=Text(examPanel.transform,"",new Vector2(.07f,.42f),new Vector2(.93f,.96f),25);answers=new Button[3];
        for(int i=0;i<3;i++){int index=i;answers[i]=Button(examPanel.transform,"",new Vector2(.1f,.3f-i*.09f),new Vector2(.9f,.38f-i*.09f),()=>{if(campaign.Progress.exam!=null && campaign.Progress.exam.Question==displayedQuestion)campaign.AnswerCourseExam(index);});}
        Button(examPanel.transform,"Nộp bài / Thoát",new Vector2(.3f,.015f),new Vector2(.7f,.105f),ConfirmSubmit);
        modal.SetActive(false);examPanel.SetActive(false);noticePanel.SetActive(false);gameBar.SetActive(false);bedBar.SetActive(false);
    }
    TMP_FontAsset Font;
    public void Show(string heading,string message,Action accepted,Action rejected=null,string acceptLabel="Tiếp tục")
    {
        title.text=heading;body.text=message;confirm=accepted;cancel=rejected;
        yes.GetComponentInChildren<TMP_Text>().text=acceptLabel;no.gameObject.SetActive(rejected!=null);modal.transform.SetAsLastSibling();modal.SetActive(true);
    }
    public void RequestSleep()
    {
        if(!campaign.Progress.CanSleep)return;
        var preview=campaign.Progress.PreviewTransition();
        if(preview.To<=preview.From){Show("Cuối ngày","Hãy dự bữa lẩu trước khi sang Năm 2. Bạn vẫn có thể nằm nghỉ.",()=>{});return;}
        string destination=preview.Destination==preview.Source?"Cuối ngày • Chuẩn bị ăn lẩu":preview.ToYearTwo?"Năm 2":YearOneSchedule.ById(preview.Destination).Title;
        string warnings=preview.Warnings.Length==0?"Không có việc mới bị bỏ lỡ.":string.Join("\n",preview.Warnings);
        Show("Ngủ → "+destination,warnings+"\n\nLịch sử, môn nợ và tín chỉ sẽ được giữ nguyên.",()=>SleepRequested?.Invoke(preview),()=>{});
    }
    public void ConfirmSubmit()=>Show("Nộp bài thi?","Câu chưa trả lời tính là sai. Không thể thi lại trong lượt này.",()=>campaign.SubmitCourseExam(),()=>{},"Nộp bài");
    public void Notify(string text)
    {reminderSession=null;LastNotification=text;notice.text=text;noticeUntil=Time.unscaledTime+5;noticePanel.SetActive(true);}
    public void ShowYearTwo()
    {
        var snapshot=campaign.Progress.yearTwo;if(snapshot==null)return;
        Show("NĂM 2 • Kết quả bàn giao",$"Tín chỉ: {snapshot.Credits}/{snapshot.Target}\nHK1: {snapshot.Semester1Credits}/16 • HK2: {snapshot.Semester2Credits}/16\nMôn nợ:\n"+(snapshot.Debt.Length==0?"Không có":string.Join("\n",snapshot.Debt.Select(id=>YearOneSchedule.Find(id).Name+" • "+Array.Find(snapshot.Results,r=>r.CourseId==id).Reason)))+"\n\nHọc lại sẽ được triển khai ở giai đoạn sau.",()=>{});
    }
    void LateUpdate()
    {
        if(campaign==null || !campaign.Initialized || academic.StartupVisible){root.SetActive(false);return;}
        // root is separate from this component, so startup can hide it safely.
        root.SetActive(true);var p=campaign.Progress;
        hud.transform.parent.gameObject.SetActive(!academic.StudyVisible && !desk.ComputerVisible && !BlocksInput);
        saveError.text=string.IsNullOrEmpty(campaign.SaveError)?"":"Chưa lưu được: "+campaign.SaveError+" • Thao tác chuyển trạng thái chưa được xác nhận.";
        gameBar.SetActive((campaign.IsComputerGaming || campaign.Computer==YearOneCampaignController.ComputerState.Makeup) && !desk.ComputerVisible && !BlocksInput && !year.DialogueVisible);
        bedBar.SetActive(year.Player.CurrentState==PlayerInteractionState.Lying && !BlocksInput && !p.Has("year:two"));
        var next=p.NextSession;
        string activity=campaign.IsComputerGaming?"Đang chơi "+campaign.ComputerGame+" • Tăng tốc x4":campaign.Computer==YearOneCampaignController.ComputerState.Makeup?"Đang học bù • x1":year.Puzzle.IsOpen?"Trả lời câu hỏi • Tạm dừng":year.CurrentPhase==YearOneMilestoneController.Phase.Lecture?"Đang học x1":"Sinh hoạt x1";
        string nextText=next==null?"Hôm nay không còn tiết / kỳ thi phía trước.":YearOneSchedule.Find(next.CourseId).Name+" • "+YearOneSchedule.Find(next.CourseId).RoomName+" • "+Format(next.Start)+"–"+Format(next.End)+" • "+(p.Has("attendance:"+next.Id)?"Đã điểm danh":p.absoluteMinutes<next.Start?"Còn "+Math.Ceiling(next.Start-p.absoluteMinutes)+" phút":"Đang trong giờ • "+(p.absoluteMinutes>next.Start+15?"Trễ":"Có thể điểm danh"));
        if(!p.OpeningComplete)nextText=!p.Has("opening:claim")?"Tới giường và bấm [E] để xí giường":!p.Has("opening:friends")?"Làm quen các bạn cùng phòng":"Ra hành lang, xuống căn tin ăn bữa đầu";
        double eta=year.EstimateTravelMinutes();
        hud.text=YearOneSchedule.DayTitle(p.day)+" • "+p.Clock+" • "+p.TimeOfDay+" • "+activity+"\n"+nextText+(next==null?"":"\nƯớc tính đi tới lớp: "+Math.Ceiling(eta)+" phút • Nên xuất phát trước "+Format(year.RecommendedDeparture(next)));
        if(p.Milestone.MissableBus && !p.Has(p.TripId(false)))hud.text+="\nXe 33 chuyến đầu "+Format(p.Milestone.DepartureMinute)+" • "+(p.Has("day:4:missed")?"Đã lỡ • Có chuyến sau":"Còn "+Math.Ceiling(p.morningSecondsRemaining)+" phút game");
        if(p.ReadyForFeast && !p.Has("year:feast"))hud.text+="\nTới bếp lẩu [E] để ăn mừng kết thúc năm, bất kể tín chỉ.";
        if(p.Has("year:feast") && !p.Has("year:two"))hud.text+="\nTới giường → Ngủ để sang Năm 2.";
        if(p.Has("year:two"))hud.text="NĂM 2 • Tín chỉ mang theo: "+p.Credits+"/32 • Môn nợ: "+p.OwedCourses;
        if(noticePanel.activeSelf && Time.unscaledTime>=noticeUntil)noticePanel.SetActive(false);
        if(!academic.StartupVisible && !academic.StudyVisible && !desk.ComputerVisible && !BlocksInput && next!=null)
        {
            int threshold=p.PendingReminder(next);
            if(threshold>=0){p.MarkReminder(next,threshold);campaign.Save();Notify(ReminderText(next));reminderSession=next;}
        }
        if(noticePanel.activeSelf && reminderSession!=null){LastNotification=ReminderText(reminderSession);notice.text=LastNotification;}
        var current=p.exam;
        examPanel.SetActive(current!=null);
        if(current!=null)
        {
            displayedQuestion=current.Question;lastExam=current.CourseId;var c=YearOneSchedule.Find(current.CourseId);var exercise=c.ExerciseFor((YearOneSchedule.Work)(2+current.Question));
            examText.text=c.Name+" • THI • Câu "+(current.Question+1)+"/3 • "+Math.Ceiling(current.Seconds)+"s\n\n"+exercise.Question;
            for(int i=0;i<3;i++)answers[i].GetComponentInChildren<TMP_Text>().text=exercise.Choices[i];
        }
        else if(lastExam!=null)
        {
            var r=p.Result(lastExam);lastExam=null;ExamFinished?.Invoke();
            if(r!=null)Show(r.Passed?"PASS":"FAIL",YearOneSchedule.Find(r.CourseId).Name+" • "+r.Correct+"/3\n"+r.Reason+"\nTổng tín chỉ: "+p.Credits+"/32"+TermSummary(YearOneSchedule.Find(r.CourseId).Semester),()=>{});
        }
        if(current!=null && !ModalVisible && UnityEngine.InputSystem.Keyboard.current?.escapeKey.wasPressedThisFrame==true)ConfirmSubmit();
    }
    string ReminderText(YearOneSchedule.Session session)=>YearOneSchedule.Find(session.CourseId).Name+" • "+Format(session.Start)+"–"+Format(session.End)+"\n"+(campaign.Progress.absoluteMinutes<session.Start?"Còn "+Math.Ceiling(session.Start-campaign.Progress.absoluteMinutes)+" phút":"Đã tới giờ học")+" • Nên xuất phát trước "+Format(year.RecommendedDeparture(session));
    string TermSummary(int term)
    {
        var p=campaign.Progress;var courses=YearOneSchedule.Courses.Where(c=>c.Semester==term).ToArray();
        if(courses.Any(c=>p.Result(c.Id)==null))return "";
        return "\n\nTỔNG KẾT HK"+term+"\n"+string.Join("\n",courses.Select(c=>c.Name+": "+(p.Result(c.Id).Passed?"PASS • "+c.Credits+" TC":"FAIL • "+p.Result(c.Id).Reason)));
    }
    public static string Format(double minutes){int m=(int)Math.Max(0,minutes)%1440;return $"{m/60:00}:{m%60:00}";}
    RectTransform Box(Transform parent,string name,Vector2 min,Vector2 max,Color color)
    {var go=new GameObject(name,typeof(RectTransform),typeof(Image));var r=go.GetComponent<RectTransform>();r.SetParent(parent,false);r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;go.GetComponent<Image>().color=color;return r;}
    TMP_Text Text(Transform parent,string text,Vector2 min,Vector2 max,int size)
    {var go=new GameObject("Label",typeof(RectTransform),typeof(TextMeshProUGUI));var r=go.GetComponent<RectTransform>();r.SetParent(parent,false);r.anchorMin=min;r.anchorMax=max;r.offsetMin=new Vector2(12,4);r.offsetMax=new Vector2(-12,-4);var t=go.GetComponent<TextMeshProUGUI>();t.font=Font;t.text=text;t.fontSize=size;t.color=Color.white;t.alignment=TextAlignmentOptions.MidlineLeft;t.raycastTarget=false;return t;}
    Button Button(Transform parent,string label,Vector2 min,Vector2 max,UnityEngine.Events.UnityAction action)
    {var r=Box(parent,label,min,max,new Color(.12f,.29f,.4f,1));var b=r.gameObject.AddComponent<Button>();b.targetGraphic=r.GetComponent<Image>();Text(r,label,Vector2.zero,Vector2.one,20).alignment=TextAlignmentOptions.Center;b.onClick.AddListener(action);return b;}
    void OnDestroy(){if(Active==this)Active=null;if(root!=null)Destroy(root);}
}
