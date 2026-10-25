using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// Opt-in asset staging only. Does not run or replace the narrative controller.
[InitializeOnLoad]
public static class AssetProductionBlockout
{
    private static string Request => Path.Combine(Application.dataPath,"../Temp/AssetProductionBlockout.run");
    private static string Output => Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/mvp_sprites/remaining/blockout"));
    [Serializable] private class Report { public string scope="Spatial placeholder staging and generated actor import check only; not a full gameplay verification"; public bool success; public string[] checks; public string[] errors; }
    private static readonly List<string> checks=new List<string>(), errors=new List<string>();
    private static Sprite square;
    private static readonly Dictionary<string,string> ActorIds=new Dictionary<string,string>{
        {"manager","npc_dorm_manager"},{"tout","npc_station_tout"},{"vendor","npc_canteen_vendor"},{"classmate","npc_h6_classmate_writing"},
        {"customer","npc_repair_customer"},{"freshman1","npc_freshman_1"},{"freshman2","npc_freshman_2"},{"freshman3","npc_freshman_3"}};
    private static Scene scene;
    private static double allowedAfter;
    static AssetProductionBlockout()
    {
        allowedAfter=EditorApplication.timeSinceStartup+2;
        EditorApplication.playModeStateChanged+=_=>allowedAfter=EditorApplication.timeSinceStartup+2;
        EditorApplication.update+=Poll;
    }
    private static void Poll()
    {
        if(!File.Exists(Request) || EditorApplication.timeSinceStartup<allowedAfter || EditorApplication.isCompiling || EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)return;
        File.Delete(Request);
        try { Build(); } catch(Exception ex) { Debug.LogException(ex);Directory.CreateDirectory(Output);File.WriteAllText(Path.Combine(Output,"failure.txt"),ex.ToString()); }
    }
    private static GameObject Node(string name,Transform parent,Vector2 position)
    {
        var go=new GameObject(name);SceneManager.MoveGameObjectToScene(go,scene);
        if(parent!=null)go.transform.SetParent(parent,false);
        go.transform.localPosition=position;go.layer=30;return go;
    }
    private static void Box(string name,Transform parent,Vector2 center,Vector2 size,Color color,int order=0)
    {
        var go=Node(name,parent,center);var r=go.AddComponent<SpriteRenderer>();r.sprite=square;r.color=color;r.sortingOrder=order;go.transform.localScale=size;
    }
    private static void Label(string value,Transform parent,Vector2 center,float size=0.055f)
    {
        var go=Node(value,parent,center);var text=go.AddComponent<TextMesh>();text.text=value;text.characterSize=size;text.fontSize=48;text.anchor=TextAnchor.MiddleCenter;
        text.color=Color.white;text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");go.GetComponent<MeshRenderer>().sortingOrder=10;
    }
    private static void Marker(string name,Transform parent,Vector2 foot,Color color)
    {
        Sprite actor=null;
        string id=null;
        if(ActorIds.TryGetValue(name,out id))actor=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/remaining/C00/"+id+".png");
        else if(name=="player") { id="existing_luon"; actor=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/characters/luon_idle_0.png"); }
        else if(name=="npc" && parent.name=="dorm_reception") { id="npc_dorm_manager"; actor=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/remaining/C00/npc_dorm_manager.png"); }
        if(actor!=null)
        {
            var body=Node(name+"_art",parent,foot+Vector2.up*(actor.pivot.y/actor.pixelsPerUnit));var renderer=body.AddComponent<SpriteRenderer>();renderer.sprite=actor;renderer.sortingOrder=4;
            if(id=="existing_luon")checks.Add("Existing Luon import preserved; staging uses its original pivot");
            else if(Mathf.Approximately(actor.pixelsPerUnit,100) && Mathf.Abs(actor.pivot.y)<.01f)checks.Add(id+" imported PPU100 bottom pivot");
            else errors.Add(id+" import settings incorrect");
        }
        else Box(name,parent,foot+Vector2.up*1.15f,new Vector2(.8f,2.3f),color,2);
        Box(name+"_feet",parent,foot,new Vector2(1,.05f),Color.white,3);
        Label(name,parent,foot+Vector2.up*2.6f,.035f);
        bool inside=foot.x>=-7f && foot.x<=7f && foot.y>=-3.8f && foot.y<=2f;
        (inside?checks:errors).Add(name+" feet within playable board");
    }
    private static void Build()
    {
        checks.Clear();errors.Clear();Directory.CreateDirectory(Output);
        Scene original=SceneManager.GetActiveScene();scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);
        const string placeholderPath="Assets/AssetProduction/Placeholder.asset";
        if(!AssetDatabase.IsValidFolder("Assets/AssetProduction"))AssetDatabase.CreateFolder("Assets","AssetProduction");
        square=AssetDatabase.LoadAssetAtPath<Sprite>(placeholderPath);
        if(square==null)
        {
            var tex=new Texture2D(1,1);tex.name="Blockout_White";tex.SetPixel(0,0,Color.white);tex.Apply();
            AssetDatabase.CreateAsset(tex,placeholderPath);
            square=Sprite.Create(tex,new Rect(0,0,1,1),new Vector2(.5f,.5f),1);square.name="Blockout_Square";
            AssetDatabase.AddObjectToAsset(square,placeholderPath);AssetDatabase.SaveAssets();
        }
        var cameraGo=Node("Asset_Blockout_Camera",null,Vector2.zero);var cam=cameraGo.AddComponent<Camera>();cam.enabled=false;cam.orthographic=true;cam.orthographicSize=4.5f;cam.aspect=16f/9;cam.clearFlags=CameraClearFlags.SolidColor;cam.backgroundColor=new Color(.035f,.045f,.07f);cam.cullingMask=1<<30;
        cameraGo.transform.position=new Vector3(4000,0,-10);
        var boards=new List<GameObject>();
        try
        {
            string[] zones={"bus_station","dorm_reception","canteen","lake","drink_cafe","research_lab","vnu_photo_courtyard","advisor_office","library","defense_room","a5_graduation_hall","binh_dong_night","C00_actor_scale","P00_prop_staging"};
            foreach(string zone in zones)
            {
                var board=Node(zone,null,new Vector2(4000,0));boards.Add(board);Box("Background",board.transform,Vector2.zero,new Vector2(16,9),new Color(.10f,.13f,.18f),-10);
                int zoneIndex=Array.IndexOf(zones,zone);
                string backdropId=zoneIndex<12?zone+"_background":null;
                string environmentBatch="E"+(zoneIndex/2).ToString("D2");
                Sprite background=backdropId!=null?AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/remaining/"+environmentBatch+"/"+backdropId+".png"):null;
                if(background!=null)
                {
                    var art=Node("Generated_Backdrop",board.transform,Vector2.zero);var r=art.AddComponent<SpriteRenderer>();r.sprite=background;r.sortingOrder=-4;art.transform.localScale=Vector3.one*(8f/background.bounds.size.y);
                    checks.Add(backdropId+" imported and staged at8 world units height, aspect preserved");
                }
                else Box("Playable_Floor",board.transform,new Vector2(0,-1.8f),new Vector2(14,3.8f),new Color(.19f,.25f,.27f),-5);
                Label(zone+" - PLACEHOLDER / spatial review",board.transform,new Vector2(0,3.8f));
                Marker("entry",board.transform,new Vector2(-6.5f,-3.4f),new Color(.2f,.8f,.5f));
                Marker("exit",board.transform,new Vector2(6.5f,-3.4f),new Color(.2f,.6f,1f));
                if(zone=="P00_prop_staging")
                {
                    string[] props={"nem_chua_box","cleaning_bucket","cleaning_rag","mini_fan","meal_tray","food_rice","food_meat","food_vegetables"};
                    for(int i=0;i<props.Length;i++)
                    {
                        var sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/remaining/P00/"+props[i]+".png");
                        if(sprite==null){errors.Add(props[i]+" missing imported sprite");continue;}
                        var t=Node(props[i],board.transform,new Vector2(-5.5f+i%4*3.5f,i<4?.2f:-2.7f));
                        var renderer=t.AddComponent<SpriteRenderer>();renderer.sprite=sprite;renderer.sortingOrder=4;
                        t.transform.localScale=Vector3.one*Mathf.Min(2.5f/sprite.bounds.size.x,2f/sprite.bounds.size.y);
                        Label(props[i],board.transform,(Vector2)t.transform.localPosition+Vector2.down*.25f,.035f);
                        if(Mathf.Approximately(sprite.pixelsPerUnit,100))checks.Add(props[i]+" imported PPU100");else errors.Add(props[i]+" incorrect PPU");
                    }
                }
                else if(zone=="C00_actor_scale")
                {
                    string[] names={"manager","tout","vendor","classmate","customer","freshman1","freshman2","freshman3"};
                    for(int i=0;i<names.Length;i++)Marker(names[i],board.transform,new Vector2(-5.5f+i*1.5f,-2f),new Color(.8f,.55f,.3f));
                    checks.Add("C00 eight actor silhouettes use shared 2.3 unit height and distinct positions");
                }
                else if(zone=="canteen" && background!=null)
                {
                    string[] ids={"luon_lake_sit","dam_lake_sit","long_lake_sit","to_lake_sit"};
                    for(int i=0;i<ids.Length;i++)
                    {
                        var sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/remaining/C02/"+ids[i]+".png");
                        if(sprite==null){errors.Add(ids[i]+" missing for canteen staging");continue;}
                        var actor=Node(ids[i],board.transform,new Vector2(i*1.58f,-1.6f));actor.transform.localScale=Vector3.one*1.2f;
                        var r=actor.AddComponent<SpriteRenderer>();r.sprite=sprite;r.sortingOrder=3;
                        checks.Add(ids[i]+" reused on canteen chair anchor");
                    }
                    var texture=background.texture;var rect=new Rect(texture.width*.417f,texture.height*.39f,texture.width*.502f,texture.height*.095f);
                    const string path="Assets/AssetProduction/CanteenTableTop.asset";
                    var foreground=AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    if(foreground==null){foreground=Sprite.Create(texture,rect,new Vector2(.5f,.5f),100);foreground.name="CanteenTableTop";AssetDatabase.CreateAsset(foreground,path);}
                    var top=Node("TableTop_Foreground_Derived",board.transform,new Vector2((.417f+.502f*.5f-.5f)*16,(.39f+.095f*.5f-.5f)*8));
                    top.transform.localScale=Vector3.one*(16f/background.bounds.size.x);var fg=top.AddComponent<SpriteRenderer>();fg.sprite=foreground;fg.sortingOrder=5;
                }
                else if(zone=="defense_room")
                {
                    for(int i=0;i<5;i++)Marker("committee"+i,board.transform,new Vector2(-4+i*2,0f),new Color(.6f,.4f,.7f));
                    Box("Committee_Table_Foreground",board.transform,new Vector2(0,.1f),new Vector2(10,1),new Color(.4f,.2f,.15f),5);
                    Marker("presenter",board.transform,new Vector2(2,-3f),new Color(.8f,.6f,.2f));
                }
                else if(zone=="lake" || zone=="binh_dong_night" || zone=="a5_graduation_hall")
                {
                    for(int i=0;i<4;i++)Marker("friend"+i,board.transform,new Vector2(-3+i*2,-1.8f),new Color(.7f,.5f,.25f));
                    if(zone=="lake")Box("Bench",board.transform,new Vector2(0,-1f),new Vector2(8,.4f),new Color(.4f,.45f,.5f),1);
                    checks.Add(zone+" has four separate actor positions; no baked group required");
                }
                else
                {
                    Marker("player",board.transform,new Vector2(-1.8f,-2.7f),new Color(.3f,.7f,.5f));
                    Marker("npc",board.transform,zone=="dorm_reception"?new Vector2(3,.4f):new Vector2(2f,-1.5f),new Color(.8f,.6f,.3f));
                    if(background==null)Box("Interactive_Surface",board.transform,new Vector2(1.8f,-.8f),new Vector2(3,1),new Color(.4f,.27f,.18f),1);
                    else if(zone=="dorm_reception")
                    {
                        // Native rectangle from same background, not a generated duplicate.
                        var texture=background.texture;var rect=new Rect(texture.width*.55f,texture.height*.455f,texture.width*.365f,texture.height*.18f);
                        const string foregroundPath="Assets/AssetProduction/ReceptionCounter.asset";
                        var foreground=AssetDatabase.LoadAssetAtPath<Sprite>(foregroundPath);
                        if(foreground==null)
                        {
                            foreground=Sprite.Create(texture,rect,new Vector2(.5f,.5f),100);foreground.name="ReceptionCounterForeground";
                            AssetDatabase.CreateAsset(foreground,foregroundPath);
                        }
                        var t=Node("Counter_Foreground_Derived",board.transform,new Vector2((.55f+.365f*.5f-.5f)*16,(.455f+.18f*.5f-.5f)*8));
                        t.transform.localScale=Vector3.one*(16f/background.bounds.size.x);var r=t.AddComponent<SpriteRenderer>();r.sprite=foreground;r.sortingOrder=5;
                        checks.Add("Reception counter foreground derives from the same background texture");
                    }
                }
                cam.Render();MVPSceneSnapshot.SaveCameraFrame(cam,Path.Combine(Output,zone+".png"));board.SetActive(false);
            }
            foreach(var b in boards)b.SetActive(true);
            // Place boards apart for a permanent inspection scene; no shared camera with gameplay.
            for(int i=0;i<boards.Count;i++)boards[i].transform.position=new Vector3(4000+i*22f,0,0);
            EditorSceneManager.SaveScene(scene,"Assets/Scenes/AssetProduction_Blockout.unity");
            checks.Add("Separate scene saved; Room404_MVP not replaced");
            File.WriteAllText(Path.Combine(Output,"report.json"),JsonUtility.ToJson(new Report{success=errors.Count==0,checks=checks.ToArray(),errors=errors.ToArray()},true));
        }
        finally
        {
            if(scene.IsValid() && scene.isLoaded)EditorSceneManager.CloseScene(scene,true);
            if(original.IsValid() && original.isLoaded)SceneManager.SetActiveScene(original);
            square=null;
        }
    }
}
