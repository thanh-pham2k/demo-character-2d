using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

// File-triggered catalog/import/render verification. Does not advance gameplay.
[InitializeOnLoad]
public static class ProductionAssetCatalogBuilder
{
    [Serializable] private class Definition { public string id="",batch="",output_file="";public int[] missions=Array.Empty<int>(); }
    [Serializable] private class Batch { public string id=""; public int columns=0,rows=0; }
    [Serializable] private class Plan { public Definition[] new_assets=Array.Empty<Definition>(); public Batch[] batches=Array.Empty<Batch>(); }
    [Serializable] private class PivotConfig { public float pivotX=.5f,pivotY=0; }
    [Serializable] private class Evidence { public string id,sha256; }
    [Serializable] private class Report { public string scope="Generated art import/catalog/gallery only; gameplay integration still pending"; public bool success; public int planned,imported;public string completedUtc;public Evidence[] evidence;public string[] checkedIds,checks,errors; }
    private static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../.."));
    private static string Request=>Path.Combine(Application.dataPath,"../Temp/BuildProductionAssetCatalog.run");
    private static double allowedAfter;
    static ProductionAssetCatalogBuilder()
    {
        allowedAfter=EditorApplication.timeSinceStartup+3;
        EditorApplication.playModeStateChanged+=_=>allowedAfter=EditorApplication.timeSinceStartup+3;
        EditorApplication.update+=Poll;
    }
    private static void Poll()
    {
        if(!File.Exists(Request) || EditorApplication.timeSinceStartup<allowedAfter || EditorApplication.isCompiling || EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode)return;
        File.Delete(Request);
        try { Build(); } catch(Exception ex) { Debug.LogException(ex);var output=Path.Combine(Root,"assets/mvp_sprites/remaining/review");Directory.CreateDirectory(output);File.WriteAllText(Path.Combine(output,"failure.txt"),ex.ToString()); }
    }
    private static void Build()
    {
        var plan=JsonUtility.FromJson<Plan>(File.ReadAllText(Path.Combine(Root,"story/plans/ASSET_BATCH_PLAN.json")));
        var errors=new List<string>();var checks=new List<string>();var entries=new List<RemainingAssetCatalog.Entry>();
        var groups=new SortedDictionary<string,List<RemainingAssetCatalog.Entry>>();var ids=new HashSet<string>();var evidence=new List<Evidence>();
        foreach(var def in plan.new_assets)
        {
            if(!ids.Add(def.id)){errors.Add("Duplicate planned id: "+def.id);continue;}
            if(!File.Exists(Path.Combine(Root,def.output_file))){errors.Add(def.id+" missing planned output file");continue;}
            string path=def.output_file.Substring("mobile-2d/".Length);
            AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceUpdate);
            var sprite=AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if(sprite==null){errors.Add(def.id+" missing Sprite import");continue;}
            var importer=(TextureImporter)AssetImporter.GetAtPath(path);
            if(importer.spritePixelsPerUnit!=100 || importer.filterMode!=FilterMode.Point || importer.mipmapEnabled || importer.textureCompression!=TextureImporterCompression.Uncompressed)
                errors.Add(def.id+" import contract mismatch");
            else checks.Add(def.id+" PPU100/Point/Uncompressed/no-mips");
            var settings=new TextureImporterSettings();importer.ReadTextureSettings(settings);
            var expected=path.Contains("/E0")?new Vector2(.5f,.5f):new Vector2(.5f,0);
            string configPath=Path.ChangeExtension(path,"sprite.json");
            if(File.Exists(configPath)){var config=JsonUtility.FromJson<PivotConfig>(File.ReadAllText(configPath));expected=new Vector2(config.pivotX,config.pivotY);}
            if(Vector2.Distance(settings.spritePivot,expected)>.0001f || settings.spriteMeshType!=SpriteMeshType.FullRect)errors.Add(def.id+" pivot/full-rect mismatch");
            else checks.Add(def.id+" expected pivot and FullRect mesh");
            using(var sha=SHA256.Create())evidence.Add(new Evidence{id=def.id,sha256=BitConverter.ToString(sha.ComputeHash(File.ReadAllBytes(Path.Combine(Root,def.output_file)))).Replace("-","").ToLowerInvariant()});
            var entry=new RemainingAssetCatalog.Entry{id=def.id,sprite=sprite,batch=def.batch,missions=def.missions};entries.Add(entry);
            if(!groups.ContainsKey(def.batch))groups[def.batch]=new List<RemainingAssetCatalog.Entry>();groups[def.batch].Add(entry);
        }
        if(!AssetDatabase.IsValidFolder("Assets/AssetProduction"))AssetDatabase.CreateFolder("Assets","AssetProduction");
        const string catalogPath="Assets/AssetProduction/RemainingAssetCatalog.asset";
        var catalog=AssetDatabase.LoadAssetAtPath<RemainingAssetCatalog>(catalogPath);
        if(catalog==null){catalog=ScriptableObject.CreateInstance<RemainingAssetCatalog>();AssetDatabase.CreateAsset(catalog,catalogPath);}
        catalog.entries=entries.ToArray();EditorUtility.SetDirty(catalog);AssetDatabase.SaveAssets();
        Scene original=SceneManager.GetActiveScene();var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);
        string output=Path.Combine(Root,"assets/mvp_sprites/remaining/review");Directory.CreateDirectory(output);
        try
        {
            SceneManager.SetActiveScene(scene);
            catalog.animations=ProductionAnimationClipBuilder.Build(entries,checks,errors);
            EditorUtility.SetDirty(catalog);AssetDatabase.SaveAssets();
            var cameraGo=new GameObject("Catalog_Camera");SceneManager.MoveGameObjectToScene(cameraGo,scene);var cam=cameraGo.AddComponent<Camera>();
            cam.enabled=false;cam.orthographic=true;cam.orthographicSize=4.5f;cam.aspect=16f/9;cam.clearFlags=CameraClearFlags.SolidColor;cam.backgroundColor=new Color(.10f,.13f,.17f);cam.cullingMask=1<<29;
            int groupIndex=0;
            foreach(var group in groups)
            {
                var board=new GameObject("Batch_"+group.Key);SceneManager.MoveGameObjectToScene(board,scene);board.transform.position=new Vector3(6000+groupIndex*22,0,0);
                int cols=4,expected=0;
                foreach(var batch in plan.batches)if(batch.id==group.Key){cols=batch.columns;expected=batch.columns*batch.rows;break;}
                if(group.Value.Count!=expected)errors.Add(group.Key+" incomplete imported batch: "+group.Value.Count+"/"+expected);
                int rows=Mathf.CeilToInt(group.Value.Count/(float)cols);
                float cellW=14f/cols,cellH=7f/rows;
                for(int i=0;i<group.Value.Count;i++)
                {
                    var item=group.Value[i];var go=new GameObject(item.id);SceneManager.MoveGameObjectToScene(go,scene);go.layer=29;go.transform.SetParent(board.transform,false);
                    var r=go.AddComponent<SpriteRenderer>();r.sprite=item.sprite;
                    float scale=Mathf.Min((cellW-.3f)/r.sprite.bounds.size.x,(cellH-.55f)/r.sprite.bounds.size.y);
                    go.transform.localScale=Vector3.one*scale;
                    var center=new Vector3(-7+cellW*(i%cols+.5f),3.1f-cellH*(i/cols+.5f),0);
                    go.transform.localPosition=center-r.sprite.bounds.center*scale;
                    var label=new GameObject(item.id+"_label");SceneManager.MoveGameObjectToScene(label,scene);label.layer=29;label.transform.SetParent(board.transform,false);label.transform.localPosition=center+Vector3.down*(cellH*.5f-.14f);
                    var text=label.AddComponent<TextMesh>();text.text=item.id;text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.fontSize=48;text.characterSize=.03f;text.anchor=TextAnchor.MiddleCenter;text.color=Color.white;
                }
                cameraGo.transform.position=board.transform.position+new Vector3(0,0,-10);
                MVPSceneSnapshot.SaveCameraFrame(cam,Path.Combine(output,group.Key+"-unity.png"));groupIndex++;
            }
            cameraGo.transform.position=new Vector3(6000,0,-10);
            EditorSceneManager.SaveScene(scene,"Assets/Scenes/AssetProduction_Gallery.unity");
            var checkedIds=new List<string>();foreach(var e in entries)checkedIds.Add(e.id);
            File.WriteAllText(Path.Combine(output,"report.json"),JsonUtility.ToJson(new Report{success=errors.Count==0,planned=plan.new_assets.Length,imported=entries.Count,completedUtc=DateTime.UtcNow.ToString("O"),evidence=evidence.ToArray(),checkedIds=checkedIds.ToArray(),checks=checks.ToArray(),errors=errors.ToArray()},true));
        }
        finally
        {
            EditorSceneManager.CloseScene(scene,true);
            if(original.IsValid() && original.isLoaded)SceneManager.SetActiveScene(original);
        }
    }
}
