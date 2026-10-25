using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Object=UnityEngine.Object;

public static partial class ProductionOpeningPlayChecks
{
    private static Key AnswerKey(int answer)=>answer==0?Key.Digit1:answer==1?Key.Digit2:Key.Digit3;
    private static IEnumerator YearOneRoute(ProductionOpeningController c,bool missed)
    {
        int initialMoney=c.Progress.Money,initialTickets=c.schoolBus.TicketsPurchased;
        Check(c.Progress.CurrentMission==7 && c.Progress.Credits==0 && c.ActiveZone==3,"M07 starts a distinct morning at404 with no year credits");
        float beforePause=c.MorningRemaining;yield return KeyPress(Key.Escape);yield return Wait(.3f);
        Check(c.startPanel.activeSelf && Mathf.Abs(c.MorningRemaining-beforePause)<.1f,"Morning countdown pauses while menu is open");
        c.beginButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.MorningRemaining<=beforePause && c.MorningRemaining>beforePause-.3f,"Menu reload preserves elapsed morning instead of restarting deadline");
        yield return SchoolLink(c,ProductionSchoolLink.Route.Hallway,2);
        Check(!c.schoolMale.enabled && !c.CanUseSchoolLink(ProductionSchoolLink.Route.Lobby),"Queue cannot be skipped by using WC or stairs early");
        yield return MoveOpen(c);Check(!c.Commit(""),"Generic confirmation cannot skip queue and wash");c.confirmButton.onClick.Invoke();yield return Wait(.15f);Capture(c,missed?"year1-late-queue":"year1-on-time-queue");
        yield return Until(()=>c.QueueReady && !c.Busy,11,"Real queue clears before male restroom is enabled");
        Check(c.Progress.CurrentStep.id=="join_wc_queue" && c.schoolMale.enabled,"Waiting alone does not claim wash success");
        yield return SchoolWalk(c,c.schoolMale.transform);yield return KeyPress(Key.E);
        yield return Until(()=>c.schoolJourney.WashProgressVisible,2,"Morning wash shows real progress panel");
        var wash=GameObject.Find("SchoolWash");
        Check(wash.transform.Find("ToothbrushCup").GetComponent<UnityEngine.UI.Image>().sprite.name=="toothbrush_cup" && wash.transform.Find("FoldedTowel").GetComponent<UnityEngine.UI.Image>().sprite.name=="towel_folded","WC progress presents the authored cup/brush and towel assets");
        yield return Until(()=>c.Progress.CurrentStep.id=="catch_bus" && !c.Busy,6,"Queued morning wash commits exact M07 event");
        Check(c.MorningMissed==missed && c.Progress.Money==initialMoney,"Morning deadline selects on-time or missed outcome without charging a fare");
        yield return SchoolLink(c,ProductionSchoolLink.Route.Lobby,5);yield return SchoolWalk(c,c.schoolBus.StopAnchor);
        yield return Until(()=>c.schoolBus.BusAtStop,7,"M07 has an actual parked bus, including next-bus recovery");
        Check(c.schoolBus.MissedBus==missed && c.schoolBus.BusRenderer.enabled,"Departure host and visible vehicle match morning outcome");Capture(c,missed?"year1-next-bus":"year1-on-time-bus");
        yield return KeyPress(Key.E);yield return KeyPress(Key.E);
        yield return Until(()=>!c.Busy && c.ActiveZone==6,9,"M07 bus commits and arrives at H6");
        Check(c.Progress.Money==initialMoney-7000 && c.schoolBus.TicketsPurchased==initialTickets+1 && c.Progress.Has(missed?"travel:missed_bus_recovered":"travel:on_time") && c.Progress.Credits==0,"M07 durable choice charges one fare and does not grant32credits");
        yield return MoveOpen(c);Check(c.body.text.Contains(missed?"chuyến sau":"đúng giờ"),"Teacher attendance acknowledges actual travel choice");c.confirmButton.onClick.Invoke();
        yield return Until(()=>!c.Busy && c.ActiveZone==7 && c.Progress.CurrentStep.id=="review_commute_ledger",4,"Attendance leads into classroom school montage");
        yield return MoveOpen(c);Check(c.body.text.Contains("63.000") && c.body.text.Contains("12 lượt"),"Interactive ledger discloses nine remaining fares and full twelve-trip total");Capture(c,"year1-commute-ledger");
        c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.Money==initialMoney-70000 && c.Progress.Has("ledger:year1_commute_reserved"),"Remaining nine fares reserve63000once from the same wallet");
        yield return SchoolReload(c);Check(c.Progress.Money==initialMoney-70000,"Reload cannot charge reserved fares twice");
        int credits=0;
        foreach(var course in c.YearOneCurriculum.courses)
        {
            Check(c.Progress.CurrentStep.id=="study_y1_"+course.id,"Correct next representative lesson: "+course.name);
            yield return MoveOpen(c);Check(!c.Commit(""),"Lesson needs actual question success");c.confirmButton.onClick.Invoke();yield return Wait(.1f);
            Check(c.SchoolPuzzleVisible && c.schoolPuzzle.ActiveCorrectAnswer==course.study.answer,"Authored lesson exercise is bound for "+course.name);
            if(credits==0)
            {
                yield return KeyPress(AnswerKey((course.study.answer+1)%3));Check(c.schoolPuzzle.AwaitingRetry && c.Progress.Credits==0,"Wrong lesson answer gives retry without credits");
                yield return KeyPress(Key.R);yield return KeyPress(Key.Escape);
                Check(!c.Busy && c.player.CanInteract && c.Progress.CurrentStep.id=="study_y1_"+course.id,"Cancelling lesson restores control without marking success");
                yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
            }
            yield return KeyPress(AnswerKey(course.study.answer));
            Check(c.Progress.CurrentStep.id=="exam_y1_"+course.id && c.Progress.Credits==credits,"Lesson prepares exam but does not award its credits");
            yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
            Check(c.schoolPuzzle.ActiveCorrectAnswer==course.exam.answer,"Distinct authored exam exercise is bound");
            if(credits==0)Capture(c,"year1-first-exam");
            yield return KeyPress(AnswerKey(course.exam.answer));credits+=course.credits;
            Check(c.Progress.Credits==credits && c.Progress.Has("passed:Y1_"+course.id),"Passed exam commits exact cumulative credits for "+course.name);
            yield return KeyPress(AnswerKey(course.exam.answer));Check(c.Progress.Credits==credits && c.Progress.Money==initialMoney-70000,"Repeated answer cannot duplicate credit or fare");
            if(credits==16){yield return SchoolReload(c);Check(c.Progress.Credits==16,"Midyear checkpoint preserves passed courses and credits");}
        }
        Check(credits==32 && c.Progress.CurrentStep.id=="finish_year_exams","All six courses total32 before year summary");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        yield return MoveOpen(c);Check(c.body.text.Contains("32") && !c.body.text.Contains("CHƯA ĐẠT"),"Year results show actual six passed courses, not an automatic warmup award");Capture(c,"year1-results");
        Check(!c.Commit(""),"Results require the authored homeward transition");c.confirmButton.onClick.Invoke();
        yield return Until(()=>!c.Busy && c.ActiveZone==3 && c.Progress.CurrentMission==8,4,"Read results travels home using reserved fare and opens hotpotM08");
        Check(c.Progress.Money==initialMoney-70000 && c.Progress.Credits==32 && c.storedCooker.enabled && !c.feastPot.enabled,"M08 starts with earned credits and stored unprepared cooker");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.CurrentStep.id=="plug_cooker" && c.feastPot.enabled && !c.feastCord.activeSelf,"Taking cooker does not automatically connect power");
        yield return MoveOpen(c);Check(!c.Commit(""),"Plugging requires its real hold gesture");yield return KeyPress(Key.Space,.3f);
        Check(c.Progress.CurrentStep.id=="plug_cooker" && !c.feastCord.activeSelf,"Released partial hold cannot connect cooker");yield return KeyPress(Key.Escape);yield return MoveOpen(c);
        c.holdButton.OnPointerDown(new PointerEventData(EventSystem.current));
        yield return Until(()=>!c.Busy && c.Progress.CurrentStep.id=="add_ingredients",2,"Touch-style sustained hold commits power connection");c.holdButton.OnPointerUp(new PointerEventData(EventSystem.current));
        Check(c.feastCord.activeSelf && !c.feastIngredients.activeSelf,"Cooker is powered before ingredients are added");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.feastIngredients.activeSelf && c.Progress.CurrentStep.id=="share_feast","Adding shared ingredients is a separate action");
        Check(c.feastIngredients.transform.Find("DriedBuffaloMeat").GetComponent<SpriteRenderer>().sprite.name=="dried_buffalo_meat" && c.feastIngredients.transform.Find("SharedDrink").GetComponent<SpriteRenderer>().sprite.name=="drinking_glass","Feast stages authored dried-meat gift and separate drinking glass");
        yield return MoveOpen(c);
        Check(Array.TrueForAll(c.feastActors,a=>a.activeSelf) && Array.TrueForAll(c.roomFriends,a=>!a.activeSelf) && !c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled,"Feast uses four floor poses and hides only replaced standing presentation");
        MVPSceneSnapshot.SaveCameraFrame(c.view,Path.Combine(Output,"year1-hotpot-world.png"));Capture(c,"year1-hotpot-dialogue");
        yield return KeyPress(Key.Escape);Check(c.player.CanInteract && c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled && Array.TrueForAll(c.feastActors,a=>!a.activeSelf),"Cancelling feast restores standing actor without success");
        yield return MoveOpen(c);for(int i=0;i<3;i++){c.confirmButton.onClick.Invoke();yield return Wait(.06f);}
        Check(c.Progress.CurrentStep.id=="toast" && c.Progress.Money==initialMoney-70000,"Shared meal advances to toast without a duplicate ingredient charge");
        yield return MoveOpen(c);for(int i=0;i<2;i++){c.confirmButton.onClick.Invoke();yield return Wait(.06f);}
        Check(c.Progress.CurrentStep.id=="sleep_new_term" && c.Progress.CurrentMission==8,"Toast does not skip sleep or registration transition");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Until(()=>!c.Busy && c.Progress.CurrentMission==9,4,"Sleep opens registrationM09instead of legacy gaming");
        Check(c.Progress.Has("term:3") && c.Progress.CurrentStep.id=="refresh_registration" && c.Progress.Credits==32 && !c.Progress.Finished,"M08 final gate preserves exact32credits and unfinished full campaign");
        Check(c.player.CanInteract && c.player.GetComponent<Rigidbody2D>().simulated && !c.feastPot.enabled && Array.TrueForAll(c.feastActors,a=>!a.activeSelf),"Term transition restores actor, physics and clears feast staging");
        yield return SchoolReload(c);Check(c.Progress.CurrentMission==9 && c.Progress.Money==initialMoney-70000,"M09 entry checkpoint cannot repeat year rewards or fares");Capture(c,"year1-registration-ready");
    }
    private static IEnumerator YearOneOnlyChecks()
    {
        foreach(bool missed in new[]{false,true})
        {
            var c=Object.FindAnyObjectByType<ProductionOpeningController>();yield return Wait(.3f);
            c.morningDepartureSeconds=missed?3:120;
            string directory=Path.Combine(sandbox,missed?"late":"on-time");var session=new ProductionCampaignSession(directory,c.definition.text);
            while(session.Progress.CurrentMission<7)
            {
                var p=session.Progress;var step=p.CurrentStep;string choice=step.id=="claim_bed"?(missed?"right":"left"):step.choices.Length>0?step.choices[0].id:"";
                Check(session.Commit(p.CurrentMission,step.id,choice,out var error),"Year1 fixture earns prefix "+step.id+" "+error);
            }
            c.UseSaveDirectoryForTests(directory);c.beginButton.onClick.Invoke();yield return Wait(.1f);yield return YearOneRoute(c,missed);
            if(!missed)
            {UnityEditor.SceneManagement.EditorSceneManager.LoadSceneInPlayMode(ProductionOpeningSceneBuilder.ScenePath,new UnityEngine.SceneManagement.LoadSceneParameters(UnityEngine.SceneManagement.LoadSceneMode.Single));yield return Wait(.3f);}
        }
        yield return ExpiredParkedBusCheck();
    }
    private static IEnumerator ExpiredParkedBusCheck()
    {
        UnityEditor.SceneManagement.EditorSceneManager.LoadSceneInPlayMode(ProductionOpeningSceneBuilder.ScenePath,new UnityEngine.SceneManagement.LoadSceneParameters(UnityEngine.SceneManagement.LoadSceneMode.Single));yield return Wait(.3f);
        var c=Object.FindAnyObjectByType<ProductionOpeningController>();c.morningDepartureSeconds=20;
        string directory=Path.Combine(sandbox,"expired-parked");var session=new ProductionCampaignSession(directory,c.definition.text);
        while(session.Progress.CurrentMission<7 || session.Progress.CurrentStep.id=="join_wc_queue")
        {var p=session.Progress;var step=p.CurrentStep;Check(session.Commit(p.CurrentMission,step.id,step.choices.Length>0?step.choices[0].id:"",out _),"Parked-departure fixture earns "+step.id);}
        c.UseSaveDirectoryForTests(directory);c.beginButton.onClick.Invoke();yield return Wait(.1f);
        yield return SchoolLink(c,ProductionSchoolLink.Route.Lobby,5);yield return SchoolWalk(c,c.schoolBus.StopAnchor);
        yield return Until(()=>c.schoolBus.BusAtStop,5,"Original departure can park before morning deadline");
        Check(!c.MorningMissed,"Parked-bus fixture reaches actual first bus on time");
        Vector3 parked=c.schoolBus.BusRenderer.transform.position;int wallet=c.Progress.Money;
        yield return Until(()=>c.MorningMissed,22,"Standing at stop does not reset or stop departure clock");
        Check(!c.schoolBus.TryBoard(c.player) && c.Progress.Money==wallet,"Expired original bus cannot be boarded as if it were the next trip");
        yield return Until(()=>!c.schoolBus.BusAtStop,1,"Expired parked departure withdraws boarding availability");yield return Wait(.3f);
        Check(c.schoolBus.BusRenderer.enabled && c.schoolBus.BusRenderer.transform.position.x>parked.x+.1f,"Expired bus visibly departs before recovery vehicle arrives");Capture(c,"year1-expired-bus-departing");
        yield return Until(()=>c.schoolBus.BusAtStop,7,"Next bus physically arrives after expired parked departure");
        Check(Vector3.Distance(c.schoolBus.BusRenderer.transform.position,parked)<.02f && c.Progress.Money==wallet,"Replacement reaches stop without an early fare");
        yield return KeyPress(Key.E);yield return Until(()=>!c.Busy && c.ActiveZone==6,9,"Replacement bus reaches H6 after real boarding");
        Check(c.Progress.Has("travel:missed_bus_recovered") && c.Progress.Money==wallet-7000 && c.schoolBus.TicketsPurchased==1,"Expired parked departure recovers with exactly one durable fare");
    }

    private static IEnumerator LifecycleOnlyChecks()
    {
        var c=Object.FindAnyObjectByType<ProductionOpeningController>();yield return Wait(.3f);
        string directory=Path.Combine(sandbox,"lifecycle");var session=new ProductionCampaignSession(directory,c.definition.text);
        while(session.Progress.CurrentMission<9)
        {var p=session.Progress;var step=p.CurrentStep;Check(session.Commit(p.CurrentMission,step.id,step.choices.Length>0?step.choices[0].id:"",out _),"Lifecycle fixture earns "+step.id);}
        c.UseSaveDirectoryForTests(directory);c.beginButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Ready && c.Progress.CurrentMission==9 && c.player.CanInteract,"Lifecycle fixture resumes stable M09 checkpoint");
        Object.Destroy(c.feastActors[0]);Object.Destroy(c.queueStudents[0]);yield return Wait(.25f);
        c.enabled=false;yield return Wait(.1f);
        Check(!c.ModalVisible && c.player.CanInteract && c.player.GetComponent<Rigidbody2D>().simulated,"Updates and cleanup tolerate despawned floor/queue actors");
        Check(errors.Count==0,"No runtime exceptions during actor despawn or controller disable");
    }

}
