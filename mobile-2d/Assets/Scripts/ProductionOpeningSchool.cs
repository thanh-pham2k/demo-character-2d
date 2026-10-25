using System.Collections;
using UnityEngine;

public sealed partial class ProductionOpeningController
{
    public DormJourneyController schoolJourney;
    public CampusCommuteController schoolBus;
    public RestroomInteractable schoolMale,schoolFemale;
    public CalculusPuzzleModal schoolPuzzle;
    public SpriteRenderer schoolStudyPose;
    private bool schoolHall,schoolLobby,schoolReturnStop,schoolHidPlayer,schoolPreviousVisible;
    private string boundSchoolTrip,lastSchoolNotice;
    public bool SchoolPuzzleVisible=>schoolPuzzle!=null && schoolPuzzle.IsOpen;
    private bool InSchoolMission=>Ready && (Progress.CurrentMission==6 || Progress.CurrentMission==7);
    private void SetupSchool()
    {
        if(schoolJourney==null)return;
        schoolJourney.CommitWashCompletion=()=>
        {
            bool first=Progress.CurrentMission==6 && Step=="wash_before_class";
            bool queued=Progress.CurrentMission==7 && Step=="join_wc_queue" && queueReady;
            if(!first && !queued)return false;
            bool saved=session.Commit(Progress.CurrentMission,Step,"",out string error);
            FlushMorningClock();
            if(saved)RefreshStage(false);else Notify(error);
            return saved;
        };
        schoolBus.CampaignArrival=SchoolBusArrival;
    }
    private void ResetSchool()
    {
        schoolHall=false;schoolLobby=false;schoolReturnStop=false;boundSchoolTrip=null;lastSchoolNotice=null;
        RestoreSchoolActor();if(schoolPuzzle!=null)schoolPuzzle.Cancel();
    }
    private int SchoolZone(int fallback)
    {
        if(Ready && Progress.CurrentMission==7)return YearOneZone(fallback);
        if(!InSchoolMission)return fallback;
        switch(Step)
        {
            case "equip_backpack":return 3;
            case "wash_before_class":return schoolHall?2:3;
            case "take_bus":return schoolLobby?5:2;
            case "enter_class":return 6;
            case "solve_warmup":case "finish_lesson":return 7;
            case "return_room":return schoolReturnStop?6:7;
            default:return 3;
        }
    }
    private void RefreshSchool()
    {
        if(schoolJourney==null)return;
        schoolJourney.enabled=InSchoolMission;schoolBus.enabled=InSchoolMission;
        schoolMale.enabled=InSchoolMission && activeZone==2 && (Step=="wash_before_class" || Step=="join_wc_queue" && queueReady);
        schoolFemale.enabled=schoolMale.enabled;
        if(!InSchoolMission)return;
        schoolJourney.RestoreDailyFlags(Progress.Has("action:6:equip_backpack"),Progress.CurrentMission==7?Progress.Has("action:7:join_wc_queue"):Progress.Has("action:6:wash_before_class"));
        if(!schoolJourney.IsBusy && player.CanInteract)
        {
            if(schoolJourney.BeginJourneyTransition())
            {
                schoolJourney.ArriveInArea(SchoolArea(activeZone),null,SchoolBackdrop(activeZone));
                schoolJourney.FinishJourneyTransition();
            }
        }
        if(((Step=="take_bus" || Step=="catch_bus") && activeZone==5 || Step=="return_room" && activeZone==6) && boundSchoolTrip!=Step && !schoolJourney.IsBusy)
        {schoolBus.BindProgress(Progress.CurrentMission==7?new ProductionCommuteProgress(session,7,"catch_bus","",()=>MorningMissed):new ProductionCommuteProgress(session,6,Step));schoolBus.PrepareTrip(Step=="return_room");boundSchoolTrip=Step;}
        player.CharacterVisual.GetComponent<SpriteRenderer>().sortingOrder=activeZone==7?24:20;
        ApplyZoneActorScale();
    }
    private SpriteRenderer SchoolBackdrop(int zone)=>zone==3?null:zones[zone].Find("Backdrop").GetComponent<SpriteRenderer>();
    private DormJourneyController.JourneyArea SchoolArea(int zone)=>zone==3?DormJourneyController.JourneyArea.InRoom:zone==2?DormJourneyController.JourneyArea.InHallway:zone==5?DormJourneyController.JourneyArea.Downstairs_Campus:DormJourneyController.JourneyArea.University;
    public bool CanUseSchoolLink(ProductionSchoolLink.Route route)
    {
        if(!InSchoolMission || Busy || startPanel.activeSelf || clockFailed)return false;
        return route==ProductionSchoolLink.Route.Hallway?(Step=="wash_before_class" || Step=="join_wc_queue") && activeZone==3:
            route==ProductionSchoolLink.Route.Lobby?(Step=="take_bus" || Step=="catch_bus") && activeZone==2:
            Step=="return_room" && activeZone==7;
    }
    public void UseSchoolLink(ProductionSchoolLink.Route route)
    {
        if(!CanUseSchoolLink(route))return;
        // Bind the return host before acquiring the journey lock.
        if(route==ProductionSchoolLink.Route.ReturnStop)
        {schoolBus.BindProgress(new ProductionCommuteProgress(session,6,"return_room"));schoolBus.PrepareTrip(true);boundSchoolTrip="return_room";}
        if(!schoolJourney.BeginJourneyTransition())return;
        StartCoroutine(SchoolNavigation(route));
    }
    private IEnumerator SchoolNavigation(ProductionSchoolLink.Route route)
    {
        travelling=true;yield return schoolJourney.FadeTo(1,.2f);
        yield return new WaitForSeconds(.55f);
        if(route==ProductionSchoolLink.Route.Hallway)schoolHall=true;
        else if(route==ProductionSchoolLink.Route.Lobby)schoolLobby=true;
        else schoolReturnStop=true;
        int zone=route==ProductionSchoolLink.Route.Hallway?2:route==ProductionSchoolLink.Route.Lobby?5:6;
        ActivateSchoolZone(zone);
        yield return schoolJourney.FadeTo(0,.2f);schoolJourney.FinishJourneyTransition();travelling=false;
        RefreshSchool();RefreshHudText();Notify(SchoolGuide());
    }
    private void ActivateSchoolZone(int zone)
    {
        activeZone=zone;view.backgroundColor=zone==3?Color.white:new Color(.08f,.1f,.14f);for(int i=0;i<zones.Length;i++)zones[i].gameObject.SetActive(i==zone);
        schoolJourney.ArriveInArea(SchoolArea(zone),spawns[zone],SchoolBackdrop(zone));
        ApplyZoneActorScale();
    }
    private IEnumerator SchoolBusArrival(bool returning)
    {
        schoolBus.RestoreBoardedActor();
        if(returning)
        {
            ActivateSchoolZone(5);Notify("Xe đã về KTX • Lươn lên cầu thang về tầng 4.");
            yield return new WaitForSeconds(.6f);ActivateSchoolZone(2);
            yield return new WaitForSeconds(.5f);ActivateSchoolZone(3);
        }
        else ActivateSchoolZone(6);
        RefreshStage(false);
        yield return schoolJourney.FadeTo(0,.25f);schoolJourney.FinishJourneyTransition();
        RefreshSchool();RefreshHudText();Notify(SchoolGuide());
    }
    private string SchoolDescription()
    {
        string year=YearOneDescription();if(year!=null)return year;
        if(!InSchoolMission)return null;
        switch(Step)
        {
            case "equip_backpack":return "Sáng hôm sau. Lấy balo trên giường đã chọn, kiểm tra lịch rồi ra hành lang rửa mặt trước khi đi học.";
            case "enter_class":return "Đã tới cổng H6. Tìm lớp Giải Tích, vào học cùng các bạn Bách Khoa.";
            case "solve_warmup":return SchoolWarmupLine();
            case "finish_lesson":return dialogueLine==0?"Thầy: Đúng rồi, ghép số đầu với số cuối thành năm cặp, mỗi cặp bằng 11. Cùng ghi lại cách giải này.":"Tiết học kết thúc. Bạn cùng lớp: Hẹn ông buổi sau nhé!\nLươn cất vở, chuẩn bị ra cổng đón xe về KTX. Chưa phải kết quả cuối năm.";
            case "tell_dam":return dialogueLine==0?"Đạm: Buổi học đầu bên Bách Khoa thế nào? Tui cũng vừa từ HCMUS về.":"Lươn: Có bạn ngồi cạnh chỉ chỗ, thầy hỏi tổng từ 1 đến 10. Tui ghép cặp ra 55!\nĐạm: Ngon rồi. Mai nhớ dậy sớm, đừng để WC đông rồi lỡ xe.";
            default:return null;
        }
    }
    private string SchoolWarmupLine()=>dialogueLine==0?"Thầy: Chào lớp. Trước khi vào bài, ai tính nhanh tổng 1 + 2 + ... + 10?":dialogueLine==1?"Bạn cùng lớp: Chỗ bên cạnh còn trống, ông ngồi đây. Thử ghép số đầu với số cuối xem!":"Lươn ngồi vào bàn, mở vở và thử trả lời câu hỏi khởi động.";
    private bool HandleSchoolConfirm()
    {
        if(HandleRegistrationConfirm() || HandleYearOneConfirm())return true;
        if(!InSchoolMission)return false;
        if(Step=="enter_class"){StartCoroutine(EnterSchoolClass());return true;}
        if(Step=="solve_warmup")
        {
            if(dialogueLine++<2){body.text=SchoolWarmupLine();return true;}
            modal.SetActive(false);schoolPreviousVisible=player.CharacterVisual.GetComponent<SpriteRenderer>().enabled;
            player.CharacterVisual.GetComponent<SpriteRenderer>().enabled=false;schoolHidPlayer=true;schoolStudyPose.enabled=true;
            schoolPuzzle.Open(()=>
            {
                bool saved=session.Commit(6,"solve_warmup","",out string error);
                Close();RefreshStage(false);if(!saved)Notify(error);
            });return true;
        }
        if((Step=="finish_lesson" || Step=="tell_dam") && dialogueLine++==0){body.text=SchoolDescription();return true;}
        return false;
    }
    private IEnumerator EnterSchoolClass()
    {
        travelling=true;modal.SetActive(false);yield return FadeTo(1);
        bool saved=session.Commit(6,"enter_class","",out string error);
        RefreshStage(true);yield return FadeTo(0);travelling=false;Close();RefreshSchool();
        if(!saved)Notify(error);
    }
    private string SchoolGuide()
    {
        string year=YearOneGuide();if(year!=null)return year;
        if(!InSchoolMission)return null;
        switch(Step)
        {
            case "equip_backpack":return "Lấy balo ở giường "+(Progress.Has("bed:luon_right")?"bên phải.":"bên trái.");
            case "wash_before_class":return activeZone==3?"Tới cửa phòng, ra hành lang và ghé WC Nam.":"Đi tới WC Nam, hoàn tất rửa mặt rồi xuống cầu thang.";
            case "take_bus":return activeZone==2?"Đã rửa mặt. Tới cầu thang bên phải để xuống trạm 33.":"Đi tới biển trạm 33. Chờ xe dừng rồi bấm E lên xe • Vé 7.000đ.";
            case "enter_class":return "Tới cửa H6 để vào lớp Giải Tích.";
            case "solve_warmup":return "Tới bàn còn chỗ cạnh bạn cùng lớp để nghe câu hỏi khởi động.";
            case "finish_lesson":return "Ghi lại lời giải, học hết tiết rồi chuẩn bị về KTX.";
            case "return_room":return activeZone==7?"Tới lối ra giữa lớp để ra cổng và đón xe về.":"Tới biển xe về KTX. Mỗi chiều là một vé 7.000đ.";
            case "tell_dam":return "Đạm đã về404. Tới gặp bạn và kể buổi học đầu tiên.";
            default:return "";
        }
    }
    private void RestoreSchoolActor()
    {
        var visual=player!=null?player.CharacterVisual:null;
        var renderer=visual!=null?visual.GetComponent<SpriteRenderer>():null;
        if(schoolHidPlayer && renderer!=null)renderer.enabled=schoolPreviousVisible;
        schoolHidPlayer=false;
        if(schoolStudyPose!=null)schoolStudyPose.enabled=false;
    }
    private void UpdateSchool()
    {
        if(SchoolPuzzleVisible && UnityEngine.InputSystem.Keyboard.current!=null && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {schoolPuzzle.Cancel();Close();Notify("Đã đóng câu hỏi. Tới bàn để thử lại.");}
        if(InSchoolMission && schoolJourney.LastNotice!=lastSchoolNotice)
        {lastSchoolNotice=schoolJourney.LastNotice;if(!string.IsNullOrEmpty(lastSchoolNotice))Notify(lastSchoolNotice);}
    }
}
