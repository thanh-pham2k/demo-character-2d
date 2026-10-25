using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object=UnityEngine.Object;

// Isolated real Play Mode checks. Opt in using Temp/YearOneV2PlayChecks.run.
[InitializeOnLoad] public static class YearOneV2PlayChecks
{
    const string Key="Room404.V2PlayCheck";
    static int phase,checks;static float next;static double startAfter,deadline;
    static YearOneCampaignController campaign;static YearOneCampaignUI ui;static YearOneRuntimeUI runtime;static YearOneMilestoneController year;
    static DeskActionModal desk;static PlayerInteractionStateController player;static ChairInteractable chair;static BedInteractable bed;
    static float before;static string directory;
    static string Output=>Path.GetFullPath("PlayModeInteractionReports/year-one-v2");
    static string runtimeError="";
    static YearOneV2PlayChecks(){EditorApplication.update+=Tick;Application.logMessageReceived+=(message,stack,type)=>{if(!message.StartsWith("Curl error",StringComparison.Ordinal) && SessionState.GetBool(Key,false) && (type==LogType.Error || type==LogType.Exception || type==LogType.Assert))runtimeError=message;};}
    static void Check(bool condition,string message){checks++;if(!condition)throw new Exception(message);}
    public static void RunBatch(){SessionState.SetInt(Key+".start",0);EditorApplication.delayCall+=WaitForBatchEditor;}
    public static void RunBatchReminder(){SessionState.SetInt(Key+".start",7);EditorApplication.delayCall+=WaitForBatchEditor;}
    static void WaitForBatchEditor()
    {
        if(EditorApplication.timeSinceStartup<30 || EditorApplication.isCompiling || EditorApplication.isUpdating){EditorApplication.delayCall+=WaitForBatchEditor;return;}
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Room404_MVP.unity");
        StartBatch();
    }
    static void StartBatch()
    {
        YearOneV2Checks.Run();Directory.CreateDirectory(Output);
        File.WriteAllText(Path.Combine(Output,"play.txt"),"RUNNING BATCH\n");checks=0;runtimeError="";
        SessionState.SetBool(Key,true);SessionState.SetBool(Key+".background",Application.runInBackground);SessionState.SetInt(Key+".scenario",SessionState.GetInt(Key+".start",0));Begin();
    }
    static void ExitBatch(int code){if(Application.isBatchMode)EditorApplication.delayCall+=()=>EditorApplication.Exit(code);}
    static int Scenario=>SessionState.GetInt(Key+".scenario",0);
    static void Capture(string name)=>MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.Combine(Output,name+".png"),true);
    static void Begin(){EditorApplication.isPaused=false;phase=0;next=0;startAfter=EditorApplication.timeSinceStartup+3;deadline=EditorApplication.timeSinceStartup+100;EditorApplication.isPlaying=true;}
    static void Done()
    {
        File.AppendAllText(Path.Combine(Output,"play.txt"),"PASS scenario "+Scenario+" • assertions "+checks+"\n");
        int scenario=Scenario+1;SessionState.SetInt(Key+".scenario",scenario);EditorApplication.isPlaying=false;
        if(scenario==8){Application.runInBackground=SessionState.GetBool(Key+".background",false);SessionState.EraseBool(Key);File.AppendAllText(Path.Combine(Output,"play.txt"),"PASS: scenarios "+SessionState.GetInt(Key+".start",0)+" through "+(scenario-1)+" completed.\n");ExitBatch(0);}
        else SessionState.SetBool(Key+".restart",true);
    }
    static void Tick()
    {
        if(EditorApplication.isCompiling || EditorApplication.isUpdating)return;
        string request=Path.GetFullPath("Temp/YearOneV2PlayChecks.run");
        if(File.Exists(request) && !EditorApplication.isPlayingOrWillChangePlaymode)
        {File.Delete(request);Directory.CreateDirectory(Output);File.WriteAllText(Path.Combine(Output,"play.txt"),"RUNNING\n");checks=0;runtimeError="";SessionState.SetBool(Key+".background",Application.runInBackground);SessionState.SetBool(Key,true);SessionState.SetInt(Key+".scenario",0);Begin();return;}
        if(SessionState.GetBool(Key+".restart",false) && !EditorApplication.isPlayingOrWillChangePlaymode)
        {SessionState.EraseBool(Key+".restart");Begin();return;}
        if(SessionState.GetBool(Key,false) && EditorApplication.isPlaying && EditorApplication.timeSinceStartup>deadline)
        {
            File.AppendAllText(Path.Combine(Output,"play.txt"),"FAIL timeout scenario "+Scenario+" phase "+phase+" • Time="+Time.time+" paused="+EditorApplication.isPaused+" scale="+Time.timeScale+" milestone="+campaign?.Progress.milestoneId+" save="+campaign?.SaveError+" player="+player?.CurrentState+" modal="+runtime?.ModalVisible+"\n");
            SessionState.EraseBool(Key);SessionState.EraseBool(Key+".restart");EditorApplication.isPlaying=false;ExitBatch(1);return;
        }
        if(!SessionState.GetBool(Key,false) || !EditorApplication.isPlaying || !EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.timeSinceStartup<startAfter || Time.time<next)return;
        try
        {
            if(!string.IsNullOrEmpty(runtimeError))throw new Exception("Runtime error: "+runtimeError);
            if(EditorApplication.timeSinceStartup>deadline)throw new Exception("Scenario timed out "+Scenario+" phase "+phase);
            if(phase==0)
            {
                Application.runInBackground=true;EditorApplication.isPaused=false;
                ui=Object.FindAnyObjectByType<YearOneCampaignUI>();if(ui==null || !ui.StartupVisible)return;
                campaign=Object.FindAnyObjectByType<YearOneCampaignController>();year=Object.FindAnyObjectByType<YearOneMilestoneController>();
                var progress=DeveloperCheckpointCatalog.Build("HK1_FIRST_CLASSES");
                if(Scenario==1)progress=Array.Find(DeveloperCheckpointCatalog.Entries,e=>e.Id=="exam-active").Progress();
                if(Scenario==2)progress=DeveloperCheckpointCatalog.Final(0);
                if(Scenario==5){progress=DeveloperCheckpointCatalog.Build("HK2_START");progress.morningSecondsRemaining=0;progress.MarkMissedBus();}
                if(Scenario==7)progress.AdvanceTo(YearOneSchedule.At(2,449));
                if(Scenario==6)progress=DeveloperCheckpointCatalog.Build("HK2_DORM_LIFE");
                if(Scenario==3){progress.AdvanceTo(YearOneSchedule.At(2,475));progress.CommitTrip(false);}
                directory=Path.Combine(Path.GetTempPath(),"room404-v2-play-"+Guid.NewGuid());Directory.CreateDirectory(directory);File.WriteAllText(Path.Combine(directory,"year-one-v1.json"),"legacy sentinel");
                campaign.LoadDeveloperCheckpoint(progress,directory);if(Application.isBatchMode)Camera.main.aspect=16f/9;ui.RestoreDeveloperCheckpoint();
                runtime=ui.Runtime;player=year.Player;
                if(progress.safeLocation=="Gate")Check(Vector3.Distance(player.transform.position,year.GateSpawn.position)<.05f,"Restoring Gate checkpoint moves actual player, not only camera");desk=Object.FindAnyObjectByType<DeskActionModal>();
                chair=Object.FindObjectsByType<ChairInteractable>().OrderBy(c=>c.SeatPivot.position.x).First();bed=Object.FindObjectsByType<BedInteractable>().OrderBy(b=>b.transform.position.x).First();
                phase=1;next=Time.time+.3f;return;
            }
            if(Scenario==0)ComputerAndSleep();
            else if(Scenario==1)Exam();
            else if(Scenario==2)FeastAndYearTwo();
            else if(Scenario==3)Classroom();
            else if(Scenario==4)FailureTransaction();
            else if(Scenario==5)Bus();
            else if(Scenario==6)Coop();
            else Reminder();
        }
        catch(Exception e)
        {
            Directory.CreateDirectory(Output);File.AppendAllText(Path.Combine(Output,"play.txt"),"FAIL scenario "+Scenario+" phase "+phase+": "+e+"\n");
            Application.runInBackground=SessionState.GetBool(Key+".background",false);SessionState.EraseBool(Key);SessionState.EraseBool(Key+".restart");EditorApplication.isPlaying=false;Debug.LogException(e);ExitBatch(1);
        }
    }
    static void ComputerAndSleep()
    {
        if(phase==1)
        {
            chair.Interact(player);desk.UseComputer();Check(desk.ComputerVisible,"Windows opens");phase=10;next=Time.time+.3f;
        }
        else if(phase==10)
        {
            Capture("windows-v2");desk.Desktop.ScheduleButton.onClick.Invoke();phase=11;next=Time.time+.3f;
        }
        else if(phase==11)
        {
            Capture("schedule-v2");desk.Desktop.SemesterButton.onClick.Invoke();phase=12;next=Time.time+.2f;
        }
        else if(phase==12)
        {
            Capture("semester-v2");desk.Desktop.ShowTab(false);desk.Desktop.GameButtons[0].onClick.Invoke();
            Check(campaign.IsComputerGaming && !desk.ComputerVisible,"Playing hides Windows without ending session");
            before=campaign.Progress.clockMinutes;phase=2;next=Time.time+2;
        }
        else if(phase==2)
        {
            Check(campaign.Progress.clockMinutes-before>=7.5f,"x4 clock advances");Check(player.CharacterVisual.DeskPose==LuonCharacterVisual.DeskPoseState.SittingTyping,"Actual typing animation");Capture("playing-in-room");
            desk.UseComputer();before=campaign.Progress.clockMinutes;phase=3;next=Time.time+1;
        }
        else if(phase==3)
        {
            Check(campaign.IsComputerGaming && Mathf.Abs(campaign.Progress.clockMinutes-before)<.1f,"Windows pauses without destroying game");
            desk.Desktop.HideWindow();Check(campaign.IsComputerGaming,"Hiding the window alone keeps game running");
            desk.UseComputer();desk.Desktop.CloseButton.onClick.Invoke();Check(campaign.Computer==YearOneCampaignController.ComputerState.Off && !desk.IsTyping,"Power-off ends session and typing presentation");
            desk.UseComputer();desk.Desktop.Play("LOL");runtime.GameStopButton.onClick.Invoke();before=campaign.Progress.clockMinutes;phase=4;next=Time.time+1;
        }
        else if(phase==4)
        {
            Check(!campaign.IsComputerGaming && campaign.Progress.clockMinutes-before>.8f,"Stopping game restores x1 world time");
            chair.Interact(player);bed.ExecuteImmediateEnter(player);phase=5;next=Time.time+.2f;
        }
        else if(phase==5)
        {
            runtime.SleepButton.onClick.Invoke();Check(runtime.ModalVisible,"Sleep preview opens");before=campaign.Progress.clockMinutes;phase=6;next=Time.time+.5f;
        }
        else if(phase==6)
        {
            Check(Mathf.Abs(campaign.Progress.clockMinutes-before)<.1f,"Preview pauses world");runtime.CancelButton.onClick.Invoke();Check(campaign.Progress.Milestone.Id=="HK1_FIRST_CLASSES","Cancel does not advance milestone");
            runtime.SleepButton.onClick.Invoke();Capture("sleep-preview");runtime.ConfirmButton.onClick.Invoke();phase=7;next=Time.time+4;
        }
        else
        {
            if(campaign.Progress.milestoneId=="HK1_FIRST_CLASSES"){Check(!runtime.ModalVisible || player.CurrentState==PlayerInteractionState.Sleeping,"Sleep failed: "+campaign.SaveError);return;}
            Check(campaign.Progress.milestoneId=="HK1_GAMING_CHOICE","Sleep moves to next milestone");Check(campaign.Progress.Missed(YearOneSchedule.Courses[0],YearOneSchedule.Work.Lesson1),"Sleeping early records absence");
            Check(campaign.Progress.OwedCourses==0,"Absence is not course debt");Check(player.CurrentState==PlayerInteractionState.Standing,"Bed and physics restore");Capture("next-milestone");Done();
        }
    }
    static void Exam()
    {
        if(phase==1)
        {
            Check(campaign.Progress.exam!=null && campaign.Progress.exam.Question==1,"Resumed exact exam question");Check(campaign.Progress.exam.Seconds<=11,"Exam timer restored");Check(runtime.ExamVisible,"Exam UI visible");Capture("exam-resumed");phase=2;next=Time.time+1;
        }
        else if(phase==2)
        {
            Check(campaign.Progress.exam.Seconds<10.5f,"Exam timer ticks while world paused");var c=YearOneSchedule.Courses[0];
            runtime.ExamAnswers[c.ExerciseFor(YearOneSchedule.Work.Revision).Answer].onClick.Invoke();
            runtime.ExamAnswers[c.ExerciseFor(YearOneSchedule.Work.Revision).Answer].onClick.Invoke();
            Check(campaign.Progress.exam!=null && campaign.Progress.exam.Question==2,"Stale answer click cannot answer the next question");phase=3;next=Time.time+.2f;
        }
        else if(phase==3)
        {
            var c=YearOneSchedule.Courses[0];runtime.ExamAnswers[c.ExerciseFor(YearOneSchedule.Work.Exam).Answer].onClick.Invoke();
            Check(campaign.Progress.Result(c.Id)?.Passed==true && campaign.Progress.Credits==6,"Exam yields correct per-course credits");
            Check(!campaign.StartCourseExam(c.Id),"No exam retry/reward duplication");Check(File.ReadAllText(Path.Combine(directory,"year-one-v1.json"))=="legacy sentinel","Legacy save untouched");Done();
        }
    }
    static void FeastAndYearTwo()
    {
        if(phase==1)
        {Check(campaign.Progress.Credits==0 && campaign.Progress.OwedCourses==6,"Failed year fixture");Check(campaign.Progress.ReadyForFeast,"Zero credits can feast");year.Use(YearOneInteractable.Action.PrepareFeast);phase=2;next=Time.time+1;}
        else if(phase==2)
        {
            if(!year.DialogueVisible)return;Check(year.FeastVisible,"Real feast scene visible");Capture("feast-with-debt");
            while(year.DialogueVisible)year.AdvanceDialogue();phase=3;next=Time.time+2;
        }
        else if(phase==3)
        {
            Check(campaign.Progress.Has("year:feast") && !campaign.Progress.Has("year:two"),"Feast does not auto-transfer year");
            if(runtime.ModalVisible)runtime.ConfirmButton.onClick.Invoke();bed.ExecuteImmediateEnter(player);phase=4;next=Time.time+.2f;
        }
        else if(phase==4){runtime.SleepButton.onClick.Invoke();runtime.ConfirmButton.onClick.Invoke();phase=5;next=Time.time+4;}
        else
        {if(!campaign.Progress.Has("year:two"))return;Check(campaign.Progress.Has("year:two") && campaign.Progress.yearTwo.Credits==0 && campaign.Progress.yearTwo.Debt.Length==6,"Actual sleep hands debt to Year 2");Capture("year-two-debt");Done();}
    }
    static void Classroom()
    {
        if(phase==1){year.Use(YearOneInteractable.Action.EnterH6);phase=2;next=Time.time+1;}
        else if(phase==2){Check(year.CurrentPhase==YearOneMilestoneController.Phase.Classroom,"Classroom arrival");year.Use(YearOneInteractable.Action.SitBesideClassmate);phase=3;next=Time.time+6;}
        else if(phase==3)
        {
            Check(campaign.Progress.Has("attendance:calculus1:Lesson1"),"Seated arrival records attendance");Check(!campaign.Progress.Has("late:calculus1:Lesson1"),"Waiting before start is on time");Capture("class-attendance");phase=4;next=Time.time+15;
        }
        else
        {
            Check(year.Puzzle.IsOpen,"Class study animation reaches question");year.Puzzle.Choose(year.Puzzle.ActiveCorrectAnswer);
            Check(campaign.Progress.Has("calculus1:Lesson1") && campaign.Progress.clockMinutes>=600,"Lesson completion advances to end");Check(campaign.Progress.Credits==0,"One class grants no credits");Done();
        }
    }
    static void Bus()
    {
        var bus=Object.FindAnyObjectByType<CampusCommuteController>();var journey=bus.Journey;
        if(phase==1){Check(bus.TryDescend(player),"Real stairs transition");phase=2;next=Time.time+1.5f;}
        else if(phase==2)
        {
            float delta=bus.StopAnchor.position.x-player.transform.position.x;
            player.GetComponent<PlayerMovement>().SetSimulatedInput(Mathf.Abs(delta)>.12f?new Vector2(Mathf.Sign(delta),0):Vector2.zero);
            if(Mathf.Abs(delta)>.12f)return;phase=3;next=Time.time+4.5f;
        }
        else if(phase==3)
        {
            Check(bus.MissedBus && bus.BusAtStop,"Next bus arrives after missed departure");before=campaign.Progress.money;
            Check(bus.TryBoard(player),"Boards recovery bus");Check(campaign.Progress.money==before-7000,"Fare commits once before travel");phase=4;next=Time.time+4.5f;
        }
        else if(phase==4)
        {
            Check(year.CurrentPhase==YearOneMilestoneController.Phase.Gate && campaign.Progress.safeLocation=="Gate","Recovery reaches actual gate");
            Check(campaign.Progress.clockMinutes<480,"Bus animation does not jump clock to 08:00");Capture("recovery-bus-arrival");
            year.Use(YearOneInteractable.Action.ReturnBus);phase=40;next=Time.time;
        }
        else if(phase==40)
        {
            float delta=bus.StopAnchor.position.x-player.transform.position.x;
            player.GetComponent<PlayerMovement>().SetSimulatedInput(Mathf.Abs(delta)>.12f?new Vector2(Mathf.Sign(delta),0):Vector2.zero);
            if(Mathf.Abs(delta)>.12f)return;phase=5;next=Time.time+3;
        }
        else if(phase==5){Check(bus.TryBoard(player),"Can return before lessons are complete");phase=6;next=Time.time+4.5f;}
        else {Check(campaign.Progress.money==before-14000 && campaign.Progress.Has(campaign.Progress.TripId(true)),"Two real trips charge two fares");Check(!bus.TryBoard(player),"Duplicate boarding rejected");Done();}
    }
    static void Coop()
    {
        var actor=Object.FindAnyObjectByType<RoommateActivityController>(FindObjectsInactive.Include);
        if(phase==1){chair.Interact(player);desk.UseComputer();desk.Desktop.Play("LOL");phase=2;next=Time.time+21;}
        else if(phase==2)
        {
            Check(campaign.IsComputerGaming && campaign.Progress.Has("day:2:game"),"Long event does not force game exit at 20s");Check(actor.Current==RoommateActivityController.Activity.Gaming,"Long actually games at other chair");
            var other=Object.FindObjectsByType<ChairInteractable>().First(c=>c!=chair);Check(other.Reservation.Owner==actor && chair.Occupant==player,"Independent chair reservations");Capture("two-gamers");
            campaign.Transact(p=>p.AdvanceTo(p.Milestone.CalendarDay*1440d+1390));phase=3;next=Time.time+.5f;
        }
        else if(phase==3)
        {
            Check(year.DialogueVisible && campaign.IsComputerGaming,"Night argument pauses world without stopping session");
            Check(GameObject.Find("BookStack_Left").GetComponentInChildren<SpriteRenderer>().color.r<.5f,"Added book stacks follow shared night lighting");Capture("night-argument");while(year.DialogueVisible)year.AdvanceDialogue();
            campaign.StopComputerGame();chair.Interact(player);phase=4;next=Time.time+1;
        }
        else if(phase==4)
        {
            var light=Object.FindAnyObjectByType<MVPFlowController>().lightSwitchInteractable;Check(light.IsActive,"Optional lights action available");light.Interact();Check(campaign.Progress.Has("day:2:lights"),"Lights consequence saved");bed.ExecuteImmediateEnter(player);phase=5;next=Time.time+.3f;
        }
        else if(phase==5){runtime.SleepButton.onClick.Invoke();runtime.ConfirmButton.onClick.Invoke();phase=6;next=Time.time+4;}
        else
        {
            if(campaign.Progress.Milestone.Coop){if(actor.IsSleeping)Capture("two-sleepers");return;}
            Check(campaign.Progress.milestoneId=="HK2_PREPARATION","Coop night advances without academic pass gate");Done();
        }
    }
    static void Reminder()
    {
        if(phase==1){chair.Interact(player);desk.UseComputer();desk.Desktop.Play("LOL");phase=2;next=Time.time+3;}
        else if(phase==2)
        {
            Check(campaign.IsComputerGaming && !runtime.ModalVisible,"School alert does not pause or stop gameplay");
            Check(campaign.Progress.Has("reminder:calculus1:Lesson1:30") && GameObject.Find("SchoolReminder")!=null,"30-minute reminder visibly appears in room");
            Check(runtime.LastNotification.Contains("Còn "+Math.Ceiling(campaign.Progress.NextSession.Start-campaign.Progress.absoluteMinutes)+" phút"),"Visible alert countdown tracks the current world clock");
            Capture("reminder-30");phase=3;next=Time.time+3;
        }
        else if(phase==3)
        {
            Check(campaign.Progress.Has("reminder:calculus1:Lesson1:15") && GameObject.Find("SchoolReminder")!=null,"Reminder remains visible and updates urgency");
            var notice=GameObject.Find("SchoolReminder");notice.GetComponentsInChildren<UnityEngine.UI.Button>().Last().onClick.Invoke();
            Check(campaign.IsComputerGaming && !notice.activeSelf,"Continue dismisses alert without ending game");phase=4;next=Time.time+1;
        }
        else
        {
            Check(campaign.Progress.Has("reminder:calculus1:Lesson1:5") && GameObject.Find("SchoolReminder")!=null,"Later five-minute alert still fires after dismissal");
            Check(campaign.IsComputerGaming,"No forced exit on reminders");Capture("reminder-5");Done();
        }
    }
    static void FailureTransaction()
    {
        string beforeId=campaign.Progress.milestoneId;
        Directory.Move(directory,directory+"-backup");File.WriteAllText(directory,"block save directory");
        Check(!campaign.CommitTransition(campaign.Progress.PreviewTransition()),"Write failure rejects transition");Check(campaign.Progress.milestoneId==beforeId && !string.IsNullOrEmpty(campaign.SaveError),"State remains unchanged on failed commit");
        File.Delete(directory);Directory.Move(directory+"-backup",directory);Check(campaign.CommitTransition(campaign.Progress.PreviewTransition()),"Retry saves and advances");Check(string.IsNullOrEmpty(campaign.SaveError),"Retry clears error");Done();
    }
}
