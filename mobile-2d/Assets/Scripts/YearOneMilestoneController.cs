using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Authored Mission 06/08 sequence; furniture and legacy story remain separate.
public partial class YearOneMilestoneController : MonoBehaviour
{
    public enum Phase { Dorm, Gate, Classroom, Lecture, Blackboard, Puzzle, Reaction, AcademicCard, Hotpot, YearTwo, Canteen, Evening, AwaitLights, AwaitSleep, DayTransition }
    [SerializeField] private DormJourneyController journey;
    [SerializeField] private MVPFlowController story;
    [SerializeField] private CampusCommuteController commute;
    [SerializeField] private Transform gateSpawn, classroomSpawn, seat, board, feastArrival;
    [SerializeField] private SpriteRenderer gateBackdrop, classroomBackdrop;
    [SerializeField] private SpriteRenderer classroomForeground;
    [SerializeField] private SpriteRenderer roomBackdrop;
    private Color roomDayColor;
    [SerializeField] private SpriteRenderer[] roomVisuals;
    private Color[] roomColors;
    private bool nightTinted;
    public void ConfigureRoomVisuals(SpriteRenderer[] renderers)=>roomVisuals=renderers;
    public void ConfigureRoomBackdrop(SpriteRenderer renderer)=>roomBackdrop=renderer;
    public void ConfigureForeground(SpriteRenderer renderer)=>classroomForeground=renderer;
    [SerializeField] private GameObject feast, conversationPanel, academicPanel;
    [SerializeField] private GameObject damSurprise;
    public void ConfigureSurprise(GameObject emote)=>damSurprise=emote;
    [SerializeField] private TMP_Text speaker, dialogue;
    [SerializeField] private Button next, academicContinue;
    [SerializeField] private CalculusPuzzleModal puzzle;
    [SerializeField] private AudioSource feastAudio;
    [SerializeField] private SpriteRenderer pot;
    [SerializeField] private Sprite[] potFrames;
    private AudioClip boiling, clink;
    private string[] lines, names;
    private int line;
    private System.Action afterDialogue;
    private bool advancing;
    private Vector3 roomActorScale;
    public Phase CurrentPhase { get; private set; }
    public PlayerInteractionStateController Player=>journey.Player;
    public bool OwnsPresentation=>(CampaignActive || CurrentPhase!=Phase.Dorm) && CurrentPhase!=Phase.YearTwo;
    public bool SuppressNotice=>OwnsPresentation && CurrentPhase!=Phase.Gate && CurrentPhase!=Phase.Classroom;
    public bool PuzzleSolved { get; private set; }
    public int Credits { get; private set; }
    public int DebtSubjects=>CampaignActive?campaign.Progress.OwedCourses:0;
    public bool AcademicVisible=>academicPanel!=null && academicPanel.activeSelf;
    public bool FeastVisible=>feast!=null && feast.activeSelf;
    public bool DialogueVisible=>conversationPanel!=null && conversationPanel.activeSelf;
    public Button NextButton=>next;
    public Button AcademicContinueButton=>academicContinue;
    public CalculusPuzzleModal Puzzle=>puzzle;
    public Transform Seat=>seat;
    public Transform GateSpawn=>gateSpawn;
    public bool HeardHubris { get; private set; }

