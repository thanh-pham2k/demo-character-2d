using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public static class PlayModeInteractionSuite
{
    private static readonly List<string> passed = new List<string>();
    private static readonly List<string> errors = new List<string>();

    [Serializable]
    public class SuiteReport
    {
        public bool success;
        public string[] passed;
        public string[] errors;
    }

    public static void RunAllTests()
    {
        try
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Room404_MVP.unity", OpenSceneMode.Single);
            
            // 1. Resolve objects
            var player = GameObject.Find("Player_Luon");
            if (player == null) throw new Exception("Player_Luon not found!");
            var movement = player.GetComponent<PlayerMovement>();
            var detector = player.GetComponent<PlayerInteractionDetector>();
            var state = player.GetComponent<PlayerInteractionStateController>();
            var visual = player.GetComponentInChildren<LuonCharacterVisual>();
            var playerSr = visual.GetComponent<SpriteRenderer>();

            var door = Object.FindFirstObjectByType<HingedRoomDoor>();
            var desk = GameObject.Find("Study_Desk");
            var chairLeft = desk.transform.Find("Chair_Left")?.GetComponent<ChairInteractable>();
            var chairRight = desk.transform.Find("Chair_Right")?.GetComponent<ChairInteractable>();
            
            var bedLeftRoot = GameObject.Find("BunkBed_Left");
            var bedLeft = bedLeftRoot.GetComponentInChildren<BedInteractable>();
            var bedRightRoot = GameObject.Find("BunkBed_Right");
            var bedRight = bedRightRoot.GetComponentInChildren<BedInteractable>();

            Assert(movement != null, "PlayerMovement exists");
            Assert(detector != null, "PlayerInteractionDetector exists");
            Assert(state != null, "PlayerInteractionStateController exists");
            Assert(visual != null, "LuonCharacterVisual exists");
            Assert(visual.bedEnterSprites != null && visual.bedEnterSprites.Length == 5, "BedEnter sprites assigned");
            Assert(visual.bedIdleSprites != null && visual.bedIdleSprites.Length == 4, "BedIdle sprites assigned");
            Assert(visual.bedSleepSprite != null, "BedSleep sprite assigned");
            Assert(visual.bedExitSprites != null && visual.bedExitSprites.Length == 5, "BedExit sprites assigned");

            Assert(door != null, "HingedRoomDoor exists");
            Assert(chairLeft != null && chairRight != null, "Both ChairInteractables exist");
            Assert(bedLeft != null && bedRight != null, "Both BedInteractables exist");

            // 2. Test initial Standing state
            Assert(state.CurrentState == PlayerInteractionState.Standing, "Initial state is Standing");
            Assert(!movement.IsInteractionMovementLocked, "Initial movement is unlocked");
            Assert(playerSr.sprite == visual.idleSprite, "Initial visual sprite is idle");

            // 3. Test Door interaction
            door.Interact();
            door.Advance(1.0f);
            Assert(door.CurrentAngle > 50f, "Door opened on interact");
            door.Interact();
            door.Advance(1.0f);
            Assert(door.CurrentAngle < 1f, "Door closed on interact");

            // 4. Test Chair interaction: Sit -> Locked -> Stand -> Unlocked
            chairLeft.Interact(state);
            Assert(state.CurrentState == PlayerInteractionState.Sitting, "State entered Sitting");
            Assert(movement.IsInteractionMovementLocked, "Movement locked while Sitting");
            Assert(playerSr.sprite == visual.sitLaptopSprite, "Visual sprite is sitting sprite");
            Assert(Vector3.Distance(player.transform.position, chairLeft.SeatPivot.position) < 0.01f, "Player snapped to Chair SeatPivot");

            // Cannot walk while sitting
            movement.SetSimulatedInput(Vector2.right);
            Assert(movement.IsInteractionMovementLocked, "Movement cannot be simulated while Sitting");

            // Cannot lie on bed while sitting
            bedRight.Interact(state);
            Assert(state.CurrentState == PlayerInteractionState.Sitting, "Cannot enter Bed while Sitting");

            // Stand up
            chairLeft.Interact(state);
            Assert(state.CurrentState == PlayerInteractionState.Standing, "State returned to Standing after leaving Chair");
            Assert(!movement.IsInteractionMovementLocked, "Movement unlocked after standing");
            Assert(playerSr.sprite == visual.idleSprite, "Visual sprite reset to idle");
            Assert(Vector3.Distance(player.transform.position, chairLeft.ExitPivot.position) < 0.01f, "Player snapped to Chair ExitPivot");

            // 5. Test Bed interaction: Enter routine -> Transitioning -> Lying -> Exit routine -> Standing
            bedRight.Interact(state);
            // In synchronous test execution, start routine enters Transitioning / Lying
            Assert(state.CurrentState == PlayerInteractionState.Transitioning || state.CurrentState == PlayerInteractionState.Lying,
                "State entered Transitioning/Lying on BedRight");
            Assert(movement.IsInteractionMovementLocked, "Movement locked while on Bed");

            // Simulate coroutine completion to Lying
            state.TryEnterState(PlayerInteractionState.Lying, bedRight.BedPivot, bedRight);
            Assert(state.CurrentState == PlayerInteractionState.Lying, "State confirmed Lying");
            Assert(playerSr.sprite == visual.lyingSprite || playerSr.sprite == visual.bedEnterSprites[4], "Visual sprite is bed lying sprite");
            Assert(Vector3.Distance(player.transform.position, bedRight.BedPivot.position) < 0.01f, "Player snapped to Bed BedPivot");

            // Cannot sit on chair while lying
            chairLeft.Interact(state);
            Assert(state.CurrentState == PlayerInteractionState.Lying, "Cannot enter Chair while Lying");

            // Test Sleeping toggle
            visual.SetSleeping(true);
            Assert(state.CurrentState == PlayerInteractionState.Sleeping, "Entered Sleeping state");
            Assert(playerSr.sprite == visual.bedSleepSprite, "Sprite switched to sleeping sprite");
            visual.SetSleeping(false);
            Assert(state.CurrentState == PlayerInteractionState.Lying, "Returned to Lying state from Sleeping");

            // Get up from bed
            state.ExitInteraction(bedRight.ExitPivot);
            Assert(state.CurrentState == PlayerInteractionState.Standing, "State returned to Standing after leaving Bed");
            Assert(!movement.IsInteractionMovementLocked, "Movement unlocked after leaving Bed");
            Assert(playerSr.sprite == visual.idleSprite, "Visual sprite reset to idle");
            Assert(Vector3.Distance(player.transform.position, bedRight.ExitPivot.position) < 0.01f, "Player snapped to Bed ExitPivot");

            // 6. Test BedLeft after prerequisite satisfied
            var bedLeftProp = bedLeftRoot.GetComponent<InteractableProp>();
            if (bedLeftProp != null) bedLeftProp.SetInteractable(false);
            bedLeft.Interact(state);
            Assert(state.CurrentState == PlayerInteractionState.Transitioning || state.CurrentState == PlayerInteractionState.Lying,
                "State entered BedLeft");
            state.ExitInteraction(bedLeft.ExitPivot);
            Assert(state.CurrentState == PlayerInteractionState.Standing, "State exited BedLeft");

            // 7. Write Report
            SuiteReport report = new SuiteReport
            {
                success = errors.Count == 0,
                passed = passed.ToArray(),
                errors = errors.ToArray()
            };
            string dir = Path.Combine(Application.dataPath, "../PlayModeInteractionReports");
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, "report.json"), JsonUtility.ToJson(report, true));
            Debug.Log($"[PlayModeInteractionSuite] COMPLETED: {passed.Count} passed, {errors.Count} errors.");
        }
        catch (Exception ex)
        {
            errors.Add(ex.ToString());
            Debug.LogError("[PlayModeInteractionSuite] FAILED: " + ex);
            SuiteReport report = new SuiteReport
            {
                success = false,
                passed = passed.ToArray(),
                errors = errors.ToArray()
            };
            string dir = Path.Combine(Application.dataPath, "../PlayModeInteractionReports");
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, "report.json"), JsonUtility.ToJson(report, true));
        }
    }

    private static void Assert(bool condition, string message)
    {
        if (condition) passed.Add(message);
        else
        {
            errors.Add("FAIL: " + message);
            throw new Exception("Assertion failed: " + message);
        }
    }
}
