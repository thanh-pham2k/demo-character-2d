using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

// Runs actual Update/FixedUpdate and bed coroutines. Never writes the scene or forces pose completion.
[InitializeOnLoad]
public static partial class PlayModeInteractionSuite
{
    private const string Phase = "Room404.PlayModeSuite";
    private static readonly List<string> passed = new List<string>();
    private static readonly List<string> errors = new List<string>();
    private static readonly Stack<IEnumerator> routines = new Stack<IEnumerator>();
    private static Keyboard keyboard;
    private static InputSettings previousInputSettings;
    private static InputSettings.BackgroundBehavior previousBackgroundBehavior;
    private static InputSettings.EditorInputBehaviorInPlayMode previousEditorBehavior;
    private static bool previousRunInBackground;
    private static double deadline,nextHeartbeat;
    public static string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../PlayModeInteractionReports"));
    [Serializable] public class SuiteReport { public bool success; public bool playMode; public string finishedUtc; public string[] passed; public string[] errors; }

    static PlayModeInteractionSuite()
    {
        EditorApplication.playModeStateChanged += Changed;
        if (SessionState.GetString(Phase, "") == "entering" && EditorApplication.isPlaying)
            EditorApplication.delayCall += Start;
    }
    public static void RunAllTests()
    {
        if (SessionState.GetString(Phase, "") == "running" && EditorApplication.isPlaying) return;
        SessionState.SetString(Phase, "prepare");
        if (EditorApplication.isPlaying) EditorApplication.isPlaying = false;
        else Prepare();
    }
    private static void Changed(PlayModeStateChange change)
    {
        string phase = SessionState.GetString(Phase, "");
        if (change == PlayModeStateChange.EnteredEditMode && phase == "prepare") Prepare();
        if (change == PlayModeStateChange.EnteredPlayMode && phase == "entering") Start();
        if (change == PlayModeStateChange.EnteredEditMode && phase == "leaving") SessionState.EraseString(Phase);
    }
    private static void Prepare()
    {
        if (SceneManager.GetActiveScene().path != "Assets/Scenes/Room404_MVP.unity")
            throw new InvalidOperationException("Open Room404_MVP before running the suite.");
        SessionState.SetString(Phase, "entering");
        EditorApplication.isPlaying = true;
    }
    private static void Start()
    {
        if (SessionState.GetString(Phase, "") != "entering") return;
        SessionState.SetString(Phase, "running");
        passed.Clear(); errors.Clear(); routines.Clear();
        Directory.CreateDirectory(Output);
        Application.logMessageReceived += Log;
        // Isolate virtual-device QA from editor window focus, without saving project settings.
        previousRunInBackground = Application.runInBackground;
        Application.runInBackground = true;
        previousInputSettings = InputSystem.settings;
        previousBackgroundBehavior = previousInputSettings.backgroundBehavior;
        previousEditorBehavior = previousInputSettings.editorInputBehaviorInPlayMode;
        previousInputSettings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
        previousInputSettings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        keyboard = InputSystem.AddDevice<Keyboard>();
        keyboard.MakeCurrent();
        deadline = EditorApplication.timeSinceStartup + 1200;
        routines.Push(Checks());
        EditorApplication.update += Tick;
    }
    private static void Tick()
    {
        try
        {
            if (!EditorApplication.isPlaying) throw new Exception("Play Mode stopped before completion.");
            if(EditorApplication.timeSinceStartup>=nextHeartbeat)
            {
                nextHeartbeat=EditorApplication.timeSinceStartup+2;
                File.WriteAllText(Path.Combine(Output,"running.txt"),DateTime.UtcNow.ToString("O")+"\nphase="+SessionState.GetString(Phase,"")+" routines="+routines.Count+" passed="+passed.Count+" gameTime="+Time.time+"\n"+(passed.Count>0?passed[passed.Count-1]:"starting"));
            }
            if (EditorApplication.timeSinceStartup > deadline) throw new TimeoutException("Play Mode suite timed out.");
            if (routines.Count == 0) { Finish(); return; }
            IEnumerator top = routines.Peek();
            if (!top.MoveNext()) { routines.Pop(); return; }
            if (top.Current is IEnumerator nested) routines.Push(nested);
        }
        catch (Exception e) { errors.Add(e.ToString()); Finish(); }
    }
    private static void Log(string message, string stack, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) errors.Add(message + "\n" + stack);
    }
    private static void Finish()
    {
        YearOneMotionCapture.End();
        EditorApplication.update -= Tick;
        Application.logMessageReceived -= Log;
        MVPSceneTester.CleanupInput();
        if (keyboard != null && keyboard.added) InputSystem.RemoveDevice(keyboard);
        keyboard = null;
        if (previousInputSettings != null)
        {
            previousInputSettings.backgroundBehavior = previousBackgroundBehavior;
            previousInputSettings.editorInputBehaviorInPlayMode = previousEditorBehavior;
        }
        previousInputSettings = null;
        Application.runInBackground = previousRunInBackground;
        File.WriteAllText(Path.Combine(Output, "report.json"), JsonUtility.ToJson(new SuiteReport
        { success = errors.Count == 0, playMode = EditorApplication.isPlaying, finishedUtc = DateTime.UtcNow.ToString("O"), passed = passed.ToArray(), errors = errors.ToArray() }, true));
        Debug.Log($"[PlayModeInteractionSuite] COMPLETED: {passed.Count} passed, {errors.Count} errors.");
        SessionState.SetString(Phase, "leaving");
        EditorApplication.isPlaying = false;
    }
    public static void Check(bool condition, string message)
    {
        if (!condition) throw new Exception("FAIL: " + message);
        passed.Add(message);
    }
    public static IEnumerator Wait(float seconds)
    {
        float end = Time.time + seconds;
        while (Time.time < end) yield return null;
    }
    public static IEnumerator Until(Func<bool> condition, float timeout, string message)
    {
        float end = Time.time + timeout;
        while (!condition() && Time.time < end) yield return null;
        Check(condition(), message);
    }
    private static IEnumerator PressE() => PressKey(Key.E);
    private static IEnumerator PressKey(Key key)
    {
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
        yield return Wait(0.08f);
        InputSystem.QueueStateEvent(keyboard, new KeyboardState());
        yield return Wait(0.08f);
    }
    private static void Place(GameObject player, Vector3 position)
    {
        var body = player.GetComponent<Rigidbody2D>();
        body.linearVelocity = Vector2.zero;
        body.position = position;
        player.transform.position = position;
        Physics2D.SyncTransforms();
    }
    public static void Capture(string name)
    {
        MVPSceneSnapshot.SaveCameraFrame(Camera.main, Path.Combine(Output, name + ".png"));
    }
    private static IEnumerator WalkToX(GameObject player, float target, PlayerMovement movement)
    {
        float until = Time.time + 6f;
        while (Mathf.Abs(player.transform.position.x - target) > .15f && Time.time < until)
        {
            movement.SetSimulatedInput(new Vector2(Mathf.Sign(target - player.transform.position.x), 0f));
            yield return null;
        }
        movement.SetSimulatedInput(Vector2.zero);
        yield return Wait(.06f);
        Check(Mathf.Abs(player.transform.position.x - target) < .35f, "Walk along hallway reaches target without collider trap");
    }
    private static void CaptureUI(string name) => MVPSceneSnapshot.SaveCameraFrame(Camera.main, Path.Combine(Output,name+".png"),true);
    private static IEnumerator ObserveDeskFrames(LuonCharacterVisual visual,int expected,string prefix)
    {
        var seen=new HashSet<Sprite>();
        for(int i=0;i<6;i++)
        {
            if(visual.CurrentDeskActionFrame!=null)seen.Add(visual.CurrentDeskActionFrame);
            CaptureUI(prefix+"-frame-"+i);
            yield return Wait(.2f);
        }
        Check(seen.Count>=expected,"Activity displays distinct hand/pen animation frames over time");
    }
    private static IEnumerator JourneyChecks(GameObject player, PlayerInteractionStateController state, PlayerMovement movement)
    {
        var journey = Object.FindAnyObjectByType<DormJourneyController>();
        var flow = Object.FindAnyObjectByType<MVPFlowController>();
        var detector = player.GetComponent<PlayerInteractionDetector>();
        var body = player.GetComponent<Rigidbody2D>();
        var backpack = Object.FindAnyObjectByType<BackpackInteractable>();
        Check(backpack != null && !journey.HasBackpack, "Wall-hook backpack is available beside door");
        Place(player, backpack.transform.position + Vector3.down * .6f);
        yield return Wait(.1f);
        Check(detector.FindClosestInteractable() == (IInteractable)backpack, "Detector selects backpack wall hook");
        yield return PressE();
        Check(journey.HasBackpack && state.CharacterVisual.BackpackEquipped && !backpack.CanInteract,
            "E equips backpack once and switches idle/walk sprite set");
        Check(backpack.IsHookEmpty && backpack.Visual.enabled, "Equipping swaps hanging backpack to visible empty hook");
        CaptureUI("journey-backpack");
        var door = Object.FindAnyObjectByType<HingedRoomDoor>();
        Place(player, door.transform.position + new Vector3(-.64f,-.9f));
        yield return PressE();
        yield return Until(() => door.CurrentAngle > 69f,2f,"Travel door opens");
        keyboard.MakeCurrent();
        InputSystem.QueueStateEvent(keyboard, new KeyboardState(Key.W));
        yield return Until(() => journey.IsInHallway,3f,"Walking across doorway transfers player into Hallway_Zone");
        yield return Until(() => !journey.IsBusy,2f,"Hallway fade releases movement lock");
        yield return Wait(1f);
        Check(journey.IsInHallway && !journey.IsBusy, "Holding W across transition does not bounce player back into room");
        InputSystem.QueueStateEvent(keyboard, new KeyboardState());
        yield return Wait(.08f);
        Check(state.CanInteract && body.simulated && player.transform.position.x > 15f, "Hallway arrival is safe and player controllable");
        CaptureUI("journey-hallway-entry");
        float cameraX = Camera.main.transform.position.x;
        yield return WalkToX(player,24f,movement);
        yield return Wait(.3f);
        Check(Camera.main.transform.position.x > cameraX + 1f,"Hallway camera follows walking player");
        var stairs = Object.FindAnyObjectByType<DormStairsInteractable>();
        yield return WalkToX(player,stairs.transform.position.x,movement);
        yield return PressE();
        Check(!journey.ReadyForBus && journey.LastNotice.Contains("WC Nam"), "Stairs enforce personal-hygiene prerequisite");
        RestroomInteractable male=null,female=null;
        foreach(var restroom in Object.FindObjectsByType<RestroomInteractable>())
            if(restroom.RoomGender==RestroomInteractable.Gender.Male)male=restroom;else female=restroom;
        Check(male!=null && female!=null,"Both signed restroom doors exist");
        yield return WalkToX(player,female.transform.position.x,movement);
        Vector3 before = player.transform.position;
        yield return PressE();
        Check(!journey.IsBusy && !journey.HasRefreshed && state.CanInteract && journey.LastNotice.Contains("WC Nữ"),
            "WC Nu refuses entry and shows appropriate message");
        Check(Vector3.Distance(before,player.transform.position)<.02f,"WC Nu does not teleport or lock player");
        CaptureUI("journey-wc-female-blocked");
        yield return WalkToX(player,male.transform.position.x,movement);
        yield return PressE();
        Check(journey.IsBusy && state.CurrentState==PlayerInteractionState.Transitioning && !body.simulated,
            "WC Nam starts fast-resolve with input and physics locked");
        movement.SetSimulatedInput(Vector2.right);
        yield return Wait(.25f);
        Check(male.DoorIsOpen && Vector3.Distance(male.transform.Find("Door_Opening").GetComponent<SpriteRenderer>().bounds.center,male.GetComponent<SpriteRenderer>().bounds.center)<.01f,"WC opening aligns with original door frame");
        Check(journey.GetComponent<AudioSource>().isPlaying,"WC Nam plays running-water audio during fade");
        CaptureUI("journey-wc-washing");
        yield return Wait(.8f);
        Check(journey.WashProgressVisible && journey.WashProgress>0 && journey.WashProgress<1 && !male.DoorIsOpen,"WC closed with visible three-second progress");
        CaptureUI("journey-wc-progress");
        yield return Until(()=>!journey.IsBusy,6f,"WC Nam automatically opens door and exits after progress");
        Check(!journey.WashProgressVisible && !male.DoorIsOpen,"WC progress hides and door closes after exit");
        Check(journey.HasRefreshed && state.CanInteract && body.simulated && Vector3.Distance(player.transform.position,male.ExitPivot.position)<.02f,
            "Restroom sets refreshed flag and restores player safely at ExitPivot");
        Check(!journey.GetComponent<AudioSource>().isPlaying,"Restroom water audio stops after washing");
        CaptureUI("journey-refreshed");
        yield return WalkToX(player,stairs.transform.position.x,movement);
        yield return PressE();
        var commute=Object.FindAnyObjectByType<CampusCommuteController>();
        Check(journey.IsBusy && !body.simulated && commute.BusRenderer.GetComponent<AudioSource>().isPlaying,
            "Stairs lock input and play concrete footsteps");
        yield return Until(()=>journey.CurrentArea==DormJourneyController.JourneyArea.Downstairs_Campus && !journey.IsBusy,3f,
            "Stairs fade reaches outdoor campus and restores control");
        Check(journey.ReadyForBus && state.CanInteract,"Hygiene and backpack unlock actual campus travel");
        CaptureUI("commute-campus-arrival");
        var lobby=Object.FindAnyObjectByType<CampusLobbyInteractable>();
        yield return PressE();
        yield return Until(()=>journey.IsInHallway && !journey.IsBusy,3f,"Campus lobby allows safe return upstairs");
        yield return PressE();
        yield return Until(()=>commute.IsOutdoors && !journey.IsBusy,3f,"Can descend again without collider trap");
        float campusCameraX=Camera.main.transform.position.x;
        yield return WalkToX(player,commute.StopAnchor.position.x,movement);
        yield return Wait(.2f);
        Check(journey.CurrentArea==DormJourneyController.JourneyArea.BusStop && Camera.main.transform.position.x>campusCameraX+4f,
            "Player walks sidewalk to bus stop with camera following");
        int moneyBefore=commute.MoneyVnd;
        Check(!commute.BusAtStop,"Bus approaches only after player reaches stop");
        yield return PressE();
        Check(commute.MoneyVnd==moneyBefore && commute.TicketsPurchased==0,"Cannot pay or board before bus arrives");
        CaptureUI("commute-bus-arriving");
        yield return Until(()=>commute.BusAtStop,4f,"Bus 33 physically arrives and stops");
        Check(commute.BusRenderer.enabled && Vector3.Distance(commute.BusRenderer.transform.position,GameObject.Find("Bus_Park").transform.position)<.01f,
            "Bus artwork parks at authored stop marker");
        Check(detector.FindClosestInteractable() is BusStopInteractable,"Bus stop uses generic interaction detector");
        CaptureUI("commute-bus-at-stop");
        yield return PressE();
        Check(commute.MoneyVnd==moneyBefore-7000 && commute.TicketsPurchased==1 && journey.IsBusy,
            "E buys exactly one 7000 VND ticket and locks boarding");
        yield return PressE();yield return PressE();
        Check(commute.MoneyVnd==moneyBefore-7000 && commute.TicketsPurchased==1,"Repeated E cannot charge a second fare");
        yield return Until(()=>journey.CurrentArea==DormJourneyController.JourneyArea.BoardedBus,2f,"Boarding reaches BoardedBus state");
        Check(!state.CharacterVisual.GetComponent<SpriteRenderer>().enabled && !body.simulated,"Player is aboard bus with world physics disabled");
        float departureX=commute.BusRenderer.transform.position.x;
        yield return Wait(.5f);
        Check(commute.BusRenderer.transform.position.x>departureX,"Bus rolls away during actual cutscene coroutine");
        CaptureUI("commute-bus-departing");
        yield return Until(()=>commute.HasCompletedTrip,4f,"Bus departure completes school journey");
        Check(!commute.BusRenderer.enabled && commute.MoneyVnd==moneyBefore-7000,
            "Bus departure retains single fare payment");
        var year=Object.FindAnyObjectByType<YearOneMilestoneController>();
        yield return Until(()=>year.CurrentPhase==YearOneMilestoneController.Phase.Gate && !journey.IsBusy,3f,"Bus fades to H6 gate and restores player control");
        Check(body.simulated && state.CanInteract && state.CharacterVisual.GetComponent<SpriteRenderer>().enabled,"Alighting restores actor and physics");
        CaptureUI("year1-h6-gate");
        yield return WalkToX(player,GameObject.Find("H6_Door_Interactable").transform.position.x,movement);
        yield return PressE();
        yield return Until(()=>year.CurrentPhase==YearOneMilestoneController.Phase.Classroom,3f,"E enters H6 classroom");
        yield return WalkToX(player,GameObject.Find("Seat_Beside_Dam").transform.position.x,movement);
        yield return PressE();
        Check(state.CurrentState==PlayerInteractionState.Sitting && year.DialogueVisible,"E sits beside Dam with lecture dialogue");
        Check(!body.simulated && Vector3.Distance(player.transform.position,year.Seat.position)<.01f,"Lecture seat locks movement at marker");
        CaptureUI("year1-lecture-seat");
        for(int i=0;i<3;i++){yield return PressKey(Key.Space);yield return Wait(.1f);}
        yield return Until(()=>year.Puzzle.IsOpen,3f,"Teacher calls Luon and actual walk opens blackboard puzzle");
        Check(!state.CanInteract && !body.simulated,"Puzzle locks world movement");
        Check(year.Puzzle.transform.Find("Blackboard/Question").GetComponent<TMPro.TMP_Text>().text.Contains("1 + 2 + 3"),"Puzzle presents easy sum from 1 to 10");
        CaptureUI("year1-puzzle");
        yield return PressKey(Key.Digit1);
        Check(year.Puzzle.AwaitingRetry && !year.PuzzleSolved,"Incorrect answer gives retry without advancing story");
        yield return PressKey(Key.R);
        yield return Until(()=>year.Puzzle.AwaitingRetry,22f,"Twenty-second puzzle deadline expires safely");
        yield return PressKey(Key.R);
        yield return PressKey(Key.Digit2);
        Check(year.PuzzleSolved && year.DialogueVisible && !year.Puzzle.IsOpen,"Correct easy-sum answer closes puzzle and starts Dam reaction");
        Check(GameObject.Find("Dam_Surprise")!=null,"Dam shows surprise emote after correct answer");
        CaptureUI("year1-dam-reaction");
        for(int i=0;i<3;i++){year.NextButton.onClick.Invoke();yield return Wait(.1f);}
        yield return Until(()=>year.AcademicVisible,3f,"Semester time skip opens academic milestone card");
        Check(year.Credits==32 && year.DebtSubjects==0 && flow.currentStage==MVPFlowController.Stage.Stage1_Year1_Arrival,"Year 1 credits are 32 of 150 with no debt before stage transition");
        CaptureUI("year1-academic-card");
        yield return PressKey(Key.Space);
        yield return Until(()=>year.FeastVisible && year.DialogueVisible,3f,"Academic continue returns to Room404 hotpot");
        Check(GameObject.Find("Room404_Hotpot").GetComponentsInChildren<SpriteRenderer>().Length==5,"Four friends and animated electric hotpot appear");
        Check(GameObject.Find("Room404_Hotpot").GetComponent<AudioSource>().isPlaying,"Hotpot bubbling sound plays");
        CaptureUI("year1-hotpot");
        for(int i=0;i<4;i++){yield return PressKey(Key.Space);yield return Wait(.15f);}
        CaptureUI("year1-toast");
        yield return PressKey(Key.Space);
        yield return Until(()=>flow.currentStage==MVPFlowController.Stage.Stage2_Year2_Temptation,4f,"Hotpot hubris dialogue transitions to actual Year2 stage");
        Check(year.HeardHubris && !year.FeastVisible && body.simulated && !journey.IsBusy,"Year2 restores standing player and removes feast");
        Check(flow.timeText.text.Contains("23:40") && flow.laptopLight.intensity>1f,"Year2 uses existing night laptop temptation setup");
        Check(GameObject.Find("Room_Backdrop").GetComponent<SpriteRenderer>().color.b<.7f,"Year2 darkens unlit room artwork as well as light settings");
        CaptureUI("year1-to-year2");
    }
    private static IEnumerator Checks()
    {
        yield return Wait(0.3f);
        var campaign=Object.FindAnyObjectByType<YearOneCampaignController>();
        if(campaign!=null)
        {
            campaign.UseStoreForTests(Path.Combine(Application.temporaryCachePath,"year1-suite-"+Guid.NewGuid().ToString("N")));
            var campaignMenu=Object.FindAnyObjectByType<YearOneCampaignUI>();
            yield return Until(()=>campaignMenu.StartupVisible,3f,"Campaign offers New Game / Continue before play");
            campaignMenu.NewGameButton.onClick.Invoke();yield return Wait(.2f);
        }
        var player = GameObject.Find("Player_Luon");
        var state = player.GetComponent<PlayerInteractionStateController>();
        var detector = player.GetComponent<PlayerInteractionDetector>();
        var movement = player.GetComponent<PlayerMovement>();
        var body = player.GetComponent<Rigidbody2D>();
        var visual = state.CharacterVisual;
        var renderer = visual.GetComponent<SpriteRenderer>();
        int standingOrder = renderer.sortingOrder;
        Vector3 standingScale = visual.transform.localScale;
        Check(state.CanInteract && body.simulated, "Standing starts unlocked with physics enabled");
        Check(visual.GetComponent<Animator>() == null || !visual.GetComponent<Animator>().enabled,
            "Visual has a single active sprite animation writer");
        Check(Object.FindAnyObjectByType<BackpackInteractable>()!=null && Object.FindAnyObjectByType<BackpackInteractable>().CanInteract,
            "Wall hook is available immediately without claiming story bed");
        Capture("room-standing");
        var door = Object.FindAnyObjectByType<HingedRoomDoor>();
        var blocker = door.GetComponent<BoxCollider2D>();
        Place(player, door.transform.position + new Vector3(-0.64f, -0.8f));
        yield return Wait(0.1f);
        Check(detector.FindClosestInteractable() == (IInteractable)door, "Door selected through detector near doorway");
        Check(door.InteractionPrompt == "E - Mở cửa", "Closed door prompt");
        yield return PressE();
        Check(!blocker.enabled, "Door blocker disabled during opening");
        yield return Until(() => door.CurrentAngle > 69f, 2f, "E opens door through real Update");
        Check(door.InteractionPrompt == "E - Đóng cửa", "Open door prompt");
        var hallway = door.transform.Find("Doorway_Opening").GetComponent<SpriteRenderer>();
        Check(hallway.sprite != null && hallway.sprite.texture.width > 1 && hallway.color == Color.white,
            "Door opening uses opaque hallway artwork instead of a dark pixel");
        Check(detector.FindClosestInteractable() == (IInteractable)door, "Opened door remains detectable through independent trigger");
        var doorShadow = door.transform.Find("Door_Shadow").GetComponent<SpriteRenderer>();
        Check(doorShadow.sprite != null && doorShadow.enabled, "Open door has projected floor shadow");
        Capture("door-open");
        float startY = player.transform.position.y;
        movement.SetSimulatedInput(Vector2.up);
        yield return Wait(0.28f);
        movement.SetSimulatedInput(Vector2.zero);
        yield return Wait(0.06f);
        Check(player.transform.position.y > startY + 0.6f, "Player physically walks through opened doorway");
        var journey = Object.FindAnyObjectByType<DormJourneyController>();
        Check(journey != null && !journey.IsInHallway && !journey.HasBackpack,
            "Leaving for class requires backpack; doorway does not transition without it");
        Place(player, door.transform.position + new Vector3(-0.64f, 0.3f));
        yield return PressE();
        yield return Wait(0.8f);
        Check(door.CurrentAngle > 69f && !blocker.enabled, "Cannot close door through player in sweep");
        Place(player, door.transform.position + new Vector3(-0.64f, -0.9f));
        yield return PressE();
        yield return Until(() => door.CurrentAngle < 0.1f, 2f, "E closes door after player clears sweep");
        Check(blocker.enabled, "Closed door restores blocker");
        Capture("door-closed");

        var other = new GameObject("OccupancyProbe");
        other.transform.position = new Vector3(1000f, 1000f, 0f);
        other.AddComponent<PlayerMovement>();
        var otherBody = other.GetComponent<Rigidbody2D>();
        otherBody.gravityScale = 0f;
        var otherState = other.AddComponent<PlayerInteractionStateController>();
        var chairs = Object.FindObjectsByType<ChairInteractable>();
        Check(chairs.Length == 2, "Exactly two chair interactions");
        foreach (var chair in chairs)
        {
            Place(player, chair.ExitPivot.position);
            yield return Wait(0.1f);
            Check(detector.FindClosestInteractable() == (IInteractable)chair, chair.name + " nearest at ExitPivot");
            yield return PressE();
            Check(state.CurrentState == PlayerInteractionState.Sitting && chair.Occupant == state, chair.name + " E sits and claims occupancy");
            chair.Interact(otherState);
            Check(chair.Occupant == state && otherState.CurrentState == PlayerInteractionState.Standing,
                chair.name + " rejects a second occupant");
            Check(renderer.sprite == visual.sitLaptopSprite, chair.name + " sitting sprite selected");
            var menu = Object.FindAnyObjectByType<DeskActionModal>();
            Check(menu != null && menu.IsOpen, "Sitting opens desk action menu");
            yield return PressKey(Key.Digit1);
            Check(menu.CurrentAction == DeskActionModal.DeskAction.Computer && menu.IsTyping,
                "Key 1 activates laptop activity and actual keyboard sound");
            Check(Object.FindAnyObjectByType<MVPFlowController>().laptopLight.intensity > 1f, "Computer action lights laptop");
            Check(visual.DeskPose==LuonCharacterVisual.DeskPoseState.SittingTyping && GameObject.Find("Activity_Screen_Glow").GetComponent<SpriteRenderer>().enabled,
                "Computer selects typing pose and visible blue screen glow");
            yield return ObserveDeskFrames(visual,2,chair.name+"-typing");
            MVPSceneSnapshot.SaveCameraFrame(Camera.main, Path.Combine(Output, chair.name + "-computer-ui.png"), true);
            if(campaign!=null)
            {
                Check(menu.ComputerVisible,"Computer opens independent Windows desktop");
                menu.Desktop.CloseButton.onClick.Invoke();
                yield return PressKey(Key.Digit2);
                var studyMenu=Object.FindAnyObjectByType<YearOneCampaignUI>();
                Check(studyMenu.StudyVisible,"Study opens six-course task/progress UI");
                foreach(var task in studyMenu.WorkButtons)Check(!task.interactable,"Opening-day course tasks remain locked");
                studyMenu.HideStudy();
            }
            else
            {
            float studyBefore = journey.StudyProgress;
            yield return PressKey(Key.Digit2);
            yield return Wait(.6f);
            Check(menu.CurrentAction == DeskActionModal.DeskAction.Study && menu.IsBookVisible && !menu.IsTyping && journey.StudyProgress > studyBefore,
                "Key 2 shows book, stops typing and advances study progress");
            Check(visual.DeskPose==LuonCharacterVisual.DeskPoseState.SittingStudying && menu.IsWriting,
                "Study selects pen-writing pose and paper audio");
            yield return ObserveDeskFrames(visual,3,chair.name+"-writing");
            MVPSceneSnapshot.SaveCameraFrame(Camera.main, Path.Combine(Output, chair.name + "-study-ui.png"), true);
            }
            Check(Vector3.Distance(player.transform.position, chair.SeatPivot.position) < 0.01f, chair.name + " registered to seat marker");
            Check(movement.IsInteractionMovementLocked && !body.simulated, chair.name + " movement and physics locked");
            Vector3 atSeat = player.transform.position;
            movement.SetSimulatedInput(Vector2.right);
            yield return Wait(0.25f);
            Check(Vector3.Distance(player.transform.position, atSeat) < 0.001f, chair.name + " no walking or physics drift");
            foreach (var bed in Object.FindObjectsByType<BedInteractable>()) bed.Interact(state);
            Check(state.CurrentState == PlayerInteractionState.Sitting, "Bed rejects seated player");
            Capture(chair.name + "-sitting");
            yield return PressE();
            Check(state.CurrentState == PlayerInteractionState.Standing && body.simulated && !movement.IsInteractionMovementLocked,
                chair.name + " E stands and unlocks");
            Check(!menu.IsOpen && !menu.IsTyping && !menu.IsWriting && visual.DeskPose==LuonCharacterVisual.DeskPoseState.None,
                "Standing closes desk menu, activity animation and all desk sounds");
            Check(Vector3.Distance(player.transform.position, chair.ExitPivot.position) < 0.01f, chair.name + " exits to marker");
            Check(renderer.sortingOrder == standingOrder && visual.transform.localScale == standingScale, "Standing presentation restored after chair");
        }
        Place(player, chairs[0].ExitPivot.position);
        yield return PressE();
        var standMenu = Object.FindAnyObjectByType<DeskActionModal>();
        standMenu.StandButton.onClick.Invoke();
        Check(state.CurrentState == PlayerInteractionState.Standing && !standMenu.IsOpen, "Stand-up button exits chair through the same component");
        var flow = Object.FindAnyObjectByType<MVPFlowController>();
        Check(flow.currentStage == MVPFlowController.Stage.Stage1_Year1_Arrival, "World interactions preserve story stage");
        flow.bedInteractable.Interact(); // Real prerequisite event; does not force its flag.
        Check(flow.backpackProp.activeSelf && !flow.bedInteractable.IsActive, "Original bed-claim story event still works");
        foreach (var bed in Object.FindObjectsByType<BedInteractable>())
        {
            Place(player, bed.ExitPivot.position);
            yield return Wait(0.1f);
            Check(detector.FindClosestInteractable() == (IInteractable)bed, bed.transform.parent.name + " selectable at entry marker");
            yield return PressE();
            Check(state.CurrentState == PlayerInteractionState.Transitioning && state.ActiveInteractable == (IInteractable)bed,
                "Bed retains owner throughout busy transition");
            Check(!body.simulated && movement.IsInteractionMovementLocked, "Bed transition locks player");
            Sprite firstEnterFrame = renderer.sprite;
            Capture(bed.transform.parent.name + "-enter");
            yield return PressE(); // Repeated input cannot interrupt the transition.
            var seenFrames = new HashSet<Sprite> { renderer.sprite, firstEnterFrame };
            int frame = 0;
            float end = Time.time + 3f;
            while (state.CurrentState == PlayerInteractionState.Transitioning && Time.time < end)
            {
                seenFrames.Add(renderer.sprite);
                Capture(bed.transform.parent.name + "-enter-frame-" + frame++);
                yield return Wait(.12f);
            }
            Check(state.CurrentState == PlayerInteractionState.Lying, "Actual enter coroutine reaches Lying");
            Check(seenFrames.Count >= 3, "Enter animation displays distinct generated frames over time");
            Check(bed.Occupant == state && Vector3.Distance(player.transform.position, bed.BedPivot.position) < 0.01f,
                "Bed keeps occupancy and settles on BedPivot");
            bed.Interact(otherState);
            Check(bed.Occupant == state && otherState.CurrentState == PlayerInteractionState.Standing, "Bed rejects a second occupant");
            foreach (var chair in chairs) chair.Interact(state);
            Check(state.CurrentState == PlayerInteractionState.Lying, "Chair rejects lying player");
            Vector3 lyingPosition = player.transform.position;
            movement.SetSimulatedInput(Vector2.left);
            Capture(bed.transform.parent.name + "-lying-a");
            yield return Wait(1f);
            Capture(bed.transform.parent.name + "-lying-b");
            Check(Vector3.Distance(player.transform.position, lyingPosition) < 0.001f && !body.simulated, "Lying cannot walk or drift");
            visual.SetSleeping(true);
            Check(state.CurrentState == PlayerInteractionState.Sleeping && renderer.sprite == visual.bedSleepSprite, "Sleep pose and state agree");
            Capture(bed.transform.parent.name + "-sleep");
            yield return PressE();
            Check(state.CurrentState == PlayerInteractionState.Transitioning, "E starts actual bed exit coroutine");
            Capture(bed.transform.parent.name + "-exit");
            frame = 0;
            end = Time.time + 3f;
            while (state.CurrentState == PlayerInteractionState.Transitioning && Time.time < end)
            {
                Capture(bed.transform.parent.name + "-exit-frame-" + frame++);
                yield return Wait(.12f);
            }
            Check(state.CurrentState == PlayerInteractionState.Standing, "Actual exit coroutine reaches Standing");
            Check(bed.Occupant == null && body.simulated && !movement.IsInteractionMovementLocked, "Bed exit releases occupancy and physics");
            Check(Vector3.Distance(player.transform.position, bed.ExitPivot.position) < 0.01f, "Bed exit reaches authored marker");
            Check(renderer.sprite == visual.CurrentStandingSprite && renderer.sortingOrder == standingOrder, "Bed exit restores standing sprite and sorting");
        }
        Object.Destroy(other);
        Place(player, new Vector3(0f, -1.5f));
        float x = player.transform.position.x;
        movement.SetSimulatedInput(Vector2.right);
        yield return Wait(0.2f);
        movement.SetSimulatedInput(Vector2.zero);
        Check(player.transform.position.x > x + 0.3f, "Normal movement works after all interactions");
        if(campaign!=null)yield return CampaignChecks(player,state,movement,campaign);
        else yield return JourneyChecks(player, state, movement);
        MVPSceneTester.SimulateMVP();
        yield return Until(() => MVPSceneTester.Completed, 60f, "Four-stage story simulation completed in Play Mode");
        Check(MVPSceneTester.Success, "Story stages 1–4 and ending pass without forcing stages");
        yield return Wait(.1f);
        Check(!GameObject.Find("Canvas_DormJourney").transform.Find("Journey_HUD").gameObject.activeSelf,
            "Journey UI yields to the existing story ending");
        Check(errors.Count == 0, "No runtime error, exception or assertion logged");
    }
}