    public void Configure(DormJourneyController dorm,MVPFlowController flow,CampusCommuteController bus,
        Transform gate,Transform classroom,Transform seatMarker,Transform boardMarker,Transform feastMarker,
        SpriteRenderer gateArt,SpriteRenderer classroomArt,GameObject feastRoot,GameObject dialogueRoot,
        TMP_Text speakerText,TMP_Text body,Button advance,GameObject academic,Button continueYear,
        CalculusPuzzleModal calculus,AudioSource audio,SpriteRenderer hotpot,Sprite[] steam)
    {
        journey=dorm;story=flow;commute=bus;gateSpawn=gate;classroomSpawn=classroom;seat=seatMarker;board=boardMarker;
        feastArrival=feastMarker;gateBackdrop=gateArt;classroomBackdrop=classroomArt;feast=feastRoot;
        conversationPanel=dialogueRoot;speaker=speakerText;dialogue=body;next=advance;academicPanel=academic;
        academicContinue=continueYear;puzzle=calculus;feastAudio=audio;pot=hotpot;potFrames=steam;
    }
    private void Awake()
    {
        conversationPanel.SetActive(false);academicPanel.SetActive(false);feast.SetActive(false);
        if(damSurprise!=null)damSurprise.SetActive(false);
        next.onClick.AddListener(AdvanceDialogue);academicContinue.onClick.AddListener(ContinueAcademic);
        boiling=DormSound.CreateHotpot();clink=DormSound.CreateGlassClink();
        if(roomBackdrop!=null)roomDayColor=roomBackdrop.color;
        roomActorScale=Player.CharacterVisual.transform.localScale;
        // Include furniture children added after the scene's original tint list (book stacks).
        if(roomChairs!=null)roomVisuals=roomVisuals.Concat(roomChairs.Where(c=>c!=null && c.BackRenderer!=null).SelectMany(c=>c.BackRenderer.GetComponentsInChildren<SpriteRenderer>(true))).Where(r=>r!=null).Distinct().ToArray();
        roomColors=new Color[roomVisuals.Length];
        for(int i=0;i<roomVisuals.Length;i++)roomColors[i]=roomVisuals[i].color;
    }
    public bool CanUse(YearOneInteractable.Action action)=>!journey.IsBusy &&
        (CampaignActive?CanUseCampaign(action):(action==YearOneInteractable.Action.EnterH6 ? CurrentPhase==Phase.Gate : CurrentPhase==Phase.Classroom));
    // Called while the bus still owns the journey transition. It hands that same
    // lock to the arrival fade, then releases it at the authored walking marker.
    public IEnumerator ArriveFromBus()
    {
        CurrentPhase=Phase.Gate;
        yield return journey.FadeTo(1,.35f);
        commute.RestoreBoardedActor();
        journey.ArriveInArea(DormJourneyController.JourneyArea.University,gateSpawn,gateBackdrop);
        yield return journey.FadeTo(0,.35f);
        journey.FinishJourneyTransition();
        Player.CharacterVisual.transform.localScale=roomActorScale*.55f;
        journey.Notify("Bách Khoa • Giảng đường H6 — Đi tới cửa lớp và bấm [E].",5f);
    }
    public void Use(YearOneInteractable.Action action)
    {
        if(!CanUse(action))return;
        if(CampaignActive){UseCampaign(action);return;}
        if(action==YearOneInteractable.Action.EnterH6)
        {
            if(journey.BeginJourneyTransition())StartCoroutine(EnterClassroom());
        }
        else if(Player.TryEnterState(PlayerInteractionState.Sitting,seat))
        {
            CurrentPhase=Phase.Lecture;
            Player.CharacterVisual.ApplyPosePresentation(classroomBackdrop,classroomForeground,seat,true);
            Player.CharacterVisual.SetDeskPose(LuonCharacterVisual.DeskPoseState.SittingStudying);
            ShowDialogue(new[]{"Bạn cùng lớp","Lê Văn Lươn","Thầy giáo"},new[]{
                "Tôi ghi kín cả trang rồi. Ông chép kịp không?",
                "Cái này thi không?",
                "Anh Lê Văn Lươn lên bảng giải bài này!"},()=>StartCoroutine(WalkToBoard()));
        }
    }
    private IEnumerator EnterClassroom()
    {
        yield return journey.FadeTo(1,.25f);
        journey.ArriveInArea(DormJourneyController.JourneyArea.University,classroomSpawn,classroomBackdrop);
        yield return journey.FadeTo(0,.25f);
        journey.FinishJourneyTransition();CurrentPhase=Phase.Classroom;
        journey.Notify("Bạn cùng lớp đang ghi chép ở dãy bàn. [E] Ngồi cạnh bạn.",5f);
    }
    private IEnumerator WalkToBoard()
    {
        Player.CharacterVisual.SetDeskPose(LuonCharacterVisual.DeskPoseState.None);
        Player.ExitInteraction();
        if(!journey.BeginJourneyTransition())yield break;
        CurrentPhase=Phase.Blackboard;
        Vector3 start=Player.transform.position;
        for(float t=0;t<1.2f;t+=Time.deltaTime){journey.MovePlayer(Vector3.Lerp(start,board.position,t/1.2f));yield return null;}
        journey.MovePlayer(board.position);CurrentPhase=Phase.Puzzle;
        if(CampaignActive && activeCourse!=null)
        {
            var course=activeCourse;var lesson=course.FirstDay==campaign.Progress.day?YearOneSchedule.Work.Lesson1:YearOneSchedule.Work.Lesson2;
            puzzle.SetCourse(course,lesson);puzzle.Open(()=>CompleteCampaignLesson(course,lesson));
        }
        else puzzle.Open(SolvedPuzzle);
    }
    private void SolvedPuzzle()
    {
        if(CurrentPhase!=Phase.Puzzle)return;
        PuzzleSolved=true;CurrentPhase=Phase.Reaction;
        if(damSurprise!=null)damSurprise.SetActive(true);
        ShowDialogue(new[]{"Thầy giáo","Bạn cùng lớp","Lê Văn Lươn"},new[]{
            "Đúng! 1 + 10 = 11, 2 + 9 = 11... Có 5 cặp, vậy tổng là 5 × 11 = 55. Em về chỗ đi.",
            "Ủa ông không cần cộng lần lượt mà tính ra được luôn hả Lươn?",
            "Mẹo cả thôi ông ơi, đi thi có câu này không đã!"},()=>StartCoroutine(FinishTerm()));
    }
    private IEnumerator FinishTerm()
    {
        yield return journey.FadeTo(1,.3f);journey.MovePlayer(seat.position);
        if(damSurprise!=null)damSurprise.SetActive(false);
        CurrentPhase=Phase.AcademicCard;Credits=32;
        academicPanel.SetActive(true);journey.RevealCompletion();
    }
    public void ContinueAcademic()
    {
        if(CurrentPhase!=Phase.AcademicCard || advancing)return;
        if(CampaignActive){ContinueCampaignAcademic();return;}
        advancing=true;academicPanel.SetActive(false);StartCoroutine(BeginFeast());
    }
    private IEnumerator BeginFeast()
    {
        yield return journey.FadeTo(1,.3f);journey.RestoreRoom();journey.MovePlayer(feastArrival.position);
        if(story.roommatesGroup!=null)story.roommatesGroup.SetActive(false);
        feast.SetActive(true);SetActorVisible(false);
        story.globalLight.color=story.duskColor;
        if(roomBackdrop!=null)roomBackdrop.color=new Color(1f,.82f,.66f,1f);
        feastAudio.clip=boiling;feastAudio.loop=true;feastAudio.volume=.3f;feastAudio.Play();
        CurrentPhase=Phase.Hotpot;yield return journey.FadeTo(0,.3f);journey.RevealCompletion();advancing=false;
        ShowDialogue(new[]{"Phùng Canh Tộ","Trần Gia Bảo Long","Ninh Đạm","Lê Văn Lươn","Cả phòng"},new[]{
            "Bếp từ đây! Thêm mì, rau và thịt. Ngồi ăn với cả phòng, kết thúc Năm 1 nào!",
            "Ăn xong làm vài ván game không? Tui mới tìm được team kéo rank ngon lắm!",
            "Sang năm môn đại cương nặng hơn đó. Mấy ông đừng chủ quan nhé.",
            "Năm nay có chuyện vui lẫn chuyện phải rút kinh nghiệm. Mai mình tính tiếp.",
            "Mừng Năm 1! Cạn ly nước ngọt nào!"},()=>StartCoroutine(EnterYearTwo()));
    }
    private void SetActorVisible(bool value)
    { foreach(var renderer in Player.CharacterVisual.GetComponentsInChildren<SpriteRenderer>())renderer.enabled=value && renderer.gameObject.name!="Desk_UpperBody"; }
    private IEnumerator EnterYearTwo()
    {
        if(CampaignActive && !campaign.CompleteFeastAndEnterYearTwo())yield break;
        HeardHubris=true;feastAudio.PlayOneShot(clink);yield return new WaitForSeconds(.7f);
        yield return journey.FadeTo(1,.35f);
        feastAudio.Stop();feast.SetActive(false);SetActorVisible(true);journey.RestoreRoom();
        if(CampaignActive && story.roommatesGroup!=null)story.roommatesGroup.SetActive(true);
        if(roomBackdrop!=null)roomBackdrop.color=roomDayColor;
        CurrentPhase=CampaignActive?Phase.Evening:Phase.YearTwo;
        yield return journey.FadeTo(0,.35f);journey.FinishJourneyTransition();
        Player.CharacterVisual.transform.localScale=roomActorScale;
        if(!CampaignActive)story.CompleteExtendedYearOne();
        SetRoomNight(true);
        if(CampaignActive)campaignUI.Runtime.Show("Bữa lẩu kết thúc","Tới giường và chọn Ngủ để sang Năm 2. Kết quả học và môn nợ sẽ được giữ nguyên.",()=>{});
    }
    private void ShowDialogue(string[] speakers,string[] messages,System.Action completed)
    {
        names=speakers;lines=messages;line=0;afterDialogue=completed;
        var rect=conversationPanel.GetComponent<RectTransform>();
        rect.anchorMin=rect.anchorMax=new Vector2(.5f,CurrentPhase==Phase.Hotpot?1:0);
        rect.anchoredPosition=new Vector2(0,CurrentPhase==Phase.Hotpot?-115:115);
        conversationPanel.SetActive(true);ShowLine();
    }
    private void ShowLine(){speaker.text=names[line];dialogue.text=lines[line];}
    public void AdvanceDialogue()
    {
        if(!DialogueVisible)return;
        line++;if(line<lines.Length){ShowLine();return;}
        conversationPanel.SetActive(false);var callback=afterDialogue;afterDialogue=null;callback?.Invoke();
    }
    private void Update()
    {
        if(CampaignActive)UpdateCampaign();
        if(CurrentPhase==Phase.YearTwo && nightTinted && story.currentStage!=MVPFlowController.Stage.Stage2_Year2_Temptation)SetRoomNight(false);
        if(CurrentPhase==Phase.Hotpot && potFrames.Length>0)pot.sprite=potFrames[(int)(Time.time*3)%potFrames.Length];
        if(Keyboard.current==null || !Keyboard.current.spaceKey.wasPressedThisFrame)return;
        if(DialogueVisible)AdvanceDialogue();else if(AcademicVisible)ContinueAcademic();
    }
    private void SetRoomNight(bool value)
    {
        nightTinted=value;
        for(int i=0;i<roomVisuals.Length;i++)if(roomVisuals[i]!=null)
            roomVisuals[i].color=value?roomColors[i]*new Color(.28f,.34f,.56f,1):roomColors[i];
    }
    private void OnDisable()
    {
        StopAllCoroutines();if(feastAudio!=null)feastAudio.Stop();
        if(CampaignActive && CurrentPhase!=Phase.YearTwo){if(deskMenu!=null)deskMenu.Close();if(longActor!=null)longActor.StopActivity();if(Player!=null && !Player.CanInteract)Player.ExitInteraction();}
        if(OwnsPresentation && Player!=null){SetActorVisible(true);if(feast!=null)feast.SetActive(false);}
        if(OwnsPresentation && journey!=null && journey.IsBusy)journey.FinishJourneyTransition();
        if(Player!=null && roomActorScale!=Vector3.zero)Player.CharacterVisual.transform.localScale=roomActorScale;
        if(nightTinted)SetRoomNight(false);
    }
    private void OnDestroy()
    {
        if(story!=null){story.BedClaimed-=OnCampaignClaim;story.RoomCleaned-=OnCampaignClean;story.RoommatesIntroduced-=OnCampaignFriends;story.CanCompleteYearOne=null;}
        if(journey!=null)journey.CampaignCanTravel=null;
        if(commute!=null)commute.CampaignArrival=null;
        if(boiling!=null)Destroy(boiling);if(clink!=null)Destroy(clink);
    }
}
