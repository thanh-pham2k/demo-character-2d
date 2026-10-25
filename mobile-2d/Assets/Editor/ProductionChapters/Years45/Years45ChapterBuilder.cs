#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using ProductionChapters.Years45;

// Explicit opt-in only: no InitializeOnLoad, scene-open, refresh or Play hooks.
public static class Years45ChapterBuilder
{
    const string Sprites="Assets/Sprites/MVP/remaining/";
    [MenuItem("Tools/Production Chapters/Years45/Create owned staging under selection")]
    public static void CreateUnderSelection()
    {
        if(Selection.activeTransform==null)throw new InvalidOperationException("Select the composition root's world parent first.");
        if(Selection.activeTransform.GetComponentInChildren<Years45ChapterModule>(true)!=null)
            throw new InvalidOperationException("Years45 already exists under this selection.");
        var root=new GameObject("Years45 Chapter (inject ProductionChapterContext)");
        Undo.RegisterCreatedObjectUndo(root,"Create Years45 staging");root.transform.SetParent(Selection.activeTransform,false);
        var module=root.AddComponent<Years45ChapterModule>();
        var stage=new GameObject("Owned staging");stage.transform.SetParent(root.transform,false);module.staging=stage.transform;
        module.backdrop=Sprite(stage.transform,"Lab / courtyard",null,new Vector3(0,0,0),-40);
        module.focalActor=Sprite(stage.transform,"Mission actor",null,new Vector3(1.5f,-.8f,0),8);
        module.labBackground=Load("E02/research_lab_background.png");module.courtyardBackground=Load("E03/vnu_photo_courtyard_background.png");
        module.labActor=Load("C03/dam_lab_pose.png");module.graduateActor=Load("C03/long_graduate.png");module.officeActor=Load("C03/to_office.png");
        string[] grads={"C03/long_graduate.png","C03/to_graduate.png","C03/dam_graduate.png"};
        string[] old={"C03/long_office.png","C03/to_office.png","C03/dam_lab_pose.png"};
        for(int i=0;i<3;i++)
        {
            module.oldRoommates[i]=Load(old[i]);module.graduates[i]=Load(grads[i]);
            module.visitingFriends[i]=Sprite(stage.transform,"Visitor "+(i+1),null,new Vector3(-2+i*2,-2.3f,0),9);module.freshmen[i]=Load("C00/npc_freshman_"+(i+1)+".png");
            module.roomSlots[i]=Sprite(stage.transform,"Slot "+(i+1),module.oldRoommates[i],new Vector3(-2.4f+i*2.4f,-1.2f,0),7);
        }
        string[] stations={"Room desk","Door and travel","Bed and alarm","WC","H6 desk","Lab and courtyard"};
        Vector3[] positions={new Vector3(-2,-2,0),new Vector3(3,-2,0),new Vector3(-3,0,0),new Vector3(3,0,0),new Vector3(0,-2,0),new Vector3(1,-2,0)};
        for(int i=0;i<stations.Length;i++)
        {
            var anchor=new GameObject(stations[i]+" anchor (align with scene)");anchor.transform.SetParent(stage.transform,false);
            anchor.transform.localPosition=positions[i];module.activityAnchors[i]=anchor.transform;
        }
        module.actionAnchor=module.activityAnchors[0];
        // Background dimensions preserve aspect; no source PNG modification.
        if(module.backdrop!=null && module.labBackground!=null)
            module.backdrop.transform.localScale=Vector3.one*(8f/module.labBackground.bounds.size.y);
        stage.SetActive(false);Selection.activeGameObject=root;
    }
    static UnityEngine.Sprite Load(string path)
    {
        var sprite=AssetDatabase.LoadAssetAtPath<UnityEngine.Sprite>(Sprites+path);
        if(sprite==null)Debug.LogWarning("Years45 missing imported Sprite: "+Sprites+path);return sprite;
    }
    static SpriteRenderer Sprite(Transform parent,string name,UnityEngine.Sprite sprite,Vector3 pos,int order)
    {
        var go=new GameObject(name);go.transform.SetParent(parent,false);go.transform.localPosition=pos;
        var r=go.AddComponent<SpriteRenderer>();r.sprite=sprite;r.sortingOrder=order;return r;
    }
}
#endif
