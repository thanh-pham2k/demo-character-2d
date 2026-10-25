using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using Object=UnityEngine.Object;

[InitializeOnLoad]
public static partial class ProductionOpeningPlayChecks
{
    private const string Phase="Room404.OpeningChecks";
    private static string Request=>Path.Combine(Application.dataPath,"../Temp/ProductionOpeningPlayChecks.run");
    private static string SchoolRequest=>Path.Combine(Application.dataPath,"../Temp/ProductionSchoolPlayChecks.run");
    private static string YearOneRequest=>Path.Combine(Application.dataPath,"../Temp/ProductionYearOnePlayChecks.run");
    private static string LifecycleRequest=>Path.Combine(Application.dataPath,"../Temp/ProductionOpeningLifecycleChecks.run");
    private static string RegistrationRequest=>Path.Combine(Application.dataPath,"../Temp/ProductionRegistrationPlayChecks.run");
    private static bool RegistrationOnly=>SessionState.GetBool(Phase+".registrationOnly",false);
    private static bool LifecycleOnly=>SessionState.GetBool(Phase+".lifecycleOnly",false);
    private static bool YearOneOnly=>SessionState.GetBool(Phase+".yearOneOnly",false);
    private static bool SchoolOnly=>SessionState.GetBool(Phase+".schoolOnly",false);
    private static string Output=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/mvp_sprites/remaining/review/"+(RegistrationOnly?"registration-playmode":LifecycleOnly?"opening-lifecycle-playmode":YearOneOnly?"year-one-production-playmode":SchoolOnly?"school-playmode":"opening-playmode")));
    private static readonly Stack<IEnumerator> routines=new Stack<IEnumerator>();
    private static readonly List<string> passed=new List<string>(),errors=new List<string>();
    private static Keyboard keyboard;
    private static InputSettings.BackgroundBehavior previousBackground;
    private static InputSettings.EditorInputBehaviorInPlayMode previousEditorInput;
    private static bool previousRun;
    private static double allowedAfter,deadline,nextHeartbeat;
    private static string sandbox;
    [Serializable] private sealed class Report { public bool success;public string finishedUtc,scope="M01–06 physical production opening and school route in separate scene; M07–45 and full game are not covered";public string[] passed,errors; }
    static ProductionOpeningPlayChecks()
    {
        allowedAfter=EditorApplication.timeSinceStartup+3;EditorApplication.update+=Poll;EditorApplication.playModeStateChanged+=Changed;
    }
    private static void Poll()
    {
        if(!File.Exists(Request) && !File.Exists(SchoolRequest) && !File.Exists(YearOneRequest) && !File.Exists(LifecycleRequest) && !File.Exists(RegistrationRequest) ||EditorApplication.timeSinceStartup<allowedAfter||EditorApplication.isCompiling||EditorApplication.isUpdating||EditorApplication.isPlayingOrWillChangePlaymode)return;
        SessionState.SetBool(Phase+".registrationOnly",File.Exists(RegistrationRequest));SessionState.SetBool(Phase+".lifecycleOnly",!RegistrationOnly && File.Exists(LifecycleRequest));SessionState.SetBool(Phase+".yearOneOnly",!RegistrationOnly && !LifecycleOnly && File.Exists(YearOneRequest));SessionState.SetBool(Phase+".schoolOnly",!RegistrationOnly && !LifecycleOnly && !YearOneOnly && File.Exists(SchoolRequest));File.Delete(RegistrationOnly?RegistrationRequest:LifecycleOnly?LifecycleRequest:YearOneOnly?YearOneRequest:SchoolOnly?SchoolRequest:Request);
        var active=SceneManager.GetActiveScene();
        if(active.isDirty)throw new InvalidOperationException("Save current scene before opening checks.");
        SessionState.SetString(Phase+".original",active.path);SessionState.SetString(Phase,"entering");
        EditorSceneManager.OpenScene(ProductionOpeningSceneBuilder.ScenePath);
        EditorApplication.isPlaying=true;
    }
    private static void Changed(PlayModeStateChange state)
    {
        if(state==PlayModeStateChange.EnteredPlayMode && SessionState.GetString(Phase,"")=="entering")Start();
        if(state==PlayModeStateChange.EnteredEditMode && SessionState.GetString(Phase,"")=="leaving")
        {
            string original=SessionState.GetString(Phase+".original","");SessionState.EraseString(Phase);
            if(!string.IsNullOrEmpty(original))EditorSceneManager.OpenScene(original);
        }
    }
    private static void Start()
    {
        SessionState.SetString(Phase,"running");passed.Clear();errors.Clear();routines.Clear();Directory.CreateDirectory(Output);
        sandbox=Path.Combine(Application.temporaryCachePath,"production-opening-"+Guid.NewGuid().ToString("N"));
        previousRun=Application.runInBackground;Application.runInBackground=true;
        previousBackground=InputSystem.settings.backgroundBehavior;previousEditorInput=InputSystem.settings.editorInputBehaviorInPlayMode;
        InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;InputSystem.settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
        keyboard=InputSystem.AddDevice<Keyboard>();keyboard.MakeCurrent();
        Application.logMessageReceived+=Log;deadline=EditorApplication.timeSinceStartup+480;
        routines.Push(RegistrationOnly?RegistrationOnlyChecks():LifecycleOnly?LifecycleOnlyChecks():YearOneOnly?YearOneOnlyChecks():SchoolOnly?SchoolOnlyChecks():Checks());EditorApplication.update+=Tick;
    }
    private static void Log(string message,string stack,LogType type){if(type==LogType.Error || type==LogType.Exception || type==LogType.Assert)errors.Add(message+"\n"+stack);}
    private static void Tick()
    {
        try
        {
            if(!EditorApplication.isPlaying)throw new Exception("Play Mode stopped before opening checks completed");
            if(EditorApplication.timeSinceStartup>deadline)throw new TimeoutException("Opening checks timed out");
            if(EditorApplication.timeSinceStartup>nextHeartbeat)
            {nextHeartbeat=EditorApplication.timeSinceStartup+2;File.WriteAllText(Path.Combine(Output,"running.txt"),DateTime.UtcNow.ToString("O")+"\npassed="+passed.Count+" routines="+routines.Count+"\n"+(passed.Count>0?passed[passed.Count-1]:"starting"));}
            if(routines.Count==0){Finish();return;}
            var current=routines.Peek();if(!current.MoveNext()){routines.Pop();return;}if(current.Current is IEnumerator child)routines.Push(child);
        }
        catch(Exception ex){errors.Add(ex.ToString());Finish();}
    }
    private static void Finish()
    {
        EditorApplication.update-=Tick;Application.logMessageReceived-=Log;
        File.WriteAllText(Path.Combine(Output,"report.json"),JsonUtility.ToJson(new Report{scope=RegistrationOnly?"M09 physical computer/portal interaction with earned model prefix; M10-M45 and full opening not covered":LifecycleOnly?"Runtime despawn and cleanup at M09 with earned model prefix; physical opening route not covered":YearOneOnly?"M07-M08 physical production route, on-time/next-bus choices with earned model prefix M01-M06; M09-M45 and full game not covered":SchoolOnly?"M06 physical production route with earned model prefix M01-M05; full opening and M07-M45 not covered":"M01-M09 physical production opening, school, hotpot and registration; M10-M45 and full game not covered",success=errors.Count==0,finishedUtc=DateTime.UtcNow.ToString("O"),passed=passed.ToArray(),errors=errors.ToArray()},true));
        if(keyboard!=null && keyboard.added)InputSystem.RemoveDevice(keyboard);
        InputSystem.settings.backgroundBehavior=previousBackground;InputSystem.settings.editorInputBehaviorInPlayMode=previousEditorInput;Application.runInBackground=previousRun;
        SessionState.SetString(Phase,"leaving");EditorApplication.isPlaying=false;
    }
    private static void Check(bool value,string description){if(!value)throw new Exception("FAIL: "+description);passed.Add(description);}
    private static IEnumerator Wait(float seconds){double end=EditorApplication.timeSinceStartup+seconds;while(EditorApplication.timeSinceStartup<end)yield return null;}
    private static IEnumerator Until(Func<bool> test,float seconds,string label)
    {double end=EditorApplication.timeSinceStartup+seconds;while(!test() && EditorApplication.timeSinceStartup<end)yield return null;Check(test(),label);}
    private static IEnumerator KeyPress(Key key,float duration=.08f)
    {InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return Wait(duration);InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return Wait(.1f);}
    private static void Capture(ProductionOpeningController c,string name)=>MVPSceneSnapshot.SaveCameraFrame(c.view,Path.Combine(Output,name+".png"),true);
    private static IEnumerator MoveOpen(ProductionOpeningController c)
    {
        ProductionOpeningAction target=null;
        foreach(var action in c.actions)if(action.mission==c.Progress.CurrentMission && action.step==c.Progress.CurrentStep.id && action.isActiveAndEnabled)target=action;
        Check(target!=null,"Current step has a physical interaction anchor");
        var movement=c.player.GetComponent<PlayerMovement>();double end=EditorApplication.timeSinceStartup+8;
        Vector3 point=target.transform.position+Vector3.down*.15f;
        while(Vector2.Distance(c.player.transform.position,point)>.2f && EditorApplication.timeSinceStartup<end)
        {movement.SetSimulatedInput(((Vector2)(point-c.player.transform.position)).normalized);yield return null;}
        movement.SetSimulatedInput(Vector2.zero);yield return Wait(.1f);
        Check(Vector2.Distance(c.player.transform.position,point)<.35f,"Walk reaches "+target.step+" without teleporting");
        yield return KeyPress(Key.E);
        Check(c.ModalVisible && c.Busy,"E opens correct action modal and acquires ownership");
    }
    private static void Drag(ProductionDocumentCard card,ProductionDocumentDrop drop)
    {
        var data=new PointerEventData(EventSystem.current){pointerDrag=card.gameObject,position=card.transform.position};
        ExecuteEvents.Execute(card.gameObject,data,ExecuteEvents.beginDragHandler);
        data.position=drop.transform.position;ExecuteEvents.Execute(card.gameObject,data,ExecuteEvents.dragHandler);
        ExecuteEvents.Execute(drop.gameObject,data,ExecuteEvents.dropHandler);
        ExecuteEvents.Execute(card.gameObject,data,ExecuteEvents.endDragHandler);
    }
    private static IEnumerator Route(string bed,bool thorough)
    {
        var c=Object.FindAnyObjectByType<ProductionOpeningController>();yield return Wait(.2f);
        c.morningDepartureSeconds=bed=="right"?3:120;c.UseSaveDirectoryForTests(Path.Combine(sandbox,bed));c.beginButton.onClick.Invoke();yield return Wait(.15f);
        Check(c.Ready && c.Progress.CurrentMission==1 && c.Progress.Money==35000,"Fresh opening uses isolated save and35000wallet");
        Check(Mathf.Approximately(c.cleaningTimeLimit,60),"Authored cleaning limit is60seconds before test override");
        Check(!c.player.GetComponent<PlayerInteractionDetector>().TryInteract() && !c.ModalVisible,"Remote E cannot use distant tout");
        if(thorough)Capture(c,"station");
        yield return MoveOpen(c);
        if(thorough)
        {
            Vector3 before=c.player.transform.position;c.player.GetComponent<PlayerMovement>().SetSimulatedInput(Vector2.right);yield return Wait(.2f);
            Check(Vector2.Distance(before,c.player.transform.position)<.02f && !c.player.GetComponent<Rigidbody2D>().simulated,"Conversation locks movement and physics");
            yield return KeyPress(Key.Escape);Check(!c.ModalVisible && c.player.CanInteract && c.Progress.ExportEvents().Length==0,"Cancel restores input without recording success");yield return MoveOpen(c);
        }
        c.confirmButton.onClick.Invoke();yield return Wait(.05f);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.CurrentStep.id=="buy_ticket","Tout conversation finishes before ticket purchase");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.Money==28000 && c.Progress.Has("item:dorm_ticket"),"Ticket charges7000once and creates inventory item");c.confirmButton.onClick.Invoke();Check(c.Progress.Money==28000,"Double confirm cannot charge twice");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Until(()=>!c.Busy && c.Progress.CurrentMission==2,4,"Bus transition arrives at reception and releases ownership");
        Check(c.ActiveZone==1 && !c.Progress.Has("item:dorm_ticket"),"Bus ticket consumed on arrival");
        yield return MoveOpen(c);var cards=c.documents.GetComponentsInChildren<ProductionDocumentCard>();var drop=c.documents.GetComponentInChildren<ProductionDocumentDrop>();
        ProductionDocumentCard good=null,bad=null;foreach(var card in cards){if(card.item=="item:admission_documents")good=card;else bad=card;}
        if(thorough)
        {
            Capture(c,"document-inventory");Drag(bad,drop);Check(c.Progress.CurrentStep.id=="present_documents" && c.ModalVisible,"Dragging backpack cannot submit admission document");
            Check(!c.Commit(""),"Document action cannot bypass valid drag/drop");
        }
        Drag(good,drop);yield return Wait(.1f);Check(c.Progress.CurrentStep.id=="receive_room_key" && !c.Progress.Has("item:admission_documents"),"Correct document drop records submission and removes paper");
        yield return MoveOpen(c);if(thorough)Capture(c,"receive-key");c.confirmButton.onClick.Invoke();yield return Wait(.1f);Check(c.Progress.Has("item:room_key"),"Key received after completed paperwork");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Until(()=>!c.Busy && c.ActiveZone==2,4,"Stairs reach hallway404");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Until(()=>!c.Busy && c.ActiveZone==3,4,"Using404key opens room");
        Check(Mathf.Abs((c.player.transform.position-c.zones[3].position).x)<3.3f,"Room entry is inside the visible central floor");
        yield return MoveOpen(c);if(bed=="left")c.leftButton.onClick.Invoke();else c.rightButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.Has("bed:luon_"+bed),"Chosen bed persists as"+bed);
        yield return KeyPress(Key.Escape);Check(c.startPanel.activeSelf && !c.player.CanInteract,"Pause prevents movement");c.beginButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.Has("bed:luon_"+bed) && c.Progress.CurrentStep.id=="place_backpack" && c.player.CanInteract,"Resume reconstructs bed choice at safe room checkpoint");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.placedBag.enabled && Vector2.Distance(c.placedBag.transform.position,(bed=="left"?c.leftBag:c.rightBag).position)<.01f && !c.player.CharacterVisual.BackpackEquipped,"Backpack placed on selected bed and removed from actor");
        yield return MoveOpen(c);
        if(thorough)
        {
            Canvas.ForceUpdateCanvases();Check(c.holdMeter.fillRect.rect.width<1,"Empty hold meter has no filled width");
            Check(!c.Commit(""),"Fan cannot complete without hold");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.Space));yield return Wait(.3f);Canvas.ForceUpdateCanvases();
            Check(Mathf.Abs(c.holdMeter.fillRect.rect.width-c.holdMeter.GetComponent<RectTransform>().rect.width*c.HoldProgress)<2,"Hold meter width matches actual progress");
            c.SendMessage("OnApplicationPause",true);yield return Wait(.1f);
            Check(c.HoldProgress==0 && c.Progress.CurrentStep.id=="repair_fan","Background pause resets hold without completing action");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());c.SendMessage("OnApplicationPause",false);yield return Wait(.1f);
            yield return KeyPress(Key.Space,.5f);Check(c.HoldProgress==0 && c.Progress.CurrentStep.id=="repair_fan","Release resets partial hold without committing");
            yield return KeyPress(Key.Escape);yield return KeyPress(Key.Space,.2f);Check(c.Progress.CurrentStep.id=="repair_fan","Space outside modal cannot repair fan");yield return MoveOpen(c);
        }
        if(thorough)Capture(c,"repair-fan");yield return KeyPress(Key.Space,3.2f);yield return Wait(.1f);
        Check(c.Progress.CurrentMission==4 && c.Progress.CompletedMissions==3 && c.Progress.Has("action:3:repair_fan") && !c.Progress.Finished,"Successful hold completesM03and opensM04without finishing game");
        Check(c.Progress.ExportEvents().Length==10 && c.Progress.Money==28000 && c.player.CanInteract && !c.Busy,"Ten opening events committed exactly once with restored movement");
        var restored=new ProductionCampaignSession(Path.Combine(sandbox,bed),c.definition.text);
        Check(restored.Progress.CurrentMission==4 && restored.Progress.Has("bed:luon_"+bed) && restored.Progress.Has("room:backpack_stored"),"Disk reload preserves opening mission,bed and bag");
        if(thorough)
        {
            var movement=c.player.GetComponent<PlayerMovement>();var visual=c.player.CharacterVisual;var renderer=visual.GetComponent<SpriteRenderer>();
            foreach(var direction in new[]{Vector2.up,Vector2.down,Vector2.left,Vector2.right})
            {
                movement.SetSimulatedInput(direction);yield return Wait(.14f);movement.SetSimulatedInput(Vector2.zero);yield return Wait(.05f);
                string expected=direction==Vector2.up?"luon_casual_back_idle":direction==Vector2.down?"luon_casual_front_idle":"casual_right_idle";
                Check(renderer.sprite.name==expected,"Directional idle follows movement:"+direction);
                Check(Mathf.Abs(renderer.bounds.min.y-c.player.transform.position.y)<.01f,"Registered feet remain at player pivot");
                if(direction.x!=0)Check(renderer.flipX==(direction.x<0),"Single visual writer owns horizontal mirroring");
            }
            visual.SetProductionOutfit(LuonCharacterVisual.ProductionOutfit.Shirt);movement.SetSimulatedInput(Vector2.up);yield return Wait(.14f);movement.SetSimulatedInput(Vector2.zero);yield return Wait(.05f);
            Check(renderer.sprite.name=="luon_shirt_back_idle","Shirt outfit uses generated directional set");
            visual.SetProductionOutfit(LuonCharacterVisual.ProductionOutfit.Robe);movement.SetSimulatedInput(Vector2.right);yield return Wait(.14f);movement.SetSimulatedInput(Vector2.zero);yield return Wait(.05f);
            Check(renderer.sprite.name=="luon_robe_right_idle" && visual.GetComponent<Animator>()==null,"Robe set works without second Animator sprite writer");
            visual.SetProductionOutfit(LuonCharacterVisual.ProductionOutfit.Casual);yield return Wait(.1f);Capture(c,"room-ready");
        }
    }
    private static IEnumerator CleanSpot(ProductionOpeningController c,int index)
    {
        var spot=c.cleaningSpots[index];var movement=c.player.GetComponent<PlayerMovement>();double end=EditorApplication.timeSinceStartup+8;
        Vector3 point=spot.transform.position+Vector3.down*.15f;
        while(Vector2.Distance(c.player.transform.position,point)>.2f && EditorApplication.timeSinceStartup<end)
        {movement.SetSimulatedInput(((Vector2)(point-c.player.transform.position)).normalized);yield return null;}
        movement.SetSimulatedInput(Vector2.zero);yield return Wait(.1f);
        Check(Vector2.Distance(c.player.transform.position,point)<.35f,"Walk reaches dirty spot"+index);
        yield return KeyPress(Key.E);yield return Until(()=>!c.Busy,2,"Scrub releases ownership at spot"+index);
        Check(!spot.dirt.enabled,"Scrub removes only the selected dirt spot");
    }
    private static IEnumerator Housekeeping(ProductionOpeningController c,string bed,bool thorough)
    {
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Until(()=>!c.Busy && c.ActiveZone==4,3,"Room door leads to supplies counter without committing purchase");
        Check(c.Progress.Money==28000 && c.Progress.CurrentStep.id=="buy_cleaning_kit","Entering shop has no financial effect");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Until(()=>!c.Busy && c.ActiveZone==3,4,"Purchase returns supplies to room");
        Check(c.Progress.Money==23000 && c.Progress.Has("item:cleaning_kit") && c.bucket.enabled,"One5000purchase grants reusable cleaning kit");c.confirmButton.onClick.Invoke();Check(c.Progress.Money==23000,"Duplicate confirmation cannot buy kit twice");
        if(thorough)
        {
            c.cleaningTimeLimit=.12f;yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.03f);Check(c.CleaningActive,"Cleaning timer starts from authored action");
            yield return Until(()=>!c.CleaningActive && !c.Busy,2,"Real timer timeout leaves cleaning checkpoint available");
            Check(c.Progress.CurrentStep.id=="clean_four_spots" && c.Progress.Money==23000 && c.CleanedCount==0,"Timeout retries only cleaning without charging again");
        }
        c.cleaningTimeLimit=60;yield return MoveOpen(c);yield return KeyPress(Key.Space);
        Check(c.CleaningActive && !c.player.GetComponent<PlayerMovement>().IsDodging,"Space starts cleaning without leaking into dodge");
        if(thorough)
        {
            float remaining=c.CleaningRemaining;c.SendMessage("OnApplicationPause",true);yield return Wait(.2f);Check(Mathf.Abs(c.CleaningRemaining-remaining)<.03f,"Background pause freezes cleaning timer");c.SendMessage("OnApplicationPause",false);
            yield return CleanSpot(c,0);Capture(c,"cleaning-partial");
            yield return KeyPress(Key.Escape);remaining=c.CleaningRemaining;yield return Wait(.2f);Check(Mathf.Abs(c.CleaningRemaining-remaining)<.03f,"Pause menu freezes cleaning timer");
            c.beginButton.onClick.Invoke();yield return Wait(.1f);
            Check(!c.CleaningActive && c.CleanedCount==0 && c.Progress.Money==23000 && c.Progress.Has("item:cleaning_kit"),"Reload restores safe cleaning checkpoint with purchased kit");
            yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        }
        for(int i=0;i<4;i++)yield return CleanSpot(c,i);
        Check(c.Progress.CurrentStep.id=="meet_roommates" && c.Progress.ExportEvents().Length==12,"Only all four spots commit cleaning once");
        yield return MoveOpen(c);Check(c.roomFriends[0].activeSelf && !c.roomFriends[1].activeSelf,"Long arrives first");Check(!c.Commit(""),"Cannot skip roommate conversation");
        c.confirmButton.onClick.Invoke();yield return Wait(.05f);Check(c.roomFriends[1].activeSelf && !c.roomFriends[2].activeSelf,"To arrives after Long");
        c.confirmButton.onClick.Invoke();yield return Wait(.05f);Check(c.roomFriends[2].activeSelf,"Dam arrives third and identifies HCMUS");
        if(thorough)Capture(c,"roommates");c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.CurrentMission==5 && c.ActiveZone==3,"Introductions lead to meal invitation in room");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Until(()=>!c.Busy && c.ActiveZone==4,3,"Meal invitation leads to canteen without charging meal");
        yield return MoveOpen(c);
        if(thorough)
        {
            foreach(var option in c.Progress.CurrentStep.choices)
            {
                bool meat=option.id=="meat" || option.id=="standard" || option.id=="meat_tea" || option.id=="tea";
                bool veg=option.id!="meat" && option.id!="meat_tea";bool tea=option.id.EndsWith("_tea") || option.id=="tea";
                bool[] wanted={true,meat,veg,tea};for(int i=0;i<4;i++)if(c.MealItemSelected(i)!=wanted[i])c.mealButtons[i].onClick.Invoke();
                Check(c.MealChoice==option.id && c.MealPrice==option.cost && c.confirmButton.interactable,"Menu price matches event contract:"+option.id);
            }
            // Last option is standard+tea (23000); add extra meat to exceed wallet.
            c.extraMeatButton.onClick.Invoke();int money=c.Progress.Money;c.confirmButton.onClick.Invoke();
            Check(!c.confirmButton.interactable && c.Progress.Money==money && c.Progress.CurrentStep.id=="choose_meal","Over-budget cart cannot commit or charge");
            c.extraMeatButton.onClick.Invoke();Capture(c,"meal-menu");
        }
        else c.mealButtons[3].onClick.Invoke();
        int cost=c.MealPrice;string chosen=c.MealChoice;c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.Money==23000-cost && c.Progress.Has("meal:"+chosen),"Selected meal charged once with durable choice");
        yield return KeyPress(Key.Escape);c.beginButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.ActiveZone==4 && c.Progress.Money==23000-cost && c.Progress.CurrentStep.id=="eat_together","Meal checkpoint resumes in canteen without repeat charge");
        yield return MoveOpen(c);Check(!c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled && c.seatedFriends[0].activeSelf,"Dinner transfers presentation to registered seated Luon");
        if(thorough){Capture(c,"dinner-dialogue");MVPSceneSnapshot.SaveCameraFrame(c.view,Path.Combine(Output,"dinner-world.png"));}
        for(int i=0;i<4;i++){c.confirmButton.onClick.Invoke();yield return Wait(.05f);}
        Check(c.Progress.CurrentStep.id=="receive_schedule" && c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled && !c.seatedFriends[0].activeSelf,"Dinner closes with restored standing presentation");
        yield return MoveOpen(c);int before=c.Progress.Money;c.confirmButton.onClick.Invoke();yield return Wait(.1f);Check(c.Progress.Money==before+100000,"Phone schedule includes one-time family support");c.confirmButton.onClick.Invoke();Check(c.Progress.Money==before+100000,"Repeated confirmation cannot duplicate support");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Until(()=>!c.Busy && c.ActiveZone==3 && c.Progress.CurrentMission==6,4,"Dinner return and sleep lead to morningM06");
        Check(c.Progress.Credits==0 && !c.Progress.Finished && c.Progress.ExportEvents().Length==17 && c.Progress.CompletedMissions==5,"M01–05does not skip to year-end or graduation");
        if(thorough)Capture(c,"morning-ready");
    }
    private static IEnumerator Checks()
    {
        yield return Route("left",true);yield return Housekeeping(Object.FindAnyObjectByType<ProductionOpeningController>(),"left",true);yield return SchoolRoute(Object.FindAnyObjectByType<ProductionOpeningController>(),true);yield return YearOneRoute(Object.FindAnyObjectByType<ProductionOpeningController>(),false);yield return RegistrationRoute(Object.FindAnyObjectByType<ProductionOpeningController>(),true);
        EditorSceneManager.LoadSceneInPlayMode(ProductionOpeningSceneBuilder.ScenePath,new LoadSceneParameters(LoadSceneMode.Single));yield return Wait(.3f);
        yield return Route("right",false);yield return Housekeeping(Object.FindAnyObjectByType<ProductionOpeningController>(),"right",false);yield return SchoolRoute(Object.FindAnyObjectByType<ProductionOpeningController>(),false);yield return YearOneRoute(Object.FindAnyObjectByType<ProductionOpeningController>(),true);yield return RegistrationRoute(Object.FindAnyObjectByType<ProductionOpeningController>(),false);
        var final=Object.FindAnyObjectByType<ProductionOpeningController>();
        Object.Destroy(final.feastActors[0]);yield return Wait(.1f);final.enabled=false;yield return Wait(.1f);
        Check(!final.ModalVisible && final.player.CanInteract,"Scene cleanup tolerates already-destroyed feast actors without losing player control");
        Check(errors.Count==0,"No runtime errors in either bed route");
    }
}
