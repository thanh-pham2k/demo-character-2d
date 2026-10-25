using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

// Opt-in real Play Mode regression check; never changes the user's save.
[InitializeOnLoad]
public static class RoomRestVisualChecks
{
    const string Key = "Room404.RestVisualChecks";
    static int phase;
    static float next;
    static BedInteractable[] beds;
    static PlayerInteractionStateController player;
    static string Output => Path.GetFullPath("PlayModeInteractionReports/room-rest-fix");
    static RoomRestVisualChecks() { EditorApplication.update += Tick; }
    static void Require(bool condition, string message) { if (!condition) throw new Exception(message); }
    static void Tick()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating) return;
        string request = Path.GetFullPath("Temp/RoomRestVisualChecks.run");
        if (File.Exists(request) && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            File.Delete(request); Directory.CreateDirectory(Output);
            SessionState.SetBool(Key, true); phase = 0; EditorApplication.isPlaying = true; return;
        }
        if (!SessionState.GetBool(Key, false) || !EditorApplication.isPlaying || Time.time < 2 || Time.time < next) return;
        try
        {
            var story = Object.FindAnyObjectByType<MVPFlowController>();
            if (phase == 0)
            {
                var campaign = Object.FindAnyObjectByType<YearOneCampaignController>();
                campaign.UseStoreForTests(Path.Combine(Path.GetTempPath(), "room-rest-fix-" + Guid.NewGuid()));
                Object.FindAnyObjectByType<YearOneCampaignUI>().NewGameButton.onClick.Invoke();
                player = Object.FindAnyObjectByType<PlayerInteractionStateController>();
                beds = Object.FindObjectsByType<BedInteractable>();
                Array.Sort(beds, (a,b) => a.transform.position.x.CompareTo(b.transform.position.x));
                Require(!story.trashPileProp.activeInHierarchy, "Trash visible on arrival");
                MVPSceneSnapshot.SaveCameraFrame(Camera.main, Path.Combine(Output, "clean-room.png"));
                story.OnClaimBed();
                Require(campaign.Progress.Has("opening:clean"), "Clean-room completion missing");
                Require(!story.trashPileProp.activeInHierarchy && !story.trashInteractable.IsActive, "Trash reactivated after claim");
                next = Time.time + 2; phase = 1;
            }
            else if (phase == 1 || phase == 3)
            {
                beds[phase == 1 ? 0 : 1].Interact(player);
                next = Time.time + 2; phase++;
            }
            else if (phase == 2 || phase == 4)
            {
                Require(player.CurrentState == PlayerInteractionState.Lying, "Bed enter did not reach Lying");
                var visual = player.CharacterVisual;
                Require(visual.GetComponent<SpriteRenderer>().sprite == visual.bedSleepSprite, "Resting eyes reopened after idle animation");
                MVPSceneSnapshot.SaveCameraFrame(Camera.main, Path.Combine(Output, phase == 2 ? "left-rest.png" : "right-rest.png"));
                beds[phase == 2 ? 0 : 1].Interact(player);
                next = Time.time + 2; phase++;
            }
            else
            {
                Require(player.CurrentState == PlayerInteractionState.Standing, "Bed exit failed");
                File.WriteAllText(Path.Combine(Output, "result.txt"), "PASS: clean arrival, automatic campaign clean completion, both real bed enter/idle/exit routines, closed-eye rest persists beyond two breathing frames.\n");
                SessionState.EraseBool(Key); EditorApplication.isPlaying = false;
            }
        }
        catch (Exception e)
        {
            Directory.CreateDirectory(Output); File.WriteAllText(Path.Combine(Output, "result.txt"), "FAIL: " + e);
            SessionState.EraseBool(Key); EditorApplication.isPlaying = false; Debug.LogException(e);
        }
    }
}
