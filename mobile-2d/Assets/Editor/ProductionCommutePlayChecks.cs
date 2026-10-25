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
public static class ProductionCommutePlayChecks
{
    private const string Phase="Room404.CommuteChecks";
    private static string Request=>Path.Combine(Application.dataPath,"../Temp/ProductionCommutePlayChecks.run");
    private static string Output=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/mvp_sprites/remaining/review/commute-playmode"));
    private static readonly Stack<IEnumerator> routines=new Stack<IEnumerator>();
    private static readonly List<string> passed=new List<string>(),errors=new List<string>();
    private static Keyboard keyboard;
    private static InputSettings.BackgroundBehavior previousBackground;
    private static InputSettings.EditorInputBehaviorInPlayMode previousEditorInput;
    private static bool previousRun;
    private static double allowedAfter,deadline,nextHeartbeat;
    private static string sandbox;
    [Serializable] private sealed class Report { public bool success;public string finishedUtc,scope="Production trip host on physical bus and WC fixture; authored M06–08 route integration and full game are not covered";public string[] passed,errors; }
    static ProductionCommutePlayChecks()
    {
        allowedAfter=EditorApplication.timeSinceStartup+3;EditorApplication.update+=Poll;EditorApplication.playModeStateChanged+=Changed;
    }
    private static void Poll()
    {
        if(!File.Exists(Request)||EditorApplication.timeSinceStartup<allowedAfter||EditorApplication.isCompiling||EditorApplication.isUpdating||EditorApplication.isPlayingOrWillChangePlaymode)return;
        File.Delete(Request);
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
        Application.logMessageReceived+=Log;deadline=EditorApplication.timeSinceStartup+240;
        routines.Push(Checks());EditorApplication.update+=Tick;
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
        File.WriteAllText(Path.Combine(Output,"report.json"),JsonUtility.ToJson(new Report{success=errors.Count==0,finishedUtc=DateTime.UtcNow.ToString("O"),passed=passed.ToArray(),errors=errors.ToArray()},true));
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
    private static Transform Marker(string name,Vector3 position)
    {var go=new GameObject(name);go.transform.position=position;return go.transform;}
    private static IEnumerator Checks()
    {
        var c=Object.FindAnyObjectByType<ProductionOpeningController>();yield return Wait(.3f);
        c.enabled=false;c.startPanel.SetActive(false);c.player.ExitInteraction();
        if(c.view.GetComponent<AudioListener>()==null)c.view.gameObject.AddComponent<AudioListener>();
        var session=new ProductionCampaignSession(sandbox,c.definition.text);
        for(int i=0;i<199 && !(session.Progress.CurrentMission==6 && session.Progress.CurrentStep.id=="take_bus");i++)
        {
            var p=session.Progress;var step=p.CurrentStep;
            Check(session.Commit(p.CurrentMission,step.id,step.choices.Length>0?step.choices[0].id:"",out var error),"Fixture earns event "+p.CurrentMission+":"+step.id+" "+error);
        }
        Check(session.Progress.CurrentStep.id=="take_bus","Fixture reaches production school trip through model events");
        // Runtime-only fixture: existing scene art, real player/physics/transition controllers.
        // No scene is saved and no legacy campaign is configured.
        var root=new GameObject("ProductionCommuteFixture");root.SetActive(false);
        var journey=root.AddComponent<DormJourneyController>();var bus=root.AddComponent<CampusCommuteController>();
        var audio=root.AddComponent<AudioSource>();
        var background=c.zones[0].Find("Backdrop").GetComponent<SpriteRenderer>();
        var vehicle=new GameObject("FixtureBus").AddComponent<SpriteRenderer>();
        vehicle.sprite=c.zones[0].Find("DormBus").GetComponent<SpriteRenderer>().sprite;
        vehicle.gameObject.layer=background.gameObject.layer;c.zones[0].Find("DormBus").gameObject.SetActive(false);
        vehicle.transform.localScale=Vector3.one*.65f;vehicle.sortingOrder=20;
        var stop=Marker("FixtureStop",new Vector3(0,-2.7f,0));
        var park=Marker("FixturePark",new Vector3(1,-2.5f,0));
        var entry=Marker("FixtureEntry",new Vector3(-12,-2.5f,0));
        var exit=Marker("FixtureExit",new Vector3(13,-2.5f,0));
        journey.Configure(c.player,c.view,c.spawns[0],c.spawns[0],background,null,null,null,null,null,audio,null);
        bus.Configure(journey,c.spawns[0],c.spawns[0],stop,park,exit,stop,background,background,vehicle,audio,null,null,null);
        bus.ConfigureEntry(entry);journey.ConfigureCommute(bus);
        Vector3 explicitRoomCamera=c.view.transform.position+Vector3.right*24;
        journey.ConfigureRoomView(explicitRoomCamera,3.75f);
        root.SetActive(true);
        Check(journey.BeginJourneyTransition(),"Fixture can restore explicit production room view");
        journey.RestoreRoom();
        Check(Vector3.Distance(c.view.transform.position,explicitRoomCamera)<.01f && Mathf.Approximately(c.view.orthographicSize,3.75f),"Configured room camera survives Awake at a different opening zone");
        journey.FinishJourneyTransition();journey.ConfigureRoomView(new Vector3(0,0,-10),4.5f);journey.RestoreDailyFlags(true,true);
        var host=new ProductionCommuteProgress(session,6,"take_bus");bus.BindProgress(host);
        bool arrived=false;
        bus.CampaignArrival=returning=>Arrive(journey,bus,c,session,background,refArrival:()=>arrived=true);
        Check(journey.BeginJourneyTransition(),"Fixture takes journey transition ownership");
        journey.ArriveInArea(DormJourneyController.JourneyArea.BusStop,stop,background);journey.FinishJourneyTransition();
        int wallet=session.Progress.Money;
        Check(!bus.TryBoard(c.player) && session.Progress.Money==wallet,"Cannot board or charge before bus parks");
        yield return Until(()=>bus.BusAtStop,5,"Actual vehicle approaches and parks at stop");
        Check(vehicle.enabled && Vector3.Distance(vehicle.transform.position,park.position)<.02f,"Bus is visible at physical boarding point");
        Capture(c,"host-bus-parked");
        string obstruction=Path.Combine(sandbox,"production-campaign-v1.json.tmp");Directory.CreateDirectory(obstruction);
        Check(bus.TryBoard(c.player) && !bus.TryBoard(c.player),"Boarding takes one lock and rejects repeated input");
        yield return Until(()=>!journey.IsBusy,8,"Save failure returns actor to safe stop and releases lock");
        Check(!arrived && !bus.HasCompletedTrip && bus.BusAtStop && bus.TicketsPurchased==0 && session.Progress.Money==wallet,"Failed durable arrival neither charges fare nor enters destination");
        Check(c.player.CanInteract && c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled && c.player.GetComponent<Rigidbody2D>().simulated,"Failed arrival restores actor visibility, control and physics");
        Directory.Delete(obstruction);
        Check(bus.TryBoard(c.player),"Player can retry same visible bus after storage failure");
        yield return Until(()=>arrived && !journey.IsBusy,8,"Successful retry commits before destination unlock");
        Check(bus.HasCompletedTrip && bus.TicketsPurchased==1 && bus.MoneyVnd==wallet-7000 && session.Progress.CurrentStep.id=="enter_class","One fare from production wallet and correct durable next step");
        Check(!bus.TryBoard(c.player) && session.Progress.Money==wallet-7000,"Completed physical trip cannot repeat fare");
        var reload=new ProductionCampaignSession(sandbox,c.definition.text);
        Check(reload.Progress.Money==wallet-7000 && reload.Progress.CurrentStep.id=="enter_class" && reload.Progress.Credits==0,"Physical trip checkpoint reloads without credits or duplicate charge");
        // Next morning: rebind to an earned M07 step, wait for the recovery vehicle,
        // then interrupt boarding. A cancelled presentation must not publish a trip.
        for(int i=0;i<199 && !(session.Progress.CurrentMission==7 && session.Progress.CurrentStep.id=="catch_bus");i++)
        {
            var p=session.Progress;var step=p.CurrentStep;
            Check(session.Commit(p.CurrentMission,step.id,step.choices.Length>0?step.choices[0].id:"",out var error),"Recovery fixture earns "+step.id+" "+error);
        }
        var recovery=new ProductionCommuteProgress(session,7,"catch_bus","next_bus");
        bus.BindProgress(recovery);bus.PrepareTrip(false);
        Check(journey.BeginJourneyTransition(),"Recovery fixture starts at a new morning stop");
        journey.ArriveInArea(DormJourneyController.JourneyArea.BusStop,stop,background);journey.FinishJourneyTransition();
        int beforeRecovery=session.Progress.Money;
        yield return Until(()=>bus.BusAtStop,6,"Production recovery host waits for actual next bus");
        Check(bus.MissedBus && vehicle.enabled && session.Progress.Money==beforeRecovery,"Recovery vehicle is visible before any fare or progress");
        Check(bus.TryBoard(c.player),"Recovery boarding takes actor lock");
        bus.enabled=false;yield return Wait(.15f);
        Check(!journey.IsBusy && c.player.CanInteract && c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled && c.player.GetComponent<Rigidbody2D>().simulated,"Interrupted boarding restores renderer, control and physics");
        Check(session.Progress.CurrentStep.id=="catch_bus" && session.Progress.Money==beforeRecovery && !session.Progress.Has("travel:missed_bus_recovered"),"Interrupted recovery leaves fare and event unchanged");
        // WC's success callback must run after washing and before the actor unlocks.
        bus.enabled=false;
        Check(journey.BeginJourneyTransition(),"Fixture can enter hallway for WC adapter check");
        journey.ArriveInArea(DormJourneyController.JourneyArea.InHallway,stop,background);journey.FinishJourneyTransition();journey.RestoreDailyFlags(true,false);
        int washCalls=0;bool allowWash=false;
        journey.CommitWashCompletion=()=>{Check(journey.IsBusy && !c.player.CanInteract,"Wash event is committed while actor is still locked");washCalls++;return allowWash;};
        Check(!journey.UseRestroom(c.player,RestroomInteractable.Gender.Female,stop),"Female restroom rejection cannot commit progress");
        Check(journey.UseRestroom(c.player,RestroomInteractable.Gender.Male,stop),"Male restroom starts physical wash");
        yield return Until(()=>!journey.IsBusy,6,"Failed wash save releases actor after completed presentation");
        Check(washCalls==1 && !journey.HasRefreshed && c.player.CanInteract,"Failed wash commit keeps morning gate closed");
        allowWash=true;
        Check(journey.UseRestroom(c.player,RestroomInteractable.Gender.Male,stop),"Wash can retry after failed save");
        yield return Until(()=>!journey.IsBusy,6,"Successful wash commit releases actor");
        Check(washCalls==2 && journey.HasRefreshed && c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled,"Wash success fires once and restores actor");
        Check(errors.Count==0,"No runtime exceptions in transport and wash adapter fixture");
    }
    private static IEnumerator Arrive(DormJourneyController journey,CampusCommuteController bus,ProductionOpeningController c,ProductionCampaignSession session,SpriteRenderer backdrop,Action refArrival)
    {
        Check(journey.IsBusy && !c.player.CanInteract && session.Progress.CurrentStep.id=="enter_class","Destination callback sees durable progress before unlocking");
        bus.RestoreBoardedActor();journey.ArriveInArea(DormJourneyController.JourneyArea.University,c.spawns[0],backdrop);
        yield return journey.FadeTo(0,.1f);journey.FinishJourneyTransition();refArrival();
    }
}
