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
    private static string Request => Path.Combine(Application.dataPath, "../Temp/RoomInteractionSceneSetup.run");

    static RoomInteractionSceneSetup()
    {
        if (!Application.isBatchMode && File.Exists(Request)) EditorApplication.delayCall += ApplyRequestedSetup;
    }

    private static void ApplyRequestedSetup()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            EditorApplication.playModeStateChanged -= ApplyAfterPlayMode;
            EditorApplication.playModeStateChanged += ApplyAfterPlayMode;
            return;
        }
        var scene = SceneManager.GetActiveScene();
        if (scene.path != "Assets/Scenes/Room404_MVP.unity") throw new InvalidOperationException("Open Room404_MVP first.");
        Configure(scene);
        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(scene);
        if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save interaction integration.");
        File.Delete(Request);
        Debug.Log("Room integration complete: door, chairs, beds, wall hook, desk actions, hallway, campus and bus 33.");
    }

    private static void ApplyAfterPlayMode(PlayModeStateChange change)
    {
        if (change != PlayModeStateChange.EnteredEditMode) return;
        EditorApplication.playModeStateChanged -= ApplyAfterPlayMode;
        if (File.Exists(Request)) EditorApplication.delayCall += ApplyRequestedSetup;
    }

    public static void Configure(Scene scene)
    {
        GameObject player = Find(scene, "Player_Luon");
        var state = PlayerInteractionStateController.GetOrCreate(player);
        var visual = player.GetComponentInChildren<LuonCharacterVisual>(true);
        visual.sitLaptopSprite = LoadBatchSprite("characters/room_visual_v2/sitting.png", new Vector2(.5f, .5f));
        visual.lyingSprite = LoadBatchSprite("characters/room_visual_v2/lying_a.png", new Vector2(.5f, .5f));
        Sprite enter1 = LoadBatchSprite("characters/room_visual_v2/enter_01.png", new Vector2(.5f, .5f));
        Sprite enter2 = LoadBatchSprite("characters/room_visual_v2/enter_02.png", new Vector2(.5f, .5f));
        Sprite enter3 = LoadBatchSprite("characters/room_visual_v2/enter_03.png", new Vector2(.5f, .5f));
        Sprite breath = LoadBatchSprite("characters/room_visual_v2/lying_b.png", new Vector2(.5f, .5f));
        visual.bedEnterSprites = new[] { enter1, enter2, enter3, visual.lyingSprite };
        visual.bedIdleSprites = new[] { visual.lyingSprite, breath };
        visual.bedSleepSprite = LoadBatchSprite("characters/room_visual_v2/sleeping.png", new Vector2(.5f, .5f));
        visual.bedExitSprites = new[] { visual.lyingSprite, enter3, enter2, enter1 };
        visual.bedAnimFrameRate = 4f;
        // LuonCharacterVisual owns sprite animation. Do not add an Animator writer to this hierarchy.
        var animator = visual.GetComponent<Animator>();
        if (animator != null) animator.enabled = false;

        Transform shadow = player.transform.Find("ContactShadow");
        visual.ConfigurePoseShadow(shadow != null ? shadow.GetComponent<SpriteRenderer>() : null);
        EditorUtility.SetDirty(visual);
        BedAnimationClipGenerator.GenerateClips();

        var desk = Find(scene, "Study_Desk").GetComponent<SpriteRenderer>();
        Transform existingSeat = Find(scene, "Desk_Seat_Marker").transform;
        ConfigureChair(desk, "Chair_Left", new Vector2(220f, 245f), existingSeat, state, false);
        ConfigureChair(desk, "Chair_Right", new Vector2(490f, 220f), null, state, true);
        ConfigureBed(Find(scene, "BunkBed_Left"), state, true);
        ConfigureBed(Find(scene, "BunkBed_Right"), state, false);
        var door = Find(scene, "Room_Door").GetComponent<HingedRoomDoor>();
        // Detection must survive disabling the physical leaf collider when the door opens.
        Transform doorTrigger = Child(door.transform, "Door_Interaction");
        doorTrigger.localPosition = new Vector3(-.64f, 0f, 0f);
        var doorInteractionCollider = doorTrigger.GetComponent<CircleCollider2D>();
        if (doorInteractionCollider == null) doorInteractionCollider = doorTrigger.gameObject.AddComponent<CircleCollider2D>();
        doorInteractionCollider.isTrigger = true;
        doorInteractionCollider.radius = .6f;
        var opening = door.transform.Find("Doorway_Opening").GetComponent<SpriteRenderer>();
        opening.sprite = LoadBatchSprite("environment/room_visual_v2/hallway.png", Vector2.zero);
        opening.color = Color.white;
        opening.transform.localScale = Vector3.one;
        var leaf = door.transform.Find("Door_Leaf").GetComponent<SpriteRenderer>();
        Transform doorShadow = Child(door.transform, "Door_Shadow");
        var shadowRenderer = doorShadow.GetComponent<SpriteRenderer>();
        if (shadowRenderer == null) shadowRenderer = doorShadow.gameObject.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/shadows/shadow_small.png");
        shadowRenderer.color = new Color(0f, 0f, 0f, .22f);
        shadowRenderer.sortingLayerID = leaf.sortingLayerID;
        shadowRenderer.sortingOrder = leaf.sortingOrder - 1;
        door.ConfigureShadow(shadowRenderer);
        door.SetPreviewAngle(0f);
        CreatePrompt(Find(scene, "Canvas_MVP").transform, player.GetComponent<PlayerInteractionDetector>());
        DormJourneySceneSetup.Configure(scene);
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

    private static Sprite LoadBatchSprite(string relative, Vector2 pivot)
    {
        string path = "Assets/Sprites/MVP/" + relative;
        var importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null) throw new InvalidOperationException("Missing batch asset: " + path);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritePixelsPerUnit = 100f;
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
        SpriteRenderer foreground = ConfigureDeskForeground(desk);
        interaction.Configure(seat, exit, player, foreground, faceLeft, desk);
        EditorUtility.SetDirty(interaction);
    }

    private static SpriteRenderer ConfigureDeskForeground(SpriteRenderer desk)
    {
        Sprite source = desk.sprite;
        // Native crop of the existing chair backs / table front. Upper body stays visible above it.
        Rect rect = new Rect(source.rect.x, source.rect.y, source.rect.width, source.rect.height - 135f);
        const string path = "Assets/Room404Layout/DeskForeground.asset";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite == null)
        {
            sprite = Sprite.Create(source.texture, rect, new Vector2(.5f, .5f), source.pixelsPerUnit, 0, SpriteMeshType.FullRect);
            sprite.name = "OriginalDeskChairForeground";
            AssetDatabase.CreateAsset(sprite, path);
        }
        Transform child = Child(desk.transform, "Desk_Foreground");
        Vector2 center = (rect.center - source.pivot) / source.pixelsPerUnit;
        float shear = desk.sharedMaterial.HasProperty("_ShearY") ? desk.sharedMaterial.GetFloat("_ShearY") : 0f;
        child.localPosition = new Vector3(center.x, center.y + shear * center.x, 0f);
        var renderer = child.GetComponent<SpriteRenderer>();
        if (renderer == null) renderer = child.gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sharedMaterial = desk.sharedMaterial;
        renderer.sortingLayerID = desk.sortingLayerID;
        renderer.sortingOrder = desk.sortingOrder + 2;
        return renderer;
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
        Rect crop = new Rect(0f, 0f, sprite.rect.width, Mathf.Round(sprite.rect.height * 0.22f));
        const string path = "Assets/Room404Layout/BedForegroundV2.asset";
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
        pose.localPosition = new Vector3((sprite.rect.width * 0.52f - sprite.pivot.x) / sprite.pixelsPerUnit,
            (sprite.rect.height * 0.275f - sprite.pivot.y) / sprite.pixelsPerUnit, 0f);
        pose.rotation = Quaternion.Euler(0f, 0f, left ? 10f : -10f);
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
