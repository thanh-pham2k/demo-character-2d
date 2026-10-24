using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.SceneManagement;

public class PrototypeSceneBuilder : EditorWindow
{
    [MenuItem("Tools/Build Character Test Scene")]
    public static void BuildScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // 1. Camera
        GameObject camGo = new GameObject("Main Camera");
        Camera cam = camGo.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 7.5f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.12f, 0.14f, 0.18f);
        camGo.transform.position = new Vector3(0, 0, -10f);
        CameraController camController = camGo.AddComponent<CameraController>();
        camGo.tag = "MainCamera";

        // 2. Ground Grid Visual
        GameObject groundGo = new GameObject("GroundGrid");
        groundGo.transform.position = Vector3.zero;

        // 3. Slash VFX Prefab
        GameObject vfxGo = new GameObject("SlashVFX_Prefab");
        SpriteRenderer vfxSr = vfxGo.AddComponent<SpriteRenderer>();
        vfxSr.sortingOrder = 10;
        vfxGo.transform.localScale = new Vector3(2.5f, 2.0f, 1f); // Huge slash 2.5x
        SlashVFX slashComponent = vfxGo.AddComponent<SlashVFX>();

        Sprite[] slashes = new Sprite[4];
        for (int i = 0; i < 4; i++)
        {
            slashes[i] = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/Sprites/VFX/slash_{i}.png");
        }
        
        SerializedObject vfxSo = new SerializedObject(slashComponent);
        SerializedProperty framesProp = vfxSo.FindProperty("slashFrames");
        framesProp.arraySize = 4;
        for (int i = 0; i < 4; i++)
        {
            framesProp.GetArrayElementAtIndex(i).objectReferenceValue = slashes[i];
        }
        vfxSo.ApplyModifiedProperties();

        string prefabPath = "Assets/Prefabs/SlashVFX.prefab";
        GameObject vfxPrefab = PrefabUtility.SaveAsPrefabAsset(vfxGo, prefabPath);
        DestroyImmediate(vfxGo);

        // 4. Create Player
        GameObject player = new GameObject("Player");
        player.transform.position = new Vector3(-3.5f, 0, 0);
        player.tag = "Player";

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        CircleCollider2D col = player.AddComponent<CircleCollider2D>();
        col.radius = 0.5f;
        col.offset = new Vector2(0, -0.2f);

        SpriteRenderer sr = player.AddComponent<SpriteRenderer>();
        Sprite idleSprite0 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Hero/hero_idle_0.png");
        Sprite idleSprite1 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Hero/hero_idle_1.png");
        Sprite walkSprite0 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Hero/hero_walk_0.png");
        Sprite walkSprite1 = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Hero/hero_walk_1.png");
        Sprite atkWindup = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Hero/hero_attack_0.png");
        Sprite atkImpact = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Hero/hero_attack_1.png");
        Sprite dodgeSpr = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Hero/hero_dodge_0.png");
        Sprite hitSpr = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Hero/hero_hit_0.png");

        if (idleSprite0 != null)
        {
            sr.sprite = idleSprite0;
            sr.sortingOrder = 5;
        }

        PlayerMovement movement = player.AddComponent<PlayerMovement>();
        PlayerCombat combat = player.AddComponent<PlayerCombat>();
        PlayerAnimationDriver animDriver = player.AddComponent<PlayerAnimationDriver>();
        animDriver.SetSprites(
            new Sprite[] { idleSprite0, idleSprite1 },
            new Sprite[] { walkSprite0, walkSprite1 },
            atkWindup,
            atkImpact,
            dodgeSpr,
            hitSpr
        );

        camController.SetTarget(player.transform);

        SerializedObject combatSo = new SerializedObject(combat);
        combatSo.FindProperty("slashVFXPrefab").objectReferenceValue = vfxPrefab.GetComponent<SlashVFX>();
        combatSo.FindProperty("enemyLayer").intValue = ~0;
        combatSo.ApplyModifiedProperties();

        // 5. Create Dummy Enemy
        GameObject dummy = new GameObject("TrainingDummy");
        dummy.transform.position = new Vector3(2.5f, 0, 0);

        Rigidbody2D dummyRb = dummy.AddComponent<Rigidbody2D>();
        dummyRb.gravityScale = 0;
        dummyRb.linearDamping = 4.0f;
        dummyRb.freezeRotation = true;

        CircleCollider2D dummyCol = dummy.AddComponent<CircleCollider2D>();
        dummyCol.radius = 0.6f;

        SpriteRenderer dummySr = dummy.AddComponent<SpriteRenderer>();
        dummySr.sprite = hitSpr;
        dummySr.color = new Color(0.9f, 0.6f, 0.6f);
        dummySr.flipX = true;
        dummySr.sortingOrder = 4;

        dummy.AddComponent<DummyEnemy>();

        // 6. UI Canvas
        GameObject canvasGo = new GameObject("Canvas");
        Canvas canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920);
        canvasGo.AddComponent<GraphicRaycaster>();

        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();

        // Joystick
        GameObject joystickGo = new GameObject("VirtualJoystick");
        joystickGo.transform.SetParent(canvasGo.transform, false);
        RectTransform joyRect = joystickGo.AddComponent<RectTransform>();
        joyRect.anchorMin = new Vector2(0, 0);
        joyRect.anchorMax = new Vector2(0, 0);
        joyRect.pivot = new Vector2(0.5f, 0.5f);
        joyRect.anchoredPosition = new Vector2(180, 220);
        joyRect.sizeDelta = new Vector2(220, 220);
        Image joyBg = joystickGo.AddComponent<Image>();
        joyBg.color = new Color(1, 1, 1, 0.2f);

        GameObject handleGo = new GameObject("Handle");
        handleGo.transform.SetParent(joystickGo.transform, false);
        RectTransform handleRect = handleGo.AddComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(90, 90);
        Image handleImg = handleGo.AddComponent<Image>();
        handleImg.color = new Color(1, 1, 1, 0.5f);

        VirtualJoystick vj = joystickGo.AddComponent<VirtualJoystick>();
        SerializedObject vjSo = new SerializedObject(vj);
        vjSo.FindProperty("background").objectReferenceValue = joyRect;
        vjSo.FindProperty("handle").objectReferenceValue = handleRect;
        vjSo.ApplyModifiedProperties();

        // Attack Button
        GameObject atkBtnGo = new GameObject("Btn_Attack");
        atkBtnGo.transform.SetParent(canvasGo.transform, false);
        RectTransform atkRect = atkBtnGo.AddComponent<RectTransform>();
        atkRect.anchorMin = new Vector2(1, 0);
        atkRect.anchorMax = new Vector2(1, 0);
        atkRect.pivot = new Vector2(0.5f, 0.5f);
        atkRect.anchoredPosition = new Vector2(-180, 220);
        atkRect.sizeDelta = new Vector2(140, 140);
        Image atkImg = atkBtnGo.AddComponent<Image>();
        atkImg.color = new Color(0.9f, 0.3f, 0.3f, 0.7f);
        Button atkBtn = atkBtnGo.AddComponent<Button>();

        // Dodge Button
        GameObject dodgeBtnGo = new GameObject("Btn_Dodge");
        dodgeBtnGo.transform.SetParent(canvasGo.transform, false);
        RectTransform dodgeRect = dodgeBtnGo.AddComponent<RectTransform>();
        dodgeRect.anchorMin = new Vector2(1, 0);
        dodgeRect.anchorMax = new Vector2(1, 0);
        dodgeRect.pivot = new Vector2(0.5f, 0.5f);
        dodgeRect.anchoredPosition = new Vector2(-340, 180);
        dodgeRect.sizeDelta = new Vector2(110, 110);
        Image dodgeImg = dodgeBtnGo.AddComponent<Image>();
        dodgeImg.color = new Color(0.3f, 0.6f, 0.9f, 0.7f);
        Button dodgeBtn = dodgeBtnGo.AddComponent<Button>();

        MobileControlsUI ui = canvasGo.AddComponent<MobileControlsUI>();
        SerializedObject uiSo = new SerializedObject(ui);
        uiSo.FindProperty("attackButton").objectReferenceValue = atkBtn;
        uiSo.FindProperty("dodgeButton").objectReferenceValue = dodgeBtn;
        uiSo.FindProperty("playerCombat").objectReferenceValue = combat;
        uiSo.FindProperty("playerMovement").objectReferenceValue = movement;
        uiSo.ApplyModifiedProperties();

        // 7. Auto Simulation
        GameObject simGo = new GameObject("AutoSimulation");
        simGo.AddComponent<AutoSimulationController>();

        EditorSceneManager.SaveScene(scene, "Assets/Scenes/CharacterTestScene.unity");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Rebuilt CharacterTestScene completely!");
    }
}
