using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CaptureRoom404Comparison
{
    public static string Main()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != "Assets/dorm_room/Scenes/Room404_Blockout.unity") throw new Exception("Unexpected active scene");
        var go = GameObject.Find("GameplayCamera");
        if (go == null) throw new Exception("Missing GameplayCamera");
        var camera = go.GetComponent<Camera>();
        if (camera == null) throw new Exception("Missing Camera component");
        var previousTarget = camera.targetTexture;
        var previousActive = RenderTexture.active;
        var previousRect = camera.rect;
        var previousAspect = camera.aspect;
        var output = new RenderTexture(768, 512, 24, RenderTextureFormat.ARGB32);
        Texture2D result = null;
        try
        {
            camera.rect = new Rect(0, 0, 1, 1);
            camera.aspect = 3f / 2f;
            camera.targetTexture = output;
            camera.Render();
            RenderTexture.active = output;
            result = new Texture2D(768, 512, TextureFormat.RGB24, false);
            result.ReadPixels(new Rect(0, 0, 768, 512), 0, 0);
            result.Apply();
            const string path = "Assets/Temp/room404-prototype-comparison.png";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, result.EncodeToPNG());
            return path + " — 768x512, camera aspect 3:2, no UI; state restored.";
        }
        finally
        {
            camera.targetTexture = previousTarget;
            camera.rect = previousRect;
            camera.aspect = previousAspect;
            RenderTexture.active = previousActive;
            output.Release();
            UnityEngine.Object.DestroyImmediate(output);
            if (result != null) UnityEngine.Object.DestroyImmediate(result);
        }
    }
}
