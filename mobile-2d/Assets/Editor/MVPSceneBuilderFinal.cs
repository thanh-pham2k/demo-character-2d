using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;

public static class MVPSceneBuilderFinal
{
    // With its skew corrected, the rear tabletop edge sits just below the window sill.
    private static readonly Vector3 DeskPosition = new Vector3(-0.95f, 0.16f, 0f);
    private static readonly Vector3 LaptopPosition = DeskPosition + new Vector3(0f, 0.53f, 0f);
    private static readonly Vector3 LaptopGlowPosition = DeskPosition + new Vector3(0f, 0.6f, 0f);
    private static readonly Vector3 DiplomaPosition = DeskPosition + new Vector3(0.45f, 0.25f, 0f);
    private static readonly Vector3 DeskSeatPosition = DeskPosition + new Vector3(0f, -0.35f, 0f);
    private static readonly Vector3 FanPosition = new Vector3(0f, 2.65f, 0f);
    private static readonly Vector3 FanRotorOffset = new Vector3(0f, 0.14f, 0f);
    private static readonly Vector3 SocketPosition = new Vector3(0.65f, 1f, 0f);
    private const float FanScale = 0.3f;
    private const float SocketScale = 0.55f;
    private const float LaptopScale = 0.1f;

    [MenuItem("KTX404/Build Room 404 Scene", true)]
    private static bool CanBuildScene() => !EditorApplication.isPlayingOrWillChangePlaymode;

