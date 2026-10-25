using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Object=UnityEngine.Object;

public static partial class ProductionOpeningPlayChecks
{
    private static IEnumerator SchoolWalk(ProductionOpeningController c,Transform target)
    {
        var movement=c.player.GetComponent<PlayerMovement>();double end=UnityEditor.EditorApplication.timeSinceStartup+8;
        Vector3 point=target.position;point.y=c.player.transform.position.y;
        while(Vector2.Distance(c.player.transform.position,point)>.18f && UnityEditor.EditorApplication.timeSinceStartup<end)
        {movement.SetSimulatedInput(((Vector2)(point-c.player.transform.position)).normalized);yield return null;}
        movement.SetSimulatedInput(Vector2.zero);yield return Wait(.1f);
        Check(Vector2.Distance(c.player.transform.position,point)<.35f,"School walk reaches "+target.name+" through movement");
    }
    private static IEnumerator SchoolLink(ProductionOpeningController c,ProductionSchoolLink.Route route,int zone)
    {
        var link=Array.Find(Object.FindObjectsByType<ProductionSchoolLink>(),a=>a.route==route);
        Check(link!=null && link.CanInteract,"Current school step enables physical "+route+" link");
        yield return SchoolWalk(c,link.transform);yield return KeyPress(Key.E);
        yield return Until(()=>c.ActiveZone==zone && !c.Busy,4,"School link arrives at zone "+zone+" and releases ownership");
    }
    private static IEnumerator SchoolReload(ProductionOpeningController c)
    {
        int money=c.Progress.Money,events=c.Progress.ExportEvents().Length;string step=c.Progress.CurrentStep.id;
        yield return KeyPress(Key.Escape);Check(c.startPanel.activeSelf,"School pause opens checkpoint menu");
        c.beginButton.onClick.Invoke();yield return Wait(.15f);
        Check(c.Progress.Money==money && c.Progress.ExportEvents().Length==events && c.Progress.CurrentStep.id==step && c.player.CanInteract,"School checkpoint restores step, wallet and standing control");
    }
    private static IEnumerator SchoolRoute(ProductionOpeningController c,bool thorough)
    {
        int initialMoney=c.Progress.Money;
        Check(c.Progress.CurrentMission==6 && c.ActiveZone==3 && c.placedBag.enabled,"M06 begins beside stored backpack in claimed bed");
        yield return MoveOpen(c);
        Check(c.Progress.Has("bed:luon_right")?c.player.transform.position.x>c.zones[3].position.x:c.player.transform.position.x<c.zones[3].position.x,"Backpack interaction follows selected bed");
        c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.CurrentStep.id=="wash_before_class" && c.Progress.SafeZone=="Room" && !c.placedBag.enabled && c.schoolJourney.HasBackpack,"Equipping bag persists at room and switches to carried visual");
        if(thorough)yield return SchoolReload(c);
        yield return SchoolLink(c,ProductionSchoolLink.Route.Hallway,2);
        Check(!c.CanUseSchoolLink(ProductionSchoolLink.Route.Lobby),"Cannot descend before successful wash");
        if(thorough)
        {
            yield return SchoolWalk(c,c.schoolFemale.transform);yield return KeyPress(Key.E);
            Check(!c.Busy && c.Progress.CurrentStep.id=="wash_before_class" && !c.schoolJourney.HasRefreshed && c.schoolJourney.LastNotice.Contains("WC Nữ"),"Wrong restroom leaves event and morning gate unchanged");
        }
        yield return SchoolWalk(c,c.schoolMale.transform);
        Check(c.player.GetComponent<PlayerInteractionDetector>().FindClosestInteractable()==c.schoolMale,"Walking to male WC selects its real doorway collider");
        yield return KeyPress(Key.E);
        Check(c.Busy && !c.player.CanInteract && !c.player.GetComponent<Rigidbody2D>().simulated,"WC presentation owns actor and disables physics");
        if(thorough)Capture(c,"school-washing");
        yield return Until(()=>!c.Busy && c.Progress.CurrentStep.id=="take_bus",6,"Physical wash commits event before releasing actor");
        Check(c.Progress.SafeZone=="Hallway" && c.schoolJourney.HasRefreshed && c.Progress.Money==initialMoney,"Wash checkpoint stays in hallway and costs no fare");
        if(thorough)yield return SchoolReload(c);
        yield return SchoolLink(c,ProductionSchoolLink.Route.Lobby,5);
        yield return SchoolWalk(c,c.schoolBus.StopAnchor);yield return KeyPress(Key.E);
        Check(c.Progress.CurrentStep.id=="take_bus" && c.Progress.Money==initialMoney,"Early boarding input cannot buy a ticket before arrival");
        yield return Until(()=>c.schoolBus.BusAtStop,5,"School bus physically approaches and stops");
        Check(c.schoolBus.BusRenderer.enabled,"School bus has a visible vehicle at boarding");if(thorough)Capture(c,"school-outbound-bus");
        yield return KeyPress(Key.E);yield return KeyPress(Key.E);
        yield return Until(()=>!c.Busy && c.ActiveZone==6,9,"Outbound bus arrives at H6 and unlocks actor");
        Check(c.Progress.CurrentStep.id=="enter_class" && c.Progress.Money==initialMoney-7000 && c.schoolBus.TicketsPurchased==1 && c.Progress.Credits==0,"Outbound commits exactly one production fare and no year credits");
        if(thorough)yield return SchoolReload(c);
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Until(()=>!c.Busy && c.ActiveZone==7,4,"H6 door enters actual classroom");
        Check(c.Progress.SafeZone=="H6Classroom" && c.Progress.CurrentStep.id=="solve_warmup","Classroom entry creates a stable classroom checkpoint");
        if(thorough)MVPSceneSnapshot.SaveCameraFrame(c.view,Path.Combine(Output,"school-classroom-world.png"));
        yield return MoveOpen(c);
        Check(!c.Commit(""),"Generic confirmation cannot bypass the warmup puzzle");
        for(int i=0;i<3;i++){c.confirmButton.onClick.Invoke();yield return Wait(.06f);}
        Check(c.SchoolPuzzleVisible && c.schoolStudyPose.enabled && !c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled,"Teacher/classmate dialogue opens timed question with seated presentation");
        if(thorough)
        {
            yield return KeyPress(Key.Digit1);
            Check(c.schoolPuzzle.AwaitingRetry && c.Progress.CurrentStep.id=="solve_warmup","Wrong answer requires retry without awarding success");
            yield return KeyPress(Key.Escape);
            Check(!c.SchoolPuzzleVisible && c.player.CanInteract && c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled,"Cancel question restores standing control without progression");
            yield return MoveOpen(c);for(int i=0;i<3;i++){c.confirmButton.onClick.Invoke();yield return Wait(.06f);}
            Capture(c,"school-warmup");MVPSceneSnapshot.SaveCameraFrame(c.view,Path.Combine(Output,"school-seated-world.png"));
        }
        yield return KeyPress(Key.Digit2);
        Check(!c.Busy && c.Progress.CurrentStep.id=="finish_lesson" && c.Progress.Credits==0 && !c.schoolStudyPose.enabled,"Correct55 completes only warmup and restores player");
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.06f);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.CurrentStep.id=="return_room" && c.Progress.Credits==0,"Lesson ends before homeward trip, without premature32credits");
        yield return SchoolLink(c,ProductionSchoolLink.Route.ReturnStop,6);
        yield return SchoolWalk(c,c.schoolBus.StopAnchor);yield return Until(()=>c.schoolBus.BusAtStop,5,"Return vehicle physically approaches H6 stop");
        Check(c.schoolBus.IsReturning && c.schoolBus.BusRenderer.flipX,"Homeward bus faces return direction");
        yield return KeyPress(Key.E);yield return KeyPress(Key.E);
        yield return Until(()=>!c.Busy && c.ActiveZone==3,10,"Return trip presents KTX, stairs and Room404 before unlocking");
        Check(c.Progress.Money==initialMoney-14000 && c.schoolBus.TicketsPurchased==2 && c.Progress.CurrentStep.id=="tell_dam" && c.Progress.SafeZone=="Room","Separate return fare reaches durable dorm reaction checkpoint");
        if(thorough)yield return SchoolReload(c);
        yield return MoveOpen(c);c.confirmButton.onClick.Invoke();yield return Wait(.06f);c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.CurrentMission==7 && c.Progress.CurrentStep.id=="join_wc_queue" && c.Progress.CompletedMissions==6 && c.Progress.Credits==0,"Dam dorm reaction opens another morningM07, not year-end or gaming");
        Check(c.player.CanInteract && c.player.GetComponent<Rigidbody2D>().simulated && c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled,"M06 end restores movement, physics and single actor presentation");
        if(thorough)Capture(c,"school-m06-complete");
    }
    private static IEnumerator SchoolOnlyChecks()
    {
        foreach(string bed in new[]{"left","right"})
        {
            var c=Object.FindAnyObjectByType<ProductionOpeningController>();yield return Wait(.3f);
            string directory=Path.Combine(sandbox,bed);var session=new ProductionCampaignSession(directory,c.definition.text);
            while(session.Progress.CurrentMission<6)
            {
                var p=session.Progress;var step=p.CurrentStep;
                string choice=step.id=="claim_bed"?bed:step.choices.Length>0?step.choices[0].id:"";
                Check(session.Commit(p.CurrentMission,step.id,choice,out var error),"School-only fixture prefix "+p.CurrentMission+":"+step.id+" "+error);
            }
            c.UseSaveDirectoryForTests(directory);c.beginButton.onClick.Invoke();yield return Wait(.1f);
            yield return SchoolRoute(c,bed=="left");
            if(bed=="left")
            {
                UnityEditor.SceneManagement.EditorSceneManager.LoadSceneInPlayMode(ProductionOpeningSceneBuilder.ScenePath,new UnityEngine.SceneManagement.LoadSceneParameters(UnityEngine.SceneManagement.LoadSceneMode.Single));
                yield return Wait(.3f);
            }
        }
    }

}
