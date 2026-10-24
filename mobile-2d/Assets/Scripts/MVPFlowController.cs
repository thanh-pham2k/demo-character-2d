using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using TMPro;

public class MVPFlowController : MonoBehaviour
{
    public static MVPFlowController Instance;

    public enum Stage
    {
        Stage1_Year1_Arrival,
        Stage2_Year2_Temptation,
        Stage3_Year5_Awakening,
        Stage4_Year6_Graduation,
        Finished
    }

    [Header("Current State")]
    public Stage currentStage = Stage.Stage1_Year1_Arrival;

    [Header("Environment & Lighting")]
    public Light2D globalLight;
    public Light2D roomCeilingLight;
    public Light2D laptopLight;
    public Color dayColor = new Color(1f, 0.98f, 0.9f, 1f);
    public Color duskColor = new Color(0.95f, 0.65f, 0.4f, 1f);
    public Color nightColor = new Color(0.12f, 0.15f, 0.35f, 1f);

    [Header("Scene Props")]
    public GameObject backpackProp;
    public GameObject trashPileProp;
    public GameObject diplomaProp;
    public GameObject roommatesGroup; // Dam, Long, To
    public GameObject laptopProp;
    public InteractableProp bedInteractable;
    public InteractableProp trashInteractable;
    public InteractableProp laptopInteractable;
    public InteractableProp lightSwitchInteractable;

    [Header("Player")]
    public GameObject playerObj;
    public LuonCharacterVisual playerVisual;
    public Transform deskSeatTransform;
    public Transform doorTransform;

    [Header("UI References")]
    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI yearText;
    
    [Header("Dialogue Box UI")]
    public GameObject dialoguePanel;
    public Image portraitImage;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueBodyText;
    public Button dialogueNextButton;

    [Header("Modal UI")]
    public GameObject yesNoModal;
    public Button yesButton;
    public Button noButton;
    public TextMeshProUGUI modalPromptText;

    [Header("Grade & Result UI")]
    public GameObject gradeStampPanel;
    public Image gradeStampImage;
    public TextMeshProUGUI gradeTitleText;
    public TextMeshProUGUI gradeSubText;
    public Sprite gradeStampF;
    public Sprite gradeStampA;

    [Header("Uninstall Minigame UI")]
    public GameObject uninstallModal;
    public Slider uninstallProgressBar;
    public TextMeshProUGUI uninstallButtonText;

    [Header("Ending Canvas")]
    public GameObject endingPanel;
    public TextMeshProUGUI endingText;

    [Header("Avatars")]
    public Sprite portraitLuon;
    public Sprite portraitDam;
    public Sprite portraitLong;
    public Sprite portraitTo;

    private int trashCleanCount = 0;
    private bool isHoldingUninstall = false;
    private float uninstallProgress = 0f;
    private Coroutine currentDialogueRoutine;
    private PlayerInteractionStateController playerInteractionState;
    private bool storyPoseActive;

    private void Awake()
    {
        Instance = this;
        ResolvePlayerInteractionState();
    }

    private PlayerInteractionStateController ResolvePlayerInteractionState()
    {
        if (playerInteractionState == null)
            playerInteractionState = PlayerInteractionStateController.GetOrCreate(playerObj);
        return playerInteractionState;
    }

    private void ReleaseStoryPose()
    {
        if (!storyPoseActive) return;
        if (playerInteractionState != null && playerInteractionState.CurrentState == PlayerInteractionState.Sitting &&
            playerInteractionState.ActiveInteractable == null)
            playerInteractionState.ExitInteraction();
        storyPoseActive = false;
    }

    private void OnDisable()
    {
        ReleaseStoryPose();
    }

    private void Start()
    {
        InitStage1();
    }