    [MenuItem("KTX404/Build Room 404 Scene")]
    public static void BuildScene()
    {
        if(EditorApplication.isPlayingOrWillChangePlaymode)return;
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 1. Camera setup - Perfectly centered isometric view
        GameObject camObj = new GameObject("Main Camera");
        Camera cam = camObj.AddComponent<Camera>();
        camObj.tag = "MainCamera";
        cam.orthographic = true;
        // The backdrop is 1037x627 at 100 PPU; fit it to the game view.
        cam.orthographicSize = 3.15f;
        camObj.transform.position = new Vector3(0, 0, -10);
        cam.backgroundColor = Color.white;
        cam.clearFlags = CameraClearFlags.SolidColor;
        camObj.AddComponent<AudioListener>();
        camObj.AddComponent<UniversalAdditionalCameraData>();

        // 2. Lights setup
        GameObject lightsRoot = new GameObject("--- LIGHTS ---");
        
        GameObject globalLightObj = new GameObject("Global 2D Light");
        globalLightObj.transform.SetParent(lightsRoot.transform);
        Light2D globalLight = globalLightObj.AddComponent<Light2D>();
        globalLight.lightType = Light2D.LightType.Global;
        globalLight.color = new Color(1f, 0.98f, 0.92f, 1f);
        globalLight.intensity = 1.05f;

        GameObject ceilingLightObj = new GameObject("Ceiling Light");
        ceilingLightObj.transform.SetParent(lightsRoot.transform);
        ceilingLightObj.transform.position = new Vector3(0, 2.0f, 0);
        Light2D ceilingLight = ceilingLightObj.AddComponent<Light2D>();
        ceilingLight.lightType = Light2D.LightType.Point;
        ceilingLight.pointLightOuterRadius = 7.0f;
        ceilingLight.pointLightInnerRadius = 1.8f;
        ceilingLight.color = new Color(1f, 0.95f, 0.85f, 1f);
        ceilingLight.intensity = 0.6f;

        GameObject laptopLightObj = new GameObject("Laptop Screen Glow");
        laptopLightObj.transform.SetParent(lightsRoot.transform);
        laptopLightObj.transform.position = LaptopGlowPosition;
        Light2D laptopLight = laptopLightObj.AddComponent<Light2D>();
        laptopLight.lightType = Light2D.LightType.Point;
        laptopLight.pointLightOuterRadius = 1.6f;
        laptopLight.pointLightInnerRadius = 0.2f;
        laptopLight.color = new Color(0.35f, 0.75f, 1f, 1f);
        laptopLight.intensity = 0.0f;

        // 3. Load Sprites
        Sprite backdropSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/environment/room_404_backdrop.png");
        Sprite bunkBedSprite = LoadLengthwiseBunkBedSprite();
        Sprite deskSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/environment/study_desk_chairs.png");
        Sprite fanRodSprite = LoadCenteredFanSprite("Assets/Sprites/MVP/environment/fan_mount_stem.png");
        Sprite fanBladesSprite = LoadCenteredFanSprite("Assets/Sprites/MVP/environment/fan_rotor.png");
        Sprite cleanSocketSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/environment/wall_socket_clean.png");

        Sprite shadowChar = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/shadows/shadow_character.png");
        Sprite shadowDesk = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/shadows/shadow_desk.png");
        Sprite shadowBed = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/shadows/shadow_bed.png");
        Sprite shadowSmall = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/shadows/shadow_small.png");

        // 4. Environment
        GameObject envRoot = new GameObject("--- ENVIRONMENT ---");

        // Backdrop
        GameObject backdropObj = new GameObject("Room_Backdrop");
        backdropObj.transform.SetParent(envRoot.transform);
        backdropObj.transform.position = Vector3.zero;
        SpriteRenderer backdropSr = backdropObj.AddComponent<SpriteRenderer>();
        backdropSr.sprite = backdropSprite;
        backdropSr.sortingOrder = -100;
        CreateOrUpdateRoomDoor(backdropSr, envRoot.transform);

        // One bunk on each side wall, mirrored across the room center.
        InteractableProp bedInteract = CreateBunkBed(
            envRoot.transform, "BunkBed_Left", bunkBedSprite, true, shadowBed, true);
        CreateBunkBed(
            envRoot.transform, "BunkBed_Right", bunkBedSprite, false, shadowBed, false);

        // Rear legs meet the back wall below the window, leaving the doorway clear.
        GameObject deskObj = new GameObject("Study_Desk");
        deskObj.transform.SetParent(envRoot.transform);
        deskObj.transform.position = DeskPosition;
        deskObj.transform.localScale = new Vector3(0.42f, 0.42f, 1f); // Proportional to room
        SpriteRenderer deskSr = deskObj.AddComponent<SpriteRenderer>();
        deskSr.sprite = deskSprite;
        deskSr.sortingOrder = -10;
        deskSr.sharedMaterial = GetProjectionMaterial("DeskProjection", -60f / 647f);
        BoxCollider2D deskCol = deskObj.AddComponent<BoxCollider2D>();
        deskCol.size = new Vector2(4.5f, 1.8f);
        deskCol.offset = new Vector2(0, -0.6f);
        CreateShadow(deskObj, shadowDesk, new Vector3(0, -1.5f, 0), new Vector3(2.6f, 1.5f, 1f), -15);

        // Ceiling Fan: Separated into Fixed Mount Rod + Tilting Spinning Blades
        GameObject fanObj = new GameObject("Ceiling_Fan");
        fanObj.transform.SetParent(envRoot.transform);
        fanObj.transform.position = FanPosition;
        fanObj.transform.localScale = new Vector3(FanScale, FanScale, 1f);

        // Mount Rod (Stationary)
        GameObject rodObj = new GameObject("Mount_Rod");
        rodObj.transform.SetParent(fanObj.transform);
        rodObj.transform.localPosition = Vector3.zero;
        rodObj.transform.localScale = Vector3.one;
        SpriteRenderer rodSr = rodObj.AddComponent<SpriteRenderer>();
        rodSr.sprite = fanRodSprite;
        rodSr.sortingOrder = 200;

        // Blades (Spinning horizontally in isometric plane)
        GameObject bladesObj = new GameObject("Blades_Rotor");
        bladesObj.transform.SetParent(fanObj.transform);
        bladesObj.transform.localPosition = FanRotorOffset; // Join the tilted motor housing to the fixed stem.
        // Undo the source artwork's vertical foreshortening before spinning it in the ceiling plane.
        bladesObj.transform.localScale = new Vector3(1f, 2f, 1f);
        bladesObj.transform.localRotation = Quaternion.Euler(75f, 0f, 0f);
        SpriteRenderer bladesSr = bladesObj.AddComponent<SpriteRenderer>();
        bladesSr.sprite = fanBladesSprite;
        bladesSr.sortingOrder = 201;

        CeilingFanIsometric fanComp = fanObj.AddComponent<CeilingFanIsometric>();
        fanComp.SetBlades(bladesObj.transform);

        // Front-facing socket mounted on the back wall beside the desk, clear of the beds.
        GameObject switchObj = new GameObject("Light_Switch");
        switchObj.transform.SetParent(envRoot.transform);
        switchObj.transform.position = SocketPosition;
        switchObj.transform.localScale = new Vector3(SocketScale, SocketScale, 1f);
        SpriteRenderer switchSr = switchObj.AddComponent<SpriteRenderer>();
        switchSr.sprite = cleanSocketSprite;
        switchSr.sortingOrder = 5;
        switchObj.AddComponent<BoxCollider2D>().isTrigger = true;
        InteractableProp switchInteract = switchObj.AddComponent<InteractableProp>();
        switchInteract.SetPrompt("Bấm [E] tắt đèn phòng 404");

        // 5. Props
        GameObject propsRoot = new GameObject("--- PROPS ---");

        Sprite backpackSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/props/luon_backpack.png");
        Sprite laptopSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/props/laptop_open.png");
        Sprite diplomaSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/props/diploma_degree.png");
        Sprite trashNoodles = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/props/trash_noodles.png");
        Sprite trashCans = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/props/trash_cans.png");
        Sprite trashBags = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/props/trash_bags.png");

        // Backpack on Bed
        GameObject backpackObj = new GameObject("Backpack_Prop");
        backpackObj.transform.SetParent(propsRoot.transform);
        backpackObj.transform.position = new Vector3(-3.3f, -0.1f, 0);
        backpackObj.transform.localScale = new Vector3(0.18f, 0.18f, 1f);
        SpriteRenderer bpSr = backpackObj.AddComponent<SpriteRenderer>();
        bpSr.sprite = backpackSprite;
        bpSr.sortingOrder = -5;
        backpackObj.SetActive(false);

        // Realistic Scattered Trash Clusters (Small, natural, in corners)
        GameObject trashRoot = new GameObject("Trash_Clusters_Group");
        trashRoot.SetActive(false);
        trashRoot.transform.SetParent(propsRoot.transform);
        CreateTrashCluster(trashRoot, "Trash_Noodles", trashNoodles, new Vector3(1.2f, -1.0f, 0), 0.14f, shadowSmall);
        CreateTrashCluster(trashRoot, "Trash_Cans", trashCans, new Vector3(-1.6f, -1.2f, 0), 0.13f, shadowSmall);
        CreateTrashCluster(trashRoot, "Trash_Bags", trashBags, new Vector3(-3.6f, -2.2f, 0), 0.14f, shadowSmall);

        InteractableProp trashInteract = trashRoot.AddComponent<InteractableProp>();
        trashInteract.SetPrompt("Bấm [E] quét dọn rác");
        CircleCollider2D trashCol = trashRoot.AddComponent<CircleCollider2D>();
        trashCol.radius = 2.2f;
        trashCol.isTrigger = true;

        // Laptop - Scaled to REAL LAPTOP RATIO on desk (0.11 scale)
        GameObject laptopObj = new GameObject("Laptop_Prop");
        laptopObj.transform.SetParent(propsRoot.transform);
        laptopObj.transform.position = LaptopPosition;
        laptopObj.transform.localScale = new Vector3(LaptopScale, LaptopScale, 1f);
        SpriteRenderer lapSr = laptopObj.AddComponent<SpriteRenderer>();
        lapSr.sprite = laptopSprite;
        lapSr.sortingOrder = 5;
        CreateShadow(laptopObj, shadowSmall, new Vector3(0, -1.5f, 0), new Vector3(1.0f, 0.6f, 1f), 4);
        laptopObj.AddComponent<BoxCollider2D>().isTrigger = true;
        InteractableProp laptopInteract = laptopObj.AddComponent<InteractableProp>();
        laptopInteract.SetPrompt("Bấm [E] mở máy tính");

        // Diploma Degree
        GameObject diplomaObj = new GameObject("Diploma_Prop");
        diplomaObj.transform.SetParent(propsRoot.transform);
        diplomaObj.transform.position = DiplomaPosition;
        diplomaObj.transform.localScale = new Vector3(0.11f, 0.11f, 1f);
        SpriteRenderer dipSr = diplomaObj.AddComponent<SpriteRenderer>();
        dipSr.sprite = diplomaSprite;
        dipSr.sortingOrder = 5;
        diplomaObj.SetActive(false);

        // 6. Roommates Group
        GameObject roommatesRoot = new GameObject("--- ROOMMATES_GROUP ---");
        Sprite damSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/dam_standing.png");
        Sprite longSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/long_standing.png");
        Sprite toSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/to_standing.png");

        CreateCharacterNPC(roommatesRoot, "Dam_NPC", damSprite, new Vector3(1.6f, -0.8f, 0), 0.62f, shadowChar);
        CreateCharacterNPC(roommatesRoot, "Long_NPC", longSprite, new Vector3(-0.9f, -0.8f, 0), 0.62f, shadowChar);
        CreateCharacterNPC(roommatesRoot, "To_NPC", toSprite, new Vector3(2.5f, 0.1f, 0), 0.62f, shadowChar);
        roommatesRoot.SetActive(false);

        // 7. Player (Lê Văn Lươn) - Prominent, positioned at open center floor
        GameObject playerObj = new GameObject("Player_Luon");
        playerObj.transform.position = new Vector3(-0.2f, -1.8f, 0); // Center floor walking area!
        
        Rigidbody2D playerRb = playerObj.AddComponent<Rigidbody2D>();
        playerRb.gravityScale = 0f;
        playerRb.freezeRotation = true;
        playerRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        CapsuleCollider2D playerCol = playerObj.AddComponent<CapsuleCollider2D>();
        playerCol.size = new Vector2(0.5f, 0.7f);
        playerCol.offset = new Vector2(0, -0.3f);

        Sprite luonIdle = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/luon_idle_0.png");
        Sprite luonWalk0 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/luon_walk_0.png");
        Sprite luonWalk1 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/luon_walk_1.png");
        Sprite luonWalk2 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/luon_walk_2.png");
        Sprite luonWalk3 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/luon_walk_3.png");
        Sprite luonSit = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/luon_sitting_laptop.png");

        CreateShadow(playerObj, shadowChar, new Vector3(0, -0.65f, 0), new Vector3(1.0f, 0.8f, 1f), 0);

        GameObject visualChild = new GameObject("Visual");
        visualChild.transform.SetParent(playerObj.transform);
        visualChild.transform.localPosition = Vector3.zero;
        visualChild.transform.localScale = new Vector3(0.75f, 0.75f, 1f);

        SpriteRenderer playerSr = visualChild.AddComponent<SpriteRenderer>();
        playerSr.sprite = luonIdle;
        playerSr.sortingOrder = 10;

        LuonCharacterVisual playerVisual = visualChild.AddComponent<LuonCharacterVisual>();
        playerVisual.idleSprite = luonIdle;
        playerVisual.walkSprites = new Sprite[] { luonWalk0, luonWalk1, luonWalk2, luonWalk3 };
        playerVisual.sitLaptopSprite = luonSit;

        playerObj.AddComponent<PlayerMovement>();
        playerObj.AddComponent<PlayerInteractionStateController>();
        playerObj.AddComponent<PlayerInteractionDetector>();

        GameObject deskSeat = new GameObject("Desk_Seat_Marker");
        deskSeat.transform.position = DeskSeatPosition;

        // Room Boundary Colliders
        GameObject boundsObj = new GameObject("Room_Colliders");
        boundsObj.transform.SetParent(envRoot.transform);
        CreateColliderBox(boundsObj, new Vector2(0, 2.5f), new Vector2(10f, 1f));
        CreateColliderBox(boundsObj, new Vector2(0, -3.4f), new Vector2(10f, 1f));
        CreateColliderBox(boundsObj, new Vector2(-4.8f, 0), new Vector2(1f, 6.5f));
        CreateColliderBox(boundsObj, new Vector2(4.8f, 0), new Vector2(1f, 6.5f));

        // 8. Build UI Canvas
        GameObject canvasObj = new GameObject("Canvas_MVP");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        GameObject eventSys = new GameObject("EventSystem");
        eventSys.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSys.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();

        Sprite portraitLuon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/portrait_luon.png");
        Sprite portraitDam = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/portrait_dam.png");
        Sprite portraitLong = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/portrait_long.png");
        Sprite portraitTo = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/portrait_to.png");
        Sprite dialogueBoxSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/dialogue_box.png");
        Sprite buttonYesSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/button_yes.png");
        Sprite buttonNoSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/button_no.png");
        Sprite gradeFSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/grade_stamp_f.png");
        Sprite gradeASprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/grade_stamp_a.png");

        GameObject topInfoObj = CreateUIPanel(canvasObj, "TopInfoPanel", new Vector2(0, 1), new Vector2(0, 1), new Vector2(120, -60), new Vector2(500, 100));
        TextMeshProUGUI yearText = CreateTMP(topInfoObj, "YearText", "NĂM 1 — TÂN SINH VIÊN (201X)", 24, TextAlignmentOptions.TopLeft, new Color(1f, 0.85f, 0.4f));
        yearText.rectTransform.anchoredPosition = new Vector2(150, 20);
        TextMeshProUGUI timeText = CreateTMP(topInfoObj, "TimeText", "14:20 • THỨ HAI", 20, TextAlignmentOptions.TopLeft, Color.white);
        timeText.rectTransform.anchoredPosition = new Vector2(150, -15);

        GameObject objectiveObj = CreateUIPanel(canvasObj, "ObjectivePanel", new Vector2(0, 0), new Vector2(0, 0), new Vector2(120, 80), new Vector2(650, 80));
        TextMeshProUGUI objectiveText = CreateTMP(objectiveObj, "ObjectiveText", "Mục tiêu: Đang nạp...", 20, TextAlignmentOptions.Left, new Color(0.9f, 0.95f, 1f));
        objectiveText.rectTransform.anchoredPosition = new Vector2(250, 0);

        GameObject dialoguePanel = CreateUIPanel(canvasObj, "DialoguePanel", new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 160), new Vector2(1100, 260));
        Image dlgBg = dialoguePanel.AddComponent<Image>();
        dlgBg.sprite = dialogueBoxSprite;
        dlgBg.color = new Color(0.12f, 0.12f, 0.16f, 0.95f);

