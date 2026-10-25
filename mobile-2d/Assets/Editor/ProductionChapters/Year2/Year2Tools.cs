using UnityEngine;
using UnityEditor;
using ProductionChapters.Year2;

public static class Year2Tools
{
    [MenuItem("Tools/Production/Year2/Add module to selected root (opt-in)")]
    public static void Build()
    {
        if(Selection.activeGameObject==null)throw new System.InvalidOperationException("Select the chapter root. Builder never changes a scene automatically.");
        var root=Selection.activeGameObject;
        if(root.GetComponentInChildren<Year2Module>(true)!=null)throw new System.InvalidOperationException("Year2 already exists under this root.");
        var host=new GameObject("Year2 module — inject context before activation");Undo.RegisterCreatedObjectUndo(host,"Add Year2");host.transform.SetParent(root.transform,false);
        var module=host.AddComponent<Year2Module>();
        string[] stations={"Laptop","Giường","Cửa","Ổ điện","Tủ","H6"};
        for(int i=0;i<stations.Length;i++)
        {
            var go=new GameObject("Year2 "+stations[i]);go.transform.SetParent(host.transform,false);go.transform.localPosition=new Vector3(i*2,0,0);
            var action=go.AddComponent<Year2WorldAction>();action.module=module;action.station=stations[i];
        }
        module.visiblePot=Prop(host,"Visible pot","Assets/Sprites/MVP/year1/pot_0.png",new Vector3(0,-1,0));
        module.hiddenPot=Prop(host,"Hidden pot","Assets/Sprites/MVP/year1/pot_0.png",new Vector3(8,-1,0));
        module.trashLayer=Prop(host,"Gaming trash","Assets/Sprites/MVP/props/trash_piles.png",new Vector3(1,-1,0));
        module.guard=Prop(host,"Guard","Assets/Sprites/MVP/remaining/C01/guard_inspecting.png",new Vector3(4,0,0));
        module.visiblePot.SetActive(false);module.hiddenPot.SetActive(false);module.trashLayer.SetActive(false);module.guard.SetActive(false);
        Selection.activeGameObject=host;
        Debug.Log("Year2 added. Place station anchors at existing props/H6, bind pot/plug/trash and pose transforms; composition must Configure(sharedContext). No asset import or scene save was invoked.");
    }
    private static GameObject Prop(GameObject host,string name,string path,Vector3 position)
    {
        var go=new GameObject(name);go.transform.SetParent(host.transform,false);go.transform.localPosition=position;
        var renderer=go.AddComponent<SpriteRenderer>();renderer.sprite=AssetDatabase.LoadAssetAtPath<Sprite>(path);renderer.sortingOrder=15;
        return go;
    }
    [MenuItem("Tools/Production/Year2/Run isolated challenge checks (opt-in)")]
    public static void Check()
    {
        int count=0;
        System.Action<bool,string> check=(ok,message)=>{count++;if(!ok)throw new System.Exception("Year2 check: "+message);};
        var c=new Year2Challenge();
        check(!c.Hide(1),"hide cannot precede unplug");check(c.Hide(0),"unplug");check(!c.Hide(0),"no repeated unplug");check(c.Hide(1),"hide");check(c.Hide(2),"sleep");
        c.Tick(10,true,false);check(c.Caught,"10 second expiry");check(!c.Hide(3),"caught attempt cannot progress");c.RetryStealth();check(!c.Caught && c.StealthRemaining==10,"retry timer");
        c.Tick(1198,false,true);check(!c.DeadlineMissed,"23:59:58 valid");c.Tick(1,false,true);check(c.DeadlineMissed,"23:59:59 deadline");c.RetryAssignment();check(!c.DeadlineMissed,"deadline retry");
        for(int i=0;i<5;i++)check(c.Answer(0),"answer accepted");check(c.ExamPercent==100,"correct answers stay 100 percent");check(!c.Answer(0),"no sixth answer");c.ResetExam();for(int i=0;i<5;i++)c.Answer(1);check(c.ExamPercent==0,"wrong answers score zero");
        check(Year2Module.StationFor(12,"unplug_cooker")=="Ổ điện","plug anchor");check(Year2Module.StationFor(12,"hide_pot")=="Tủ","cupboard anchor");check(Year2Module.StationFor(14,"arrive_exam")=="H6","exam anchor");
        Debug.Log("Year2 isolated challenge checks passed: "+count+". Does not certify scene wiring, durable commits or Play Mode.");
    }
}
