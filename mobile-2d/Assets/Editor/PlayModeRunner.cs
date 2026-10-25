using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PlayModeRunner
{
    private static string RunFile => Path.Combine(Application.dataPath, "../Temp/RunInteractionSuite.run");

    private static double allowedAfter;
    static PlayModeRunner()
    {
        allowedAfter=EditorApplication.timeSinceStartup+3;
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        if (EditorApplication.isCompiling || EditorApplication.isUpdating || EditorApplication.timeSinceStartup<allowedAfter)return;
        if (File.Exists(RunFile))
        {
            File.Delete(RunFile);
            PlayModeInteractionSuite.RunAllTests();
        }
    }
}
