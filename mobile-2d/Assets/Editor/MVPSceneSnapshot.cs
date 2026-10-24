using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class MVPSceneSnapshot
{
    [MenuItem("KTX404/Capture MVP Scene Snapshot")]
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

        int width = 1280;
        int height = 720;
        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
        cam.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();
        cam.targetTexture = null;
        RenderTexture.active = null;
        Object.DestroyImmediate(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        string outPath = "e:/demo-character-2d/mvp_scene_snapshot.png";
        File.WriteAllBytes(outPath, bytes);
        Debug.Log("Snapshot saved to: " + outPath);
    }
}