        GameObject portraitObj = CreateUIPanel(dialoguePanel, "Portrait", new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(140, 0), new Vector2(190, 190));
        Image portImg = portraitObj.AddComponent<Image>();
        portImg.sprite = portraitLuon;

        TextMeshProUGUI speakerText = CreateTMP(dialoguePanel, "SpeakerName", "Lê Văn Lươn", 26, TextAlignmentOptions.TopLeft, new Color(1f, 0.85f, 0.2f));
        speakerText.rectTransform.anchoredPosition = new Vector2(500, 80);

        TextMeshProUGUI bodyText = CreateTMP(dialoguePanel, "DialogueBody", "Hội thoại...", 22, TextAlignmentOptions.TopLeft, Color.white);
        bodyText.rectTransform.anchoredPosition = new Vector2(500, 10);
        bodyText.rectTransform.sizeDelta = new Vector2(750, 120);

        GameObject nextBtnObj = CreateUIPanel(dialoguePanel, "NextButton", new Vector2(1, 0), new Vector2(1, 0), new Vector2(-80, 50), new Vector2(120, 50));
        Button nextBtn = nextBtnObj.AddComponent<Button>();
        Image nextBtnImg = nextBtnObj.AddComponent<Image>();
        nextBtnImg.color = new Color(0.3f, 0.5f, 0.8f, 0.8f);
        CreateTMP(nextBtnObj, "NextText", "Tiếp [Space]", 16, TextAlignmentOptions.Center, Color.white);
        dialoguePanel.SetActive(false);

