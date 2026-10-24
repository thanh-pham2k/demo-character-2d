using System.IO;
using UnityEditor;
using UnityEngine;

public static class BedAnimationClipGenerator
{
    public static void GenerateClips()
    {
        string spriteFolder = "Assets/Sprites/MVP/characters/bed_interaction";
        string animFolder = "Assets/Animations/BedInteraction";
        Directory.CreateDirectory(animFolder);

        Sprite enter1 = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(spriteFolder, "luon_bed_enter_01.png"));
        Sprite enter2 = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(spriteFolder, "luon_bed_enter_02.png"));
        Sprite enter3 = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(spriteFolder, "luon_bed_enter_03.png"));
        Sprite enter4 = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(spriteFolder, "luon_bed_enter_04.png"));

        Sprite lyingBack = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(spriteFolder, "luon_bed_lying_back.png"));
        Sprite lyingLeft = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(spriteFolder, "luon_bed_lying_left.png"));
        Sprite lyingRight = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(spriteFolder, "luon_bed_lying_right.png"));
        Sprite sleeping = AssetDatabase.LoadAssetAtPath<Sprite>(Path.Combine(spriteFolder, "luon_bed_sleeping.png"));

        // 1. Luon_Bed_Enter Clip
        CreateSpriteClip(Path.Combine(animFolder, "Luon_Bed_Enter.anim"), 6f, false,
            new Sprite[] { enter1, enter2, enter3, enter4, lyingBack });

        // 2. Luon_Bed_Idle Clip
        CreateSpriteClip(Path.Combine(animFolder, "Luon_Bed_Idle.anim"), 1f, true,
            new Sprite[] { lyingBack, lyingLeft, lyingBack, lyingRight });

        // 3. Luon_Bed_Sleep Clip
        CreateSpriteClip(Path.Combine(animFolder, "Luon_Bed_Sleep.anim"), 1f, true,
            new Sprite[] { sleeping });

        // 4. Luon_Bed_Exit Clip (Reverse Enter)
        CreateSpriteClip(Path.Combine(animFolder, "Luon_Bed_Exit.anim"), 6f, false,
            new Sprite[] { lyingBack, enter4, enter3, enter2, enter1 });

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[BedAnimationClipGenerator] Generated 4 Bed Animation Clips successfully!");
    }

    private static void CreateSpriteClip(string path, float frameRate, bool loop, Sprite[] frames)
    {
        AnimationClip clip = new AnimationClip();
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
        AssetDatabase.CreateAsset(clip, path);
    }
}