    private void Update()
    {
        // Uninstall holding logic
        if (currentStage == Stage.Stage3_Year5_Awakening && uninstallModal != null && uninstallModal.activeSelf)
        {
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                uninstallProgress += Time.deltaTime / 3.0f; // 3 seconds to complete
                if (uninstallProgressBar != null) uninstallProgressBar.value = uninstallProgress;
                if (uninstallProgress >= 1f)
                {
                    OnUninstallCompleted();
                }
            }
            else
            {
                if (uninstallProgress > 0f)
                {
                    uninstallProgress = Mathf.Max(0f, uninstallProgress - Time.deltaTime * 2f);
                    if (uninstallProgressBar != null) uninstallProgressBar.value = uninstallProgress;
                }
            }
        }
    }

    // ========================================================
    // STAGE 1: NĂM 1 - TÂN SINH VIÊN NHẬP HỌC & XÍ GIƯỜNG
    // ========================================================
    private void InitStage1()
    {
        currentStage = Stage.Stage1_Year1_Arrival;
        yearText.text = "NĂM 1 — TÂN SINH VIÊN (201X)";
        timeText.text = "14:20 • THỨ HAI";
        objectiveText.text = "Mục tiêu: Tiến lại chiếc giường góc cửa sổ và bấm [E] để xí giường.";

        if (globalLight != null) globalLight.color = dayColor;
        if (roomCeilingLight != null) roomCeilingLight.intensity = 0.5f;
        if (laptopLight != null) laptopLight.intensity = 0f;

        if (backpackProp != null) backpackProp.SetActive(false);
        if (trashPileProp != null) trashPileProp.SetActive(true);
        if (roommatesGroup != null) roommatesGroup.SetActive(false);
        if (diplomaProp != null) diplomaProp.SetActive(false);

        if (bedInteractable != null)
        {
            bedInteractable.SetInteractable(true);
            bedInteractable.SetPrompt("Bấm [E] để xí giường");
            bedInteractable.onInteract.RemoveAllListeners();
            bedInteractable.onInteract.AddListener(OnClaimBed);
        }

        if (trashInteractable != null)
        {
            trashInteractable.SetInteractable(false);
        }

        if (lightSwitchInteractable != null)
        {
            lightSwitchInteractable.SetInteractable(false);
        }

        if (laptopInteractable != null)
        {
            laptopInteractable.SetInteractable(false);
        }
    }

    public void OnClaimBed()
    {
        if (currentStage != Stage.Stage1_Year1_Arrival) return;

        // Place backpack
        if (backpackProp != null) backpackProp.SetActive(true);
        if (bedInteractable != null) bedInteractable.SetInteractable(false);

        objectiveText.text = "Mục tiêu: Dùng chổi quét sạch đống rác [E] trước khi bạn mới tới.";

        if (trashInteractable != null)
        {
            trashInteractable.SetInteractable(true);
            trashInteractable.SetPrompt("Bấm [E] quét rác");
            trashInteractable.onInteract.RemoveAllListeners();
            trashInteractable.onInteract.AddListener(OnCleanTrash);
        }
    }

    public void OnCleanTrash()
    {
        trashCleanCount++;
        if (trashCleanCount < 3)
        {
            objectiveText.text = $"Mục tiêu: Dọn rác phòng KTX... ({trashCleanCount}/3)";
            if (trashPileProp != null) trashPileProp.transform.localScale *= 0.7f;
        }
        else
        {
            if (trashPileProp != null) trashPileProp.SetActive(false);
            if (trashInteractable != null) trashInteractable.SetInteractable(false);

            StartCoroutine(RoommatesEnterRoutine());
        }
    }

    private IEnumerator RoommatesEnterRoutine()
    {
        objectiveText.text = "Các bạn cùng phòng đang bước vào...";
        yield return new WaitForSeconds(1.0f);

        if (roommatesGroup != null) roommatesGroup.SetActive(true);

        // Sequence dialogue
        yield return StartDialogue("Lê Văn Lươn", portraitLuon, "Phù... giường này sát ổ điện với cửa sổ lộng gió, xí được cái góc ngon nhất phòng rồi!");
        yield return StartDialogue("Trần Gia Bảo Long", portraitLong, "Hê lô đồng bào! Tui là Long, Quận 8. Ai cày Liên Minh hay Dota tối nay lập team kéo rank liền nha!");
        yield return StartDialogue("Ninh Đạm", portraitDam, "Chào mấy ông. Tôi là Đạm, học Tự Nhiên. Phòng mình 11 giờ đêm tắt đèn để học bài nha.");
        yield return StartDialogue("Phùng Canh Tộ", portraitTo, "Chào cả nhà iu~ Tui là Tộ, mang chùm mận trên quê Cao Bằng xuống nè, tí ăn xong rửa bát phụ tui nghen!");

        yield return new WaitForSeconds(0.5f);
        TransitionToStage2();
    }

    // ========================================================
    // STAGE 2: NĂM 2 - CÁM DỖ THÂU ĐÊM & RỚT MÔN
    // ========================================================
    private void TransitionToStage2()
    {
        currentStage = Stage.Stage2_Year2_Temptation;
        yearText.text = "NĂM 2 — HAM CHƠI & BẪY THỨC ĐÊM (201X+1)";
        timeText.text = "23:40 • ĐÊM KHUYA";
        objectiveText.text = "Mục tiêu: Nghe lời rủ rê của Long ở bàn máy tính.";

        // Lighting transitions to night
        if (globalLight != null) globalLight.color = nightColor;
        if (roomCeilingLight != null) roomCeilingLight.intensity = 0.2f;
        if (laptopLight != null) laptopLight.intensity = 1.2f;

        StartCoroutine(Stage2TemptationRoutine());
    }

    private IEnumerator Stage2TemptationRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        yield return StartDialogue("Trần Gia Bảo Long", portraitLong, "Lươn ơi! Làm nốt ván nữa đi mạy! Đang chuỗi thắng 5 trận, bỏ là mất chuỗi uổng lắm!");

        // Show YES / NO Modal
        modalPromptText.text = "Long rủ làm thêm ván game thâu đêm... Bạn chọn?";
        yesNoModal.SetActive(true);

        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(OnChooseYesLateGame);

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() =>
        {
            // Peer pressure pushes YES
            StartCoroutine(StartDialogue("Trần Gia Bảo Long", portraitLong, "Hèn thế ông nội! Đàn ông Bách Khoa cày thâu đêm mai ngủ bù có sao đâu! Chơi đi!!"));
        });
    }

    public void OnChooseYesLateGame()
    {
        yesNoModal.SetActive(false);
        StartCoroutine(TimeLapseAndOversleepRoutine());
    }

    private IEnumerator TimeLapseAndOversleepRoutine()
    {
        objectiveText.text = "Đang cày game thâu đêm...";
        yield return StartDialogue("Lê Văn Lươn", portraitLuon, "Thôi được rồi, đúng 1 ván nữa thôi đó nha!!");

        // Time lapse
        timeText.text = "01:15 • GIAO TRANH NGHẸT THỞ";
        yield return new WaitForSeconds(1.0f);
        timeText.text = "03:42 • THUA NGƯỢC CAY CÚ";
        yield return new WaitForSeconds(1.0f);
        timeText.text = "05:10 • MẮT DÍU LẠI...";
        yield return new WaitForSeconds(1.0f);

        // Morning 06:30 Alarm
        timeText.text = "06:30 • CHUÔNG BÁO THỨC ĐI THI";
        objectiveText.text = "Chuông reo! Bấm phím [SPACE] để SNOOZE ngủ tiếp!";

        float snoozeTimer = 0f;
        bool snoozed = false;
        while (snoozeTimer < 4.0f)
        {
            snoozeTimer += Time.deltaTime;
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                snoozed = true;
                break;
            }
            yield return null;
        }

        timeText.text = "11:47 • BẬT DẬY TRỄ GIỜ THI";
        yield return StartDialogue("Lê Văn Lươn", portraitLuon, "Á á á! 11 giờ rưỡi rồi?! Tiêu rồi, ca thi Giải Tích 2 bắt đầu từ lúc 7 rưỡi sáng!");

        // Drop Grade F
        gradeStampImage.sprite = gradeStampF;
        gradeTitleText.text = "CẢNH BÁO HỌC VỤ: RỚT MÔN GIẢI TÍCH 2";
        gradeSubText.text = "Điểm: 1.5 (F) • Mất môn tiên quyết • Không thể đăng ký đồ án đúng hạn!";
        gradeStampPanel.SetActive(true);

        yield return new WaitForSeconds(3.5f);
        gradeStampPanel.SetActive(false);

        TransitionToStage3();
    }

    // ========================================================
    // STAGE 3: NĂM 5 - NGƯỜI Ở LẠI & THỨC TỈNH
    // ========================================================
    private void TransitionToStage3()
    {
        currentStage = Stage.Stage3_Year5_Awakening;
        yearText.text = "NĂM 5 — NGƯỜI Ở LẠI & THỨC TỈNH (201X+4)";
        timeText.text = "19:00 • PHÒNG VẮNG TANH";
        objectiveText.text = "Mục tiêu: Đạm, Long, Tộ đã tốt nghiệp ra trường. Tiến lại bàn laptop bấm [E].";

        // Roommates left
        if (roommatesGroup != null) roommatesGroup.SetActive(false);
        if (globalLight != null) globalLight.color = duskColor;

        if (laptopInteractable != null)
        {
            laptopInteractable.SetInteractable(true);
            laptopInteractable.SetPrompt("Bấm [E] mở máy tính");
            laptopInteractable.onInteract.RemoveAllListeners();
            laptopInteractable.onInteract.AddListener(OnInteractLaptopAwakening);
        }
    }

    public void OnInteractLaptopAwakening()
    {
        if (laptopInteractable != null) laptopInteractable.SetInteractable(false);

        StartCoroutine(AwakeningSequenceRoutine());
    }

    private IEnumerator AwakeningSequenceRoutine()
    {
        // The story requests a pose; the player foundation owns positioning, visual and movement locks.
        if (!storyPoseActive && deskSeatTransform != null)
        {
            var interaction = ResolvePlayerInteractionState();
            if (interaction != null)
                storyPoseActive = interaction.TryEnterState(PlayerInteractionState.Sitting, deskSeatTransform);
        }

        yield return StartDialogue("Lê Văn Lươn", portraitLuon, "Mấy đứa tụi nó đi làm hết rồi... Giờ chỉ còn mỗi mình ở lại căn phòng 404 này cày nợ môn.");
        yield return StartDialogue("Lê Văn Lươn", portraitLuon, "Không thể buông thả như thế này mãi được. Mẹ ở quê tóc bạc thêm rồi!");

        // Show Uninstall Minigame
        uninstallProgress = 0f;
        uninstallProgressBar.value = 0f;
        uninstallModal.SetActive(true);
        objectiveText.text = "Mục tiêu: ĐÈ GIỮ CHUỘT TRÁI để gỡ cài đặt game!";
    }

    private void OnUninstallCompleted()
    {
        uninstallModal.SetActive(false);
        ReleaseStoryPose();

        StartCoroutine(PostUninstallRoutine());
    }

    private IEnumerator PostUninstallRoutine()
    {
        objectiveText.text = "Đã gỡ game! Bắt đầu chuỗi ngày thức trắng làm đồ án tốt nghiệp...";
        yield return StartDialogue("Lê Văn Lươn", portraitLuon, "Xong! Xóa sạch game. Từ nay mỗi sáng dậy từ 6h00 không báo thức, quyết lấy bằng Bách Khoa!");

        timeText.text = "THÁNG 6 • NĂM THỨ 6";
        yield return new WaitForSeconds(2.0f);

        TransitionToStage4();
    }

    // ========================================================
    // STAGE 4: NĂM 6 - BẢO VỆ ĐỒ ÁN & TẮT ĐÈN HẠ MÀN
    // ========================================================
    private void TransitionToStage4()
    {
        currentStage = Stage.Stage4_Year6_Graduation;
        yearText.text = "NĂM 6 — BẢO VỆ TỐT NGHIỆP & CHIA TAY (201X+5)";
        timeText.text = "17:30 • BUỔI CHIỀU CUỐI CÙNG";
        objectiveText.text = "Chúc mừng! Bạn đã hoàn thành bảo vệ đồ án!";

        // Show Grade A
        gradeStampImage.sprite = gradeStampA;
        gradeTitleText.text = "BẢO VỆ ĐỒ ÁN TỐT NGHIỆP XUẤT SẮC";
        gradeSubText.text = "Điểm: 9.0 (A) • Đủ điều kiện công nhận tốt nghiệp Kỹ sư CNTT Bách Khoa!";
        gradeStampPanel.SetActive(true);

        if (diplomaProp != null) diplomaProp.SetActive(true);

        StartCoroutine(FinalGraduationRoutine());
    }

    private IEnumerator FinalGraduationRoutine()
    {
        yield return new WaitForSeconds(3.5f);
        gradeStampPanel.SetActive(false);

        yield return StartDialogue("Lê Văn Lươn", portraitLuon, "Cuối cùng... 6 năm ròng rã, tao cũng cầm được tấm bằng đỏ Bách Khoa trên tay rồi!");

        objectiveText.text = "Mục tiêu cuối cùng: Bấm [E] tại công tắc đèn gần cửa để tắt đèn phòng 404.";

        if (lightSwitchInteractable != null)
        {
            lightSwitchInteractable.SetInteractable(true);
            lightSwitchInteractable.SetPrompt("Bấm [E] tắt đèn phòng 404");
            lightSwitchInteractable.onInteract.RemoveAllListeners();
            lightSwitchInteractable.onInteract.AddListener(OnTurnOffLightsFinal);
        }
    }

    public void OnTurnOffLightsFinal()
    {
        if (currentStage != Stage.Stage4_Year6_Graduation) return;
        currentStage = Stage.Finished;

        if (lightSwitchInteractable != null) lightSwitchInteractable.SetInteractable(false);

        // Lights out completely!
        if (globalLight != null) globalLight.intensity = 0.05f;
        if (roomCeilingLight != null) roomCeilingLight.intensity = 0f;
        if (laptopLight != null) laptopLight.intensity = 0f;

        StartCoroutine(EndingSequenceRoutine());
    }

    private IEnumerator EndingSequenceRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        if (endingPanel != null) endingPanel.SetActive(true);
        if (endingText != null)
        {
            endingText.text = "Căn phòng 404 chìm vào bóng tối...\n\n" +
                              "6 năm thanh xuân, bao nhiêu lần vấp ngã và tự đứng dậy.\n" +
                              "Cảm ơn tuổi trẻ tại Ký Túc Xá ĐHQG TP.HCM.\n\n" +
                              "— BẢN VERY MVP HOÀN TẤT THÀNH CÔNG —";
        }

        objectiveText.text = "HOÀN THÀNH DEMO VERY MVP";
    }

    // ========================================================
    // HELPER: DIALOGUE COROUTINE
    // ========================================================
    private IEnumerator StartDialogue(string speaker, Sprite portrait, string message)
    {
        if (dialoguePanel == null) yield break;

        dialoguePanel.SetActive(true);
        speakerNameText.text = speaker;
        dialogueBodyText.text = message;
        portraitImage.sprite = portrait;

        bool hasProceeded = false;
        dialogueNextButton.onClick.RemoveAllListeners();
        dialogueNextButton.onClick.AddListener(() => hasProceeded = true);

        // Allow Space or Enter or Click
        float readWait = 0.5f;
        while (!hasProceeded)
        {
            readWait -= Time.deltaTime;
            if (readWait <= 0f && Keyboard.current != null && 
               (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.eKey.wasPressedThisFrame))
            {
                hasProceeded = true;
            }
            yield return null;
        }

        dialoguePanel.SetActive(false);
    }
}
