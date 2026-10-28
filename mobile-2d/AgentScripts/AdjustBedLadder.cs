using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class AdjustBedLadder
{
    static void Part(Transform parent,string name,Vector3 pos,Vector3 size,Material mat)
    {
        var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=name;o.layer=8;o.transform.SetParent(parent,false);o.transform.localPosition=pos;o.transform.localScale=size;o.GetComponent<Renderer>().sharedMaterial=mat;
    }
    public static string Main()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(scene.path!="Assets/dorm_room/Scenes/Room404_Blockout.unity" || scene.isDirty)throw new System.Exception("Unexpected or dirty scene");
        string path="Assets/dorm_room/Blockout/Bed.prefab";var root=PrefabUtility.LoadPrefabContents(path);var ladder=root.transform.Find("Ladder");
        while(ladder.childCount>0)Object.DestroyImmediate(ladder.GetChild(0).gameObject);
        var metal=AssetDatabase.LoadAssetAtPath<Material>("Assets/dorm_room/Blockout/Metal.mat");
        Part(ladder,"InnerRail",new Vector3(1.12f,1.36f,-.66f),new Vector3(.07f,2.72f,.07f),metal);
        Part(ladder,"OuterRail",new Vector3(1.50f,1.36f,-.66f),new Vector3(.07f,2.72f,.07f),metal);
        for(int i=0;i<7;i++)Part(ladder,"Rung",new Vector3(1.31f,.27f+i*.33f,-.66f),new Vector3(.38f,.06f,.07f),metal);
        root.transform.Find("ClimbPoint").localPosition=new Vector3(1.55f,0,-1.05f);
        root.transform.Find("ExitPoint").localPosition=new Vector3(1.55f,0,-1.05f);
        PrefabUtility.SaveAsPrefabAsset(root,path);PrefabUtility.UnloadPrefabContents(root);
        var l=GameObject.Find("LeftSet").transform;var r=GameObject.Find("RightSet").transform;
        l.localPosition=new Vector3(-4f,0,1f);r.localPosition=new Vector3(4f,0,1f);
        var rightLadder=r.Find("Bed/Ladder");rightLadder.localPosition=new Vector3(0,0,1.32f);
        foreach (var name in new[] { "ClimbPoint", "ExitPoint" }) r.Find("Bed/" + name).localPosition=new Vector3(1.55f,0,1.05f);
        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();return "Ladder rebuilt as visible front-facing geometry; set positions remain provisional.";
    }
}
