using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object=UnityEngine.Object;

[InitializeOnLoad]
public static class ComputerFlowChecks
{
    const string Key="Room404.ComputerChecks";
    static int phase;static float next;static double startAfter;
    static DeskActionModal desk;static YearOneCampaignController campaign;static PlayerInteractionStateController player;static ChairInteractable chair;
    static float clockBefore,stoppedAt;
    static string Output=>Path.GetFullPath("PlayModeInteractionReports/computer-flow");
    static ComputerFlowChecks(){EditorApplication.update+=Tick;}
    static void Assert(bool ok,string message){if(!ok)throw new Exception(message);}
    static YearOneProgress DayOne()
    {
        var p=new YearOneProgress();p.CompleteOpening("claim");p.CompleteOpening("clean");p.CompleteOpening("friends");p.CompleteOpening("meal");return p;
    }
    static void Models()
    {
        var early=DayOne();early.AdvanceGaming(2);Assert(early.OwedCourses==0 && early.CanComplete(YearOneSchedule.Courses[0],YearOneSchedule.Work.Lesson1),"Early exit harms attendance");
        var late=DayOne();late.AdvanceGaming(15);late.CommitTrip(false);late.Complete("calculus1",YearOneSchedule.Work.Lesson1);Assert(late.Has("late:calculus1:Lesson1"),"Late attendance not recorded");
        var p=DayOne();p.AdvanceGaming(70);Assert(p.Clock=="12:00" && p.TimeOfDay=="Trưa" && p.OwedCourses==2,"Morning misses/time incorrect");
        Assert(p.DayClassesResolved && !p.LessonsDoneToday && !p.CanSleep && !p.CommitTrip(true),"Missed classes trap travel or permit sleep");
        Assert(!p.CanComplete(YearOneSchedule.Courses[0],YearOneSchedule.Work.Homework),"Missed class permits homework");
        Assert(!p.Complete("calculus1",YearOneSchedule.Work.Lesson1),"Missed lesson can be credited as attended");
        Assert(p.CompleteMakeup("calculus1",YearOneSchedule.Work.Lesson1) && p.OwedCourses==1 && p.CanComplete(YearOneSchedule.Courses[0],YearOneSchedule.Work.Homework),"Makeup failed to restore prerequisites");
        Assert(!p.CompleteMakeup("calculus1",YearOneSchedule.Work.Lesson1),"Duplicate makeup credit");
        var folder=Path.Combine(Path.GetTempPath(),"computer-model-"+Guid.NewGuid());var store=new YearOneCheckpointStore(folder);Assert(store.Save(p),"Save rejected time/debt");var loaded=store.Load();Assert(loaded!=null && loaded.Clock==p.Clock && loaded.OwedCourses==1,"Resume loses time/debt");
        var small=DayOne();for(int i=0;i<700;i++)small.AdvanceGaming(.1f);Assert(small.OwedCourses==2,"Attendance depends on frame size");
        p.AdvanceGaming(30);Assert(p.TimeOfDay=="Chiều","Afternoon missing");p.AdvanceGaming(60);Assert(p.TimeOfDay=="Tối","Evening missing");
        p.AdvanceGaming(10000);Assert(p.Clock=="24:00" && p.OwedCourses==1,"Midnight loses debt or overflows");
        Assert(!p.AdvanceGaming(float.NaN),"Invalid delta accepted");
        Assert(p.CompleteMakeup("physics1",YearOneSchedule.Work.Lesson1),"Second makeup failed");
        for(int day=1;day<=6;day++)
        {
            Assert(p.day==day,"Recovered campaign lost day");
            if(day>1)Assert(p.CommitTrip(false),"Recovered campaign cannot travel");
            foreach(var c in YearOneSchedule.Lessons(day))
            {
                var lesson=YearOneProgress.LessonWork(c,day);
                if(!p.Has(c.Activity(lesson)))Assert(p.Complete(c.Id,lesson),"Recovered lesson blocked");
                if(!p.Has(c.Activity(YearOneSchedule.Work.Homework)))Assert(p.Complete(c.Id,YearOneSchedule.Work.Homework),"Recovered homework blocked");
            }
            if(day>1)Assert(p.CommitTrip(true),"Recovered return blocked");p.safeLocation="Room";
            if(day==2){p.MarkEvening("game");p.MarkEvening("lights");}
            if(day==3 || day==6)foreach(var c in YearOneSchedule.Courses)
            {
                if((c.FirstDay<=3)!=(day==3))continue;
                Assert(p.Complete(c.Id,YearOneSchedule.Work.Revision) && p.Complete(c.Id,YearOneSchedule.Work.Exam),"Recovered exam blocked");
            }
            Assert(p.FinishDay(),"Recovered sleep blocked");
        }
        Assert(p.Credits==32 && p.OwedCourses==0 && p.ReadyForFeast,"Gaming recovery cannot finish Year 1");
        // Old JSON has no clock fields; normal campaign migration initializes them lazily.
        var old=JsonUtility.FromJson<YearOneProgress>("{\"version\":1,\"day\":0,\"money\":35000,\"completed\":[]}");old.EnsureClock();Assert(old.IsValid() && old.Clock=="14:20","Old checkpoint invalid");
    }
    static void Tick()
    {
        if(EditorApplication.isCompiling || EditorApplication.isUpdating)return;
        var request=Path.GetFullPath("Temp/ComputerFlowChecks.run");
        if(File.Exists(request) && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            File.Delete(request);Directory.CreateDirectory(Output);
            try{Models();}catch(Exception e){File.WriteAllText(Path.Combine(Output,"result.txt"),"FAIL model: "+e);return;}
            SessionState.SetBool(Key,true);phase=0;next=0;startAfter=EditorApplication.timeSinceStartup+3;EditorApplication.isPlaying=true;return;
        }
        if(!SessionState.GetBool(Key,false) || !EditorApplication.isPlaying || !EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.timeSinceStartup<startAfter || Time.time<2 || Time.time<next)return;
        try
        {
            if(phase==0)
            {
                campaign=Object.FindAnyObjectByType<YearOneCampaignController>();campaign.UseStoreForTests(Path.Combine(Path.GetTempPath(),"computer-play-"+Guid.NewGuid()));
                Object.FindAnyObjectByType<YearOneCampaignUI>().NewGameButton.onClick.Invoke();
                campaign.CompleteOpening("claim");campaign.CompleteOpening("clean");campaign.CompleteOpening("friends");campaign.CompleteOpening("meal");
                Object.FindAnyObjectByType<MVPFlowController>().RestoreYearOneOpening(true,true,true);
                player=Object.FindAnyObjectByType<PlayerInteractionStateController>();chair=Object.FindAnyObjectByType<ChairInteractable>();chair.Interact(player);
                desk=Object.FindAnyObjectByType<DeskActionModal>();desk.UseComputer();Assert(desk.ComputerVisible && campaign.Computer==YearOneCampaignController.ComputerState.Desktop,"Seated computer did not open desktop");
                next=Time.time+.5f;phase=11;
            }
            else if(phase==11)
            {
                Canvas.ForceUpdateCanvases();
                var hits=new System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult>();
                var target=desk.Desktop.GameButtons[0];
                UnityEngine.EventSystems.EventSystem.current.RaycastAll(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current){position=RectTransformUtility.WorldToScreenPoint(null,target.transform.position)},hits);
                string raycast="target="+target.transform.position+" canvas="+target.GetComponentInParent<Canvas>().renderMode+" screen="+Screen.width+"x"+Screen.height+" raycasters="+UnityEngine.EventSystems.RaycasterManager.GetRaycasters().Count;
                foreach(var hit in hits)raycast+="; "+hit.gameObject.name+" order="+hit.sortingOrder;
                Assert(hits.Count>0 && hits[0].gameObject.GetComponentInParent<UnityEngine.UI.Button>()==target,"Desktop game tile cannot receive pointer input: "+raycast);
                MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.Combine(Output,"windows-games.png"),true);
                desk.Desktop.ScheduleButton.onClick.Invoke();MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.Combine(Output,"windows-schedule.png"),true);
                desk.Desktop.ShowTab(false);clockBefore=campaign.Progress.clockMinutes;desk.Desktop.GameButtons[0].onClick.Invoke();
                Assert(campaign.IsComputerGaming && campaign.ComputerGame=="LOL","Game tile did not start actual session");next=Time.time+2;phase=1;
            }
            else if(phase==1)
            {
                Assert(campaign.ComputerSessionSeconds>=1.8f && campaign.Progress.clockMinutes>clockBefore+7,"Real Update did not spend gameplay time");
                Assert(campaign.Progress.OwedCourses==0,"Early game caused debt");
                MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.Combine(Output,"playing.png"),true);
                campaign.MenuPausesClock=true;stoppedAt=campaign.Progress.clockMinutes;next=Time.time+1;phase=10;
            }
            else if(phase==10)
            {
                Assert(Mathf.Approximately(stoppedAt,campaign.Progress.clockMinutes),"Paused game spent time");
                campaign.MenuPausesClock=false;desk.Desktop.StopButton.onClick.Invoke();stoppedAt=campaign.Progress.clockMinutes;next=Time.time+2;phase=2;
            }
            else if(phase==2)
            {
                Assert(Mathf.Approximately(stoppedAt,campaign.Progress.clockMinutes),"Clock continued after exit");
                foreach(string game in YearOneCampaignController.ComputerGames){Assert(campaign.PlayComputerGame(game),"Cannot launch "+game);campaign.StopComputerGame();}
                campaign.Progress.AdvanceGaming(70);campaign.Save();
                Assert(campaign.Progress.OwedCourses==2,"Long game has no academic consequence");
                Assert(campaign.BeginComputerMakeup("calculus1",YearOneSchedule.Work.Lesson1),"Cannot start makeup");next=Time.time+1;phase=3;
            }
            else if(phase==3)
            {
                campaign.CancelComputerActivity();Assert(campaign.Progress.OwedCourses==2,"Canceled makeup credited attendance");
                Assert(campaign.BeginComputerMakeup("calculus1",YearOneSchedule.Work.Lesson1),"Cannot restart makeup");next=Time.time+16;phase=4;
            }
            else if(phase==4)
            {
                Assert(campaign.Progress.OwedCourses==1 && campaign.Computer==YearOneCampaignController.ComputerState.Desktop,"Real-time makeup failed");
                desk.Desktop.ShowTab(true);MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.Combine(Output,"attendance-debt.png"),true);
                campaign.PlayComputerGame("CS2");chair.Interact(player);Assert(!campaign.IsComputerGaming && !desk.ComputerVisible && player.CanInteract,"Standing did not stop session/unlock movement");
                File.WriteAllText(Path.Combine(Output,"result.txt"),"PASS: early exit; late/missed attendance; morning/noon/afternoon/night; prerequisites and makeup; save/load; frame-size independence; real seated Windows UI; all four games; real ticking and stop; canceled and completed real-time makeup; paused clock; stand cleanup; full six-day recovery to 32 credits.\n");
                SessionState.EraseBool(Key);EditorApplication.isPlaying=false;
            }
        }
        catch(Exception e){File.WriteAllText(Path.Combine(Output,"result.txt"),"FAIL play: "+e);SessionState.EraseBool(Key);EditorApplication.isPlaying=false;Debug.LogException(e);}
    }
}
