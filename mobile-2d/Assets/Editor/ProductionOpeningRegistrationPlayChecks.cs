using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using Object=UnityEngine.Object;

public static partial class ProductionOpeningPlayChecks
{
    private static IEnumerator RegistrationRoute(ProductionOpeningController c,bool thorough)
    {
        int money=c.Progress.Money,history=c.Progress.ExportEvents().Length;
        Check(c.Progress.CurrentMission==9 && c.Progress.CurrentStep.id=="refresh_registration" && c.Progress.Credits==32,"M09 begins only after year1 and hotpot sleep");
        yield return MoveOpen(c);
        Check(c.RegistrationVisible && c.registrationTyping.enabled && !c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled,"Physical laptop opens portal and uses typing presentation");
        Check(!c.Commit(""),"Generic confirmation cannot skip portal opening");
        yield return KeyPress(Key.F5);
        Check(c.Progress.CurrentStep.id=="refresh_registration" && c.Progress.ExportEvents().Length==history,"F5before09:00cannot complete registration gate");
        if(thorough){Capture(c,"registration-before-open");MVPSceneSnapshot.SaveCameraFrame(c.view,Path.Combine(Output,"registration-typing-world.png"));}
        yield return Until(()=>c.RegistrationRemaining<=0,6,"Portal waits for authored opening time");
        Check(c.Progress.CurrentStep.id=="refresh_registration","Waiting for09:00alone does not auto-submit F5");
        yield return KeyPress(Key.F5);
        Check(c.Progress.CurrentStep.id=="select_classes" && c.RegistrationVisible && c.Busy && !c.player.CanInteract,"Successful F5 keeps same computer interaction locked for class selection");
        Check(!c.registrationFullClass.interactable && !c.confirmButton.interactable && c.RegistrationSelectedCount==0,"Full09:00section cannot be selected; empty draft cannot submit");
        c.confirmButton.onClick.Invoke();Check(c.Progress.CurrentStep.id=="select_classes","Direct click cannot confirm an incomplete draft");
        c.registrationClasses[0].onClick.Invoke();c.registrationClasses[1].onClick.Invoke();
        Check(c.RegistrationSelectedCount==2 && !c.confirmButton.interactable,"Selecting two subjects is still incomplete");
        yield return KeyPress(Key.Escape);
        Check(!c.RegistrationVisible && c.player.CanInteract && c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled && !c.registrationTyping.enabled,"Closing an uncommitted draft restores standing player");
        yield return SchoolReload(c);yield return MoveOpen(c);
        Check(c.RegistrationSelectedCount==0 && c.Progress.CurrentStep.id=="select_classes","Reload does not invent a saved draft or repeat portal gate");
        c.registrationClasses[2].onClick.Invoke();c.registrationClasses[0].onClick.Invoke();c.registrationClasses[1].onClick.Invoke();
        Check(c.RegistrationSelectedCount==3 && c.confirmButton.interactable,"All three non-overlapping available sections enable submission");
        if(thorough)Capture(c,"registration-classes");
        c.confirmButton.onClick.Invoke();c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Progress.CurrentStep.id=="confirm_schedule" && c.Progress.CurrentMission==9 && c.RegistrationVisible,"Double click cannot skip final schedule review in same frame");
        yield return KeyPress(Key.Escape);yield return SchoolReload(c);yield return MoveOpen(c);
        Check(c.RegistrationSelectedCount==3 && c.registrationSummary.text.Contains("06:45") && c.registrationSummary.text.Contains("13:00"),"Saved selection reconstructs exact fixed class schedule after reload");
        Check(!c.Commit(""),"Final schedule still requires its authored confirmation handler");
        if(thorough)Capture(c,"registration-schedule");
        c.confirmButton.onClick.Invoke();yield return Wait(.1f);
        Check(c.Busy && !c.player.CanInteract && !c.cancelButton.gameObject.activeSelf,"Semester/week bridge retains actor lock until finished");
        yield return Until(()=>!c.Busy && c.Progress.CurrentMission==10,5,"Confirmed schedule crosses representative weeks to Long invitation");
        Check(c.Progress.Has("schedule:0645") && c.Progress.CurrentStep.id=="choose_night" && c.Progress.CompletedMissions==9 && !c.Progress.Finished,"Registration opens M10choice without skipping story years");
        Check(c.Progress.Money==money && c.Progress.Credits==32 && c.Progress.ExportEvents().Length==history+3,"Three registration events preserve wallet and credits exactly");
        Check(!c.RegistrationVisible && !c.registrationTyping.enabled && c.player.CharacterVisual.GetComponent<SpriteRenderer>().enabled && c.player.GetComponent<Rigidbody2D>().simulated,"Registration bridge restores actor visibility and physics");
        yield return SchoolReload(c);
        Check(c.Progress.CurrentMission==10 && c.Progress.Money==money && c.Progress.Has("schedule:0645"),"Next-evening checkpoint cannot replay registration or change wallet");
        if(thorough)Capture(c,"registration-m10-ready");
    }
    private static IEnumerator RegistrationOnlyChecks()
    {
        var c=Object.FindAnyObjectByType<ProductionOpeningController>();yield return Wait(.3f);
        string directory=Path.Combine(sandbox,"registration");var session=new ProductionCampaignSession(directory,c.definition.text);
        while(session.Progress.CurrentMission<9)
        {var p=session.Progress;var step=p.CurrentStep;Check(session.Commit(p.CurrentMission,step.id,step.choices.Length>0?step.choices[0].id:"",out _),"Registration fixture earns "+step.id);}
        c.UseSaveDirectoryForTests(directory);c.beginButton.onClick.Invoke();yield return Wait(.1f);
        yield return RegistrationRoute(c,true);
        Check(errors.Count==0,"No runtime exceptions in registration fixture");
    }
}
