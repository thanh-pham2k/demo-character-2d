using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public static class RefineRoomShell
{
    static GameObject Box(Transform parent, string name, Vector3 position, Vector3 size, Material material, bool collider)
    {
        var obj=GameObject.CreatePrimitive(PrimitiveType.Cube);obj.name=name;obj.layer=8;obj.transform.SetParent(parent,false);obj.transform.localPosition=position;obj.transform.localScale=size;obj.GetComponent<Renderer>().sharedMaterial=material;
        if(!collider) Object.DestroyImmediate(obj.GetComponent<Collider>());
        return obj;
    }
    public static string Main()
    {
        var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if(scene.path!="Assets/dorm_room/Scenes/Room404_Blockout.unity") throw new System.Exception("Unexpected scene");
        var room=GameObject.Find("Room404_Blockout");var old=room.transform.Find("BackWall");if(old==null || room.transform.Find("BackWall_Left")!=null)throw new System.Exception("Expected unmodified wall");
        var wall=AssetDatabase.LoadAssetAtPath<Material>("Assets/dorm_room/Blockout/Wall.mat");var trim=AssetDatabase.LoadAssetAtPath<Material>("Assets/dorm_room/Blockout/Trim.mat");
        Object.DestroyImmediate(old.gameObject);
        Box(room.transform,"BackWall_Left",new Vector3(-2.385f,1.8f,3.4f),new Vector3(6.23f,3.6f,.1f),wall,true);
        Box(room.transform,"BackWall_Right",new Vector3(3.915f,1.8f,3.4f),new Vector3(3.17f,3.6f,.1f),wall,true);
        Box(room.transform,"BackWall_AboveDoor",new Vector3(1.54f,2.88f,3.4f),new Vector3(1.65f,1.44f,.1f),wall,true);
        var window=room.transform.Find("WindowPlaceholder");
        foreach(var e in new[]{new Vector3(-.5f,0,0),new Vector3(.5f,0,0)}) Box(window,"VerticalFrame",e,new Vector3(.04f,1.05f,1.3f),trim,false);
        foreach(var e in new[]{new Vector3(0,-.51f,0),new Vector3(0,.51f,0)}) Box(window,"HorizontalFrame",e,new Vector3(1.1f,.06f,1.3f),trim,false);
        Box(window,"CenterFrame",Vector3.zero,new Vector3(.035f,1f,1.3f),trim,false);
        Box(room.transform,"LightSwitchPlaceholder",new Vector3(.6f,1.4f,3.31f),new Vector3(.15f,.25f,.04f),trim,false);
        Box(room.transform,"DoorFrameLeft",new Vector3(.72f,1.09f,3.31f),new Vector3(.09f,2.18f,.12f),trim,true);
        Box(room.transform,"DoorFrameRight",new Vector3(2.36f,1.09f,3.31f),new Vector3(.09f,2.18f,.12f),trim,true);
        Box(room.transform,"DoorFrameTop",new Vector3(1.54f,2.20f,3.31f),new Vector3(1.73f,.08f,.12f),trim,true);
        Box(room.transform,"CeilingTrimBlockout",new Vector3(0,2.68f,3.27f),new Vector3(11,.32f,.1f),trim,false);
        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);return "Split doorway wall and added window/door/switch blockouts.";
    }
}
