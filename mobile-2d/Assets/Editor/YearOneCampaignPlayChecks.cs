using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Object=UnityEngine.Object;

public static partial class PlayModeInteractionSuite
{
    private static IEnumerator Reach(GameObject player,Vector3 destination,PlayerMovement movement)
    {
        float end=Time.time+8f;
        while(Vector2.Distance(player.transform.position,destination)>.12f && Time.time<end)
        {movement.SetSimulatedInput(((Vector2)(destination-player.transform.position)).normalized);yield return null;}
        movement.SetSimulatedInput(Vector2.zero);yield return Wait(.1f);
        if(Vector2.Distance(player.transform.position,destination)>=.3f)CaptureUI("campaign-walk-blocked");
        Check(Vector2.Distance(player.transform.position,destination)<.3f,"Walk reaches authored interaction marker: "+destination+" from "+player.transform.position);
    }
    private static IEnumerator YearDialogue(YearOneMilestoneController year)
    {
        float end=Time.time+12;
        while(year.DialogueVisible && Time.time<end){yield return PressKey(Key.Space);yield return Wait(.12f);}
        Check(!year.DialogueVisible,"Authored conversation completes through Space input");
    }
    private static IEnumerator ResumeCheckpointUI(YearOneCampaignUI ui,PlayerInteractionStateController state)
    {
        yield return PressKey(Key.Escape);
        yield return Until(()=>ui.StartupVisible && ui.ContinueButton.interactable,2,"Pause offers an enabled Continue checkpoint button");
        ui.ContinueButton.onClick.Invoke();yield return Wait(.2f);
        Check(!ui.StartupVisible && state.CanInteract,"Continue restores safe standing actor through visible UI");
    }
    private static IEnumerator EnterHall(GameObject player,PlayerMovement movement,DormJourneyController journey)
    {
        var door=Object.FindAnyObjectByType<HingedRoomDoor>();
        yield return Reach(player,door.transform.position+new Vector3(-.64f,-.9f),movement);
        if(door.CurrentAngle<69){yield return PressE();yield return Until(()=>door.CurrentAngle>69,3,"Room door opens before hallway crossing");}
        movement.SetSimulatedInput(Vector2.up);
        yield return Until(()=>journey.IsInHallway,4,"Player walks from Room404 into hallway");
        movement.SetSimulatedInput(Vector2.zero);yield return Until(()=>!journey.IsBusy,3,"Hallway arrival releases movement");
    }
    private static IEnumerator ReturnRoom(GameObject player,PlayerMovement movement,DormJourneyController journey)
    {
        var lobby=Object.FindAnyObjectByType<CampusLobbyInteractable>();
        yield return Reach(player,lobby.transform.position,movement);yield return PressE();
        yield return Until(()=>journey.IsInHallway && !journey.IsBusy,4,"Return bus is followed by stairs back to hallway");
        yield return WalkToX(player,GameObject.Find("Room404_ReturnTrigger").transform.position.x,movement);
        movement.SetSimulatedInput(Vector2.up);
        yield return Until(()=>journey.CurrentArea==DormJourneyController.JourneyArea.InRoom,4,"Player returns through Room404 doorway");
        movement.SetSimulatedInput(Vector2.zero);
        var year=Object.FindAnyObjectByType<YearOneMilestoneController>();
        var campaign=Object.FindAnyObjectByType<YearOneCampaignController>();
        if(campaign.Progress.LessonsDoneToday && !campaign.Progress.Has("story:h6-told-dam"))
        {
            yield return Until(()=>year.DialogueVisible,4,"First return to404 starts Dam reaction after room arrival fade");
            yield return YearDialogue(year);
            Check(campaign.Progress.Has("story:h6-told-dam"),"Dorm reaction commits once after conversation");
        }
        yield return Until(()=>!journey.IsBusy,3,"Room return restores normal control");
    }
    private static IEnumerator CampaignChecks(GameObject player,PlayerInteractionStateController state,PlayerMovement movement,YearOneCampaignController campaign)
    {
        var year=Object.FindAnyObjectByType<YearOneMilestoneController>();var ui=Object.FindAnyObjectByType<YearOneCampaignUI>();
        var journey=Object.FindAnyObjectByType<DormJourneyController>();var flow=Object.FindAnyObjectByType<MVPFlowController>();
        var bus=Object.FindAnyObjectByType<CampusCommuteController>();var desk=Object.FindAnyObjectByType<DeskActionModal>();
        Check(!campaign.Progress.ReadyForFeast && !year.FeastVisible && campaign.Progress.Credits==0,"Fresh Year1 cannot skip directly to feast or credits");
        for(int i=0;i<3;i++)flow.trashInteractable.Interact();
        float introEnd=Time.time+15;
        while(!campaign.Progress.Has("opening:friends") && Time.time<introEnd)
        {if(flow.dialoguePanel.activeSelf)flow.dialogueNextButton.onClick.Invoke();yield return Wait(.2f);}
        Check(campaign.Progress.Has("opening:friends") && flow.currentStage==MVPFlowController.Stage.Stage1_Year1_Arrival,"Cleaning and introductions finish without premature Year2");
        var bag=Object.FindAnyObjectByType<BackpackInteractable>();Place(player,bag.transform.position+Vector3.down*.6f);yield return PressE();
        Check(journey.HasBackpack,"Player equips backpack before leaving dorm");
        yield return EnterHall(player,movement,journey);
        yield return WalkToX(player,GameObject.Find("Canteen_Entry").transform.position.x,movement);yield return PressE();
        yield return Until(()=>ui.DinnerVisible,4,"Opening includes canteen meal choice UI");CaptureUI("campaign-opening-meal");
        ui.DinnerButtons[0].onClick.Invoke();ui.DinnerButtons[0].onClick.Invoke();
        Check(campaign.Progress.money==110000 && campaign.Progress.day==1,"Meal costs25k and grants100k exactly once");
        yield return YearDialogue(year);yield return Until(()=>!journey.IsBusy && journey.CurrentArea==DormJourneyController.JourneyArea.InRoom,4,"Dinner leads back to Room404 before day1");
        var chairs=Object.FindObjectsByType<ChairInteractable>();Array.Sort(chairs,(a,b)=>b.transform.position.x.CompareTo(a.transform.position.x));
        for(int day=1;day<=6;day++)
        {
            Check(campaign.Progress.day==day,"Representative day "+day+" starts in sequence");
            Check(!campaign.Progress.CanSleep,"Day "+day+" cannot be skipped by sleeping early");
            if(day==4)
            {
                float remaining=campaign.Progress.morningSecondsRemaining;
                ui.ShowStartup();yield return Wait(.3f);
                Check(Mathf.Abs(campaign.Progress.morningSecondsRemaining-remaining)<.05f,"Day4 deadline pauses while menu is open");
                ui.ContinueButton.onClick.Invoke();yield return Wait(.1f);
                campaign.ReachedBusStop();campaign.Progress.morningSecondsRemaining=.15f;yield return Wait(.3f);
                Check(!campaign.Progress.Has("day:4:missed"),"On-time stop reservation preserves boarding during vehicle arrival");
                campaign.Progress.completed.Remove("day:4:stop");campaign.Progress.safeLocation="Room";
                // A nearly-expired, otherwise genuinely earned day4 checkpoint.
                campaign.Progress.morningSecondsRemaining=.15f;campaign.Save();yield return ResumeCheckpointUI(ui,state);
                yield return Wait(.4f);Check(campaign.Progress.Has("day:4:missed"),"Near-deadline checkpoint triggers real missed-bus timer");
            }
            yield return EnterHall(player,movement,journey);
            RestroomInteractable male=null;
            foreach(var restroom in Object.FindObjectsByType<RestroomInteractable>())if(restroom.RoomGender==RestroomInteractable.Gender.Male)male=restroom;
            yield return WalkToX(player,male.transform.position.x,movement);yield return PressE();
            yield return Until(()=>!journey.IsBusy,6,"Day "+day+" WC completes without trapping player");
            var stairs=Object.FindAnyObjectByType<DormStairsInteractable>();yield return WalkToX(player,stairs.transform.position.x,movement);yield return PressE();
            yield return Until(()=>bus.IsOutdoors && !journey.IsBusy,4,"Morning stairs reach campus");
            if(day==1)YearOneMotionCapture.Begin("bus-outbound");
            yield return WalkToX(player,bus.StopAnchor.position.x,movement);
            int before=bus.MoneyVnd;
            if(day!=4)
            {
                float x=bus.BusRenderer.transform.position.x;yield return Wait(.4f);
                Check(bus.BusRenderer.transform.position.x>x && !bus.BusRenderer.flipX,"Outbound bus approaches forward from the left");
                yield return Until(()=>bus.BusAtStop,4,"Outbound bus parks before boarding");
            }
            else
            {
                Check(bus.MissedBus && !bus.HasCompletedTrip,"Missed morning offers a later vehicle instead of teleporting");
                if(!bus.BusAtStop)Check(!bus.TryBoard(state) && bus.MoneyVnd==before && !campaign.Progress.Has(campaign.Progress.TripId(false)),"Recovery cannot charge or complete before next bus arrives");
                yield return Until(()=>bus.BusAtStop,6,"Next bus visibly parks after missed departure");
                Check(bus.BusRenderer.enabled && !bus.BusRenderer.flipX,"Recovery reuses a visible forward-facing bus");
                CaptureUI("campaign-day4-next-bus");
            }
            int ticketsBefore=bus.TicketsPurchased;
            yield return PressE();yield return PressE();
            yield return Until(()=>year.CurrentPhase==YearOneMilestoneController.Phase.Gate && !journey.IsBusy && campaign.Progress.Has(campaign.Progress.TripId(false)),7,"Trip reaches H6 safely and commits fare");
            if(day==1)YearOneMotionCapture.End();
            Check(bus.MoneyVnd==before-7000 && bus.TicketsPurchased==ticketsBefore+1,"Exactly one outbound fare even after duplicate E or missed departure");
            if(day==4)
            {
                Check(campaign.Progress.Has("day:4:next-bus"),"Next-bus completion is durably marked");
                string directory=Path.Combine(Application.temporaryCachePath,"next-bus-check-"+Guid.NewGuid().ToString("N"));
                try
                {
                    var checkpoint=new YearOneCheckpointStore(directory);
                    Check(checkpoint.Save(campaign.Progress) && checkpoint.Load().Has("day:4:next-bus") && checkpoint.Load().money==campaign.Progress.money,"Recovered trip reload preserves fare and marker");
                }
                finally{if(Directory.Exists(directory))Directory.Delete(directory,true);}
            }
            CaptureUI("campaign-day"+day+"-h6");
            foreach(var course in YearOneSchedule.Lessons(day))
            {
                yield return WalkToX(player,GameObject.Find("H6_Door_Interactable").transform.position.x,movement);yield return PressE();
                yield return Until(()=>year.CurrentPhase==YearOneMilestoneController.Phase.Classroom && !journey.IsBusy,4,"Enter scheduled classroom for "+course.Name);
                yield return Reach(player,GameObject.Find("Seat_Beside_Dam").transform.position,movement);yield return PressE();
                Check(state.CurrentState==PlayerInteractionState.Sitting,"Scheduled lesson sits player beside classmate");
                if(day==4)Check(year.NextButton.transform.parent.Find("Body").GetComponent<TMPro.TMP_Text>().text.Contains("chuyến sau"),"Teacher acknowledges recovery arrival without blocking class");
                if(day==1 || course.UsesComputer)CaptureUI("campaign-classroom-"+course.Id);
                yield return YearDialogue(year);yield return Until(()=>year.Puzzle.IsOpen,4,"Teacher walk opens subject-specific puzzle");
                if(day==1 && course.Id=="calculus1")
                {
                    yield return PressKey(Key.Digit1);Check(year.Puzzle.AwaitingRetry,"Wrong lesson answer offers retry");yield return PressKey(Key.R);
                    yield return Until(()=>year.Puzzle.AwaitingRetry,22,"Lesson timer expires without advancing progress");yield return PressKey(Key.R);
                }
                yield return PressKey((Key)((int)Key.Digit1+course.Answer));yield return YearDialogue(year);
                yield return Until(()=>year.CurrentPhase==YearOneMilestoneController.Phase.Classroom && !journey.IsBusy,4,"Lesson result returns standing control");
                var lesson=course.FirstDay==day?YearOneSchedule.Work.Lesson1:YearOneSchedule.Work.Lesson2;
                Check(campaign.Progress.Has(course.Activity(lesson)),"Recorded actual lesson: "+course.Name+" "+lesson);
                yield return Reach(player,GameObject.Find("Classroom_Exit").transform.position,movement);yield return PressE();
                yield return Until(()=>year.CurrentPhase==YearOneMilestoneController.Phase.Gate && !journey.IsBusy,4,"Exit class to H6 before next destination");
                if(day==1 && course.Id=="calculus1")
                {
                    yield return ResumeCheckpointUI(ui,state);
                    Check(year.CurrentPhase==YearOneMilestoneController.Phase.Gate && campaign.Progress.Has(course.Activity(lesson)),"Lesson checkpoint preserves completion and safe H6 entry");
                }
            }
            if(day==1)YearOneMotionCapture.Begin("bus-homebound");
            yield return WalkToX(player,GameObject.Find("Return_Bus_Entry").transform.position.x,movement);yield return PressE();
            float approach=bus.BusRenderer.transform.position.x;yield return Wait(.4f);
            Check(bus.BusRenderer.flipX && bus.BusRenderer.transform.position.x<approach,"Homebound bus faces and moves left");
            yield return Until(()=>bus.BusAtStop,4,"Return bus parks");before=bus.MoneyVnd;yield return PressE();yield return PressE();
            yield return Until(()=>journey.CurrentArea==DormJourneyController.JourneyArea.Downstairs_Campus && !journey.IsBusy,7,"School day returns by bus to dorm campus");
            if(day==1)YearOneMotionCapture.End();
            Check(bus.MoneyVnd==before-7000,"Return trip charges one fare");yield return ReturnRoom(player,movement,journey);
            yield return Reach(player,chairs[0].ExitPivot.position,movement);yield return PressE();yield return PressKey(Key.Digit2);
            for(int i=0;i<YearOneSchedule.Courses.Length;i++)
            {
                var c=YearOneSchedule.Courses[i];if(c.FirstDay>day)continue;
                foreach(var work in new[]{YearOneSchedule.Work.Homework,YearOneSchedule.Work.Revision,YearOneSchedule.Work.Exam})
                {
                    if(work!=YearOneSchedule.Work.Homework && day!=3 && day!=6)continue;
                    if(!campaign.Progress.CanComplete(c,work))continue;
                    ui.CourseButtons[i].onClick.Invoke();ui.WorkButtons[(int)work-(int)YearOneSchedule.Work.Homework].onClick.Invoke();
                    yield return Until(()=>year.Puzzle.IsOpen,2,"Desk opens eligible "+work+" for "+c.Name);
                    yield return PressKey((Key)((int)Key.Digit1+year.Puzzle.ActiveCorrectAnswer));
                    Check(campaign.Progress.Has(c.Activity(work)),"Desk activity persists "+c.Name+" "+work);
                }
            }
            if(day==1){yield return ObserveDeskFrames(state.CharacterVisual,3,"campaign-writing");CaptureUI("campaign-study-ui");}
            yield return PressE();Check(state.CanInteract,"Standing closes nested study UI and unlocks movement");
            if(day==1)
            {
                yield return ResumeCheckpointUI(ui,state);
                Check(journey.CurrentArea==DormJourneyController.JourneyArea.InRoom && state.CanInteract,"Checkpoint resume restores safe standing room state");
            }
            if(day==2)
            {
                yield return Reach(player,chairs[0].ExitPivot.position,movement);yield return PressE();yield return PressKey(Key.Digit1);
                var desktop=Object.FindAnyObjectByType<DeskActionModal>().Desktop;
                Check(desktop!=null && desktop.Visible,"Opening computer shows Windows before launching a game");
                desktop.GameButtons[0].onClick.Invoke();
                var companion=Object.FindAnyObjectByType<RoommateActivityController>();
                yield return Until(()=>companion.Current==RoommateActivityController.Activity.Gaming,5,"Long reserves other chair and joins gaming");CaptureUI("campaign-two-gaming");
                YearOneMotionCapture.Begin("two-gaming");
                yield return Until(()=>year.DialogueVisible,23,"Shared game leads to nighttime argument");YearOneMotionCapture.End();
                YearOneMotionCapture.Begin("night-lights-off");yield return YearDialogue(year);
                yield return PressE();Check(state.CanInteract,"Player manually stands after argument");
                yield return Reach(player,flow.lightSwitchInteractable.transform.position+Vector3.right*.2f,movement);yield return PressE();
                Check(campaign.Progress.Has("day:2:lights"),"Player manually reaches switch and turns lights off");CaptureUI("campaign-lights-off");
                YearOneMotionCapture.End();
                yield return ResumeCheckpointUI(ui,state);
                Check(year.CurrentPhase==YearOneMilestoneController.Phase.AwaitSleep && campaign.Progress.CanSleep,"Lights-off checkpoint resumes sleep goal without replaying game");
                yield return Reach(player,chairs[0].ExitPivot.position,movement);yield return PressE();yield return PressKey(Key.Digit1);
                Check(year.CurrentPhase==YearOneMilestoneController.Phase.AwaitSleep && companion.Current==RoommateActivityController.Activity.Idle,"Completed coop game cannot replay argument after checkpoint");
                yield return PressE();
                YearOneMotionCapture.Begin("two-sleepers");
            }
            Check(campaign.Progress.CanSleep,"Daily academic and social requirements unlock sleep");
            var beds=Object.FindObjectsByType<BedInteractable>();Array.Sort(beds,(a,b)=>a.transform.position.x.CompareTo(b.transform.position.x));
            yield return Reach(player,beds[0].ExitPivot.position,movement);yield return PressE();
            yield return Until(()=>state.CurrentState==PlayerInteractionState.Sleeping,5,"Player enters bed and sleeping pose");
            if(day==1)
            {
                yield return Until(()=>Object.FindAnyObjectByType<RoommateActivityController>().IsSleeping,5,"Cancellation fixture reaches both sleepers first");
                yield return Wait(.4f);yield return PressE();yield return Until(()=>state.CanInteract,4,"Leaving bed during sleep countdown restores movement");
                yield return Wait(3.3f);
                Check(Object.FindAnyObjectByType<RoommateActivityController>().Current==RoommateActivityController.Activity.Idle && beds[1].Reservation.Owner==null && campaign.Progress.day==1,"Cancelled night releases NPC bed without advancing day");
                yield return Reach(player,beds[0].ExitPivot.position,movement);yield return PressE();
                yield return Until(()=>state.CurrentState==PlayerInteractionState.Sleeping,5,"Sleep can restart after cancellation");
            }
            yield return Until(()=>Object.FindAnyObjectByType<RoommateActivityController>().IsSleeping,5,"Long reaches the other bed and sleeps");
            yield return Wait(.4f);CaptureUI("campaign-day"+day+"-two-sleeping");
            yield return Until(()=>campaign.Progress.day==day+1 && ((state.CanInteract && !journey.IsBusy) || year.AcademicVisible),12,"Both sleepers wake and next day restores control");
            if(day==2)YearOneMotionCapture.End();
            if(year.AcademicVisible){CaptureUI("campaign-term-card-"+day);year.AcademicContinueButton.onClick.Invoke();yield return Wait(.1f);}
        }
        Check(campaign.Progress.Credits==32 && campaign.Progress.money==26000 && campaign.Progress.ReadyForFeast,"Six-day completion earns32credits with12paid trips including recovery");
        yield return ResumeCheckpointUI(ui,state);
        Check(campaign.Progress.ReadyForFeast && flow.currentStage==MVPFlowController.Stage.Stage1_Year1_Arrival,"Final checkpoint retains Year1 until feast finishes");
        yield return Reach(player,GameObject.Find("Hotpot_Preparation").transform.position,movement);yield return PressE();
        yield return Until(()=>year.FeastVisible && year.DialogueVisible,4,"Only completed year unlocks actual hotpot preparation");CaptureUI("campaign-final-feast");
        yield return YearDialogue(year);yield return Until(()=>flow.currentStage==MVPFlowController.Stage.Stage2_Year2_Temptation,5,"Feast automatically hands off to Year2 exactly once");
        Check(campaign.Progress.Has("year:two") && year.HeardHubris,"Year2 handoff and hubris are checkpointed");
        Check(!campaign.CompleteFeastAndEnterYearTwo(),"Repeated Year2 completion event cannot replay handoff");CaptureUI("campaign-year2");
    }
}
