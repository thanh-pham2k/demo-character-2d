using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public static class AddForegroundBlockout
{
    const string Dir="Assets/dorm_room/Blockout/";
    static GameObject Box(Transform parent,string name,Vector3 pos,Vector3 size,Material material,bool collision=true)
    {
        var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=name;o.layer=8;o.transform.SetParent(parent,false);o.transform.localPosition=pos;o.transform.localScale=size;o.GetComponent<Renderer>().sharedMaterial=material;if(!collision)Object.DestroyImmediate(o.GetComponent<Collider>());return o;
    }
    static GameObject MakePrefab(Scene scene,string name,System.Action<Transform> make)
    {
        var path=Dir+name+".prefab";if(AssetDatabase.LoadAssetAtPath<GameObject>(path)!=null)throw new System.Exception("Asset already exists: "+path);
        var o=new GameObject(name);o.layer=8;make(o.transform);var asset=PrefabUtility.SaveAsPrefabAsset(o,path);Object.DestroyImmediate(o);return asset;
    }
    public static string Main()
    {
        var scene=SceneManager.GetActiveScene();if(scene.path!="Assets/dorm_room/Scenes/Room404_Blockout.unity" || scene.isDirty)throw new System.Exception("Unexpected or dirty scene");
        var room=GameObject.Find("Room404_Blockout").transform;if(room.Find("RugBlockout")!=null)throw new System.Exception("Already added");
        var blue=AssetDatabase.LoadAssetAtPath<Material>(Dir+"BedBlue.mat");var wood=AssetDatabase.LoadAssetAtPath<Material>(Dir+"DeskWood.mat");var metal=AssetDatabase.LoadAssetAtPath<Material>(Dir+"Metal.mat");
        Box(room,"RugBlockout",new Vector3(-.06f,.015f,-3.95f),new Vector3(5.1f,.03f,2.65f),blue,false);
        var table=MakePrefab(scene,"CenterTable_Blockout",t=>{Box(t,"Top",new Vector3(0,.46f,0),new Vector3(1.86f,.10f,.82f),wood);foreach(float x in new[]{-.78f,.78f})foreach(float z in new[]{-.3f,.3f})Box(t,"Leg",new Vector3(x,.22f,z),new Vector3(.08f,.44f,.08f),metal);});
        var tab=(GameObject)PrefabUtility.InstantiatePrefab(table,scene);tab.name="CenterTable";tab.transform.SetParent(room,false);tab.transform.localPosition=new Vector3(-.06f,0,-4.4f);
        var shelf=MakePrefab(scene,"Storage_Blockout",t=>{Box(t,"Cabinet",new Vector3(0,1.05f,0),new Vector3(.65f,2.10f,.55f),wood);});
        foreach(float x in new[]{-5.0f,5.0f}){var inst=(GameObject)PrefabUtility.InstantiatePrefab(shelf,scene);inst.name=x<0?"StorageLeft":"StorageRight";inst.transform.SetParent(room,false);inst.transform.localPosition=new Vector3(x,0,-3.6f);}
        EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);return "Added provisional rug, table and reusable storage blockouts.";
    }
}
