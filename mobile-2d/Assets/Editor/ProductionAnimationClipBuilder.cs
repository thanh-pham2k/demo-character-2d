using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Native reusable clips, not a second sprite writer on the gameplay player.
public static class ProductionAnimationClipBuilder
{
    public static RemainingAssetCatalog.ClipEntry[] Build(List<RemainingAssetCatalog.Entry> entries,List<string> checks,List<string> errors)
    {
        const string folder="Assets/AssetProduction/Animations";
        if(!AssetDatabase.IsValidFolder(folder))AssetDatabase.CreateFolder("Assets/AssetProduction","Animations");
        var sprites=new Dictionary<string,Sprite>();foreach(var e in entries)sprites[e.id]=e.sprite;
        var clips=new List<RemainingAssetCatalog.ClipEntry>();
        foreach(var entry in entries)
        {
            if(!entry.id.EndsWith("_walk_0"))continue;
            string prefix=entry.id.Substring(0,entry.id.Length-7);var frames=new Sprite[4];bool valid=true;
            for(int i=0;i<4;i++)if(!sprites.TryGetValue(prefix+"_walk_"+i,out frames[i])){errors.Add(prefix+" missing frame"+i);valid=false;}
            if(!valid)continue;
            string id=prefix+"_walk",path=folder+"/"+id+".anim";
            var clip=AssetDatabase.LoadAssetAtPath<AnimationClip>(path);bool fresh=clip==null;
            if(fresh)clip=new AnimationClip();clip.name=id;clip.frameRate=8;
            var binding=new EditorCurveBinding{type=typeof(SpriteRenderer),path="",propertyName="m_Sprite"};
            var keys=new ObjectReferenceKeyframe[5];
            for(int i=0;i<5;i++)keys[i]=new ObjectReferenceKeyframe{time=i/8f,value=frames[i%4]};
            AnimationUtility.SetObjectReferenceCurve(clip,binding,keys);
            var settings=AnimationUtility.GetAnimationClipSettings(clip);settings.loopTime=true;AnimationUtility.SetAnimationClipSettings(clip,settings);
            if(fresh)AssetDatabase.CreateAsset(clip,path);else EditorUtility.SetDirty(clip);
            var probe=new GameObject("ClipProbe_"+id);var renderer=probe.AddComponent<SpriteRenderer>();probe.AddComponent<Animator>();bool samples=true;
            AnimationMode.StartAnimationMode();
            try
            {
                for(int i=0;i<5;i++)
                {
                    AnimationMode.BeginSampling();
                    AnimationMode.SampleAnimationClip(probe,clip,i/8f);
                    AnimationMode.EndSampling();
                    if(renderer.sprite!=frames[i%4]){errors.Add(id+" sample mismatch at frame"+i);samples=false;}
                }
                if(samples)checks.Add(id+" native clip samples four frames at8fps and wraps to0");
            }
            finally { AnimationMode.StopAnimationMode();Object.DestroyImmediate(probe); }
            clips.Add(new RemainingAssetCatalog.ClipEntry{id=id,clip=clip,frames=frames,fps=8});
        }
        return clips.ToArray();
    }
}
