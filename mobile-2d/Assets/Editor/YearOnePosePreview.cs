using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object=UnityEngine.Object;

// Opt-in isolated visual fixture. Not an acceptance test or gameplay shortcut.
[InitializeOnLoad]
public static class YearOnePosePreview
{
    const string Key="Room404.PosePreview";
    static float next;
    static int phase;
    static bool lightTriggered;
    static float markerDeadline;
    static void OnLightProbe()=>lightTriggered=true;
    static YearOnePosePreview(){EditorApplication.update+=Tick;}
    static void Tick()
    {
        var flag=Path.GetFullPath("Temp/YearOnePosePreview.run");
        if(File.Exists(flag) && !EditorApplication.isPlaying)
        {File.Delete(flag);SessionState.SetBool(Key,true);EditorApplication.isPlaying=true;return;}
        if(!EditorApplication.isPlaying || !SessionState.GetBool(Key,false) || Time.time<2 || Time.time<next)return;
        var state=Object.FindAnyObjectByType<PlayerInteractionStateController>();
        var npc=Object.FindAnyObjectByType<RoommateActivityController>(FindObjectsInactive.Include);
        var chairs=Object.FindObjectsByType<ChairInteractable>();Array.Sort(chairs,(a,b)=>a.transform.position.x.CompareTo(b.transform.position.x));
        var beds=Object.FindObjectsByType<BedInteractable>();Array.Sort(beds,(a,b)=>a.transform.position.x.CompareTo(b.transform.position.x));
        if(phase==0)
        {
            var campaign=Object.FindAnyObjectByType<YearOneCampaignController>();campaign.UseStoreForTests(Path.Combine(Path.GetTempPath(),"room404-pose-preview"));
            Object.FindAnyObjectByType<YearOneCampaignUI>().NewGameButton.onClick.Invoke();
            var story=Object.FindAnyObjectByType<MVPFlowController>();story.RestoreYearOneOpening(true,true,true);story.roommatesGroup.SetActive(true);
            YearOneMotionCapture.Begin("pose-registration-review");
            chairs[1].Interact(state);Object.FindAnyObjectByType<DeskActionModal>().UseComputer();npc.StartGaming(chairs[0],null);next=Time.time+3;phase=1;
        }
        else if(phase==1)
        {
            MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.GetFullPath("PlayModeInteractionReports/pose-preview-gaming.png"));
            chairs[1].Interact(state);npc.LeaveGaming();next=Time.time+1.5f;phase=2;
        }
        else if(phase==2)
        {
            MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.GetFullPath("PlayModeInteractionReports/pose-preview-standing.png"));
            var light=Object.FindAnyObjectByType<MVPFlowController>().lightSwitchInteractable;
            lightTriggered=false;light.SetInteractable(true);light.onInteract.AddListener(OnLightProbe);
            markerDeadline=Time.time+5;next=0;phase=4;
        }
        else if(phase==4)
        {
            var light=Object.FindAnyObjectByType<MVPFlowController>().lightSwitchInteractable;
            Vector3 target=light.transform.position+Vector3.right*.2f;
            var movement=state.GetComponent<PlayerMovement>();
            if(Vector2.Distance(state.transform.position,target)>.12f && Time.time<markerDeadline)
            {movement.SetSimulatedInput(((Vector2)(target-state.transform.position)).normalized);return;}
            movement.SetSimulatedInput(Vector2.zero);
            bool reached=Vector2.Distance(state.transform.position,target)<.3f;
            bool selected=state.GetComponent<PlayerInteractionDetector>().FindClosestInteractable()==(IInteractable)light;
            if(reached && selected)light.Interact();
            string result=(reached && selected && lightTriggered?"PASS":"FAIL")+" floor marker geometry, nearest detector and generic Interact event. Position "+state.transform.position+"; target "+target+". Quest/E gating covered separately by full campaign suite.";
            File.WriteAllText(Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/reviews/year1_campaign/light-marker-review.txt")),result);
            MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.GetFullPath("PlayModeInteractionReports/light-marker-review.png"),true);
            light.onInteract.RemoveListener(OnLightProbe);light.SetInteractable(false);
            npc.StopActivity();beds[0].ExecuteImmediateEnter(state);state.CharacterVisual.SetSleeping(true);npc.StartSleeping(beds[1],null);next=Time.time+4;phase=3;
        }
        else
        {
            YearOneMotionCapture.End();
            MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.GetFullPath("PlayModeInteractionReports/pose-preview-sleeping.png"));
            SessionState.EraseBool(Key);phase=0;EditorApplication.isPlaying=false;
        }
    }
}
