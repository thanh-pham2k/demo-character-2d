using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PlayModeRunner
{
    private static string RunFile => Path.Combine(Application.dataPath, "../Temp/RunInteractionSuite.run");

    static PlayModeRunner()
    {
        EditorApplication.update += Update;
    }

    private static void Update()
    {
        if (File.Exists(RunFile))
        {
            File.Delete(RunFile);
            PlayModeInteractionSuite.RunAllTests();
        }
    }
}
