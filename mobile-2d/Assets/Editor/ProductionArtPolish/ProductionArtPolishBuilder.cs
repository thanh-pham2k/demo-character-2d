using System;
using UnityEditor;
using UnityEngine;

namespace ProductionArtPolish
{
    // Explicit editor entrypoint only. No InitializeOnLoad, scene lookup or import callback.
    public static class ProductionArtPolishBuilder
    {
        public const string Output = "Assets/AssetProduction/ParallelArt/Room404NoMatte.asset";
        const string Source = "Assets/Sprites/MVP/environment/room_404_backdrop.png";

        [MenuItem("Tools/Production Art Polish/Build room silhouette mesh (assets only)")]
        public static void BuildRoomSilhouette()
        {
            var source = AssetDatabase.LoadAssetAtPath<Sprite>(Source);
            if (source == null) throw new InvalidOperationException("Missing room sprite: " + Source);
            // Coordinates measured against the inspected 1037 x 627 source.
            // Keep the roof/wall silhouette; discard the two opaque white corner triangles.
            var outline = new[] {
                new Vector2(0, 0), new Vector2(1037, 0),
                new Vector2(1037, 415), new Vector2(891, 627),
                new Vector2(195, 627), new Vector2(0, 415)
            };
            var rect = source.rect;
            var pivot = new Vector2(source.pivot.x / rect.width, source.pivot.y / rect.height);
            var result = Sprite.Create(source.texture, rect, pivot, source.pixelsPerUnit, 0, SpriteMeshType.FullRect);
            result.name = "Room404NoMatte";
            var vertices = new Vector2[outline.Length];
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = (Vector2.Scale(outline[i], new Vector2(rect.width / 1037f, rect.height / 627f)) - source.pivot) / source.pixelsPerUnit;
            result.OverrideGeometry(vertices, new ushort[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5 });
            var existing = AssetDatabase.LoadAssetAtPath<Sprite>(Output);
            if (existing == null) AssetDatabase.CreateAsset(result, Output);
            else { EditorUtility.CopySerialized(result, existing); UnityEngine.Object.DestroyImmediate(result); EditorUtility.SetDirty(existing); }
            AssetDatabase.SaveAssets();
        }

        // Use a dedicated child so animation never moves the actor root/collider/feet pivot.
        // The coordinator supplies measured offsets after the 4-frame in-scene contact test.
        public static AnimationClip CreateVisualOffsetClip(string name, string visualChildPath, Vector2[] offsets, float fps)
        {
            if (string.IsNullOrEmpty(visualChildPath) || offsets == null || offsets.Length != 4 || fps <= 0)
                throw new ArgumentException("A dedicated visual child, four measured offsets and positive fps are required.");
            var clip = new AnimationClip { name = name, frameRate = fps };
            var x = new Keyframe[5]; var y = new Keyframe[5];
            for (int i = 0; i < 5; i++) {
                var offset = offsets[i % 4];
                x[i] = new Keyframe(i / fps, offset.x); y[i] = new Keyframe(i / fps, offset.y);
            }
            clip.SetCurve(visualChildPath, typeof(Transform), "m_LocalPosition.x", new AnimationCurve(x));
            clip.SetCurve(visualChildPath, typeof(Transform), "m_LocalPosition.y", new AnimationCurve(y));
            foreach (var binding in AnimationUtility.GetCurveBindings(clip)) {
                var curve = AnimationUtility.GetEditorCurve(clip, binding);
                for (int i = 0; i < curve.length; i++) AnimationUtility.SetKeyRightTangentMode(curve, i, AnimationUtility.TangentMode.Constant);
                AnimationUtility.SetEditorCurve(clip, binding, curve);
            }
            var settings = AnimationUtility.GetAnimationClipSettings(clip); settings.loopTime = true;
            AnimationUtility.SetAnimationClipSettings(clip, settings);
            return clip;
        }

        // Align an authored prop grip (pixels from bottom-left of sprite rect) to a hand anchor.
        // Caller owns animation/visibility and must not claim this removes baked bowl/utensils.
        public static SpriteRenderer AttachProp(Transform handAnchor, Sprite sprite, Vector2 gripPixels, float uniformScale, int sortingOrder)
        {
            if (handAnchor == null || sprite == null || uniformScale <= 0) throw new ArgumentException("Invalid contact setup.");
            var node = new GameObject("ArtPolish_" + sprite.name);
            node.transform.SetParent(handAnchor, false);
            node.transform.localScale = Vector3.one * uniformScale;
            node.transform.localPosition = -(Vector3)((gripPixels - sprite.pivot) / sprite.pixelsPerUnit * uniformScale);
            var renderer = node.AddComponent<SpriteRenderer>(); renderer.sprite = sprite; renderer.sortingOrder = sortingOrder;
            return renderer;
        }
    }
}
