using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

// Drives public interaction events and UI over real game time. Never assigns currentStage.
public static class MVPSceneTester
{
    public static bool Completed { get; private set; }
    public static bool Success { get; private set; }
    private static MVPFlowController flow;
    private static Mouse mouse;
    private static float deadline;
    private static float nextDialogueClick;
    private static bool sawF, sawA, sawStorySeat, sawStoryRelease;
    private static readonly HashSet<MVPFlowController.Stage> stages = new HashSet<MVPFlowController.Stage>();
    private static readonly List<string> errors = new List<string>();
    [Serializable] private class Report { public bool success; public string[] stages; public bool gradeF, gradeA, storySeat, storyRelease, ending; public string[] errors; }

    public static void SimulateMVP()
    {
        if (!EditorApplication.isPlaying) throw new InvalidOperationException("SimulateMVP requires Play Mode. Use PlayModeInteractionSuite.RunAllTests for a full fresh run.");
        EditorApplication.update -= Tick;
        CleanupInput();
        flow = UnityEngine.Object.FindFirstObjectByType<MVPFlowController>();
        var milestone=UnityEngine.Object.FindFirstObjectByType<YearOneMilestoneController>();
        bool completedYearOne=milestone!=null && milestone.HeardHubris && milestone.Credits==32 &&
            flow!=null && flow.currentStage==MVPFlowController.Stage.Stage2_Year2_Temptation;
        if (flow == null || (flow.currentStage != MVPFlowController.Stage.Stage1_Year1_Arrival && !completedYearOne))
            throw new InvalidOperationException("Story simulation must start in Stage 1.");
        Completed = false; Success = false;
        sawF = sawA = sawStorySeat = sawStoryRelease = false;
        stages.Clear(); errors.Clear();
        // The full interaction suite already observed Stage 1 through H6/hotpot.
        if(completedYearOne)stages.Add(MVPFlowController.Stage.Stage1_Year1_Arrival);
        else flow.ConfigureExtendedYearOne(false); // Standalone legacy-path regression.
        mouse = InputSystem.AddDevice<Mouse>();
        mouse.MakeCurrent();
        deadline = Time.time + 55f;
        nextDialogueClick = Time.time + 0.2f;
        EditorApplication.update += Tick;
    }
    private static void Tick()
    {
        try
        {
            if (!EditorApplication.isPlaying || flow == null) throw new Exception("Story simulation lost Play Mode or flow.");
            if (Time.time > deadline) throw new TimeoutException("Story did not reach ending through actual stage transitions.");
            stages.Add(flow.currentStage);
            var state = flow.playerObj.GetComponent<PlayerInteractionStateController>();
            if (flow.currentStage == MVPFlowController.Stage.Stage1_Year1_Arrival)
            {
                if (flow.bedInteractable.IsActive) flow.bedInteractable.Interact();
                else if (flow.trashInteractable.IsActive) flow.trashInteractable.Interact();
            }
            if (flow.dialoguePanel.activeSelf && Time.time >= nextDialogueClick)
            {
                flow.dialogueNextButton.onClick.Invoke();
                nextDialogueClick = Time.time + 0.2f;
            }
            if (flow.yesNoModal.activeSelf) flow.yesButton.onClick.Invoke();
            if (flow.gradeStampPanel.activeSelf)
            {
                sawF |= flow.gradeStampImage.sprite == flow.gradeStampF;
                sawA |= flow.gradeStampImage.sprite == flow.gradeStampA;
            }
            if (flow.currentStage == MVPFlowController.Stage.Stage3_Year5_Awakening)
            {
                if (flow.laptopInteractable.IsActive) flow.laptopInteractable.Interact();
                sawStorySeat |= state.CurrentState == PlayerInteractionState.Sitting &&
                    flow.playerObj.GetComponent<PlayerMovement>().IsInteractionMovementLocked;
                mouse.MakeCurrent();
                InputSystem.QueueStateEvent(mouse, flow.uninstallModal.activeSelf
                    ? new MouseState().WithButton(MouseButton.Left) : new MouseState());
                if (sawStorySeat && !flow.uninstallModal.activeSelf && state.CurrentState == PlayerInteractionState.Standing)
                    sawStoryRelease = true;
            }
            if (flow.currentStage == MVPFlowController.Stage.Stage4_Year6_Graduation && flow.lightSwitchInteractable.IsActive)
                flow.lightSwitchInteractable.Interact();
            if (flow.currentStage == MVPFlowController.Stage.Finished && flow.endingPanel.activeSelf)
            {
                if (stages.Count != 5 || !sawF || !sawA || !sawStorySeat || !sawStoryRelease ||
                    state.CurrentState != PlayerInteractionState.Standing || flow.globalLight.intensity > 0.06f)
                    throw new Exception("Ending reached but required stage, grade, sitting/release or light checks failed.");
                Finish(true);
            }
        }
        catch (Exception e) { errors.Add(e.ToString()); Finish(false); }
    }
    public static void CleanupInput()
    {
        if (mouse != null && mouse.added) InputSystem.RemoveDevice(mouse);
        mouse = null;
        EditorApplication.update -= Tick;
    }
    private static void Finish(bool success)
    {
        Success = success;
        Completed = true;
        CleanupInput();
        Directory.CreateDirectory(PlayModeInteractionSuite.Output);
        var visited = new List<string>();
        foreach (var stage in stages) visited.Add(stage.ToString());
        File.WriteAllText(Path.Combine(PlayModeInteractionSuite.Output, "story-report.json"), JsonUtility.ToJson(new Report
        { success = success, stages = visited.ToArray(), gradeF = sawF, gradeA = sawA,
          storySeat = sawStorySeat, storyRelease = sawStoryRelease,
          ending = flow != null && flow.endingPanel.activeSelf, errors = errors.ToArray() }, true));
        Debug.Log("[MVPSceneTester] Actual story flow " + (success ? "PASSED" : "FAILED"));
    }
}
