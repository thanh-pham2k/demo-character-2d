using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DeskActionModal : MonoBehaviour
{
    public enum DeskAction { None, Computer, Study }
    [SerializeField] private GameObject panel, bookIcon;
    [SerializeField] private TMP_Text status;
    [SerializeField] private Slider studyProgress;
    [SerializeField] private Button computerButton, studyButton, standButton;
    [SerializeField] private Light2D laptopLight;
    [SerializeField] private AudioSource typingAudio;
    [SerializeField] private DormJourneyController journey;
    [SerializeField] private MVPFlowController story;
    private ChairInteractable chair;
    private PlayerInteractionStateController player;
    [SerializeField] private YearOneCampaignUI campaignUI;
    public void ConfigureCampaign(YearOneCampaignUI ui)=>campaignUI=ui;
    private CanvasGroup panelPresentation;
    private ComputerDesktopUI desktop;
    public ComputerDesktopUI Desktop => desktop;
    public bool ComputerVisible => desktop != null && desktop.Visible;
    private float previousLight;
    private MVPFlowController.Stage openedStage;
    public Button StandButton => standButton;
    [SerializeField] private SpriteRenderer laptopScreenGlow;
    private AudioClip typingClip, writingClip;
    public bool IsWriting => CurrentAction == DeskAction.Study && typingAudio != null && typingAudio.isPlaying;
    public void ConfigureScreenGlow(SpriteRenderer screen) => laptopScreenGlow = screen;
    public bool IsOpen => chair != null && panel != null && panel.activeSelf;
    public DeskAction CurrentAction { get; private set; }
    public bool IsTyping => CurrentAction == DeskAction.Computer && typingAudio != null && typingAudio.isPlaying;
    public bool IsBookVisible => bookIcon != null && bookIcon.activeSelf;
    public void Configure(GameObject modalPanel, GameObject book, TMP_Text label, Slider progress,
        Button computer, Button study, Button stand, Light2D light, AudioSource audio, DormJourneyController controller, MVPFlowController flow)
    {
        panel = modalPanel; bookIcon = book; status = label; studyProgress = progress;
        computerButton = computer; studyButton = study; standButton = stand; laptopLight = light;
        typingAudio = audio; journey = controller; story = flow;
    }
    private void Awake()
    {
        computerButton.onClick.AddListener(UseComputer);
        studyButton.onClick.AddListener(Study);
        standButton.onClick.AddListener(StandUp);
        typingClip = DormSound.CreateTyping();
        writingClip = DormSound.CreateWriting();
        typingAudio.clip = typingClip; typingAudio.loop = true; typingAudio.volume = .2f;
        panel.SetActive(false);
        bookIcon.SetActive(false);
    }
    public void Open(ChairInteractable owner, PlayerInteractionStateController actor)
    {
        if (actor == null || owner == null || owner.Occupant != actor) return;
        Close(); chair = owner; player = actor;
        previousLight = laptopLight != null ? laptopLight.intensity : 0f;
        if (story != null) openedStage = story.currentStage;
        CurrentAction = DeskAction.None;
        panel.SetActive(true);
        status.text = "Chọn hoạt động tại bàn học";
    }
    public void UseComputer()
    {
        if (!IsOpen || chair.Occupant != player) return;
        CurrentAction = DeskAction.Computer;
        if (laptopLight != null) laptopLight.intensity = 1.2f;
        typingAudio.clip = typingClip; typingAudio.Play(); bookIcon.SetActive(false);
        player.CharacterVisual?.SetDeskPose(LuonCharacterVisual.DeskPoseState.SittingTyping);
        if (laptopScreenGlow != null) laptopScreenGlow.enabled = true;
        status.text = "Đang dùng máy tính • Gõ phím / chơi game";
        if(campaignUI!=null && campaignUI.Active)
        {
            if(desktop==null)
            {
                var controller=FindAnyObjectByType<YearOneCampaignController>();
                desktop=gameObject.AddComponent<ComputerDesktopUI>();
                desktop.Initialize(this,controller,campaignUI,status.font);
            }
            if(!desktop.Visible)desktop.Open();
        }
    }
    public void TurnOffComputer()
    {
        if(typingAudio!=null)typingAudio.Stop();
        if(player!=null)player.CharacterVisual?.SetDeskPose(LuonCharacterVisual.DeskPoseState.None);
        if(laptopScreenGlow!=null)laptopScreenGlow.enabled=false;
        if(laptopLight!=null && (story==null || story.currentStage==openedStage))laptopLight.intensity=previousLight;
        if(bookIcon!=null)bookIcon.SetActive(false);
        CurrentAction=DeskAction.None;
        if(status!=null)status.text="Máy tính đã tắt • Chọn hoạt động tại bàn";
    }
    public void Study()
    {
        if (!IsOpen || chair.Occupant != player) return;
        if(ComputerVisible)return;
        if(campaignUI!=null && campaignUI.Active){campaignUI.ShowStudy();return;}
        CurrentAction = DeskAction.Study;
        typingAudio.clip = writingClip; typingAudio.Play(); bookIcon.SetActive(true);
        player.CharacterVisual?.SetDeskPose(LuonCharacterVisual.DeskPoseState.SittingStudying);
        if (laptopScreenGlow != null) laptopScreenGlow.enabled = false;
        if (laptopLight != null && (story == null || story.currentStage == openedStage)) laptopLight.intensity = previousLight;
        status.text = "Đang học Giải Tích • Tiến độ tăng dần";
    }
    public void StandUp() { if (chair != null && player != null) chair.Interact(player); }
    public void Close(ChairInteractable owner = null)
    {
        if (owner != null && chair != owner) return;
        if(desktop!=null)desktop.Close();
        if(campaignUI!=null)campaignUI.HideStudy();
        if (chair != null && laptopLight != null && CurrentAction == DeskAction.Computer && (story == null || story.currentStage == openedStage) && Mathf.Approximately(laptopLight.intensity, 1.2f))
            laptopLight.intensity = previousLight;
        if (player != null) player.CharacterVisual?.SetDeskPose(LuonCharacterVisual.DeskPoseState.None);
        if (laptopScreenGlow != null) laptopScreenGlow.enabled = false;
        if (typingAudio != null) typingAudio.Stop();
        if (panel != null) panel.SetActive(false);
        if (bookIcon != null) bookIcon.SetActive(false);
        chair = null; player = null; CurrentAction = DeskAction.None;
    }
    private void Update()
    {
        if (chair == null) return;
        if (player == null || chair.Occupant != player) { Close(); return; }
        // Existing story modals take priority; do not consume their interaction.
        bool storyModal = story != null && (story.dialoguePanel.activeSelf || story.yesNoModal.activeSelf || story.uninstallModal.activeSelf || story.endingPanel.activeSelf || story.gradeStampPanel.activeSelf);
        if(storyModal && desktop!=null)desktop.Close();
        panel.SetActive(!storyModal);
        if(panelPresentation==null){panelPresentation=panel.GetComponent<CanvasGroup>();if(panelPresentation==null)panelPresentation=panel.AddComponent<CanvasGroup>();}
        bool hidePanel=desktop!=null && desktop.InRoomSession;
        panelPresentation.alpha=hidePanel?0:1;panelPresentation.interactable=!hidePanel;panelPresentation.blocksRaycasts=!hidePanel;
        if (storyModal) { if (typingAudio.isPlaying) typingAudio.Stop(); return; }
        var keyboard = Keyboard.current;
        bool menuInput=!(desktop!=null && desktop.InRoomSession) && !ComputerVisible && (campaignUI==null || !campaignUI.IsModalOpen);
        if (menuInput && keyboard != null && (keyboard.digit1Key.wasPressedThisFrame || keyboard.numpad1Key.wasPressedThisFrame)) UseComputer();
        if (menuInput && keyboard != null && (keyboard.digit2Key.wasPressedThisFrame || keyboard.numpad2Key.wasPressedThisFrame)) Study();
        if (CurrentAction == DeskAction.Study && journey != null && (campaignUI==null || !campaignUI.Active)) journey.AddStudyProgress(Time.deltaTime * .025f);
        if(CurrentAction==DeskAction.Computer && player!=null)typingAudio.volume=DeskPoseTiming.IsKeyOrClick(player.CharacterVisual.DeskAnimationFrame)?.2f:0;
        if (studyProgress != null && journey != null) studyProgress.value = campaignUI!=null && campaignUI.Active?campaignUI.SelectedCourseProgress:journey.StudyProgress;
    }
    private void OnDisable() => Close();
    public void BeginCoursePose(YearOneSchedule.Course course)
    {
        if(!IsOpen || course==null)return;
        CurrentAction=course.UsesComputer?DeskAction.Computer:DeskAction.Study;
        typingAudio.clip=course.UsesComputer?typingClip:writingClip;typingAudio.volume=.2f;typingAudio.Play();
        bookIcon.SetActive(!course.UsesComputer);
        if(laptopScreenGlow!=null)laptopScreenGlow.enabled=course.UsesComputer;
        if(laptopLight!=null)laptopLight.intensity=course.UsesComputer?1.2f:previousLight;
        player.CharacterVisual.SetDeskPose(course.UsesComputer?LuonCharacterVisual.DeskPoseState.SittingTyping:LuonCharacterVisual.DeskPoseState.SittingStudying);
        status.text="Đang học • "+course.Name;
    }
    private void OnDestroy() { if (typingClip != null) Destroy(typingClip); if (writingClip != null) Destroy(writingClip); }
}
