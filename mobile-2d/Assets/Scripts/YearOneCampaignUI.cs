using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class YearOneCampaignUI : MonoBehaviour
{
    [SerializeField] private YearOneCampaignController campaign;
    [SerializeField] private GameObject startPanel,studyPanel;
    [SerializeField] private TMP_Text selectedText,objective;
    [SerializeField] private Button newGame,continueGame,closeStudy;
    [SerializeField] private Button[] courses,work;
    [SerializeField] private GameObject dinnerPanel;
    [SerializeField] private Button[] dinnerChoices=new Button[0];
    public Action DinnerChosen;
    public bool WorkInProgress { get; private set; }
    public YearOneRuntimeUI Runtime {get;private set;}
    public bool IsModalOpen=>(Runtime!=null && Runtime.BlocksInput) || StudyVisible || WorkInProgress || (computerDesk!=null && computerDesk.ComputerVisible);
    private DeskActionModal computerDesk;
    [SerializeField] private GameObject dayCard;
    [SerializeField] private TMP_Text dayTitle;
    public void ConfigureDayCard(GameObject panel,TMP_Text title){dayCard=panel;dayTitle=title;}
    public void ShowDayCard(int day){dayTitle.text=YearOneSchedule.DayTitle(day);dayCard.SetActive(true);}

    public void HideDayCard(){if(dayCard!=null)dayCard.SetActive(false);}
    public float SelectedCourseProgress
    {
        get{if(selected==null || !Active)return 0;int done=0;foreach(YearOneSchedule.Work task in Enum.GetValues(typeof(YearOneSchedule.Work)))if(campaign.Progress.Has(selected.Activity(task)))done++;return done/5f;}
    }
    public void ConfigureDinner(GameObject panel,Button[] choices){dinnerPanel=panel;dinnerChoices=choices;}
    public void ShowDinner(){var label=dinnerPanel.transform.Find("Budget")?.GetComponent<TMP_Text>();if(label!=null)label.text="Cơm: 25.000đ • Hỗ trợ năm học v2: 120.000đ (dự phòng vé xe).";dinnerPanel.SetActive(true);}
    public void HideDinner(){if(dinnerPanel!=null)dinnerPanel.SetActive(false);}
    private YearOneSchedule.Course selected;
    public Action StartedNewGame,ResumedGame,GameRequested;
    public Action PauseRequested;
    public Func<YearOneSchedule.Course,YearOneSchedule.Work,bool> WorkRequested;
    public bool Active=>campaign!=null && campaign.Initialized;
    public bool StartupVisible=>startPanel!=null && startPanel.activeSelf;
    public bool StudyVisible=>studyPanel!=null && studyPanel.activeSelf;
    public Button NewGameButton=>newGame;
    public Button ContinueButton=>continueGame;
    public Button[] CourseButtons=>courses;
    public Button[] WorkButtons=>work;
    public Button[] DinnerButtons=>dinnerChoices;
    public bool DinnerVisible=>dinnerPanel!=null && dinnerPanel.activeSelf;
    public void SetObjectiveVisible(bool value){if(objective!=null)objective.transform.parent.gameObject.SetActive(value);}
    public void Configure(YearOneCampaignController controller,GameObject welcome,GameObject study,TMP_Text detail,TMP_Text hud,
        Button fresh,Button resume,Button back,Button[] courseButtons,Button[] taskButtons)
    {campaign=controller;startPanel=welcome;studyPanel=study;selectedText=detail;objective=hud;newGame=fresh;continueGame=resume;closeStudy=back;courses=courseButtons;work=taskButtons;}
    private void Awake()
    {
        computerDesk=FindAnyObjectByType<DeskActionModal>();
        Runtime=gameObject.AddComponent<YearOneRuntimeUI>();Runtime.Configure(campaign,this,selectedText.font);
        newGame.onClick.AddListener(()=>{if(campaign.BeginNewGame()){HideStartup();StartedNewGame?.Invoke();}else RefreshWelcome();});
        continueGame.onClick.AddListener(()=>{if(campaign.ContinueGame()){HideStartup();ResumedGame?.Invoke();}else RefreshWelcome();});
        closeStudy.onClick.AddListener(()=>{HideStudy();if(computerDesk!=null && computerDesk.IsOpen)computerDesk.UseComputer();});
        foreach(var choice in dinnerChoices)choice.onClick.AddListener(()=>DinnerChosen?.Invoke());
        HideDinner();
        HideDayCard();
        for(int i=0;i<courses.Length;i++){int index=i;courses[i].onClick.AddListener(()=>SelectCourse(index));}
        for(int i=0;i<work.Length;i++)
        {var activity=(YearOneSchedule.Work)((int)YearOneSchedule.Work.Homework+i);work[i].onClick.AddListener(()=>RequestWork(activity));}
        campaign.Changed+=Refresh;studyPanel.SetActive(false);startPanel.SetActive(false);
    }
#if UNITY_EDITOR
    public void RestoreDeveloperCheckpoint()
    {
        HideStartup();HideStudy();HideDinner();HideDayCard();ResumedGame?.Invoke();
    }
#endif
    private void RefreshWelcome()
    {
        var text=startPanel.transform.Find("Summary")?.GetComponent<TMP_Text>();
        if(text!=null){text.fontSize=20;text.text="10 ngày đại diện • 6 môn • 32 tín chỉ mục tiêu\nLựa chọn được lưu; ngủ không bắt buộc PASS."+(campaign.HasLegacyCheckpoint?"\nSave v1 được giữ nguyên; bản v2 cần New Game.":"")+(string.IsNullOrEmpty(campaign.SaveError)?"":"\n"+campaign.SaveError);}
    }
    public void ShowStartup(){continueGame.interactable=campaign.HasCheckpoint;RefreshWelcome();startPanel.SetActive(true);campaign.MenuPausesClock=true;}
    private void HideStartup(){startPanel.SetActive(false);campaign.MenuPausesClock=false;}
    public void ShowStudy(){if(!Active)return;studyPanel.SetActive(true);campaign.MenuPausesClock=true;if(selected==null)SelectCourse(0);else Refresh();}
    public void HideStudy(){WorkInProgress=false;if(studyPanel!=null)studyPanel.SetActive(false);if(campaign!=null)campaign.MenuPausesClock=false;}
    public void RequestGame(){if(Active)GameRequested?.Invoke();}
    private void SelectCourse(int index){selected=YearOneSchedule.Courses[index];Refresh();}
    private void RequestWork(YearOneSchedule.Work activity)
    {
        if(selected==null || activity==YearOneSchedule.Work.Exam || !campaign.Progress.CanComplete(selected,activity))return;
        if(WorkRequested!=null && WorkRequested(selected,activity)){WorkInProgress=true;studyPanel.SetActive(false);campaign.MenuPausesClock=false;}
    }
    public void WorkFinished(){WorkInProgress=false;ShowStudy();}
    public void SetObjective(string text){if(objective!=null)objective.text=text;}
    public void Refresh()
    {
        if(!Active)return;
        for(int i=0;i<courses.Length;i++)
        {
            var c=YearOneSchedule.Courses[i];int lessons=(campaign.Progress.Has(c.Activity(YearOneSchedule.Work.Lesson1))?1:0)+(campaign.Progress.Has(c.Activity(YearOneSchedule.Work.Lesson2))?1:0);
            courses[i].GetComponentInChildren<TMP_Text>().text=c.Name+" • "+lessons+"/2 buổi";
        }
        if(selected==null)return;
        var result=campaign.Progress.Result(selected.Id);
        string report=selected.Name+" • "+selected.Credits+" TC\nVắng: "+campaign.Progress.Absences(selected)+"/2 • "+(result==null?"Chưa kết luận":result.Passed?"PASS":"FAIL: "+result.Reason)+"\n";
        for(int i=0;i<work.Length;i++)
        {
            var task=(YearOneSchedule.Work)((int)YearOneSchedule.Work.Homework+i);
            work[i].interactable=task!=YearOneSchedule.Work.Exam && campaign.Progress.CanComplete(selected,task);
            report+=(task==YearOneSchedule.Work.Homework?"Bài tập":task==YearOneSchedule.Work.Revision?"Ôn thi":"Kiểm tra")+": "+
                (campaign.Progress.Has(selected.Activity(task))?"Đã xong":work[i].interactable?"Có thể làm":task==YearOneSchedule.Work.Homework && campaign.Progress.Has("overdue:"+selected.Id)?"Quá hạn":"Chưa mở")+"\n";
        }
        if(campaign.Progress.day<selected.FirstDay)report+="Môn này bắt đầu từ ngày "+selected.FirstDay+".";
        else report+="Thi tại trường theo lịch. Vắng tối đa 1/2 buổi và nộp bài trước hạn để được dự thi.\nDeadline: ngày lịch "+(int)(YearOneSchedule.Deadline(selected)/1440)+" • 22:00";
        selectedText.text=report;
    }
    private void OnDestroy(){if(campaign!=null)campaign.Changed-=Refresh;}
    private void Update()
    {
        if(Active && (computerDesk==null || computerDesk.Desktop==null || !computerDesk.Desktop.EscapeHandledThisFrame) && !StartupVisible && !IsModalOpen && !DinnerVisible && UnityEngine.InputSystem.Keyboard.current!=null &&
            UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)PauseRequested?.Invoke();
    }
}
