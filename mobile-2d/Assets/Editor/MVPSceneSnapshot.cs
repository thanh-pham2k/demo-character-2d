using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class MVPSceneSnapshot
{
    public static void CaptureSnapshot()
    {
        string scenePath = "Assets/Scenes/Room404_MVP.unity";
        EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("No main camera found!");
            return;
        }

        string outPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../..", "mvp_scene_snapshot.png"));
        SaveCameraFrame(cam, outPath);
        Debug.Log("Snapshot saved to: " + outPath);
    }

    public static void CaptureFanRotationReview()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/Room404_MVP.unity", OpenSceneMode.Single);
        Camera cam = Camera.main;
        GameObject fan = GameObject.Find("Ceiling_Fan");
        Transform blades = fan != null ? fan.transform.Find("Blades_Rotor") : null;
        if (cam == null || blades == null)
            throw new System.InvalidOperationException("The scene needs its camera and ceiling fan.");

        string output = Path.GetFullPath(Path.Combine(Application.dataPath, "../Temp/FanRotationReview"));
        Directory.CreateDirectory(output);
        Quaternion initialRotation = blades.localRotation;
        try
        {
            // One full revolution, using the same tilted local rotation axis as the live animation.
            for (int frame = 0; frame < 24; frame++)
            {
                blades.localRotation = initialRotation * Quaternion.AngleAxis(-frame * 15f, Vector3.forward);
                SaveCameraFrame(cam, Path.Combine(output, $"frame-{frame:D2}.png"));
            }
        }
        finally
        {
            blades.localRotation = initialRotation;
        }
        Debug.Log("Fan rotation review frames saved to: " + output);
    }

    public static void CaptureDoorSwingReview()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            throw new System.InvalidOperationException("Stop Play Mode before capturing the door review.");
        EditorSceneManager.OpenScene("Assets/Scenes/Room404_MVP.unity", OpenSceneMode.Single);
        Camera cam = Camera.main;
        HingedRoomDoor door = Object.FindFirstObjectByType<HingedRoomDoor>();
        if (cam == null || door == null)
            throw new System.InvalidOperationException("The scene needs its camera and hinged door.");

        Physics2D.SyncTransforms();
        bool detected = false;
        foreach (Collider2D hit in Physics2D.OverlapCircleAll(door.transform.position + Vector3.down * 0.5f, 2f))
            if (hit.GetComponent<IInteractable>() == (IInteractable)door) detected = true;
        if (!detected) throw new System.InvalidOperationException("The door cannot be reached by the interaction detector.");

        string output = Path.GetFullPath(Path.Combine(Application.dataPath, "../Temp/DoorSwingReview"));
        Directory.CreateDirectory(output);
        float initialAngle = door.CurrentAngle;
        Vector3 hingePosition = door.transform.position;
        try
        {
            door.SetPreviewAngle(0f);
            door.Interact();
            for (int frame = 0; frame < 16; frame++)
            {
                SaveCameraFrame(cam, Path.Combine(output, $"frame-{frame:D2}.png"));
                door.Advance(0.05f);
                if (door.transform.position != hingePosition)
                    throw new System.InvalidOperationException("The door hinge moved during its swing.");
            }
            if (Mathf.Abs(door.CurrentAngle - 70f) > 0.01f)
                throw new System.InvalidOperationException("The door did not open fully.");
            door.Interact();
            for (int frame = 16; frame < 32; frame++)
            {
                SaveCameraFrame(cam, Path.Combine(output, $"frame-{frame:D2}.png"));
                door.Advance(0.05f);
            }
            if (door.CurrentAngle > 0.01f)
                throw new System.InvalidOperationException("The door did not close.");
        }
        finally
        {
            door.SetPreviewAngle(initialAngle);
        }
        Debug.Log("Door review passed: reachable interaction, fixed hinge, opens to 70 degrees and closes. Frames: " + output);
    }

    public static void SaveCameraFrame(Camera cam, string output, bool includeOverlay = false)
    {
        const int width = 1280;
        int height = includeOverlay ? Mathf.Max(1, Mathf.RoundToInt(width / cam.aspect)) : 720;
        RenderTexture previousTarget = cam.targetTexture;
        RenderTexture previousActive = RenderTexture.active;
        var overlays = new System.Collections.Generic.List<Canvas>();
        if (includeOverlay)
            foreach (var canvas in Object.FindObjectsByType<Canvas>())
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) overlays.Add(canvas);
        var rt = new RenderTexture(width, height, 24);
        var frame = new Texture2D(width, height, TextureFormat.RGB24, false);
        try
        {
            cam.targetTexture = rt;
            foreach (var canvas in overlays)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = cam;
                canvas.planeDistance = 1f;
            }
            Canvas.ForceUpdateCanvases();
            cam.Render();
            RenderTexture.active = rt;
            frame.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            frame.Apply();
            File.WriteAllBytes(output, frame.EncodeToPNG());
        }
        finally
        {
            foreach (var canvas in overlays) { canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.worldCamera = null; }
            cam.targetTexture = previousTarget;
            Canvas.ForceUpdateCanvases();
            RenderTexture.active = previousActive;
            Object.DestroyImmediate(frame);
            Object.DestroyImmediate(rt);
        }
    }
}
