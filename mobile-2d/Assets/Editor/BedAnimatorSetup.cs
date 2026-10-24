using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public static class BedAnimatorSetup
{
    public static void CreateAnimatorController()
    {
        string dir = "Assets/Animations/BedInteraction";
        string path = Path.Combine(dir, "Luon_Character_Animator.controller");

        var controller = AnimatorController.CreateAnimatorControllerAtPath(path);

        // Parameters
        controller.AddParameter("IsWalking", AnimatorControllerParameterType.Bool);
        controller.AddParameter("IsSitting", AnimatorControllerParameterType.Bool);
        controller.AddParameter("IsLying", AnimatorControllerParameterType.Bool);
        controller.AddParameter("IsSleeping", AnimatorControllerParameterType.Bool);
        controller.AddParameter("EnterBed", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("ExitBed", AnimatorControllerParameterType.Trigger);

        var rootStateMachine = controller.layers[0].stateMachine;

        // Load Clips
        var enterClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(Path.Combine(dir, "Luon_Bed_Enter.anim"));
        var idleBedClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(Path.Combine(dir, "Luon_Bed_Idle.anim"));
        var sleepClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(Path.Combine(dir, "Luon_Bed_Sleep.anim"));
        var exitClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(Path.Combine(dir, "Luon_Bed_Exit.anim"));

        // States
        var idleState = rootStateMachine.AddState("Idle");
        var bedEnterState = rootStateMachine.AddState("BedEnter");
        bedEnterState.motion = enterClip;

        var bedIdleState = rootStateMachine.AddState("BedIdle");
        bedIdleState.motion = idleBedClip;

        var bedSleepState = rootStateMachine.AddState("BedSleep");
        bedSleepState.motion = sleepClip;

        var bedExitState = rootStateMachine.AddState("BedExit");
        bedExitState.motion = exitClip;

        rootStateMachine.defaultState = idleState;

        // Transitions:
        // AnyState / Idle -> BedEnter on EnterBed trigger
        var enterTrans = rootStateMachine.AddAnyStateTransition(bedEnterState);
        enterTrans.AddCondition(AnimatorConditionMode.If, 0, "EnterBed");
        enterTrans.duration = 0f;
        enterTrans.canTransitionToSelf = false;

        // BedEnter -> BedIdle on completion
        var enterToIdle = bedEnterState.AddTransition(bedIdleState);
        enterToIdle.hasExitTime = true;
        enterToIdle.exitTime = 1.0f;
        enterToIdle.duration = 0f;

        // BedIdle -> BedSleep on IsSleeping == true
        var idleToSleep = bedIdleState.AddTransition(bedSleepState);
        idleToSleep.AddCondition(AnimatorConditionMode.If, 0, "IsSleeping");
        idleToSleep.duration = 0.2f;

        // BedSleep -> BedIdle on IsSleeping == false
        var sleepToIdle = bedSleepState.AddTransition(bedIdleState);
        sleepToIdle.AddCondition(AnimatorConditionMode.IfNot, 0, "IsSleeping");
        sleepToIdle.duration = 0.2f;

        // BedIdle / BedSleep -> BedExit on ExitBed trigger
        var exitTrans1 = bedIdleState.AddTransition(bedExitState);
        exitTrans1.AddCondition(AnimatorConditionMode.If, 0, "ExitBed");
        exitTrans1.duration = 0f;

        var exitTrans2 = bedSleepState.AddTransition(bedExitState);
        exitTrans2.AddCondition(AnimatorConditionMode.If, 0, "ExitBed");
        exitTrans2.duration = 0f;

        // BedExit -> Idle on completion
        var exitToIdle = bedExitState.AddTransition(idleState);
        exitToIdle.hasExitTime = true;
        exitToIdle.exitTime = 1.0f;
        exitToIdle.duration = 0f;

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[BedAnimatorSetup] Created Luon_Character_Animator.controller successfully!");
    }
}
