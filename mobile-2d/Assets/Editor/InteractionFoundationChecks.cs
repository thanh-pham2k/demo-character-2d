using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

// Opt-in Play Mode checks. No menu entries and no scene/prefab writes.
// Create Temp/InteractionFoundationChecks.run and recompile this file to run.
[InitializeOnLoad]
public static class InteractionFoundationChecks
{
    private const string PhaseKey = "Room404.InteractionFoundationChecks";
    private static string RequestPath => Path.Combine(Application.dataPath, "../Temp/InteractionFoundationChecks.run");
    private static string ReportPath => Path.Combine(Application.dataPath, "../Temp/InteractionFoundationChecks/report.json");
    private static IEnumerator checks;
    private static double deadline;
    private static readonly List<string> passed = new List<string>();
    private static readonly List<string> errors = new List<string>();

    [Serializable]
    private class Report
    {
        public bool success;
        public string[] passed;
        public string[] errors;
    }

    private sealed class NullTransformTarget : IInteractable
    {
        public bool CanInteract => true;
        public string InteractionPrompt => "test";
        public Transform GetTransform() => null;
        public void Interact() { }
    }

    static InteractionFoundationChecks()
    {
        if (Application.isBatchMode) return;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
        if (File.Exists(RequestPath))
        {
            File.Delete(RequestPath);
            SessionState.SetString(PhaseKey, "prepare");
        }
        string phase = SessionState.GetString(PhaseKey, "");
        if (phase == "prepare") EditorApplication.delayCall += Prepare;
        else if (phase == "entering" && EditorApplication.isPlaying) EditorApplication.delayCall += StartChecks;
        else if (phase == "leaving" && !EditorApplication.isPlaying) SessionState.EraseString(PhaseKey);
    }

    private static void OnPlayModeChanged(PlayModeStateChange change)
    {
        string phase = SessionState.GetString(PhaseKey, "");
        if (change == PlayModeStateChange.EnteredEditMode && phase == "prepare") EditorApplication.delayCall += Prepare;
        if (change == PlayModeStateChange.EnteredPlayMode && phase == "entering") StartChecks();
        if (change == PlayModeStateChange.EnteredEditMode && phase == "leaving") SessionState.EraseString(PhaseKey);
    }

