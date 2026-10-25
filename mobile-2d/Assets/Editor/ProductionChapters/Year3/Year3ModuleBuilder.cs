using UnityEditor;
using UnityEngine;
using ProductionChapters.Year3;

// Explicit menu only. Importing these scripts never changes the shared scene.
public static class Year3ModuleBuilder
{
    const string Root="Assets/Sprites/MVP/remaining/";
    [MenuItem("Tools/Production Chapters/Year3/Create selected module")]
    public static void Create()
    {
        var go=new GameObject("Production Year3 module");Undo.RegisterCreatedObjectUndo(go,"Create Year3 module");
        if(Selection.activeTransform!=null)go.transform.SetParent(Selection.activeTransform,false);
        var module=go.AddComponent<Year3ChapterModule>();
        module.cafe=Sprite("E02/drink_cafe_background");module.lake=Sprite("E01/lake_background");
        module.repairPose=Sprite("C02/luon_repair_laptop");module.jacketPose=Sprite("C02/luon_borrowed_jacket");
        module.crush=Sprite("C02/crush_idle");module.boyfriend=Sprite("C02/crush_boyfriend_idle");
        module.bench=Sprite("P03/lake_bench");module.tea=Sprite("P03/peach_tea");module.food=Sprite("P03/rice_paper_snack");module.sign=Sprite("P02/service_sign_blank");
        module.friends=new[]{Sprite("C02/luon_lake_sit"),Sprite("C02/long_lake_sit"),Sprite("C02/to_lake_sit"),Sprite("C02/dam_lake_sit")};
        Selection.activeGameObject=go;
    }
    static Sprite Sprite(string path)=>AssetDatabase.LoadAssetAtPath<Sprite>(Root+path+".png");
    [MenuItem("Tools/Production Chapters/Year3/Check definition coverage")]
    public static void Check()
    {
        var source=Resources.Load<TextAsset>("ProductionCampaign/missions");
        var definition=JsonUtility.FromJson<ProductionCampaignDefinition>(source.text);
        int count=0;
        foreach(var mission in definition.missions){if(mission.id<16 || mission.id>23)continue;
            foreach(var step in mission.steps){if(step.id!="answer_mother" && Year3Activities.Get(step.id)==null)throw new System.Exception("Missing Year3 adapter: "+step.id);count++;}}
        Debug.Log("Year3 activity coverage: "+count+" authored steps. This is not Play Mode verification.");
    }
}
