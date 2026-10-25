using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Runtime-built uGUI desktop: uses the existing Canvas, font and event system.
public sealed class ComputerDesktopUI : MonoBehaviour
{
    private DeskActionModal desk;
    private YearOneCampaignController campaign;
    private YearOneCampaignUI academic;
    private GameObject window, games, schedule, activity;
    private TMP_Text clock, summary, session;
    private TMP_Text[] rows;
    private Button[] makeup;
    private TMP_FontAsset font;
    private bool todayOnly=true;
    private int selectedSemester=1;
    private GameObject semesterTabs;
    public Button SemesterButton {get;private set;}
    private int escapeFrame=-1;
    public bool EscapeHandledThisFrame => escapeFrame==Time.frameCount;
    public bool InRoomSession => !Visible && (campaign.IsComputerGaming || campaign.Computer==YearOneCampaignController.ComputerState.Makeup);
    public bool Visible => window != null && window.activeSelf;
    public Button[] GameButtons { get; private set; }
    public Button ScheduleButton { get; private set; }
    public Button StopButton { get; private set; }
    public Button CloseButton { get; private set; }
    public void Initialize(DeskActionModal owner, YearOneCampaignController controller, YearOneCampaignUI ui, TMP_FontAsset typeface)
    {
        desk=owner;campaign=controller;academic=ui;font=typeface;
        var canvas=owner.GetComponentInParent<Canvas>();
        var overlay=Box(canvas.transform,"ComputerOverlay",Vector2.zero,Vector2.one,new Color(.015f,.025f,.045f,1));window=overlay.gameObject;
        var desktopCanvas=window.AddComponent<Canvas>();desktopCanvas.overrideSorting=true;desktopCanvas.sortingLayerID=canvas.sortingLayerID;desktopCanvas.sortingOrder=1000;window.AddComponent<GraphicRaycaster>();
        var root=Box(overlay,"ComputerDesktop",new Vector2(.04f,.07f),new Vector2(.96f,.93f),new Color(.025f,.07f,.15f,1));
        Box(root,"Wallpaper",new Vector2(.2f,.08f),new Vector2(1,.9f),new Color(.04f,.2f,.35f));
        Label(root,"WINDOWS  •  Phòng 404",new Vector2(.025f,.91f),new Vector2(.55f,.985f),24);
        clock=Label(root,"",new Vector2(.52f,.91f),new Vector2(.71f,.985f),20);
        Button(root,"Về phòng",new Vector2(.71f,.91f),new Vector2(.87f,.985f),HideWindow);
        CloseButton=Button(root,"Tắt máy  ×",new Vector2(.875f,.91f),new Vector2(.99f,.985f),Close);
        Label(root,"ỨNG DỤNG",new Vector2(.02f,.78f),new Vector2(.19f,.87f),17);
        Button(root,"GAME",new Vector2(.02f,.64f),new Vector2(.18f,.76f),()=>ShowTab(false));
        ScheduleButton=Button(root,"LỊCH HỌC",new Vector2(.02f,.49f),new Vector2(.18f,.61f),()=>ShowTab(true));
        Label(root,"Sinh hoạt x1\nChơi game x4\n\nWindows tạm dừng\nđồng hồ.\n\nMỗi môn PASS\ncấp tín chỉ một lần.",new Vector2(.025f,.13f),new Vector2(.185f,.46f),17);
        Label(root,"● Start     |     Máy tính cá nhân  •  Năm 1",new Vector2(.02f,0),new Vector2(.85f,.075f),18);
        games=Box(root,"Games",new Vector2(.22f,.1f),new Vector2(.98f,.89f),Color.clear).gameObject;
        Label(games.transform,"GAME LIBRARY",new Vector2(.04f,.82f),new Vector2(.96f,.98f),30);
        GameButtons=new Button[4];
        for(int i=0;i<4;i++)
        {
            string game=YearOneCampaignController.ComputerGames[i];int col=i%2,row=i/2;
            GameButtons[i]=Button(games.transform,game+"\nChơi ngay",new Vector2(.04f+col*.48f,.45f-row*.34f),new Vector2(.48f+col*.48f,.75f-row*.34f),()=>Play(game));
        }
        schedule=Box(root,"Schedule",new Vector2(.22f,.1f),new Vector2(.98f,.89f),Color.clear).gameObject;
        summary=Label(schedule.transform,"",new Vector2(.03f,.77f),new Vector2(.97f,.99f),21);
        Button(schedule.transform,"Hôm nay",new Vector2(.03f,.70f),new Vector2(.48f,.78f),()=>{todayOnly=true;RefreshSchedule();});
        SemesterButton=Button(schedule.transform,"Học kỳ",new Vector2(.52f,.70f),new Vector2(.97f,.78f),()=>{todayOnly=false;selectedSemester=campaign.Progress.Milestone.Semester;RefreshSchedule();});
        semesterTabs=Box(schedule.transform,"SemesterTabs",new Vector2(.03f,.62f),new Vector2(.97f,.69f),Color.clear).gameObject;
        Button(semesterTabs.transform,"HK1",Vector2.zero,new Vector2(.48f,1),()=>{selectedSemester=1;RefreshSchedule();});
        Button(semesterTabs.transform,"HK2",new Vector2(.52f,0),Vector2.one,()=>{selectedSemester=2;RefreshSchedule();});
        rows=new TMP_Text[YearOneSchedule.Courses.Length];makeup=new Button[rows.Length];
        for(int i=0;i<rows.Length;i++)
        {
            int index=i;float top=.68f-i*.085f;
            rows[i]=Label(schedule.transform,"",new Vector2(.03f,top-.09f),new Vector2(.77f,top),16);
            makeup[i]=Button(schedule.transform,"Học bù",new Vector2(.79f,top-.08f),new Vector2(.97f,top-.01f),()=>StartMakeup(index));
        }
        Button(schedule.transform,"Bài tập / Ôn thi / Kiểm tra",new Vector2(.03f,.025f),new Vector2(.97f,.14f),()=>{Close();academic.ShowStudy();});
        activity=Box(root,"Activity",new Vector2(.22f,.1f),new Vector2(.98f,.89f),new Color(.03f,.08f,.12f)).gameObject;
        session=Label(activity.transform,"",new Vector2(.07f,.25f),new Vector2(.93f,.95f),24);
        StopButton=Button(activity.transform,"Thoát hoạt động • Về Windows",new Vector2(.16f,.06f),new Vector2(.84f,.2f),()=>{campaign.CancelComputerActivity();ShowTab(false);});
        window.SetActive(false);
    }
    public bool Open()
    {
        if(campaign.Computer==YearOneCampaignController.ComputerState.Off && !campaign.OpenComputer(desk))return false;
        window.SetActive(true);window.transform.SetAsLastSibling();clock.text=campaign.Progress.TimeOfDay+"  "+campaign.Progress.Clock;ShowTab(false);return true;
    }
    public void HideWindow(){if(window!=null)window.SetActive(false);}
    public void Close(){if(campaign!=null)campaign.CloseComputer();if(desk!=null)desk.TurnOffComputer();if(window!=null)window.SetActive(false);}
    public void Play(string game)
    {
        if(!campaign.PlayComputerGame(game))return;
        games.SetActive(false);schedule.SetActive(false);activity.SetActive(true);HideWindow();
        // The authored Long scene listens to the same live game session.
        academic.RequestGame();
    }
    public void ShowTab(bool study)
    {

        games.SetActive(!study);schedule.SetActive(study);activity.SetActive(false);RefreshSchedule();
    }
    public void StartMakeup(int index)
    {
        var c=YearOneSchedule.Courses[index];var work=campaign.Progress.NeedsMakeup(c,YearOneSchedule.Work.Lesson1)?YearOneSchedule.Work.Lesson1:YearOneSchedule.Work.Lesson2;
        if(!campaign.BeginComputerMakeup(c.Id,work))return;
        games.SetActive(false);schedule.SetActive(false);activity.SetActive(true);HideWindow();
    }
    private void RefreshSchedule()
    {
        var p=campaign.Progress;
        summary.text="LỊCH HỌC  •  "+YearOneSchedule.DayTitle(p.day)+"\nTín chỉ hoàn thành: "+p.Credits+" / 32    |    Môn FAIL: "+p.OwedCourses+"    |    Chưa đạt: "+p.RemainingCourses+" môn";
        semesterTabs.SetActive(!todayOnly);
        int visible=0;
        for(int i=0;i<rows.Length;i++)
        {
            var c=YearOneSchedule.Courses[i];bool show=todayOnly?Array.Exists(YearOneSchedule.Today(p.milestoneId),s=>s.CourseId==c.Id):c.Semester==selectedSemester;
            rows[i].gameObject.SetActive(show);makeup[i].gameObject.SetActive(show);if(!show)continue;
            float top=(todayOnly?.68f:.60f)-visible++*(todayOnly?.085f:.15f);
            rows[i].rectTransform.anchorMin=new Vector2(.03f,top-(todayOnly?.08f:.145f));rows[i].rectTransform.anchorMax=new Vector2(.77f,top);
            var rect=makeup[i].GetComponent<RectTransform>();rect.anchorMin=new Vector2(.79f,top-.07f);rect.anchorMax=new Vector2(.97f,top-.01f);
            var a=YearOneSchedule.SessionFor(c,YearOneSchedule.Work.Lesson1);var b=YearOneSchedule.SessionFor(c,YearOneSchedule.Work.Lesson2);
            var exam=YearOneSchedule.SessionFor(c,YearOneSchedule.Work.Exam);var result=p.Result(c.Id);
            bool owe=p.NeedsMakeup(c,YearOneSchedule.Work.Lesson1)||p.NeedsMakeup(c,YearOneSchedule.Work.Lesson2);
            string state=result==null?"Chưa kết luận":result.Passed?"PASS":"FAIL • "+result.Reason;
            string times=todayOnly?string.Join(" | ",Array.FindAll(YearOneSchedule.Today(p.milestoneId),s=>s.CourseId==c.Id).Select(s=>(s.Work==YearOneSchedule.Work.Exam?"Thi ":"")+YearOneRuntimeUI.Format(s.Start)+"–"+YearOneRuntimeUI.Format(s.End)+" "+(s.Work==YearOneSchedule.Work.Exam?(p.Result(c.Id)?.Reason??(p.Eligible(c)?"Đủ ĐK":"Chưa đủ ĐK")):LessonStatus(p,c,s.Work)))):"B1 "+YearOneRuntimeUI.Format(a.Start)+"–"+YearOneRuntimeUI.Format(a.End)+" "+LessonStatus(p,c,a.Work)+" | B2 "+YearOneRuntimeUI.Format(b.Start)+"–"+YearOneRuntimeUI.Format(b.End)+" "+LessonStatus(p,c,b.Work);
            rows[i].fontSize=todayOnly?16:14;
            rows[i].text=c.Name+" • "+c.Credits+" TC • "+c.RoomName+" • "+state+"\n"+times+" • Vắng "+p.Absences(c)+"/2 • Bài: "+(p.Has(c.Activity(YearOneSchedule.Work.Homework))?"đã nộp":p.Has("overdue:"+c.Id)?"quá hạn":"chưa nộp");
            if(!todayOnly)
            {
                string homework=p.Has(c.Activity(YearOneSchedule.Work.Homework))?"đã nộp":p.Has("overdue:"+c.Id)?"quá hạn":"chưa nộp";
                rows[i].text=c.Name+" • "+c.Credits+" TC • "+c.RoomName+" • "+state+"\n"+
                    "Ngày "+c.FirstDay+" "+YearOneRuntimeUI.Format(a.Start)+"–"+YearOneRuntimeUI.Format(a.End)+" "+LessonStatus(p,c,a.Work)+" | Ngày "+c.SecondDay+" "+YearOneRuntimeUI.Format(b.Start)+"–"+YearOneRuntimeUI.Format(b.End)+" "+LessonStatus(p,c,b.Work)+"\n"+
                    "Bài: "+homework+" • Hạn ngày lịch "+(int)(YearOneSchedule.Deadline(c)/1440)+" 22:00 • Ôn: "+(p.Has(c.Activity(YearOneSchedule.Work.Revision))?"đã ôn":"chưa ôn")+"\n"+
                    "Thi ngày lịch "+(int)(exam.Start/1440)+" "+YearOneRuntimeUI.Format(exam.Start)+"–"+YearOneRuntimeUI.Format(exam.End)+" • Vắng "+p.Absences(c)+"/2";
            }
            makeup[i].interactable=owe && campaign.Computer==YearOneCampaignController.ComputerState.Desktop;
        }
        foreach(var button in GameButtons)button.interactable=campaign.Computer==YearOneCampaignController.ComputerState.Desktop;
    }
    private static string LessonStatus(YearOneProgress p,YearOneSchedule.Course c,YearOneSchedule.Work work)
    {
        if(p.Has("attendance:"+c.Activity(work)))return p.Has("late:"+c.Activity(work))?"Đi trễ":"Đã học";
        if(p.Missed(c,work))return p.Has("makeup:"+c.Activity(work))?"Vắng (đã học bù)":"Vắng";
        var s=YearOneSchedule.SessionFor(c,work);return p.absoluteMinutes<s.Start?"Sắp học":p.absoluteMinutes>s.Start+15?"Trễ":"Trong giờ";
    }
    private void Update()
    {
        if(!Visible)return;
        if(!desk.IsOpen || campaign.Computer==YearOneCampaignController.ComputerState.Off){Close();return;}
        var p=campaign.Progress;clock.text=p.TimeOfDay+"  "+p.Clock;
        if(activity.activeSelf)
        {
            bool playing=campaign.IsComputerGaming;
            if(!playing && campaign.Computer!=YearOneCampaignController.ComputerState.Makeup){ShowTab(true);return;}
            session.text=playing?"ĐANG CHƠI  •  "+campaign.ComputerGame+"\n\nThời gian thực: "+campaign.ComputerSessionSeconds.ToString("0")+" giây\n"+p.TimeOfDay+" • "+p.Clock+"\n\nMôn đang nợ: "+p.OwedCourses+"\nVắng học chặn bài tập, ôn và thi.\nThoát sớm để kịp lịch; học bù trong LỊCH HỌC.":"ĐANG HỌC BÙ\n\n"+campaign.MakeupSeconds.ToString("0")+" / "+YearOneCampaignController.MakeupDuration+" giây\n\nHoàn tất mới được tính buổi học.\nHủy giữa chừng sẽ giữ nguyên môn nợ.";
        }
        else if(schedule.activeSelf)RefreshSchedule();
        if(Keyboard.current!=null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {escapeFrame=Time.frameCount;if(activity.activeSelf){campaign.CancelComputerActivity();ShowTab(false);}else HideWindow();}
    }
    private RectTransform Box(Transform parent,string name,Vector2 min,Vector2 max,Color color)
    {
        var go=new GameObject(name,typeof(RectTransform),typeof(CanvasRenderer),typeof(Image));var r=go.GetComponent<RectTransform>();r.SetParent(parent,false);r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;go.GetComponent<Image>().color=color;return r;
    }
    private TMP_Text Label(Transform parent,string text,Vector2 min,Vector2 max,int size)
    {
        var go=new GameObject("Text",typeof(RectTransform),typeof(TextMeshProUGUI));var r=go.GetComponent<RectTransform>();r.SetParent(parent,false);r.anchorMin=min;r.anchorMax=max;r.offsetMin=new Vector2(6,3);r.offsetMax=new Vector2(-6,-3);
        var label=go.GetComponent<TextMeshProUGUI>();label.font=font;label.text=text;label.fontSize=size;label.color=Color.white;label.alignment=TextAlignmentOptions.MidlineLeft;label.raycastTarget=false;return label;
    }
    private Button Button(Transform parent,string text,Vector2 min,Vector2 max,UnityEngine.Events.UnityAction action)
    {var r=Box(parent,text,min,max,new Color(.12f,.3f,.45f));var b=r.gameObject.AddComponent<Button>();b.targetGraphic=r.GetComponent<Image>();b.onClick.AddListener(action);Label(r,text,Vector2.zero,Vector2.one,20).alignment=TextAlignmentOptions.Center;return b;}
    private void OnDestroy(){if(window!=null)Destroy(window);}
}
