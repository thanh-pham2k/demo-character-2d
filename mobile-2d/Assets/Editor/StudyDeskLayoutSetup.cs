using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DormJourneySceneSetup;

// Deterministic arrangement using existing textures. No new raster artwork.
[InitializeOnLoad]
public static class StudyDeskLayoutSetup
{
    static StudyDeskLayoutSetup(){EditorApplication.update+=Poll;}
    static void Poll()
    {
        string request=Path.GetFullPath("Temp/StudyDeskLayoutSetup.run");
        if(!File.Exists(request) || EditorApplication.isCompiling || EditorApplication.isUpdating || EditorApplication.isPlayingOrWillChangePlaymode)return;
        var scene=SceneManager.GetActiveScene();
        if(scene.path!="Assets/Scenes/Room404_MVP.unity")return;
        File.Delete(request);Configure(scene);
        Find(scene,"MVP_Flow_Controller").GetComponent<MVPFlowController>().trashPileProp.SetActive(false);
        AssetDatabase.SaveAssets();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        string output=Path.GetFullPath("PlayModeInteractionReports/study-desk-layout");Directory.CreateDirectory(output);
        MVPSceneSnapshot.SaveCameraFrame(Camera.main,Path.Combine(output,"room.png"));
        var cam=Camera.main;var oldPosition=cam.transform.position;float oldSize=cam.orthographicSize;
        try
        {
            var centre=Find(scene,"Study_Desk").transform.position;
            cam.transform.position=new Vector3(centre.x,centre.y+.15f,oldPosition.z);cam.orthographicSize=1.05f;
            MVPSceneSnapshot.SaveCameraFrame(cam,Path.Combine(output,"desk-closeup.png"));
        }
        finally{cam.transform.position=oldPosition;cam.orthographicSize=oldSize;}
        File.WriteAllText(Path.Combine(output,"result.txt"),"Applied: two aligned existing laptops and two book stacks; original desk, chairs and room preserved.\n");
    }
    public static void Configure(Scene scene)
    {
        var desk=Find(scene,"Study_Desk").GetComponent<SpriteRenderer>();
        var left=Find(scene,"Laptop_Prop").GetComponent<SpriteRenderer>();
        var right=Find(scene,"Laptop_Second").GetComponent<SpriteRenderer>();
        var chairs=desk.GetComponentsInChildren<ChairInteractable>();Array.Sort(chairs,(a,b)=>a.SeatPivot.position.x.CompareTo(b.SeatPivot.position.x));
        if(chairs.Length!=2)throw new InvalidOperationException("Desk must have exactly two chairs.");
        var material=Projection("LaptopAlignment",.1244f,-.18f);
        // Same front edge and footprint, centred on each authored chair marker.
        float surfaceY=desk.transform.position.y+.51f;
        AlignLaptop(left,chairs[0].SeatPivot.position.x,surfaceY,material);
        AlignLaptop(right,chairs[1].SeatPivot.position.x,surfaceY,material);
        var source=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/remaining/P04/thesis_book_blank.png");
        var books=Projection("DeskBookAlignment",-.39f,.55f);
        float half=desk.sprite.bounds.size.x*desk.transform.lossyScale.x*.5f;
        Stack(desk,"BookStack_Left",source,new Vector3(desk.transform.position.x-half+.21f,surfaceY-.09f,0),books,left.sortingLayerID,left.sortingOrder);
        Stack(desk,"BookStack_Right",source,new Vector3(desk.transform.position.x+half-.34f,surfaceY-.09f,0),books,left.sortingLayerID,left.sortingOrder);
        // Re-register the existing laptop glow to the straightened screen.
        var glow=left.transform.Find("Activity_Screen_Glow");
        if(glow!=null){glow.localPosition=new Vector3(.25f,1.03f,0);glow.localRotation=Quaternion.identity;glow.localScale=new Vector3(3.6f,2.1f,1);}
        var light=Find(scene,"Laptop Screen Glow");light.transform.position=left.transform.position;
    }
    static Material Projection(string name,float y,float x)
    {
        string path="Assets/Room404Layout/"+name+".mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(mat==null){mat=new Material(Shader.Find("Room404/SpriteProjection"));AssetDatabase.CreateAsset(mat,path);}
        mat.SetFloat("_ShearY",y);mat.SetFloat("_ShearX",x);mat.SetFloat("_CompressX",1);EditorUtility.SetDirty(mat);return mat;
    }
    static void AlignLaptop(SpriteRenderer renderer,float x,float y,Material material)
    {
        Undo.RecordObject(renderer.transform,"Align desk laptop");Undo.RecordObject(renderer,"Align desk laptop");
        renderer.sharedMaterial=material;renderer.transform.rotation=Quaternion.identity;
        float scale=.55f/renderer.sprite.bounds.size.x;renderer.transform.localScale=new Vector3(scale,scale,1);
        // Asset uses a centred pivot. Projection shifts its visible centre slightly right.
        renderer.transform.position=new Vector3(x-.025f,y,0);EditorUtility.SetDirty(renderer);
    }
    static void Stack(SpriteRenderer desk,string name,Sprite source,Vector3 position,Material material,int layer,int order)
    {
        var root=Child(desk.transform,name);root.position=position;root.rotation=Quaternion.identity;root.localScale=new Vector3(1/desk.transform.lossyScale.x,1/desk.transform.lossyScale.y,1);
        for(int i=0;i<3;i++)
        {
            var book=Child(root,"Book_"+i);book.localPosition=new Vector3(0,i*.033f,0);book.localRotation=Quaternion.identity;
            float scale=.44f/source.bounds.size.x;book.localScale=new Vector3(scale,scale*.48f,1);
            var sr=Component<SpriteRenderer>(book.gameObject);sr.sprite=source;sr.sharedMaterial=material;sr.sortingLayerID=layer;sr.sortingOrder=order+i;sr.color=i==0?new Color(.7f,.8f,.85f):i==1?new Color(.7f,.8f,.65f):Color.white;
        }
    }
}
