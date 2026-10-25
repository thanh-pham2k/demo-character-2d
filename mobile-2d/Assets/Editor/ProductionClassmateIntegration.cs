using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using TMPro;

// Small migration of the authored H6 scene; keeps serialized object references intact.
[InitializeOnLoad]
public static class ProductionClassmateIntegration
{
    private const string SpritePath="Assets/Sprites/MVP/remaining/C00/npc_h6_classmate_writing.png";
    private static string Request=>Path.Combine(Application.dataPath,"../Temp/ProductionClassmate.run");
    private static double allowedAfter;
    static ProductionClassmateIntegration()
    {
        allowedAfter=EditorApplication.timeSinceStartup+3;
        EditorApplication.playModeStateChanged+=_=>allowedAfter=EditorApplication.timeSinceStartup+3;
        EditorApplication.update+=Poll;
    }
    public static void Configure(Transform classroom)
    {
        var sprite=AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath);
        if(sprite==null)throw new InvalidOperationException("Import C00 classmate before H6 setup.");
        // Historical object names remain to preserve existing scene/test references.
        var student=classroom.Find("Dam_Writing").GetComponent<SpriteRenderer>();
        student.sprite=sprite;
        student.transform.localScale=Vector3.one*.55f;
        student.transform.localPosition=new Vector3(1,-.15f,0);
        var animation=classroom.GetComponent<ClassroomNpcAnimation>();
        if(animation!=null)animation.ConfigureClassmate(sprite);
        var label=classroom.Find("Seat_Label").GetComponent<TMP_Text>();
        label.text="[E] Ngồi cạnh bạn cùng lớp";
    }
    private static void Poll()
    {
        if(!File.Exists(Request)||EditorApplication.timeSinceStartup<allowedAfter||EditorApplication.isCompiling||EditorApplication.isPlayingOrWillChangePlaymode)return;
        File.Delete(Request);
        string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/mvp_sprites/remaining/review/classmate-integration.txt"));
        try
        {
            var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if(scene.path!="Assets/Scenes/Room404_MVP.unity" || scene.isDirty)throw new InvalidOperationException("H6 migration requires a clean saved Room404_MVP scene.");
            Transform classroom=null;
            foreach(var root in scene.GetRootGameObjects())if(root.name=="H6_Classroom_Zone")classroom=root.transform;
            if(classroom==null)throw new InvalidOperationException("Missing H6 zone.");
            Configure(classroom);
            EditorSceneManager.MarkSceneDirty(scene);
            if(!EditorSceneManager.SaveScene(scene))throw new InvalidOperationException("Scene save failed.");
            AssetDatabase.SaveAssets();
            var renderer=classroom.Find("Dam_Writing").GetComponent<SpriteRenderer>();
            if(renderer.sprite!=AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath))throw new InvalidOperationException("Classmate sprite binding mismatch.");
            var anim=classroom.GetComponent<ClassroomNpcAnimation>();
            if(anim!=null)
            {
                var serialized=new SerializedObject(anim);
                if(serialized.FindProperty("writing").GetArrayElementAtIndex(0).objectReferenceValue!=renderer.sprite || serialized.FindProperty("surprised").objectReferenceValue!=renderer.sprite)
                    throw new InvalidOperationException("NPC animation would overwrite classmate with old identity.");
            }
            File.WriteAllText(output,"PASS "+DateTime.UtcNow.ToString("O")+"\nH6 C00 classmate bound in saved scene and NPC writer; prompt updated.\nScope: editor reference migration only; full classroom playthrough pending.\n");
        }
        catch(Exception ex){File.WriteAllText(output,"FAIL "+ex);Debug.LogException(ex);}
    }
}
