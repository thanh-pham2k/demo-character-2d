using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

public static class BuildRoom404Blockout
{
    const string Folder = "Assets/dorm_room/Blockout";
    static Material Mat(string name, Color color)
    {
        string path = Folder + "/" + name + ".mat";
        var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null) return existing;
        Shader shader = Shader.Find("Standard");
        var material = new Material(shader); material.color = color;
        AssetDatabase.CreateAsset(material, path); return material;
    }
    static GameObject Box(Transform parent, string name, Vector3 position, Vector3 scale, Material material)
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Cube); obj.name = name; obj.layer = 8;
        obj.transform.SetParent(parent, false); obj.transform.localPosition = position; obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().sharedMaterial = material; return obj;
    }
    static void Anchor(Transform parent, string name, Vector3 position)
    {
        var obj = new GameObject(name); obj.layer = 8; obj.transform.SetParent(parent, false); obj.transform.localPosition = position;
    }
    static GameObject Prefab(string name, System.Action<Transform> build)
    {
        string path = Folder + "/" + name + ".prefab";
        if (AssetDatabase.LoadAssetAtPath<GameObject>(path) != null) throw new System.Exception("Refusing to overwrite " + path);
        var root = new GameObject(name); root.layer = 8; build(root.transform);
        var asset = PrefabUtility.SaveAsPrefabAsset(root, path); Object.DestroyImmediate(root); return asset;
    }
    public static string Main()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.path != "Assets/dorm_room/Scenes/Room404_Blockout.unity" || scene.rootCount != 0) throw new System.Exception("Expected empty blockout scene; refusing to modify another scene.");
        if (!AssetDatabase.IsValidFolder(Folder)) AssetDatabase.CreateFolder("Assets/dorm_room", "Blockout");
        var wall = Mat("Wall", new Color(.88f,.53f,.18f)); var floor = Mat("Floor",new Color(.72f,.38f,.18f));
        var trim = Mat("Trim",new Color(.12f,.11f,.12f)); var metal = Mat("Metal",new Color(.28f,.29f,.32f));
        var blue = Mat("BedBlue",new Color(.08f,.28f,.65f)); var wood = Mat("DeskWood",new Color(.73f,.33f,.08f));
        var glass = Mat("Window",new Color(.25f,.61f,.76f)); var player = Mat("PlayerPlaceholder",new Color(.7f,.7f,.75f));
        var bed = Prefab("Bed", t => {
            foreach(float x in new[]{-1.2f,1.2f}) foreach(float z in new[]{-.53f,.53f}) Box(t,"Leg",new Vector3(x,1.5525f,z),new Vector3(.12f,3.105f,.10f),metal);
            Box(t,"Frame",new Vector3(0,2.4975f,0),new Vector3(2.58f,.216f,1.18f),metal);
            Box(t,"ThinMat",new Vector3(0,2.673f,0),new Vector3(2.34f,.135f,1.04f),blue);
            var ladder = new GameObject("Ladder");ladder.layer=8;ladder.transform.SetParent(t,false);
            Box(ladder.transform,"RailA",new Vector3(1.26f,1.377f,-.48f),new Vector3(.096f,2.754f,.08f),metal);
            Box(ladder.transform,"RailB",new Vector3(1.26f,1.377f,.25f),new Vector3(.096f,2.754f,.08f),metal);
            for(int i=0;i<6;i++) Box(ladder.transform,"Rung",new Vector3(1.26f,(.26f+i*.29f)*1.35f,-.115f),new Vector3(.084f,.0945f,.73f),metal);
            Anchor(t,"ClimbPoint",new Vector3(1.632f,0,-.12f));Anchor(t,"SleepPoint",new Vector3(0,2.7675f,0));Anchor(t,"ExitPoint",new Vector3(1.632f,0,-.12f));
        });
        var desk = Prefab("Desk",t=>{Box(t,"Top",new Vector3(0,.78f,0),new Vector3(1.7f,.10f,.62f),wood);foreach(float x in new[]{-.73f,.73f})foreach(float z in new[]{-.24f,.24f})Box(t,"Leg",new Vector3(x,.38f,z),new Vector3(.08f,.76f,.08f),metal);Anchor(t,"LaptopPoint",new Vector3(0,.84f,0));Anchor(t,"StudyPoint",new Vector3(0,.8f,-.6f));});
        var chair = Prefab("Chair",t=>{Box(t,"Seat",new Vector3(0,.44f,0),new Vector3(.45f,.08f,.45f),blue);Box(t,"Back",new Vector3(0,.75f,-.20f),new Vector3(.45f,.56f,.08f),blue);foreach(float x in new[]{-.17f,.17f})foreach(float z in new[]{-.16f,.16f})Box(t,"Leg",new Vector3(x,.21f,z),new Vector3(.06f,.42f,.06f),metal);Anchor(t,"ChairPoint",new Vector3(0,.49f,0));});
        var door = Prefab("Door",t=>{Box(t,"DoorPanel",new Vector3(0,1.08f,0),new Vector3(1.5f,2.16f,.08f),wood);Anchor(t,"DoorInteractionPoint",new Vector3(0,0,-.75f));});
        var room = new GameObject("Room404_Blockout"); room.layer=8;
        Box(room.transform,"Floor",new Vector3(0,-.05f,-1.8f),new Vector3(11,.1f,10.4f),floor);
        Box(room.transform,"BackWall",new Vector3(0,1.8f,3.4f),new Vector3(11,3.6f,.1f),wall);
        Box(room.transform,"LeftWall",new Vector3(-5.55f,1.8f,-1.8f),new Vector3(.1f,3.6f,10.4f),wall).transform.localRotation=Quaternion.Euler(0,3.2f,0);
        Box(room.transform,"RightWall",new Vector3(5.55f,1.8f,-1.8f),new Vector3(.1f,3.6f,10.4f),wall).transform.localRotation=Quaternion.Euler(0,-3.2f,0);
        Box(room.transform,"WindowPlaceholder",new Vector3(-.95f,1.2f,3.32f),new Vector3(2.3f,1.8f,.06f),glass);
        var doorInstance=(GameObject)PrefabUtility.InstantiatePrefab(door,scene);doorInstance.name="Door";doorInstance.transform.SetParent(room.transform,false);doorInstance.transform.localPosition=new Vector3(1.54f,0,3.3f);
        var set = new GameObject("BedSet_Blockout"); set.layer=8;
        foreach(var pair in new[]{new System.Collections.Generic.KeyValuePair<GameObject,Vector3>(bed,new Vector3(0,0,0)),new System.Collections.Generic.KeyValuePair<GameObject,Vector3>(desk,new Vector3(0,0,.15f)),new System.Collections.Generic.KeyValuePair<GameObject,Vector3>(chair,new Vector3(0,0,-.5f))}) {var item=(GameObject)PrefabUtility.InstantiatePrefab(pair.Key,scene);item.transform.SetParent(set.transform,false);item.transform.localPosition=pair.Value;}
        string setPath=Folder+"/BedSet_Blockout.prefab";if(AssetDatabase.LoadAssetAtPath<GameObject>(setPath)!=null)throw new System.Exception("Refusing to overwrite set prefab");
        var setAsset=PrefabUtility.SaveAsPrefabAsset(set,setPath);Object.DestroyImmediate(set);
        foreach(float x in new[]{-4.15f,4.15f}){var item=(GameObject)PrefabUtility.InstantiatePrefab(setAsset,scene);item.name=x<0?"LeftSet":"RightSet";item.transform.SetParent(room.transform,false);item.transform.localPosition=new Vector3(x,0,1.0f);if(x>0)item.transform.Find("Bed").localRotation=Quaternion.Euler(0,180,0);}
        var person=GameObject.CreatePrimitive(PrimitiveType.Capsule);person.name="PlayerPlaceholder";person.layer=8;person.transform.SetParent(room.transform,false);person.transform.localPosition=new Vector3(0,.85f,0);person.transform.localScale=new Vector3(.45f,.85f,.45f);person.GetComponent<Renderer>().sharedMaterial=player;
        var cameraObject=new GameObject("GameplayCamera");cameraObject.layer=8;var camera=cameraObject.AddComponent<Camera>();camera.orthographic=true;camera.orthographicSize=3.9f;camera.aspect=1.5f;camera.cullingMask=1<<8;camera.backgroundColor=new Color(.12f,.12f,.15f);camera.clearFlags=CameraClearFlags.SolidColor;camera.transform.position=new Vector3(0,8,-12);camera.transform.LookAt(new Vector3(0,.5f,.4f));
        var lightObject=new GameObject("BlockoutLight");lightObject.layer=8;var light=lightObject.AddComponent<Light>();light.type=LightType.Directional;light.cullingMask=1<<8;light.intensity=1;light.transform.rotation=Quaternion.Euler(45,-30,0);
        EditorSceneManager.MarkSceneDirty(scene);if(!EditorSceneManager.SaveScene(scene))throw new System.Exception("Scene save failed");AssetDatabase.SaveAssets();return "Created provisional scene and prefabs; camera, geometry and anchors require validation.";
    }
}
