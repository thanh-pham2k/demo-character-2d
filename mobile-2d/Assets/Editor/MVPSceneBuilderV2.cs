using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;

public static class MVPSceneBuilderV2
{
    public static void BuildScene()
    {
        // Import TMP essentials if missing
        AssetDatabase.ImportPackage("Library/PackageCache/com.unity.ugui@eb824e41fbc7/Package Resources/TMP Essential Resources.unitypackage", false);

        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 1. Camera setup - Positioned to frame the room harmoniously
        GameObject camObj = new GameObject("Main Camera");
        Camera cam = camObj.AddComponent<Camera>();
        camObj.tag = "MainCamera";
        cam.orthographic = true;
        cam.orthographicSize = 4.2f; // Perfectly fits room
        camObj.transform.position = new Vector3(0, 0, -10);
        cam.backgroundColor = new Color(0.12f, 0.12f, 0.16f, 1f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        camObj.AddComponent<AudioListener>();
        camObj.AddComponent<UniversalAdditionalCameraData>();

        // 2. Lighting setup
        GameObject lightsRoot = new GameObject("--- LIGHTS ---");
        
        GameObject globalLightObj = new GameObject("Global 2D Light");
        globalLightObj.transform.SetParent(lightsRoot.transform);
        Light2D globalLight = globalLightObj.AddComponent<Light2D>();
        globalLight.lightType = Light2D.LightType.Global;
        globalLight.color = new Color(1f, 0.98f, 0.92f, 1f);
        globalLight.intensity = 1.05f;

        GameObject ceilingLightObj = new GameObject("Ceiling Lamp Light");
        ceilingLightObj.transform.SetParent(lightsRoot.transform);
        ceilingLightObj.transform.position = new Vector3(0, 2.2f, 0);
        Light2D ceilingLight = ceilingLightObj.AddComponent<Light2D>();
        ceilingLight.lightType = Light2D.LightType.Point;
        ceilingLight.pointLightOuterRadius = 7.0f;
        ceilingLight.pointLightInnerRadius = 1.8f;
        ceilingLight.color = new Color(1f, 0.95f, 0.85f, 1f);
        ceilingLight.intensity = 0.6f;

        GameObject laptopLightObj = new GameObject("Laptop Screen Light");
        laptopLightObj.transform.SetParent(lightsRoot.transform);
        laptopLightObj.transform.position = new Vector3(0.05f, 0.05f, 0); // On desk
        Light2D laptopLight = laptopLightObj.AddComponent<Light2D>();
        laptopLight.lightType = Light2D.LightType.Point;
        laptopLight.pointLightOuterRadius = 1.8f;
        laptopLight.pointLightInnerRadius = 0.2f;
        laptopLight.color = new Color(0.35f, 0.75f, 1f, 1f);
        laptopLight.intensity = 0.0f;

        // 3. Load Sprites
        Sprite backdropSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/environment/room_404_backdrop.png");
        Sprite bunkBedSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/environment/bunk_bed.png");
        Sprite deskSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/environment/study_desk_chairs.png");
        Sprite fanSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/environment/ceiling_fan.png");
        Sprite switchSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/environment/light_switch_socket.png");

        Sprite shadowChar = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/shadows/shadow_character.png");
        Sprite shadowDesk = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/shadows/shadow_desk.png");
        Sprite shadowBed = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/shadows/shadow_bed.png");
        Sprite shadowSmall = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/shadows/shadow_small.png");

        // 4. Environment - Backdrop
        GameObject envRoot = new GameObject("--- ENVIRONMENT ---");

        GameObject backdropObj = new GameObject("Room_Backdrop");
        backdropObj.transform.SetParent(envRoot.transform);
        backdropObj.transform.position = new Vector3(0, 0, 0);
        SpriteRenderer backdropSr = backdropObj.AddComponent<SpriteRenderer>();
        backdropSr.sprite = backdropSprite;
        backdropSr.sortingOrder = -100;

        // Bunk Beds: Adjusted scale and grounded perspective with contact shadows
        // Left Bed (Window side - where Luon claims)
        GameObject bedLeft = new GameObject("BunkBed_Left");
        bedLeft.transform.SetParent(envRoot.transform);
        bedLeft.transform.position = new Vector3(-3.3f, 0.4f, 0);
        bedLeft.transform.localScale = new Vector3(0.58f, 0.58f, 1f);
        SpriteRenderer bedLeftSr = bedLeft.AddComponent<SpriteRenderer>();
        bedLeftSr.sprite = bunkBedSprite;
        bedLeftSr.sortingOrder = -20;
        BoxCollider2D bedLeftCol = bedLeft.AddComponent<BoxCollider2D>();
        bedLeftCol.size = new Vector2(3.5f, 2.5f);
        bedLeftCol.offset = new Vector2(0, -1.0f);
        InteractableProp bedInteract = bedLeft.AddComponent<InteractableProp>();
        bedInteract.SetPrompt("Bấm [E] để xí giường");

        // Bed Left Shadow
        CreateShadow(bedLeft, shadowBed, new Vector3(0, -2.4f, 0), new Vector3(2.5f, 1.8f, 1f), -25);

        // Right Bed
        GameObject bedRight = new GameObject("BunkBed_Right");
        bedRight.transform.SetParent(envRoot.transform);
        bedRight.transform.position = new Vector3(3.3f, 0.4f, 0);
        bedRight.transform.localScale = new Vector3(-0.58f, 0.58f, 1f);
        SpriteRenderer bedRightSr = bedRight.AddComponent<SpriteRenderer>();
        bedRightSr.sprite = bunkBedSprite;
        bedRightSr.sortingOrder = -20;
        BoxCollider2D bedRightCol = bedRight.AddComponent<BoxCollider2D>();
        bedRightCol.size = new Vector2(3.5f, 2.5f);
        bedRightCol.offset = new Vector2(0, -1.0f);
        CreateShadow(bedRight, shadowBed, new Vector3(0, -2.4f, 0), new Vector3(2.5f, 1.8f, 1f), -25);

        // Study Desk (Moved slightly upper-center so central space remains open)
        GameObject deskObj = new GameObject("Study_Desk");
        deskObj.transform.SetParent(envRoot.transform);
        deskObj.transform.position = new Vector3(0.0f, -0.3f, 0);
        deskObj.transform.localScale = new Vector3(0.48f, 0.48f, 1f); // Balanced scale with chairs
        SpriteRenderer deskSr = deskObj.AddComponent<SpriteRenderer>();
        deskSr.sprite = deskSprite;
        deskSr.sortingOrder = -10;
        BoxCollider2D deskCol = deskObj.AddComponent<BoxCollider2D>();
        deskCol.size = new Vector2(4.5f, 1.8f);
        deskCol.offset = new Vector2(0, -0.6f);
        CreateShadow(deskObj, shadowDesk, new Vector3(0, -1.5f, 0), new Vector3(2.6f, 1.5f, 1f), -15);

        // Ceiling Fan
        GameObject fanObj = new GameObject("Ceiling_Fan");
        fanObj.transform.SetParent(envRoot.transform);
        fanObj.transform.position = new Vector3(0, 2.1f, 0);
        fanObj.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
        SpriteRenderer fanSr = fanObj.AddComponent<SpriteRenderer>();
        fanSr.sprite = fanSprite;
        fanSr.sortingOrder = 200;
        fanObj.AddComponent<RotatingFan>();

        // Light Switch (Near Door 404 at top center or left wall)
        GameObject switchObj = new GameObject("Light_Switch");
        switchObj.transform.SetParent(envRoot.transform);
        switchObj.transform.position = new Vector3(-4.1f, -1.5f, 0);
        switchObj.transform.localScale = new Vector3(0.35f, 0.35f, 1f);
        SpriteRenderer switchSr = switchObj.AddComponent<SpriteRenderer>();
        switchSr.sprite = switchSprite;
        switchSr.sortingOrder = 1;
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
        backpackObj.transform.position = new Vector3(-3.2f, -0.3f, 0);
        backpackObj.transform.localScale = new Vector3(0.22f, 0.22f, 1f); // Balanced scale on mattress
        SpriteRenderer bpSr = backpackObj.AddComponent<SpriteRenderer>();
        bpSr.sprite = backpackSprite;
        bpSr.sortingOrder = -5;
        backpackObj.SetActive(false);

        // Realistic Scattered Trash Clusters (Not a single blob!)
        GameObject trashRoot = new GameObject("Trash_Clusters_Group");
        trashRoot.transform.SetParent(propsRoot.transform);

        // Cluster A: Under desk edge (instant noodles)
        GameObject trash1 = CreateTrashCluster(trashRoot, "Trash_Noodles", trashNoodles, new Vector3(1.4f, -1.1f, 0), 0.16f, shadowSmall);
        // Cluster B: Near bed foot (cans & paper)
        GameObject trash2 = CreateTrashCluster(trashRoot, "Trash_Cans", trashCans, new Vector3(-1.8f, -1.4f, 0), 0.15f, shadowSmall);
        // Cluster C: Room corner (plastic bag)
        GameObject trash3 = CreateTrashCluster(trashRoot, "Trash_Bags", trashBags, new Vector3(-3.5f, -2.4f, 0), 0.16f, shadowSmall);

        InteractableProp trashInteract = trashRoot.AddComponent<InteractableProp>();
        trashInteract.SetPrompt("Bấm [E] quét dọn rác");
        CircleCollider2D trashCol = trashRoot.AddComponent<CircleCollider2D>();
        trashCol.radius = 2.2f;
        trashCol.isTrigger = true;

        // Laptop - Perfectly fitted on desk top (scale reduced by ~65%)
        GameObject laptopObj = new GameObject("Laptop_Prop");
        laptopObj.transform.SetParent(propsRoot.transform);
        laptopObj.transform.position = new Vector3(0.05f, -0.05f, 0); // Flat on desk surface
        laptopObj.transform.localScale = new Vector3(0.14f, 0.14f, 1f); // Realistic laptop scale!
        SpriteRenderer lapSr = laptopObj.AddComponent<SpriteRenderer>();
        lapSr.sprite = laptopSprite;
        lapSr.sortingOrder = 5;
        CreateShadow(laptopObj, shadowSmall, new Vector3(0, -0.4f, 0), new Vector3(1.2f, 0.8f, 1f), 4);
        laptopObj.AddComponent<BoxCollider2D>().isTrigger = true;
        InteractableProp laptopInteract = laptopObj.AddComponent<InteractableProp>();
        laptopInteract.SetPrompt("Bấm [E] mở máy tính");

        // Diploma Degree
        GameObject diplomaObj = new GameObject("Diploma_Prop");
        diplomaObj.transform.SetParent(propsRoot.transform);
        diplomaObj.transform.position = new Vector3(0.55f, -0.05f, 0);
        diplomaObj.transform.localScale = new Vector3(0.13f, 0.13f, 1f);
        SpriteRenderer dipSr = diplomaObj.AddComponent<SpriteRenderer>();
        dipSr.sprite = diplomaSprite;
        dipSr.sortingOrder = 5;
        diplomaObj.SetActive(false);

        // 6. Roommates Group
        GameObject roommatesRoot = new GameObject("--- ROOMMATES_GROUP ---");
        Sprite damSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/dam_standing.png");
        Sprite longSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/long_standing.png");
        Sprite toSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/to_standing.png");

        CreateCharacterNPC(roommatesRoot, "Dam_NPC", damSprite, new Vector3(1.8f, -1.0f, 0), 0.65f, shadowChar);
        CreateCharacterNPC(roommatesRoot, "Long_NPC", longSprite, new Vector3(-0.8f, -1.0f, 0), 0.65f, shadowChar);
        CreateCharacterNPC(roommatesRoot, "To_NPC", toSprite, new Vector3(2.6f, 0.0f, 0), 0.65f, shadowChar);
        roommatesRoot.SetActive(false);

        // 7. Player (Lê Văn Lươn) - Prominently spawned in open center-front area
        GameObject playerObj = new GameObject("Player_Luon");
        playerObj.transform.position = new Vector3(-0.5f, -2.0f, 0); // Open center walking area!
        
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

        // Player Shadow
        CreateShadow(playerObj, shadowChar, new Vector3(0, -0.65f, 0), new Vector3(1.0f, 0.8f, 1f), 0);

        GameObject visualChild = new GameObject("Visual");
        visualChild.transform.SetParent(playerObj.transform);
        visualChild.transform.localPosition = Vector3.zero;
        visualChild.transform.localScale = new Vector3(0.78f, 0.78f, 1f); // +20% size enhancement!

        SpriteRenderer playerSr = visualChild.AddComponent<SpriteRenderer>();
        playerSr.sprite = luonIdle;
        playerSr.sortingOrder = 10;

        LuonCharacterVisual playerVisual = visualChild.AddComponent<LuonCharacterVisual>();
        playerVisual.idleSprite = luonIdle;
        playerVisual.walkSprites = new Sprite[] { luonWalk0, luonWalk1, luonWalk2, luonWalk3 };
        playerVisual.sitLaptopSprite = luonSit;

        PlayerMovement movement = playerObj.AddComponent<PlayerMovement>();
        playerObj.AddComponent<PlayerInteractionDetector>();

        // Desk Seat Transform for Laptop
        GameObject deskSeat = new GameObject("Desk_Seat_Marker");
        deskSeat.transform.position = new Vector3(0.05f, -0.65f, 0);

        // Room Colliders (Top, Bottom, Left, Right boundaries)
        GameObject boundsObj = new GameObject("Room_Colliders");
        boundsObj.transform.SetParent(envRoot.transform);
        CreateColliderBox(boundsObj, new Vector2(0, 2.5f), new Vector2(10f, 1f));
        CreateColliderBox(boundsObj, new Vector2(0, -3.4f), new Vector2(10f, 1f));
        CreateColliderBox(boundsObj, new Vector2(-4.8f, 0), new Vector2(1f, 6.5f));
        CreateColliderBox(boundsObj, new Vector2(4.8f, 0), new Vector2(1f, 6.5f));

        // 8. Build Canvas UI (Clean Modern Layout)
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

        // UI Sprites
        Sprite portraitLuon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/portrait_luon.png");
        Sprite portraitDam = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/portrait_dam.png");
        Sprite portraitLong = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/portrait_long.png");
        Sprite portraitTo = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/portrait_to.png");
        Sprite dialogueBoxSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/dialogue_box.png");
        Sprite buttonYesSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/button_yes.png");
        Sprite buttonNoSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/button_no.png");
        Sprite gradeFSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/grade_stamp_f.png");
        Sprite gradeASprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/ui/grade_stamp_a.png");

        // Top Info Panel
        GameObject topInfoObj = CreateUIPanel(canvasObj, "TopInfoPanel", new Vector2(0, 1), new Vector2(0, 1), new Vector2(120, -60), new Vector2(500, 100));
        TextMeshProUGUI yearText = CreateTMP(topInfoObj, "YearText", "NĂM 1 — TÂN SINH VIÊN (201X)", 24, TextAlignmentOptions.TopLeft, new Color(1f, 0.85f, 0.4f));
        yearText.rectTransform.anchoredPosition = new Vector2(150, 20);
        TextMeshProUGUI timeText = CreateTMP(topInfoObj, "TimeText", "14:20 • THỨ HAI", 20, TextAlignmentOptions.TopLeft, Color.white);
        timeText.rectTransform.anchoredPosition = new Vector2(150, -15);

        // Bottom Objective Tracker
        GameObject objectiveObj = CreateUIPanel(canvasObj, "ObjectivePanel", new Vector2(0, 0), new Vector2(0, 0), new Vector2(120, 80), new Vector2(650, 80));
        TextMeshProUGUI objectiveText = CreateTMP(objectiveObj, "ObjectiveText", "Mục tiêu: Đang nạp...", 20, TextAlignmentOptions.Left, new Color(0.9f, 0.95f, 1f));
        objectiveText.rectTransform.anchoredPosition = new Vector2(250, 0);

        // Dialogue Box Panel
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

        // Modal YES / NO
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

        // Uninstall Modal
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

        // Grade Stamp Panel
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

        // Ending Panel
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

        string scenePath = "Assets/Scenes/Room404_MVP.unity";
        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log("Room404_MVP scene unified & rebuilt successfully at " + scenePath);
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
