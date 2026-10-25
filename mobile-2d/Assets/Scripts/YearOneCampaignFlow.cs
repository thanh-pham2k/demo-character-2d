using System;
using System.Collections;
using UnityEngine;

public partial class YearOneMilestoneController
{
    [SerializeField] private YearOneCampaignController campaign;
    [SerializeField] private YearOneCampaignUI campaignUI;
    [SerializeField] private DeskActionModal deskMenu;
    [SerializeField] private RoommateActivityController longActor;
    [SerializeField] private ChairInteractable[] roomChairs;
    [SerializeField] private BedInteractable[] roomBeds;
    [SerializeField] private Sprite lectureArt,labArt,lectureForeground,labForeground;
    [SerializeField] private SpriteRenderer canteenBackdrop,dormCampusBackdrop;
    [SerializeField] private Transform canteenSpawn;
    private YearOneSchedule.Course activeCourse;
    private bool nightRunning,workRunning,gameRunning;
    private float gameSeconds;
    private bool CampaignActive=>campaign!=null && campaign.Initialized;
    public YearOneCampaignController Campaign=>campaign;
    public void ConfigureCampaignFlow(YearOneCampaignController controller,YearOneCampaignUI ui,DeskActionModal desk,
        RoommateActivityController companion,ChairInteractable[] chairs,BedInteractable[] beds,Sprite classroom,
        Sprite laboratory,Sprite classFront,Sprite labFront,SpriteRenderer canteen,Transform foodSpawn,SpriteRenderer campusArt)
    {
        campaign=controller;campaignUI=ui;deskMenu=desk;longActor=companion;roomChairs=chairs;roomBeds=beds;
        lectureArt=classroom;labArt=laboratory;lectureForeground=classFront;labForeground=labFront;
        canteenBackdrop=canteen;canteenSpawn=foodSpawn;dormCampusBackdrop=campusArt;
    }
    private IEnumerator Start()
    {
        if(campaign==null)yield break;
        yield return null; // MVP's opening initialization must finish first.
        story.BedClaimed+=OnCampaignClaim;story.RoomCleaned+=OnCampaignClean;story.RoommatesIntroduced+=OnCampaignFriends;
        story.CanCompleteYearOne=()=>!CampaignActive || campaign.Progress.Has("year:two");
        journey.CampaignCanTravel=()=>!CampaignActive || campaign.Progress.OpeningComplete;
        campaign.WorldPaused=()=>DialogueVisible || story.dialoguePanel.activeSelf || puzzle.IsOpen || AcademicVisible || nightRunning || campaignUI.Runtime.ModalVisible || CurrentPhase==Phase.Hotpot;
        campaignUI.Runtime.SleepRequested=preview=>{if(!nightRunning)StartCoroutine(SleepTransition(preview));};
        campaignUI.Runtime.ExamFinished=()=>{if(activeCourse!=null)StartCoroutine(EndCampaignLesson());else journey.FinishJourneyTransition();};
        BindTravelMarkers();
        campaignUI.StartedNewGame=RestoreCampaign;campaignUI.ResumedGame=RestoreCampaign;
        campaignUI.DinnerChosen=ChooseDinner;
        campaignUI.PauseRequested=()=>{if(!journey.IsBusy && Player.CanInteract && !DialogueVisible && !AcademicVisible && CurrentPhase!=Phase.YearTwo){campaign.Save();journey.BeginJourneyTransition();campaignUI.ShowStartup();}};
        campaignUI.GameRequested=StartCoopGame;campaignUI.WorkRequested=StartDeskWork;
        commute.CampaignArrival=CampaignTripArrival;
        journey.BeginJourneyTransition();campaignUI.ShowStartup();
    }
    private void OnCampaignClaim(){if(CampaignActive)campaign.CompleteOpening("claim");}
    private void OnCampaignClean(){if(CampaignActive)campaign.CompleteOpening("clean");}
    private void OnCampaignFriends(){if(CampaignActive)campaign.CompleteOpening("friends");}
    private void RestoreCampaign()
    {
        var p=campaign.Progress;
        Credits=p.Credits;
        if(deskMenu!=null)deskMenu.Close();if(longActor!=null)longActor.StopActivity();
        if(Player.CurrentState!=PlayerInteractionState.Standing)Player.ExitInteraction();
        journey.FinishJourneyTransition();
        if(!journey.BeginJourneyTransition())throw new InvalidOperationException("Cannot restore the campaign without journey ownership.");
        story.RestoreYearOneOpening(p.Has("opening:claim"),p.Has("opening:clean"),p.Has("opening:friends"));
        journey.RestoreDailyFlags(p.backpack,p.refreshed);
        journey.RestoreRoom();CurrentPhase=p.OpeningComplete?Phase.Evening:Phase.Dorm;
        Player.CharacterVisual.transform.localScale=roomActorScale;
        if(p.safeLocation=="Gate" && !p.Has("year:two"))
        {journey.ArriveInArea(DormJourneyController.JourneyArea.University,gateSpawn,gateBackdrop);CurrentPhase=Phase.Gate;}
        else if(p.safeLocation=="Campus" && !p.Has("year:two"))
            journey.ArriveInArea(DormJourneyController.JourneyArea.Downstairs_Campus,commute.CampusArrival,dormCampusBackdrop);
        journey.FinishJourneyTransition();
        if(p.Milestone.Coop && p.Has("day:2:game") && !p.Has("day:2:lights"))CurrentPhase=Phase.AwaitLights;
        if(p.safeLocation=="Gate")Player.CharacterVisual.transform.localScale=roomActorScale*.55f;
        commute.PrepareTrip(p.safeLocation=="Gate" && p.DayClassesResolved);
        foreach(var pickup in FindObjectsByType<BackpackInteractable>(FindObjectsSortMode.None))pickup.RefreshVisual();
        if(p.Has("year:two")){Credits=p.Credits;HeardHubris=true;CurrentPhase=Phase.YearTwo;story.CompleteExtendedYearOne();SetRoomNight(true);}
        if(p.exam!=null){journey.BeginJourneyTransition();journey.RevealCompletion();}
        if(p.Has("year:two"))campaignUI.Runtime.ShowYearTwo();
        UpdateCampaignObjective();
    }
    public string CampaignPrompt(YearOneInteractable.Action action)
    {
        switch(action)
        {
            case YearOneInteractable.Action.EnterH6:return "E - Vào lớp "+(CampaignActive?NextCourse()?.RoomName:"H6");
            case YearOneInteractable.Action.SitBesideClassmate:return "E - Ngồi cạnh bạn cùng lớp";
            case YearOneInteractable.Action.ExitClass:return "E - Ra hành lang H6";
            case YearOneInteractable.Action.ReturnBus:return "E - Tới trạm xe về KTX";
            case YearOneInteractable.Action.Canteen:return "E - Xuống căn tin ăn cơm";
            case YearOneInteractable.Action.PrepareFeast:return "E - Chuẩn bị nồi lẩu ăn mừng";
            default:return "";
        }
    }
    private YearOneSchedule.Session activeSession;
    private YearOneSchedule.Course NextCourse()=>campaign.Progress.NextSession==null?null:YearOneSchedule.Find(campaign.Progress.NextSession.CourseId);
    private bool CanUseCampaign(YearOneInteractable.Action action)
    {
        var p=campaign.Progress;
        switch(action)
        {
            case YearOneInteractable.Action.EnterH6:return CurrentPhase==Phase.Gate && NextCourse()!=null;
            case YearOneInteractable.Action.SitBesideClassmate:return CurrentPhase==Phase.Classroom && activeCourse!=null;
            case YearOneInteractable.Action.ExitClass:return CurrentPhase==Phase.Classroom && !puzzle.IsOpen && p.exam==null;
            case YearOneInteractable.Action.ReturnBus:return CurrentPhase==Phase.Gate && p.Has(p.TripId(false)) && !commute.IsReturning;
            case YearOneInteractable.Action.Canteen:return !p.OpeningComplete && p.Has("opening:friends") && journey.IsInHallway;
            case YearOneInteractable.Action.PrepareFeast:return p.ReadyForFeast && journey.CurrentArea==DormJourneyController.JourneyArea.InRoom && !p.Has("year:feast");
            default:return false;
        }
    }
    private void UseCampaign(YearOneInteractable.Action action)
    {
        switch(action)
        {
            case YearOneInteractable.Action.EnterH6:
                activeSession=campaign.Progress.NextSession;activeCourse=NextCourse();classroomBackdrop.sprite=activeCourse.UsesComputer?labArt:lectureArt;
                classroomForeground.sprite=activeCourse.UsesComputer?labForeground:lectureForeground;
                if(journey.BeginJourneyTransition())StartCoroutine(EnterClassroom());break;
            case YearOneInteractable.Action.SitBesideClassmate:
                if(!Player.TryEnterState(PlayerInteractionState.Sitting,seat))return;
                CurrentPhase=Phase.Lecture;Player.CharacterVisual.ApplyPosePresentation(classroomBackdrop,classroomForeground,seat,true);
                Player.CharacterVisual.SetDeskPose(LuonCharacterVisual.DeskPoseState.SittingStudying);
                StartCoroutine(AttendSession());break;
            case YearOneInteractable.Action.ExitClass:
                if(journey.BeginJourneyTransition())StartCoroutine(ExitCampaignClass());break;
            case YearOneInteractable.Action.ReturnBus:
                commute.PrepareTrip(true);journey.Notify("Xe về KTX sẽ ghé trạm. [E] Lên xe khi xe dừng.",4);break;
            case YearOneInteractable.Action.Canteen:
                if(journey.BeginJourneyTransition())StartCoroutine(EnterCanteen());break;
            case YearOneInteractable.Action.PrepareFeast:
                if((campaign.Progress.Has("year:feast-prepared") || campaign.PrepareFeast()) && journey.BeginJourneyTransition())StartCoroutine(BeginFeast());break;
        }
    }
    private IEnumerator AttendSession()
    {
        var session=activeSession;
        while(session!=null && campaign.Progress.absoluteMinutes<session.Start && Player.CurrentState==PlayerInteractionState.Sitting)yield return null;
        if(session==null || Player.CurrentState!=PlayerInteractionState.Sitting)yield break;
        if(session.Work==YearOneSchedule.Work.Exam)
        {
            if(!campaign.StartCourseExam(session.CourseId))campaignUI.Runtime.Show("Chưa thể thi","Cần vắng tối đa 1/2 buổi và nộp bài trước hạn. Kỳ thi vẫn giữ đúng giờ kết thúc.",()=>StartCoroutine(EndCampaignLesson()));
            yield break;
        }
        if(!campaign.Progress.Has("attendance:"+session.Id) && !campaign.Transact(p=>p.RecordAttendance(session.Id))){StartCoroutine(EndCampaignLesson());yield break;}
        float elapsed=0;
        while(elapsed<15 && Player.CurrentState==PlayerInteractionState.Sitting){if(!(campaign.WorldPaused?.Invoke()??false))elapsed+=Time.deltaTime;yield return null;}
        if(Player.CurrentState!=PlayerInteractionState.Sitting)yield break;
        puzzle.SetCourse(activeCourse,session.Work);puzzle.Open(()=>CompleteCampaignLesson(activeCourse,session.Work));
    }
    private void CompleteCampaignLesson(YearOneSchedule.Course course,YearOneSchedule.Work lesson)
    {
        if(!campaign.CompleteCourse(course.Id,lesson))return;
        PuzzleSolved=true;CurrentPhase=Phase.Reaction;
        if(damSurprise!=null)damSurprise.SetActive(true);
        ShowDialogue(new[]{"Thầy giáo","Bạn cùng lớp"},new[]{"Đúng rồi! "+course.Hint,"Nhanh ghê! Hết tiết rồi, đi thôi."},()=>StartCoroutine(EndCampaignLesson()));
    }
    private IEnumerator EndCampaignLesson()
    {
        if(Player.CurrentState!=PlayerInteractionState.Standing)Player.ExitInteraction();
        yield return journey.FadeTo(1,.2f);journey.MovePlayer(classroomSpawn.position);
        if(damSurprise!=null)damSurprise.SetActive(false);activeCourse=null;activeSession=null;CurrentPhase=Phase.Classroom;
        yield return journey.FadeTo(0,.2f);journey.FinishJourneyTransition();UpdateCampaignObjective();
    }
    private IEnumerator ExitCampaignClass()
    {
        if(Player.CurrentState!=PlayerInteractionState.Standing)Player.ExitInteraction();activeCourse=null;activeSession=null;
        yield return journey.FadeTo(1,.2f);journey.ArriveInArea(DormJourneyController.JourneyArea.University,gateSpawn,gateBackdrop);
        CurrentPhase=Phase.Gate;yield return journey.FadeTo(0,.2f);journey.FinishJourneyTransition();UpdateCampaignObjective();
    }
    private IEnumerator CampaignTripArrival(bool homebound)
    {
        yield return journey.FadeTo(1,.3f);commute.RestoreBoardedActor();
        if(homebound)
        {
            journey.ArriveInArea(DormJourneyController.JourneyArea.Downstairs_Campus,commute.CampusArrival,dormCampusBackdrop);
            Player.CharacterVisual.transform.localScale=roomActorScale;CurrentPhase=Phase.Evening;
        }
        else {journey.ArriveInArea(DormJourneyController.JourneyArea.University,gateSpawn,gateBackdrop);CurrentPhase=Phase.Gate;}
        yield return journey.FadeTo(0,.3f);journey.FinishJourneyTransition();
        Player.CharacterVisual.transform.localScale=homebound?roomActorScale:roomActorScale*.55f;
        UpdateCampaignObjective();
    }
    private IEnumerator EnterCanteen()
    {
        yield return journey.FadeTo(1,.3f);journey.ArriveInArea(DormJourneyController.JourneyArea.University,canteenSpawn,canteenBackdrop);
        CurrentPhase=Phase.Canteen;yield return journey.FadeTo(0,.3f);journey.RevealCompletion();
        campaignUI.ShowDinner();
    }
    public void ChooseDinner()
    {
        if(CurrentPhase!=Phase.Canteen || !campaign.CompleteOpening("meal"))return;
        campaignUI.HideDinner();
        ShowDialogue(new[]{"Tộ","Long","Đạm","Lươn"},new[]{"Cơm 25k, có cả mặn lẫn rau. Ăn thôi!","Cùng phòng rồi, có gì cứ gọi nhau nha!","Mai bắt đầu học, mấy ông nhớ dậy sớm.","Rồi, về phòng chuẩn bị balo nào!"},()=>StartCoroutine(AfterDinner()));
    }
    private IEnumerator AfterDinner()
    {
        yield return journey.FadeTo(1,.3f);journey.RestoreRoom();CurrentPhase=Phase.Evening;
        yield return journey.FadeTo(0,.3f);journey.FinishJourneyTransition();commute.PrepareTrip(false);UpdateCampaignObjective();
    }
    private bool StartDeskWork(YearOneSchedule.Course course,YearOneSchedule.Work work)
    {
        if(workRunning || !deskMenu.IsOpen || !campaign.Progress.CanComplete(course,work) || journey.CurrentArea!=DormJourneyController.JourneyArea.InRoom)return false;
        workRunning=true;deskMenu.BeginCoursePose(course);StartCoroutine(DeskWork(course,work));return true;
    }
    private IEnumerator DeskWork(YearOneSchedule.Course course,YearOneSchedule.Work work)
    {
        float elapsed=0;while(elapsed<15 && deskMenu.IsOpen){if(!campaign.MenuPausesClock && !(campaign.WorldPaused?.Invoke()??false))elapsed+=Time.deltaTime;yield return null;}
        if(!deskMenu.IsOpen){workRunning=false;campaignUI.HideStudy();yield break;}
        if(!campaign.Progress.CanComplete(course,work)){workRunning=false;campaignUI.WorkFinished();yield break;}
        puzzle.SetCourse(course,work);puzzle.Open(()=>{if(deskMenu.IsOpen)campaign.CompleteCourse(course.Id,work);workRunning=false;Credits=campaign.Progress.Credits;campaignUI.WorkFinished();});
    }
    private void StartCoopGame()
    {
        if(gameRunning || campaign.Progress.Has("day:2:game") || !campaign.Progress.Milestone.Coop)return;
        var chair=Array.Find(roomChairs,c=>c.Occupant!=Player && c.Reservation.Owner==null);
        if(chair==null || !longActor.StartGaming(chair,null)){journey.Notify("Ghế bên cạnh đang bận.");return;}
        gameRunning=true;gameSeconds=0;
    }
    private void UpdateCampaign()
    {
        var p=campaign.Progress;
        if(!nightRunning && CurrentPhase!=Phase.YearTwo && CurrentPhase!=Phase.Hotpot && journey.CurrentArea==DormJourneyController.JourneyArea.InRoom)
        {
            float hour=p.clockMinutes/60f;
            var noon=new Color(1f,.99f,.94f,1);var dusk=new Color(.95f,.63f,.43f,1);var night=new Color(.28f,.34f,.56f,1);
            Color tint=hour<12?Color.Lerp(new Color(.85f,.91f,1f,1),noon,Mathf.InverseLerp(6,12,hour)):hour<17?Color.Lerp(noon,dusk,Mathf.InverseLerp(12,17,hour)):Color.Lerp(dusk,night,Mathf.InverseLerp(17,20,hour));
            for(int i=0;i<roomVisuals.Length;i++)if(roomVisuals[i]!=null)roomVisuals[i].color=roomColors[i]*tint;
        }
        if(workRunning && !deskMenu.IsOpen){puzzle.Cancel();workRunning=false;campaignUI.HideStudy();}
        if(!journey.IsBusy && journey.CurrentArea==DormJourneyController.JourneyArea.InRoom && CurrentPhase==Phase.Evening && p.safeLocation!="Room")
        {p.safeLocation="Room";campaign.Save();}
        if(!nightRunning && !journey.IsBusy && (p.backpack!=journey.HasBackpack || p.refreshed!=journey.HasRefreshed))
        {p.backpack=journey.HasBackpack;p.refreshed=journey.HasRefreshed;campaign.Save();}
        if(gameRunning)
        {
            if(!deskMenu.IsOpen || !campaign.IsComputerGaming){gameRunning=false;longActor.StopActivity();}
            else if(longActor.Current==RoommateActivityController.Activity.Gaming && campaign.ComputerSessionSeconds>=20 && !p.Has("day:2:game"))
            {campaign.CompleteEvening("game");campaignUI.Runtime.Notify("Long: Chơi đủ rồi nhé. Muốn ngủ thì đứng dậy tắt đèn.");CurrentPhase=Phase.AwaitLights;}
        }
        if(p.Milestone.Coop && p.Has("day:2:game") && p.clockMinutes>=1380 && !p.Has("day:2:argument") && !DialogueVisible && !deskMenu.ComputerVisible && !campaignUI.Runtime.BlocksInput)
        {
            if(campaign.CompleteEvening("argument"))ShowDialogue(new[]{"Đạm","Long","Lươn"},new[]{"Khuya rồi, tắt đèn cho người khác ngủ đi!","Để hết ván đã!","Biết rồi. Tui chọn dừng hay chơi tiếp đây."},()=>{CurrentPhase=Phase.AwaitLights;});
        }
        if(p.Milestone.Coop && p.Has("day:2:game") && !p.Has("day:2:lights") && Player.CanInteract)
        {
            longActor.LeaveGaming();story.lightSwitchInteractable.SetInteractable(true);story.lightSwitchInteractable.SetPrompt("E - Tắt đèn");
            story.lightSwitchInteractable.onInteract.RemoveListener(TurnOffCampaignLights);story.lightSwitchInteractable.onInteract.AddListener(TurnOffCampaignLights);
        }
        if(commute.IsReturning && commute.HasCompletedTrip && journey.CurrentArea==DormJourneyController.JourneyArea.InRoom)commute.PrepareTrip(false);
        UpdateCampaignObjective();
    }
    private void TurnOffCampaignLights()
    {
        if(!CampaignActive || CurrentPhase!=Phase.AwaitLights || !campaign.CompleteEvening("lights"))return;
        story.lightSwitchInteractable.SetInteractable(false);SetRoomNight(true);story.roomCeilingLight.intensity=.05f;story.laptopLight.intensity=0;CurrentPhase=Phase.AwaitSleep;
    }
    private IEnumerator SleepTransition(YearOneTransition preview)
    {
        var bed=Player.ActiveInteractable as BedInteractable;if(bed==null)yield break;
        nightRunning=true;Player.CharacterVisual.SetSleeping(true);
        var other=Array.Find(roomBeds,b=>b!=bed && b.Reservation.Owner==null);
        bool companion=other!=null && longActor.isActiveAndEnabled && longActor.StartSleeping(other,null);
        float waitUntil=Time.time+8;
        while(companion && !longActor.IsSleeping && Time.time<waitUntil)yield return null;
        yield return new WaitForSeconds(3);
        if(!campaign.CommitTransition(preview))
        {Player.CharacterVisual.SetSleeping(false);longActor.StopActivity();nightRunning=false;campaignUI.Runtime.Show("Chưa chuyển ngày",string.IsNullOrEmpty(campaign.SaveError)?"Trạng thái đã thay đổi. Hãy chọn ngủ lại.":campaign.SaveError,()=>{});yield break;}
        bed.ExecuteImmediateExit(Player);longActor.StopActivity();nightRunning=false;
        RestoreCampaign();
        if(!campaign.Progress.Has("year:two"))
        {
            int delta=campaign.Progress.Milestone.CalendarDay-YearOneSchedule.ById(preview.Source).CalendarDay;
            string heading=delta==0?"Đã nghỉ tới cuối ngày • Có thể dự lẩu":delta==1?"Sáng hôm sau":delta+" ngày sau";
            campaignUI.Runtime.Show(heading,YearOneSchedule.DayTitle(campaign.Progress.day)+"\nTín chỉ: "+campaign.Progress.Credits+"/32 • Môn nợ: "+campaign.Progress.OwedCourses+"\n"+string.Join("\n",preview.Warnings),()=>{});
        }
    }
    // Old fixture entrypoint retained; gameplay uses explicit Sleep confirmation.
    private IEnumerator FinishCampaignNight(BedInteractable playerBed,BedInteractable otherBed){yield return SleepTransition(campaign.Progress.PreviewTransition());}
    private void ShowCampaignAcademic()
    {
        if(!journey.BeginJourneyTransition())return;
        Credits=campaign.Progress.Credits;CurrentPhase=Phase.AcademicCard;academicPanel.SetActive(true);journey.RevealCompletion();
        academicPanel.transform.Find("Title").GetComponent<TMPro.TMP_Text>().text="TỔNG KẾT HỌC TẬP";
        academicPanel.transform.Find("Summary").GetComponent<TMPro.TMP_Text>().text=campaign.Progress.Milestone.Semester==1?"Đã hoàn thành các buổi học, bài tập và kiểm tra học kỳ 1.\nNhững tuần tiếp theo, cả phòng bước vào học kỳ 2.":"Đã hoàn thành đủ 12 buổi, bài tập, ôn và kiểm tra hai học kỳ.\nVề phòng chuẩn bị bữa lẩu cùng các bạn nhé!";
        academicPanel.transform.Find("Credits").GetComponent<TMPro.TMP_Text>().text="TÍN CHỈ: "+Credits+" / 32\nMÔN NỢ: "+campaign.Progress.OwedCourses;
    }
    private void ContinueCampaignAcademic(){academicPanel.SetActive(false);CurrentPhase=Phase.Evening;journey.FinishJourneyTransition();UpdateCampaignObjective();}
    private void UpdateCampaignObjective()
    {
        campaignUI.SetObjectiveVisible(false);
        if(CurrentPhase==Phase.YearTwo)return;
        Credits=campaign.Progress.Credits;
    }
    private Transform etaDoor,etaHall,etaStairs,etaClassDoor;
    private void BindTravelMarkers()
    {
        foreach(var t in Resources.FindObjectsOfTypeAll<Transform>())
        {
            if(!t.gameObject.scene.IsValid())continue;
            if(t.name=="DoorwayTrigger")etaDoor=t;
            if(t.name=="Room404_ReturnTrigger")etaHall=t;
            if(t.name=="Stairs_Trigger")etaStairs=t;
            if(t.name=="H6_Door_Interactable")etaClassDoor=t;
        }
    }
    public float EstimateTravelMinutes()
    {
        if(journey==null)return 0;float speed=Mathf.Max(.1f,Player.GetComponent<PlayerMovement>().MoveSpeed);
        Func<Vector3,Transform,float> walk=(from,to)=>to==null?0:Vector3.Distance(from,to.position)/speed;
        float classroom=Vector3.Distance(classroomSpawn.position,seat.position)/speed;
        if(CurrentPhase==Phase.Classroom || CurrentPhase==Phase.Lecture)return walk(Player.transform.position,seat);
        if(CurrentPhase==Phase.Gate)return walk(Player.transform.position,etaClassDoor)+classroom+.5f;
        float distance=0;
        if(journey.CurrentArea==DormJourneyController.JourneyArea.InRoom)
            distance=walk(Player.transform.position,etaDoor)+(etaHall!=null?walk(etaHall.position,etaStairs):0)+walk(commute.CampusArrival.position,commute.OutboundStopAnchor);
        else if(journey.IsInHallway)distance=walk(Player.transform.position,etaStairs)+walk(commute.CampusArrival.position,commute.OutboundStopAnchor);
        else distance=walk(Player.transform.position,commute.OutboundStopAnchor);
        float transitions=journey.CurrentArea==DormJourneyController.JourneyArea.InRoom?1.9f:journey.IsInHallway?1.5f:.5f; // Door .4 + stairs 1 + classroom .5.
        return distance+commute.EstimatedTripSeconds+walk(gateSpawn.position,etaClassDoor)+classroom+transitions;
    }
    public double RecommendedDeparture(YearOneSchedule.Session session)
    {
        float total=EstimateTravelMinutes();double result=session.Start-total;var p=campaign.Progress;
        if(p.Milestone.MissableBus && !p.Has(p.TripId(false)) && !p.Has("day:4:stop") && !p.Has("day:4:missed"))
        {
            float speed=Mathf.Max(.1f,Player.GetComponent<PlayerMovement>().MoveSpeed);
            float afterBus=(etaClassDoor==null?0:Vector3.Distance(gateSpawn.position,etaClassDoor.position)/speed)+Vector3.Distance(classroomSpawn.position,seat.position)/speed+.5f;
            result=Math.Min(result,p.Milestone.CalendarDay*1440d+p.Milestone.DepartureMinute-Math.Max(0,total-commute.EstimatedTripSeconds-afterBus));
        }
        return result;
    }

}
