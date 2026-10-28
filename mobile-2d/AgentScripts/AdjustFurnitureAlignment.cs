using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public static class AdjustFurnitureAlignment
{
    public static string Main()
    {
        var scene=SceneManager.GetActiveScene();if(scene.path!="Assets/dorm_room/Scenes/Room404_Blockout.unity" || scene.isDirty)throw new System.Exception("Unexpected or dirty scene");
        var bedPath="Assets/dorm_room/Blockout/Bed.prefab";var bed=PrefabUtility.LoadPrefabContents(bedPath);
        var ladder=bed.transform.Find("Ladder");foreach(Transform part in ladder){var p=part.localPosition;p.x-=.25f;part.localPosition=p;}
        foreach(var name in new[]{"ClimbPoint","ExitPoint"}){var a=bed.transform.Find(name);var p=a.localPosition;p.x-=.25f;a.localPosition=p;}
        PrefabUtility.SaveAsPrefabAsset(bed,bedPath);PrefabUtility.UnloadPrefabContents(bed);
        var deskPath="Assets/dorm_room/Blockout/Desk.prefab";var desk=PrefabUtility.LoadPrefabContents(deskPath);
        var top=desk.transform.Find("Top");var ts=top.localScale;ts.x=1.9f;top.localScale=ts;
        foreach(Transform child in desk.transform)if(child.name=="Leg"){var p=child.localPosition;p.x=Mathf.Sign(p.x)*.83f;child.localPosition=p;}
        PrefabUtility.SaveAsPrefabAsset(desk,deskPath);PrefabUtility.UnloadPrefabContents(desk);
        var chairPath="Assets/dorm_room/Blockout/Chair.prefab";var chair=PrefabUtility.LoadPrefabContents(chairPath);
        foreach(Transform child in chair.transform){if(child.name=="Seat"||child.name=="Back"){var s=child.localScale;s.x=.65f;child.localScale=s;}else if(child.name=="Leg"){var p=child.localPosition;p.x=Mathf.Sign(p.x)*.27f;child.localPosition=p;}}
        PrefabUtility.SaveAsPrefabAsset(chair,chairPath);PrefabUtility.UnloadPrefabContents(chair);
        var room=GameObject.Find("Room404_Blockout").transform;
        foreach(var side in new[]{"LeftSet","RightSet"}){var set=room.Find(side);float offset=side=="LeftSet"?-.2f:.2f;foreach(var name in new[]{"Desk","Chair"}){var t=set.Find(name);var p=t.localPosition;p.x=offset;t.localPosition=p;}}
        var rightBed=room.Find("RightSet/Bed");foreach(var name in new[]{"ClimbPoint","ExitPoint"}){var a=rightBed.Find(name);a.localPosition=new Vector3(1.3f,0,1.05f);}
        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();return "Adjusted shared furniture geometry and per-side placement; requires visual and collision validation.";
    }
}