    private static void Prepare()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
            return;
        }
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (SceneManager.GetActiveScene().path != "Assets/Scenes/Room404_MVP.unity")
        {
            errors.Add("Open Room404_MVP before running these opt-in checks.");
            Finish();
            return;
        }
        SessionState.SetString(PhaseKey, "entering");
        EditorApplication.isPlaying = true;
    }

    private static void StartChecks()
    {
        if (SessionState.GetString(PhaseKey, "") != "entering") return;
        SessionState.SetString(PhaseKey, "running");
        Application.logMessageReceived += OnLog;
        deadline = EditorApplication.timeSinceStartup + 60;
        checks = RunChecks();
        EditorApplication.update += Tick;
    }

    private static void Tick()
    {
        try
        {
            if (!EditorApplication.isPlaying) throw new Exception("Play Mode exited before checks completed.");
            if (EditorApplication.timeSinceStartup > deadline) throw new TimeoutException("Interaction checks timed out.");
            if (!checks.MoveNext()) Finish();
        }
        catch (Exception exception)
        {
            errors.Add(exception.ToString());
            Finish();
        }
    }

    private static void OnLog(string message, string stackTrace, LogType type)
    {
        if (type == LogType.Exception || type == LogType.Error || type == LogType.Assert)
            errors.Add(message + "\n" + stackTrace);
    }

    private static void Finish()
    {
        EditorApplication.update -= Tick;
        Application.logMessageReceived -= OnLog;
        Directory.CreateDirectory(Path.GetDirectoryName(ReportPath));
        File.WriteAllText(ReportPath, JsonUtility.ToJson(new Report
        {
            success = errors.Count == 0, passed = passed.ToArray(), errors = errors.ToArray()
        }, true));
        Debug.Log($"Interaction foundation checks: {passed.Count} passed, {errors.Count} errors. Report: {ReportPath}");
        SessionState.SetString(PhaseKey, "leaving");
        if (EditorApplication.isPlaying) EditorApplication.isPlaying = false;
        else SessionState.EraseString(PhaseKey);
    }

    private static void Check(bool condition, string description)
    {
        if (!condition) throw new Exception(description);
        passed.Add(description);
    }

    private static void Place(GameObject player, Vector3 position)
    {
        var body = player.GetComponent<Rigidbody2D>();
        body.linearVelocity = Vector2.zero;
        body.position = position;
        player.transform.position = position;
        Physics2D.SyncTransforms();
    }

    private static InteractableProp CreateProp(string name, Vector3 position)
    {
        var go = new GameObject(name);
        go.transform.position = position;
        go.AddComponent<CircleCollider2D>().isTrigger = true;
        return go.AddComponent<InteractableProp>();
    }

    private static IEnumerator RunChecks()
    {
        for (int frame = 0; frame < 3; frame++) yield return null;
        var flow = Object.FindFirstObjectByType<MVPFlowController>();
        Check(flow != null && flow.playerObj != null && flow.playerVisual != null, "Existing player and story references resolve");

        var player = new GameObject("InteractionFoundationCheckPlayer");
        Vector3 origin = new Vector3(1000f, 1000f, 0f);
        player.transform.position = origin;
        var body = player.AddComponent<Rigidbody2D>();
        body.gravityScale = 0;
        var shadow = new GameObject("Shadow");
        shadow.transform.SetParent(player.transform, false);
        var shadowRenderer = shadow.AddComponent<SpriteRenderer>();
        var visualObject = new GameObject("Visual");
        visualObject.transform.SetParent(player.transform, false);
        var renderer = visualObject.AddComponent<SpriteRenderer>();
        var visual = visualObject.AddComponent<LuonCharacterVisual>();
        visual.idleSprite = flow.playerVisual.idleSprite;
        visual.sitLaptopSprite = flow.playerVisual.sitLaptopSprite;
        visual.walkSprites = flow.playerVisual.walkSprites;
        var movement = player.AddComponent<PlayerMovement>();
        var detector = player.AddComponent<PlayerInteractionDetector>();
        var state = player.GetComponent<PlayerInteractionStateController>();
        Check(state != null && state.CanInteract && state.CurrentState == PlayerInteractionState.Standing,
            "Default state is Standing and detector obtains its controller without a saved scene change");

        movement.SetSimulatedInput(Vector2.left);
        float until = Time.time + 0.15f;
        while (Time.time < until) yield return null;
        Check(body.position.x < origin.x - 0.1f && body.linearVelocity.x < 0,
            "Standing movement still runs through the real Update/FixedUpdate path");
        Check(renderer.flipX && !shadowRenderer.flipX, "Movement binds the character renderer instead of the earlier shadow renderer");
        Vector2 standingVelocity = body.linearVelocity;
        state.ExitInteraction();
        Check(body.linearVelocity == standingVelocity, "Exiting an already Standing player does not interrupt normal movement");
        movement.TryDodge();
        Check(movement.IsDodging, "Standing dodge still starts");
        movement.SetInteractionMovementLocked(true);
        Check(!movement.IsDodging && body.linearVelocity == Vector2.zero, "Interaction movement lock cancels velocity and dodge");
        movement.SetInteractionMovementLocked(false);
        Place(player, origin);

        var near = CreateProp("CheckNear", origin + Vector3.right * 0.6f);
        var far = CreateProp("CheckFar", origin + Vector3.right * 1.2f);
        int nearCount = 0, farCount = 0;
        near.onInteract.AddListener(() => nearCount++);
        far.onInteract.AddListener(() => farCount++);
        Physics2D.SyncTransforms();
        Check(detector.FindClosestInteractable() == (IInteractable)near && detector.TryInteract() && nearCount == 1,
            "Legacy zero layer mask selects and dispatches the nearest available prop once");
        near.SetInteractable(false);
        Check(detector.FindClosestInteractable() == (IInteractable)far, "Unavailable nearest target is skipped through the common contract");
        near.SetInteractable(true);
        near.enabled = false;
        near.Interact();
        Check(nearCount == 1 && detector.FindClosestInteractable() == (IInteractable)far,
            "Disabled behaviour cannot be selected or invoked directly");
        near.enabled = true;
        far.gameObject.layer = 2;
        typeof(PlayerInteractionDetector).GetField("interactableLayer", BindingFlags.Instance | BindingFlags.NonPublic)
            .SetValue(detector, (LayerMask)(1 << 2));
        Check(detector.FindClosestInteractable() == (IInteractable)far, "Explicit layer mask includes a target on Ignore Raycast when requested");
        typeof(PlayerInteractionDetector).GetField("interactableLayer", BindingFlags.Instance | BindingFlags.NonPublic)
            .SetValue(detector, (LayerMask)0);
        far.gameObject.layer = 0;
        var child = new GameObject("ChildCollider");
        child.transform.SetParent(near.transform, false);
        child.AddComponent<CircleCollider2D>().isTrigger = true;
        near.GetComponent<Collider2D>().enabled = false;
        Physics2D.SyncTransforms();
        Check(detector.FindClosestInteractable() == (IInteractable)near, "Interaction resolves a component on the collider parent");
        near.gameObject.SetActive(false);
        Check(detector.FindClosestInteractable() == (IInteractable)far, "Inactive target is ignored");
        near.gameObject.SetActive(true);

        var marker = new GameObject("CheckPoseMarker").transform;
        marker.position = origin + Vector3.up * 0.2f;
        Check(state.TryEnterState(PlayerInteractionState.Sitting, marker, near), "Sitting can be entered with an owner and pose marker");
        Check(!state.TryEnterState(PlayerInteractionState.Lying) && state.CurrentState == PlayerInteractionState.Sitting,
            "Sitting and Lying cannot overlap");
        movement.SetSimulatedInput(Vector2.right);
        movement.TryDodge();
        until = Time.time + 0.1f;
        while (Time.time < until) yield return null;
        Check(!movement.IsDodging && movement.IsInteractionMovementLocked && !body.simulated &&
            Vector3.Distance(player.transform.position, marker.position) < 0.001f,
            "Seated movement, dodge and physics drift are locked at the marker");
        Check(renderer.sprite == (visual.sitLaptopSprite != null ? visual.sitLaptopSprite : visual.idleSprite),
            "Sitting uses the existing visual sprite safely");
        near.onInteract.AddListener(() => { if (state.CurrentState != PlayerInteractionState.Standing) state.ExitInteraction(); });
        child.GetComponent<Collider2D>().enabled = false;
        Check(detector.FindClosestInteractable() == (IInteractable)near,
            "The active owner can receive exit interaction even if its occupied collider is disabled");
        Check(detector.TryInteract() && state.CurrentState == PlayerInteractionState.Standing && body.simulated &&
            !movement.IsInteractionMovementLocked && Vector3.Distance(player.transform.position, origin) < 0.001f,
            "Only the active owner receives interaction while posed, then exit restores Standing and entry position");
        Check(state.TryEnterState(PlayerInteractionState.Lying) && !detector.TryInteract(),
            "Lying locks ordinary world interaction without implementing a bed");
        state.ExitInteraction();
        visual.SetSitting(true);
        Check(state.CurrentState == PlayerInteractionState.Sitting && movement.IsInteractionMovementLocked,
            "Legacy SetSitting(true) delegates to the player state");
        visual.SetSitting(false);
        Check(state.CurrentState == PlayerInteractionState.Standing && renderer.sprite == visual.idleSprite,
            "Legacy SetSitting(false) restores Standing and idle visual");

        state.TryEnterState(PlayerInteractionState.Sitting, null, near);
        near.enabled = false;
        until = Time.time + 0.05f;
        while (Time.time < until) yield return null;
        Check(state.CurrentState == PlayerInteractionState.Standing && !movement.IsInteractionMovementLocked,
            "Disabled interaction owner automatically releases the pose");
        near.enabled = true;
        var destroyedOwner = CreateProp("DestroyedOwner", origin);
        IInteractable destroyedReference = destroyedOwner;
        state.TryEnterState(PlayerInteractionState.Lying, null, destroyedOwner);
        Object.Destroy(destroyedOwner.gameObject);
        until = Time.time + 0.05f;
        while (Time.time < until) yield return null;
        Check(!InteractionTargetUtility.IsAvailable(destroyedReference) && state.CurrentState == PlayerInteractionState.Standing,
            "Destroyed interface target uses Unity null semantics and releases the pose");
        state.TryEnterState(PlayerInteractionState.Lying);
        state.enabled = false;
        Check(state.CurrentState == PlayerInteractionState.Standing && !movement.IsInteractionMovementLocked && body.simulated,
            "Disabling the controller cannot leave movement or physics locked");
        state.enabled = true;
        body.simulated = false;
        state.TryEnterState(PlayerInteractionState.Lying);
        state.ExitInteraction();
        Check(!body.simulated, "Exit preserves a pre-existing disabled physics simulation state");
        body.simulated = true;
        state.TryEnterState(PlayerInteractionState.Lying);
        movement.enabled = false;
        state.ExitInteraction();
        Check(!movement.enabled, "Exit does not enable an externally disabled movement component");
        movement.enabled = true;
        movement.SetSimulatedInput(Vector2.right);
        until = Time.time + 0.1f;
        while (Time.time < until) yield return null;
        Check(body.linearVelocity.x > 0f, "Movement resumes normally after leaving interaction");
        movement.SetInteractionMovementLocked(true);
        movement.SetInteractionMovementLocked(false);
        Place(player, origin);

        Check(!InteractionTargetUtility.IsAvailable(null) && !InteractionTargetUtility.IsAvailable(new NullTransformTarget()),
            "Null target and null transform are rejected without exceptions");
        var sparse = new GameObject("SparsePlayer");
        sparse.AddComponent<PlayerMovement>();
        sparse.GetComponent<Rigidbody2D>().gravityScale = 0;
        var sparseState = PlayerInteractionStateController.GetOrCreate(sparse);
        Check(sparseState.TryEnterState(PlayerInteractionState.Lying), "Player without optional visuals supports state and movement lock");
        sparseState.ExitInteraction();
        var rendererless = new GameObject("RendererlessVisual").AddComponent<LuonCharacterVisual>();
        rendererless.SetSitting(true);
        rendererless.SetSitting(false);
        Check(true, "Visual compatibility calls tolerate missing renderer, rigidbody and sprites");

        near.SetInteractable(false);
        far.SetInteractable(false);
        var doorObject = new GameObject("ProbeDoor");
        doorObject.transform.position = origin + Vector3.right;
        var door = doorObject.AddComponent<HingedRoomDoor>();
        door.Interact();
        Check(!door.CanInteract, "Unconfigured door is unavailable and null-safe");
        var leaf = new GameObject("Leaf");
        leaf.transform.SetParent(doorObject.transform, false);
        var leafRenderer = leaf.AddComponent<SpriteRenderer>();
        leafRenderer.sprite = visual.idleSprite;
        var doorCollider = doorObject.AddComponent<BoxCollider2D>();
        doorCollider.isTrigger = true;
        door.Configure(leafRenderer, doorCollider, 1.28f);
        Physics2D.SyncTransforms();
        Check(detector.TryInteract(), "Door dispatch uses the same IInteractable path as props");
        door.Advance(1f);
        Check(door.CurrentAngle == 0f, "Existing door close behaviour is preserved");
        Check(detector.TryInteract(), "Door remains interactable after closing");
        door.Advance(1f);
        Check(Mathf.Abs(door.CurrentAngle - 70f) < 0.01f, "Existing door open behaviour is preserved");

        var sceneDetector = flow.playerObj.GetComponent<PlayerInteractionDetector>();
        var sceneState = flow.playerObj.GetComponent<PlayerInteractionStateController>();
        Check(sceneState != null && sceneState.CanInteract, "Existing serialized player acquires the controller at runtime");
        Place(flow.playerObj, flow.bedInteractable.transform.position + new Vector3(0.9f, -0.5f, 0f));
        Check(sceneDetector.TryInteract() && !flow.bedInteractable.CanInteract && flow.trashInteractable.CanInteract &&
            flow.backpackProp.activeSelf, "Existing bed-claim story event still dispatches through the detector");
        Place(flow.playerObj, new Vector3(0f, -1f, 0f));
        for (int index = 0; index < 3; index++) Check(sceneDetector.TryInteract(), "Existing trash interaction " + (index + 1));
        Check(!flow.trashPileProp.activeSelf && !flow.trashInteractable.CanInteract, "Existing cleanup story completion still runs");
        flow.StopAllCoroutines();
        flow.currentStage = MVPFlowController.Stage.Stage3_Year5_Awakening;
        Vector3 beforeStorySeat = flow.playerObj.transform.position;
        flow.OnInteractLaptopAwakening();
        until = Time.time + 0.1f;
        while (Time.time < until) yield return null;
        Check(sceneState.CurrentState == PlayerInteractionState.Sitting &&
            Vector3.Distance(flow.playerObj.transform.position, flow.deskSeatTransform.position) < 0.001f,
            "Story seating delegates to the foundation and stays stable inside furniture colliders");
        typeof(MVPFlowController).GetMethod("OnUninstallCompleted", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(flow, null);
        Check(sceneState.CurrentState == PlayerInteractionState.Standing && sceneState.CanInteract &&
            Vector3.Distance(flow.playerObj.transform.position, beforeStorySeat) < 0.001f,
            "Story completion restores Standing, movement and the entry position");
        flow.StopAllCoroutines();
        Check(true, "Prompt UI is unassigned in the existing scene and detector runs without null-reference errors");
    }
}
