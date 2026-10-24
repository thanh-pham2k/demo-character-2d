using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Shared by the latest builder and an opt-in, one-shot editor request. No extra menu.
[InitializeOnLoad]
public static class RoomInteractionSceneSetup
{
    private const string PoseFolder = "Assets/Sprites/MVP/characters/interaction";
    private static string Request => Path.Combine(Application.dataPath, "../Temp/RoomInteractionSceneSetup.run");

    static RoomInteractionSceneSetup()
    {
        if (!Application.isBatchMode && File.Exists(Request)) EditorApplication.delayCall += ApplyRequestedSetup;
    }

    private static void ApplyRequestedSetup()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            throw new InvalidOperationException("Stop Play Mode before applying room interactions.");
        var scene = SceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/Room404_MVP.unity") throw new InvalidOperationException("Open Room404_MVP first.");
        Configure(scene);
        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(scene);
        if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save interaction integration.");
        File.Delete(Request);
        Debug.Log("Room interaction integration complete: existing door, two chairs, two beds, existing prompt API.");
    }

    public static void Configure(Scene scene)
    {
        GameObject player = Find(scene, "Player_Luon");
        var state = PlayerInteractionStateController.GetOrCreate(player);
        var visual = player.GetComponentInChildren<LuonCharacterVisual>(true);
        visual.sitLaptopSprite = LoadPose("luon_sitting_chair.png", 1.9f, new Vector2(0.5f, 0.22f));
        visual.lyingSprite = LoadPose("luon_lying.png", 1.8f, new Vector2(0.5f, 0.5f));

        // Assign Bed Animation Sprites
        string bedFolder = "Assets/Sprites/MVP/characters/bed_interaction";
        visual.bedEnterSprites = new Sprite[]
        {
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_enter_01.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_enter_02.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_enter_03.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_enter_04.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_lying_back.png")
        };
        visual.bedIdleSprites = new Sprite[]
        {
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_lying_back.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_lying_left.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_lying_back.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_lying_right.png")
        };
        visual.bedSleepSprite = AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_sleeping.png");
        visual.bedExitSprites = new Sprite[]
        {
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_lying_back.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_enter_04.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_enter_03.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_enter_02.png"),
            AssetDatabase.LoadAssetAtPath<Sprite>(bedFolder + "/luon_bed_enter_01.png")
        };

        Transform shadow = player.transform.Find("ContactShadow");
        visual.ConfigurePoseShadow(shadow != null ? shadow.GetComponent<SpriteRenderer>() : null);
        EditorUtility.SetDirty(visual);

        var desk = Find(scene, "Study_Desk").GetComponent<SpriteRenderer>();
        Transform existingSeat = Find(scene, "Desk_Seat_Marker").transform;
        ConfigureChair(desk, "Chair_Left", new Vector2(220f, 245f), existingSeat, state, false);
        ConfigureChair(desk, "Chair_Right", new Vector2(490f, 220f), null, state, true);
        ConfigureBed(Find(scene, "BunkBed_Left"), state, true);
        ConfigureBed(Find(scene, "BunkBed_Right"), state, false);
        Find(scene, "Room_Door").GetComponent<HingedRoomDoor>().SetPreviewAngle(0f);
        CreatePrompt(Find(scene, "Canvas_MVP").transform, player.GetComponent<PlayerInteractionDetector>());
    }

    private static GameObject Find(Scene scene, string name)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
            foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
                if (child.name == name) return child.gameObject;
        throw new InvalidOperationException("Missing scene object: " + name);
    }

    private static Transform Child(Transform parent, string name)
    {
        Transform child = parent.Find(name);
        if (child != null) return child;
        child = new GameObject(name).transform;
        child.SetParent(parent, false);
        return child;
    }

    private static Sprite LoadPose(string filename, float height, Vector2 pivot)
    {
        string path = PoseFolder + "/" + filename;
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null) throw new InvalidOperationException("Missing interaction pose: " + path);
        var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritePixelsPerUnit = texture.height / height;
        importer.mipmapEnabled = false;
        importer.alphaIsTransparency = true;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        var settings = new TextureImporterSettings();
        importer.ReadTextureSettings(settings);
        settings.spriteAlignment = (int)SpriteAlignment.Custom;
        settings.spritePivot = pivot;
        importer.SetTextureSettings(settings);
        importer.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static void ConfigureChair(SpriteRenderer desk, string name, Vector2 pixel,
        Transform reusableSeat, PlayerInteractionStateController player, bool faceLeft)
    {
        Transform chair = Child(desk.transform, name);
        float ppu = desk.sprite.pixelsPerUnit;
        float x = (pixel.x - desk.sprite.pivot.x) / ppu;
        float y = (desk.sprite.pivot.y - pixel.y) / ppu;
        float shear = desk.sharedMaterial.HasProperty("_ShearY") ? desk.sharedMaterial.GetFloat("_ShearY") : 0f;
        chair.localPosition = new Vector3(x, y + shear * x, 0f);
        Transform seat = reusableSeat != null ? reusableSeat : Child(chair, "SeatPivot");
        seat.SetParent(chair, false);
        seat.localPosition = Vector3.zero;
        seat.localRotation = Quaternion.identity;
        Transform exit = Child(chair, "ExitPivot");
        // Marker authoring is relative to existing geometry; runtime components contain no room coordinates.
        exit.position = chair.position + Vector3.down * player.CharacterVisual.GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;
        var collider = chair.GetComponent<CircleCollider2D>();
        if (collider == null) collider = chair.gameObject.AddComponent<CircleCollider2D>();
        collider.radius = 0.65f;
        collider.isTrigger = true;
        var interaction = chair.GetComponent<ChairInteractable>();
        if (interaction == null) interaction = chair.gameObject.AddComponent<ChairInteractable>();
        interaction.Configure(seat, exit, player, desk, faceLeft);
        EditorUtility.SetDirty(interaction);
    }

    private static void ConfigureBed(GameObject bed, PlayerInteractionStateController player, bool left)
    {
        var original = bed.GetComponent<SpriteRenderer>();
        Transform back = Child(bed.transform, "Bed_Back");
        var backRenderer = back.GetComponent<SpriteRenderer>();
        if (backRenderer == null) backRenderer = back.gameObject.AddComponent<SpriteRenderer>();
        backRenderer.sprite = original.sprite;
        backRenderer.sharedMaterial = original.sharedMaterial;
        backRenderer.sortingLayerID = original.sortingLayerID;
        backRenderer.sortingOrder = original.sortingOrder;
        original.enabled = false;

        Sprite sprite = original.sprite;
        Rect crop = new Rect(0f, 0f, sprite.rect.width, Mathf.Round(sprite.rect.height * 0.30f));
        const string path = "Assets/Room404Layout/BedForeground.asset";
        Sprite foregroundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (foregroundSprite == null)
        {
            foregroundSprite = Sprite.Create(sprite.texture, crop, new Vector2(0.5f, 0.5f),
                sprite.pixelsPerUnit, 0, SpriteMeshType.FullRect);
            foregroundSprite.name = "OriginalBedFootForeground";
            AssetDatabase.CreateAsset(foregroundSprite, path);
        }
        Transform foreground = Child(bed.transform, "Bed_Foreground");
        foreground.localPosition = new Vector3((crop.center.x - sprite.pivot.x) / sprite.pixelsPerUnit,
            (crop.center.y - sprite.pivot.y) / sprite.pixelsPerUnit, 0f);
        var foregroundRenderer = foreground.GetComponent<SpriteRenderer>();
        if (foregroundRenderer == null) foregroundRenderer = foreground.gameObject.AddComponent<SpriteRenderer>();
        foregroundRenderer.sprite = foregroundSprite;
        foregroundRenderer.sharedMaterial = original.sharedMaterial;
        foregroundRenderer.sortingLayerID = original.sortingLayerID;
        foregroundRenderer.sortingOrder = original.sortingOrder + 2;
        foregroundRenderer.enabled = false;

        Transform pose = Child(bed.transform, "BedPivot");
        pose.localPosition = new Vector3((sprite.rect.width * 0.543f - sprite.pivot.x) / sprite.pixelsPerUnit,
            (sprite.rect.height * 0.334f - sprite.pivot.y) / sprite.pixelsPerUnit, 0f);
        pose.rotation = Quaternion.Euler(0f, 0f, left ? 20f : -20f);
        Transform exit = Child(bed.transform, "ExitPivot");
        var bodyCollider = bed.GetComponent<BoxCollider2D>();
        Physics2D.SyncTransforms();
        exit.position = bed.transform.position + new Vector3(
            (left ? 1f : -1f) * bodyCollider.bounds.size.x * 0.95f, -bodyCollider.bounds.size.y * 0.7f, 0f);
        Transform interactionRoot = Child(bed.transform, "Bed_Interaction");
        interactionRoot.position = exit.position;
        var trigger = interactionRoot.GetComponent<CircleCollider2D>();
        if (trigger == null) trigger = interactionRoot.gameObject.AddComponent<CircleCollider2D>();
        trigger.isTrigger = true;
        trigger.radius = 1.2f;
        var interaction = interactionRoot.GetComponent<BedInteractable>();
        if (interaction == null) interaction = interactionRoot.gameObject.AddComponent<BedInteractable>();
        interaction.Configure(pose, exit, player, backRenderer, foregroundRenderer,
            bed.GetComponent<InteractableProp>(), !left);
        EditorUtility.SetDirty(interaction);
    }

    private static void CreatePrompt(Transform canvas, PlayerInteractionDetector detector)
    {
        Transform existing = canvas.Find("InteractionPrompt");
        GameObject panel = existing != null ? existing.gameObject : new GameObject("InteractionPrompt", typeof(RectTransform));
        panel.transform.SetParent(canvas, false);
        var rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.anchoredPosition = new Vector2(0f, 25f);
        rect.sizeDelta = new Vector2(600f, 55f);
        var text = panel.GetComponent<TextMeshProUGUI>();
        if (text == null) text = panel.AddComponent<TextMeshProUGUI>();
        text.fontSize = 24f;
        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color(0.08f, 0.1f, 0.08f, 1f);
        text.fontStyle = FontStyles.Bold;
        text.raycastTarget = false;
        text.text = string.Empty;
        detector.ConfigurePrompt(panel, text);
        panel.SetActive(false);
        EditorUtility.SetDirty(detector);
    }
}