        GameObject yesNoModal = CreateUIPanel(canvasObj, "YesNoModal", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(800, 350));
        Image modalBg = yesNoModal.AddComponent<Image>();
        modalBg.color = new Color(0.08f, 0.08f, 0.12f, 0.98f);
        TextMeshProUGUI modalPrompt = CreateTMP(yesNoModal, "PromptText", "Long rủ làm thêm ván game thâu đêm...", 24, TextAlignmentOptions.Center, Color.white);
        modalPrompt.rectTransform.anchoredPosition = new Vector2(0, 60);

        GameObject yesBtnObj = CreateUIPanel(yesNoModal, "YesButton", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-150, -60), new Vector2(220, 90));
        Button yesBtn = yesBtnObj.AddComponent<Button>();
        Image yesImg = yesBtnObj.AddComponent<Image>();
        yesImg.sprite = buttonYesSprite;

        GameObject noBtnObj = CreateUIPanel(yesNoModal, "NoButton", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(150, -60), new Vector2(200, 70));
        Button noBtn = noBtnObj.AddComponent<Button>();
        Image noImg = noBtnObj.AddComponent<Image>();
        noImg.sprite = buttonNoSprite;
        yesNoModal.SetActive(false);

        GameObject uninstallModal = CreateUIPanel(canvasObj, "UninstallModal", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(700, 300));
        Image unBg = uninstallModal.AddComponent<Image>();
        unBg.color = new Color(0.1f, 0.1f, 0.15f, 0.98f);
        TextMeshProUGUI unPrompt = CreateTMP(uninstallModal, "UnPrompt", "ĐÈ GIỮ CHUỘT TRÁI ĐỂ GỠ CÀI ĐẶT GAME!", 24, TextAlignmentOptions.Center, new Color(1f, 0.4f, 0.4f));
        unPrompt.rectTransform.anchoredPosition = new Vector2(0, 60);

        GameObject sliderObj = CreateUIPanel(uninstallModal, "ProgressBar", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, -30), new Vector2(500, 40));
        Slider unSlider = sliderObj.AddComponent<Slider>();
        Image sliderBg = sliderObj.AddComponent<Image>();
        sliderBg.color = Color.gray;

        GameObject fillArea = CreateUIPanel(sliderObj, "Fill Area", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        GameObject fill = CreateUIPanel(fillArea, "Fill", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        Image fillImg = fill.AddComponent<Image>();
        fillImg.color = new Color(0.2f, 0.8f, 0.2f, 1f);
        unSlider.fillRect = fill.GetComponent<RectTransform>();
        unSlider.value = 0f;
        uninstallModal.SetActive(false);

        GameObject gradePanel = CreateUIPanel(canvasObj, "GradePanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(900, 500));
        Image gradeBg = gradePanel.AddComponent<Image>();
        gradeBg.color = new Color(0, 0, 0, 0.85f);
        
        GameObject stampObj = CreateUIPanel(gradePanel, "StampImage", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 40), new Vector2(250, 250));
        Image stampImg = stampObj.AddComponent<Image>();
        stampImg.sprite = gradeFSprite;

        TextMeshProUGUI gradeTitle = CreateTMP(gradePanel, "GradeTitle", "CẢNH BÁO HỌC VỤ", 28, TextAlignmentOptions.Center, Color.red);
        gradeTitle.rectTransform.anchoredPosition = new Vector2(0, -120);

        TextMeshProUGUI gradeSub = CreateTMP(gradePanel, "GradeSub", "Chi tiết điểm...", 20, TextAlignmentOptions.Center, Color.white);
        gradeSub.rectTransform.anchoredPosition = new Vector2(0, -170);
        gradePanel.SetActive(false);

        GameObject endingPanel = CreateUIPanel(canvasObj, "EndingPanel", Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        Image endBg = endingPanel.AddComponent<Image>();
        endBg.color = Color.black;
        TextMeshProUGUI endingText = CreateTMP(endingPanel, "EndingText", "Căn phòng 404 chìm vào bóng tối...", 28, TextAlignmentOptions.Center, Color.white);
        endingText.rectTransform.anchoredPosition = Vector2.zero;
        endingText.rectTransform.sizeDelta = new Vector2(1000, 400);
        endingPanel.SetActive(false);

        // 9. Flow Controller Setup
        GameObject controllerObj = new GameObject("MVP_Flow_Controller");
        MVPFlowController flow = controllerObj.AddComponent<MVPFlowController>();

        flow.globalLight = globalLight;
        flow.roomCeilingLight = ceilingLight;
        flow.laptopLight = laptopLight;

        flow.backpackProp = backpackObj;
        flow.trashPileProp = trashRoot;
        flow.diplomaProp = diplomaObj;
        flow.roommatesGroup = roommatesRoot;
        flow.laptopProp = laptopObj;

        flow.bedInteractable = bedInteract;
        flow.trashInteractable = trashInteract;
        flow.laptopInteractable = laptopInteract;
        flow.lightSwitchInteractable = switchInteract;

        flow.playerObj = playerObj;
        flow.playerVisual = playerVisual;
        flow.deskSeatTransform = deskSeat.transform;
        flow.doorTransform = switchObj.transform;

        flow.objectiveText = objectiveText;
        flow.timeText = timeText;
        flow.yearText = yearText;

        flow.dialoguePanel = dialoguePanel;
        flow.portraitImage = portImg;
        flow.speakerNameText = speakerText;
        flow.dialogueBodyText = bodyText;
        flow.dialogueNextButton = nextBtn;

        flow.yesNoModal = yesNoModal;
        flow.yesButton = yesBtn;
        flow.noButton = noBtn;
        flow.modalPromptText = modalPrompt;

        flow.gradeStampPanel = gradePanel;
        flow.gradeStampImage = stampImg;
        flow.gradeTitleText = gradeTitle;
        flow.gradeSubText = gradeSub;
        flow.gradeStampF = gradeFSprite;
        flow.gradeStampA = gradeASprite;

        flow.uninstallModal = uninstallModal;
        flow.uninstallProgressBar = unSlider;

        flow.endingPanel = endingPanel;
        flow.endingText = endingText;

        flow.portraitLuon = portraitLuon;
        flow.portraitDam = portraitDam;
        flow.portraitLong = portraitLong;
        flow.portraitTo = portraitTo;

        RoomInteractionSceneSetup.Configure(scene);
        string scenePath = "Assets/Scenes/Room404_MVP.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log("Room404_MVP scene fully corrected and saved at " + scenePath);
    }

    private static void CreateShadow(GameObject parent, Sprite shadowSprite, Vector3 localPos, Vector3 localScale, int sortingOrder)
    {
        if (shadowSprite == null) return;
        GameObject shObj = new GameObject("ContactShadow");
        shObj.transform.SetParent(parent.transform);
        shObj.transform.localPosition = localPos;
        shObj.transform.localScale = localScale;
        SpriteRenderer sr = shObj.AddComponent<SpriteRenderer>();
        sr.sprite = shadowSprite;
        sr.sortingOrder = sortingOrder;
    }

    private const string LayoutAssetFolder = "Assets/Room404Layout";

    public static void ApplyRequestedRoomLayout()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            throw new System.InvalidOperationException("Stop Play Mode before saving the room layout.");
        var scene = EditorSceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/Room404_MVP.unity")
            throw new System.InvalidOperationException("Open Room404_MVP first.");

        string[] names = { "Study_Desk", "Laptop_Prop", "Laptop Screen Glow", "Diploma_Prop",
            "Desk_Seat_Marker", "BunkBed_Left", "BunkBed_Right", "Room_Backdrop" };
        var objects = new System.Collections.Generic.Dictionary<string, GameObject>();
        foreach (GameObject root in scene.GetRootGameObjects())
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
                foreach (string name in names)
                    if (child.name == name) objects.Add(name, child.gameObject);
        foreach (string name in names)
            if (!objects.ContainsKey(name)) throw new System.InvalidOperationException("Missing object: " + name);
        foreach (var entry in objects)
            if (entry.Key != "Room_Backdrop")
                Undo.RegisterFullObjectHierarchyUndo(entry.Value, "Apply requested room layout");

        objects["Study_Desk"].transform.position = DeskPosition;
        objects["Study_Desk"].GetComponent<SpriteRenderer>().sharedMaterial = GetProjectionMaterial("DeskProjection", -60f / 647f);
        objects["Laptop_Prop"].transform.position = LaptopPosition;
        objects["Laptop_Prop"].transform.localScale = new Vector3(LaptopScale, LaptopScale, 1f);
        objects["Laptop Screen Glow"].transform.position = LaptopGlowPosition;
        objects["Diploma_Prop"].transform.position = DiplomaPosition;
        objects["Desk_Seat_Marker"].transform.position = DeskSeatPosition;
        Transform laptopShadow = objects["Laptop_Prop"].transform.Find("ContactShadow");
        if (laptopShadow != null) laptopShadow.localPosition = new Vector3(0f, -1.5f, 0f);

        Sprite bedSprite = LoadLengthwiseBunkBedSprite();
        ConfigureBunkBedLayout(objects["BunkBed_Left"], bedSprite, true);
        ConfigureBunkBedLayout(objects["BunkBed_Right"], bedSprite, false);
        SpriteRenderer backdrop = objects["Room_Backdrop"].GetComponent<SpriteRenderer>();
        CreateOrUpdateRoomDoor(backdrop, backdrop.transform.parent);
        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(scene);
        if (!EditorSceneManager.SaveScene(scene))
            throw new System.InvalidOperationException("Could not save the requested room layout.");
        Debug.Log("Requested layout applied: level desk, laptop on tabletop, two mirrored beds moved forward, functional hinged door.");
    }

    private static Material GetProjectionMaterial(string name, float shear)
    {
        if (!AssetDatabase.IsValidFolder(LayoutAssetFolder))
            AssetDatabase.CreateFolder("Assets", "Room404Layout");
        string path = LayoutAssetFolder + "/" + name + ".mat";
        Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (material == null)
        {
            Shader shader = Shader.Find("Room404/SpriteProjection");
            if (shader == null) throw new System.InvalidOperationException("Missing room sprite projection shader.");
            material = new Material(shader);
            AssetDatabase.CreateAsset(material, path);
        }
        material.SetFloat("_ShearY", shear);
        material.SetFloat("_CompressX", 1f);
        EditorUtility.SetDirty(material);
        return material;
    }

    private static void CreateOrUpdateRoomDoor(SpriteRenderer backdrop, Transform parent)
    {
        Material material = GetProjectionMaterial("DoorProjection", 0f);
        string leafPath = LayoutAssetFolder + "/DoorLeaf.asset";
        Sprite leafSprite = AssetDatabase.LoadAssetAtPath<Sprite>(leafPath);
        // Reuse the original door pixels. This rect excludes the fixed frame and transom.
        Rect rect = new Rect(652f, 304f, 128f, 230f);
        float ppu = backdrop.sprite.pixelsPerUnit;
        if (leafSprite == null)
        {
            leafSprite = Sprite.Create(backdrop.sprite.texture, rect, new Vector2(1f, 0f), ppu, 0, SpriteMeshType.FullRect);
            leafSprite.name = "OriginalDoorLeaf";
            AssetDatabase.CreateAsset(leafSprite, leafPath);
        }

        string pixelPath = LayoutAssetFolder + "/DoorOpeningPixel.asset";
        Texture2D pixel = AssetDatabase.LoadAssetAtPath<Texture2D>(pixelPath);
        if (pixel == null)
        {
            pixel = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            pixel.name = "DoorOpeningPixel";
            pixel.SetPixel(0, 0, Color.white);
            pixel.Apply();
            AssetDatabase.CreateAsset(pixel, pixelPath);
        }
        string openingPath = LayoutAssetFolder + "/DoorOpeningSprite.asset";
        Sprite openingSprite = AssetDatabase.LoadAssetAtPath<Sprite>(openingPath);
        if (openingSprite == null)
        {
            openingSprite = Sprite.Create(pixel, new Rect(0f, 0f, 1f, 1f), Vector2.zero, 1f, 0, SpriteMeshType.FullRect);
            openingSprite.name = "DoorwayInterior";
            AssetDatabase.CreateAsset(openingSprite, openingPath);
        }

        Transform existing = parent.Find("Room_Door");
        GameObject door = existing != null ? existing.gameObject : new GameObject("Room_Door");
        if (existing == null) Undo.RegisterCreatedObjectUndo(door, "Add hinged room door");
        else Undo.RegisterFullObjectHierarchyUndo(door, "Update hinged room door");
        door.transform.SetParent(parent, false);
        door.transform.position = backdrop.transform.TransformPoint(new Vector3(
            (rect.xMax - backdrop.sprite.pivot.x) / ppu,
            (rect.yMin - backdrop.sprite.pivot.y) / ppu, 0f));

        Transform opening = door.transform.Find("Doorway_Opening");
        if (opening == null)
        {
            opening = new GameObject("Doorway_Opening").transform;
            opening.SetParent(door.transform, false);
            opening.gameObject.AddComponent<SpriteRenderer>();
        }
        opening.localPosition = new Vector3(-rect.width / ppu, 0f, 0f);
        opening.localScale = new Vector3(rect.width / ppu, rect.height / ppu, 1f);
        SpriteRenderer openingRenderer = opening.GetComponent<SpriteRenderer>();
        openingRenderer.sprite = openingSprite;
        openingRenderer.color = new Color(0.12f, 0.13f, 0.12f, 1f);
        openingRenderer.sortingOrder = -90;

        Transform leaf = door.transform.Find("Door_Leaf");
        if (leaf == null)
        {
            leaf = new GameObject("Door_Leaf").transform;
            leaf.SetParent(door.transform, false);
            leaf.gameObject.AddComponent<SpriteRenderer>();
        }
        leaf.localPosition = Vector3.zero;
        leaf.localRotation = Quaternion.identity;
        SpriteRenderer leafRenderer = leaf.GetComponent<SpriteRenderer>();
        leafRenderer.sprite = leafSprite;
        leafRenderer.sharedMaterial = material;
        leafRenderer.sortingOrder = -85;
        BoxCollider2D blocker = door.GetComponent<BoxCollider2D>();
        if (blocker == null) blocker = door.AddComponent<BoxCollider2D>();
        HingedRoomDoor controller = door.GetComponent<HingedRoomDoor>();
        if (controller == null) controller = door.AddComponent<HingedRoomDoor>();
        controller.Configure(leafRenderer, blocker, rect.width / ppu);
    }

    public static void UpdateRoomFurnitureLayout()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            throw new System.InvalidOperationException("Stop Play Mode before saving room layout changes.");

        var scene = EditorSceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/Room404_MVP.unity")
            throw new System.InvalidOperationException("Open Room404_MVP before updating its furniture.");

        string[] names = { "Study_Desk", "Laptop_Prop", "Laptop Screen Glow", "Diploma_Prop",
            "Desk_Seat_Marker", "Light_Switch", "Ceiling_Fan" };
        var objects = new System.Collections.Generic.Dictionary<string, GameObject>();
        foreach (GameObject root in scene.GetRootGameObjects())
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
                foreach (string name in names)
                    if (child.name == name)
                        objects.Add(name, child.gameObject);

        foreach (string name in names)
            if (!objects.ContainsKey(name))
                throw new System.InvalidOperationException("Missing room object: " + name);

        Transform fan = objects["Ceiling_Fan"].transform;
        Transform rod = fan.Find("Mount_Rod");
        Transform blades = fan.Find("Blades_Rotor");
        if (rod == null || blades == null)
            throw new System.InvalidOperationException("The ceiling fan needs its fixed stem and rotor.");
        Sprite rodSprite = LoadCenteredFanSprite("Assets/Sprites/MVP/environment/fan_mount_stem.png");
        Sprite rotorSprite = LoadCenteredFanSprite("Assets/Sprites/MVP/environment/fan_rotor.png");

        foreach (GameObject obj in objects.Values)
            Undo.RegisterFullObjectHierarchyUndo(obj, "Position room furniture against walls");

        objects["Study_Desk"].transform.position = DeskPosition;
        objects["Laptop_Prop"].transform.position = LaptopPosition;
        objects["Laptop Screen Glow"].transform.position = LaptopGlowPosition;
        objects["Diploma_Prop"].transform.position = DiplomaPosition;
        objects["Desk_Seat_Marker"].transform.position = DeskSeatPosition;

        Transform socket = objects["Light_Switch"].transform;
        socket.position = SocketPosition;
        socket.localRotation = Quaternion.identity;
        socket.localScale = new Vector3(SocketScale, SocketScale, 1f);

        fan.position = FanPosition;
        fan.localRotation = Quaternion.identity;
        fan.localScale = new Vector3(FanScale, FanScale, 1f);
        rod.localPosition = Vector3.zero;
        rod.localRotation = Quaternion.identity;
        rod.GetComponent<SpriteRenderer>().sprite = rodSprite;
        blades.localPosition = FanRotorOffset;
        blades.localScale = new Vector3(1f, 2f, 1f);
        blades.localRotation = Quaternion.Euler(75f, 0f, 0f);
        blades.GetComponent<SpriteRenderer>().sprite = rotorSprite;

        EditorSceneManager.MarkSceneDirty(scene);
        if (!EditorSceneManager.SaveScene(scene))
            throw new System.InvalidOperationException("Could not save the updated room furniture layout.");
        Debug.Log("Desk and socket aligned with back wall; ceiling fan centered with its hub pivot corrected.");
    }

    private static Sprite LoadCenteredFanSprite(string path)
    {
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null)
            throw new System.InvalidOperationException("Missing fan sprite: " + path);

        // Both layers share a 510x268 canvas. The motor center is at pixel (238, 158)
        // from the top left; Unity pivots are measured from the bottom left.
        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        Vector2 hubPivot = new Vector2(238f / 510f, 110f / 268f);
        if (settings.spriteAlignment != (int)SpriteAlignment.Custom ||
            (settings.spritePivot - hubPivot).sqrMagnitude > 0.000001f)
        {
            settings.spriteAlignment = (int)SpriteAlignment.Custom;
            settings.spritePivot = hubPivot;
            importer.SetTextureSettings(settings);
            importer.SaveAndReimport();
        }

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite == null)
            throw new System.InvalidOperationException("Could not load fan sprite: " + path);
        return sprite;
    }

    private const string LengthwiseBunkBedPath = "Assets/Sprites/MVP/environment/bunk_bed_lengthwise.png";

    public static void UpdateBunkBedLayout()
    {
        var scene = EditorSceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/Room404_MVP.unity")
            throw new System.InvalidOperationException("Open Room404_MVP before updating its bunk beds.");

        GameObject left = GameObject.Find("BunkBed_Left");
        GameObject right = GameObject.Find("BunkBed_Right");
        if (left == null || right == null)
            throw new System.InvalidOperationException("The room must contain its two existing bunk beds.");

        Sprite sprite = LoadLengthwiseBunkBedSprite();
        Undo.RegisterFullObjectHierarchyUndo(left, "Align left bunk with side wall");
        Undo.RegisterFullObjectHierarchyUndo(right, "Align right bunk with side wall");
        ConfigureBunkBedLayout(left, sprite, true);
        ConfigureBunkBedLayout(right, sprite, false);
        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("Updated only the two existing bunk beds along the side walls.");
    }

    private static Sprite LoadLengthwiseBunkBedSprite()
    {
        AssetDatabase.ImportAsset(LengthwiseBunkBedPath);
        var importer = AssetImporter.GetAtPath(LengthwiseBunkBedPath) as TextureImporter;
        if (importer == null)
            throw new System.InvalidOperationException("Missing lengthwise bunk bed sprite.");

        if (importer.textureType != TextureImporterType.Sprite ||
            importer.spriteImportMode != SpriteImportMode.Single || importer.mipmapEnabled ||
            !importer.alphaIsTransparency || importer.filterMode != FilterMode.Point ||
            importer.textureCompression != TextureImporterCompression.Uncompressed)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.spritePixelsPerUnit = 100f;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SaveAndReimport();
        }

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(LengthwiseBunkBedPath);
        if (sprite == null)
            throw new System.InvalidOperationException("Could not import the lengthwise bunk bed sprite.");
        return sprite;
    }

    private static void ConfigureBunkBedLayout(GameObject bed, Sprite sprite, bool leftWall)
    {
        // The perspective is drawn into the sprite. Z rotation would topple its upright posts.
        // This sprite looks from the foot toward the head; mirroring gives the opposite wall view.
        float scale = 4f / sprite.bounds.size.y;
        bed.transform.localRotation = Quaternion.identity;
        // Move 15% of the four-unit projected bed length toward the foreground.
        // Follow the side-wall convergence outward as the beds move forward.
        bed.transform.localPosition = new Vector3(leftWall ? -3.95f : 3.95f, -0.95f, 0f);
        bed.transform.localScale = new Vector3(leftWall ? scale : -scale, scale, 1f);
        bed.GetComponent<SpriteRenderer>().sprite = sprite;

        // Keep collision on the floor footprint, below the elevated upper bunk.
        BoxCollider2D collider = bed.GetComponent<BoxCollider2D>();
        collider.size = new Vector2(1.25f / scale, 1.65f / scale);
        collider.offset = new Vector2(-0.08f / scale, -1.05f / scale);

        Transform shadow = bed.transform.Find("ContactShadow");
        if (shadow != null)
        {
            shadow.localRotation = Quaternion.identity;
            shadow.localPosition = new Vector3(-0.08f / scale, -1.35f / scale, 0f);
            shadow.localScale = new Vector3(1f / scale, 1.35f / scale, 1f);
        }
    }

    private static InteractableProp CreateBunkBed(Transform parent, string name, Sprite sprite,
        bool leftWall, Sprite shadowSprite, bool isInteractable)
    {
        GameObject bed = new GameObject(name);
        bed.transform.SetParent(parent);

        SpriteRenderer renderer = bed.AddComponent<SpriteRenderer>();
        renderer.sortingOrder = -20;
        bed.AddComponent<BoxCollider2D>();
        CreateShadow(bed, shadowSprite, Vector3.zero, Vector3.one, -25);
        ConfigureBunkBedLayout(bed, sprite, leftWall);

        if (!isInteractable) return null;
        InteractableProp interactable = bed.AddComponent<InteractableProp>();
        interactable.SetPrompt("Bấm [E] để xí giường");
        return interactable;
    }

    private static GameObject CreateTrashCluster(GameObject parent, string name, Sprite sprite, Vector3 pos, float scale, Sprite shadowSprite)
    {
        GameObject tObj = new GameObject(name);
        tObj.transform.SetParent(parent.transform);
        tObj.transform.position = pos;
        tObj.transform.localScale = new Vector3(scale, scale, 1f);
        SpriteRenderer sr = tObj.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = -5;
        CreateShadow(tObj, shadowSprite, new Vector3(0, -0.2f, 0), new Vector3(1.0f, 0.6f, 1f), -6);
        return tObj;
    }

    private static void CreateColliderBox(GameObject parent, Vector2 pos, Vector2 size)
    {
        GameObject colObj = new GameObject("WallCol");
        colObj.transform.SetParent(parent.transform);
        colObj.transform.position = pos;
        BoxCollider2D box = colObj.AddComponent<BoxCollider2D>();
        box.size = size;
    }

    private static void CreateCharacterNPC(GameObject parent, string name, Sprite sprite, Vector3 pos, float scale, Sprite shadowSprite)
    {
        GameObject npc = new GameObject(name);
        npc.transform.SetParent(parent.transform);
        npc.transform.position = pos;
        npc.transform.localScale = new Vector3(scale, scale, 1f);
        SpriteRenderer sr = npc.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 4;
        npc.AddComponent<BoxCollider2D>();
        CreateShadow(npc, shadowSprite, new Vector3(0, -1.0f, 0), new Vector3(1.1f, 0.8f, 1f), 3);
    }

    private static GameObject CreateUIPanel(GameObject parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPos, Vector2 sizeDelta)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent.transform, false);
        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.anchorMin = anchorMin;
        rt.anchorMax = anchorMax;
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = sizeDelta;
        return panel;
    }

    private static TextMeshProUGUI CreateTMP(GameObject parent, string name, string text, float fontSize, TextAlignmentOptions align, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent.transform, false);
        TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.alignment = align;
        tmp.color = color;
        return tmp;
    }
}
