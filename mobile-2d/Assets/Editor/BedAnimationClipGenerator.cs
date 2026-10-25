using System.IO;
using UnityEditor;
using UnityEngine;

public static class BedAnimationClipGenerator
{
    public static void GenerateClips()
    {
        var visual = Object.FindAnyObjectByType<LuonCharacterVisual>();
        if (visual == null || visual.bedEnterSprites == null || visual.bedIdleSprites == null || visual.bedSleepSprite == null)
            throw new System.InvalidOperationException("Configure the room batch sprites before exporting animation clips.");
        string animFolder = "Assets/Animations/BedInteraction";
        Directory.CreateDirectory(animFolder);
        CreateSpriteClip(Path.Combine(animFolder, "Luon_Bed_Enter.anim"), visual.bedAnimFrameRate, false, visual.bedEnterSprites);
        CreateSpriteClip(Path.Combine(animFolder, "Luon_Bed_Idle.anim"), 1f / .9f, true, visual.bedIdleSprites);
        CreateSpriteClip(Path.Combine(animFolder, "Luon_Bed_Sleep.anim"), 1f, true, new[] { visual.bedSleepSprite });
        CreateSpriteClip(Path.Combine(animFolder, "Luon_Bed_Exit.anim"), visual.bedAnimFrameRate, false, visual.bedExitSprites);

        AssetDatabase.SaveAssets();
        Debug.Log("[BedAnimationClipGenerator] Generated 4 Bed Animation Clips successfully!");
    }

    private static void CreateSpriteClip(string path, float frameRate, bool loop, Sprite[] frames)
    {
        AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        bool isNew = clip == null;
        if (isNew) clip = new AnimationClip();
        clip.frameRate = frameRate;

        if (loop)
        {
            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
            settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
        }

        EditorCurveBinding binding = new EditorCurveBinding
        {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[frames.Length];
        float timeStep = 1f / frameRate;
        for (int i = 0; i < frames.Length; i++)
        {
            keyframes[i] = new ObjectReferenceKeyframe
            {
                time = i * timeStep,
                value = frames[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);
        if (isNew) AssetDatabase.CreateAsset(clip, path);
        else EditorUtility.SetDirty(clip);
    }
}
