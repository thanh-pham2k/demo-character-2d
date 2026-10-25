using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Idempotent integration, invoked by the existing Room 404 builder/setup.
public static class DormJourneySceneSetup
{
    private const string Folder = "Assets/Sprites/MVP/dorm_journey/";
    public static void Configure(Scene scene)
    {
        var player = Find(scene, "Player_Luon").GetComponent<PlayerInteractionStateController>();
        var flow = Find(scene, "MVP_Flow_Controller").GetComponent<MVPFlowController>();
        var root = Root(scene, "Dorm_Journey");
        var journey = Component<DormJourneyController>(root.gameObject);
        var hall = Root(scene, "Hallway_Zone");
        hall.position = new Vector3(24f, 0f, 0f);
        var bgTransform = Child(hall, "Hallway_Backdrop");
        var bg = Component<SpriteRenderer>(bgTransform.gameObject);
        bg.sprite = Load("hallway.png", new Vector2(.5f, .5f));
        bg.sortingOrder = -100;
        float scale = 18f / bg.sprite.bounds.size.x;
        bgTransform.localScale = new Vector3(scale, scale, 1f);
        ConfigureRailing(bg);
        // Capsule roots walk between y≈0 and .5; visual feet sit on the tiled strip.
        Wall(hall, "Back_Wall", new Vector2(0f, .65f), new Vector2(18f, .15f));
        Wall(hall, "Balcony_Rail_Blocker", new Vector2(0f, -.75f), new Vector2(18f, .15f));
        Wall(hall, "West_Boundary", new Vector2(-9f, 0f), new Vector2(.2f, 2f));
        Wall(hall, "East_Boundary", new Vector2(9f, 0f), new Vector2(.2f, 2f));
        Transform hallSpawn = Child(hall, "Room404_ExitPivot"); hallSpawn.localPosition = new Vector3(-5.7f, .1f, 0f);
        Transform roomSpawn = Child(root, "Room_Arrival"); roomSpawn.position = new Vector3(1.975f, -.9f, 0f);
        var door = Find(scene, "Room_Door").GetComponent<HingedRoomDoor>();
        Portal(door.transform, "DoorwayTrigger", new Vector3(-.64f, .15f, 0f), true, journey, door);
        Portal(hall, "Room404_ReturnTrigger", new Vector3(-5.7f, .35f, 0f), false, journey, door);
        WorldLabel(hall, "Room404_Label", "404  ·  PHÒNG CỦA LƯƠN", new Vector3(-5.7f, 2.4f), new Color(.08f,.17f,.18f));
        Restroom(hall, "WC_Nam", new Vector3(2.65f, -.25f), RestroomInteractable.Gender.Male, journey, "wc_male.png");
        Restroom(hall, "WC_Nu", new Vector3(5.85f, -.25f), RestroomInteractable.Gender.Female, journey, "wc_female.png");
        var stairs = Child(hall, "Stairs_Trigger"); stairs.localPosition = new Vector3(8.1f, .1f, 0f);
        Trigger(stairs, new Vector2(.75f, .8f));
        Component<DormStairsInteractable>(stairs.gameObject).Configure(journey);
        WorldLabel(hall, "Stairs_Label", "CẦU THANG ↓\nSẢNH KTX / XE 33", new Vector3(7.4f, 2.5f), new Color(.08f,.17f,.18f));

        var canvas = Root(scene, "Canvas_DormJourney");
        Component<RectTransform>(canvas.gameObject);
        var uiCanvas = Component<Canvas>(canvas.gameObject); uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay; uiCanvas.sortingOrder = 30;
        var scaler = Component<CanvasScaler>(canvas.gameObject); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280, 720); scaler.matchWidthOrHeight = .5f;
        Component<GraphicRaycaster>(canvas.gameObject);
        var missionPanel = Panel(canvas, "Journey_HUD", new Vector2(.5f,1f), new Vector2(0,-28), new Vector2(650,42), new Color(.06f,.12f,.16f,.92f));
        var objective = Text(missionPanel, "Mission", "Chuẩn bị đi học", 20);
        var noticePanel = Panel(canvas, "Notice", new Vector2(.5f,0f), new Vector2(0,85), new Vector2(900,55), Color.clear);
        noticePanel.anchorMin = new Vector2(.05f,0f); noticePanel.anchorMax = new Vector2(.95f,0f); noticePanel.sizeDelta = new Vector2(0f,55f);
        var notice = Text(noticePanel, "Message", "", 22); notice.color = new Color(1f,.92f,.6f);
        noticePanel.GetComponent<Image>().color = new Color(.06f,.1f,.14f,.9f);
        var modalRoot = Child(canvas, "DeskActionModal");
        var modalRect = modalRoot.GetComponent<RectTransform>();
        modalRect.anchorMin = Vector2.zero; modalRect.anchorMax = Vector2.one; modalRect.offsetMin = modalRect.offsetMax = Vector2.zero;
        var modal = Component<DeskActionModal>(modalRoot.gameObject);
        var panel = Panel(modalRoot, "Panel", new Vector2(1f,.5f), new Vector2(-220,-30), new Vector2(400,335), new Color(.05f,.1f,.14f,.97f));
        LabelAt(panel, "Heading", "BÀN HỌC PHÒNG 404", new Vector2(0,139), new Vector2(370,36), 24);
        var status = LabelAt(panel, "Status", "Chọn hoạt động tại bàn học", new Vector2(0,101), new Vector2(365,38), 18);
        var computer = Button(panel,"Computer","[1] Dùng máy tính",new Vector2(0,55));
        var study = Button(panel,"Study","[2] Học bài",new Vector2(0,1));
        var stand = Button(panel,"Stand","[E] Đứng dậy",new Vector2(0,-137));
        var book = Panel(panel, "Book_Icon", new Vector2(.5f,.5f), new Vector2(-110,-68), new Vector2(70,55), new Color(.87f,.77f,.5f));
        Panel(book, "Left_Page", new Vector2(.5f,.5f), new Vector2(-17,2), new Vector2(30,46), new Color(1f,.97f,.83f));
        Panel(book, "Right_Page", new Vector2(.5f,.5f), new Vector2(17,2), new Vector2(30,46), new Color(1f,.97f,.83f));
        LabelAt(book, "Title", "GIẢI TÍCH", new Vector2(0,0), new Vector2(68,35), 12).color = new Color(.12f,.18f,.22f);
        var bar = Panel(panel, "Study_Progress", new Vector2(.5f,.5f), new Vector2(48,-75), new Vector2(215,18), new Color(.2f,.25f,.29f));
        var fill = Panel(bar,"Fill",new Vector2(0f,.5f),Vector2.zero,new Vector2(215,18),new Color(.35f,.8f,.6f));
        fill.pivot = new Vector2(0f,.5f);
        fill.anchorMin = Vector2.zero; fill.anchorMax = Vector2.one; fill.offsetMin = fill.offsetMax = Vector2.zero;
        var progress = Component<Slider>(bar.gameObject); progress.fillRect = fill; progress.interactable = false; progress.value = 0f;
        LabelAt(panel,"Progress_Label","Tiến độ học tập",new Vector2(50,-49),new Vector2(220,26),16);
        var typing = Component<AudioSource>(modalRoot.gameObject); typing.playOnAwake = false;
        modal.Configure(panel.gameObject, book.gameObject, status, progress, computer, study, stand, flow.laptopLight, typing, journey, flow);
        foreach (var chair in UnityEngine.Object.FindObjectsByType<ChairInteractable>()) { chair.ConfigureMenu(modal); EditorUtility.SetDirty(chair); }
        var overlay = Panel(canvas, "Transition_Fade", new Vector2(.5f,.5f), Vector2.zero, Vector2.zero, Color.black);
        overlay.anchorMin = Vector2.zero; overlay.anchorMax = Vector2.one; overlay.offsetMin = overlay.offsetMax = Vector2.zero;
        overlay.GetComponent<Image>().raycastTarget = true;
        var fade = Component<CanvasGroup>(overlay.gameObject); fade.alpha = 0f; fade.blocksRaycasts = false;
        var water = Component<AudioSource>(root.gameObject); water.playOnAwake = false;
        journey.Configure(player, Camera.main, hallSpawn, roomSpawn, bg, fade, objective, notice,
            Find(scene, "ObjectivePanel"), Find(scene, "TopInfoPanel"), water, flow);
        var washPanel=Panel(canvas,"Wash_Progress_Panel",new Vector2(.5f,.5f),Vector2.zero,new Vector2(540,130),new Color(.04f,.1f,.14f,.96f));
        var washLabel=LabelAt(washPanel,"Status","Đang vệ sinh cá nhân…",new Vector2(0,30),new Vector2(510,50),23);
        var washBar=Panel(washPanel,"Progress",new Vector2(.5f,.5f),new Vector2(0,-25),new Vector2(460,24),new Color(.2f,.25f,.29f));
        var washFill=Panel(washBar,"Fill",new Vector2(0,.5f),Vector2.zero,new Vector2(460,24),new Color(.3f,.78f,.65f));
        washFill.anchorMin=Vector2.zero;washFill.anchorMax=Vector2.one;washFill.offsetMin=washFill.offsetMax=Vector2.zero;
        var washSlider=Component<Slider>(washBar.gameObject);washSlider.fillRect=washFill;washSlider.interactable=false;washSlider.value=0;
        journey.ConfigureWashProgress(washSlider,washLabel);washPanel.gameObject.SetActive(false);
        player.CharacterVisual.ConfigureBackpackSprites(Load("backpack_idle.png", new Vector2(.5f,.5f)),
            new[] { Load("backpack_walk_0.png", new Vector2(.5f,.5f)), Load("backpack_walk_1.png", new Vector2(.5f,.5f)), Load("backpack_walk_2.png", new Vector2(.5f,.5f)) });
        panel.gameObject.SetActive(false);
        book.gameObject.SetActive(false);
        noticePanel.gameObject.SetActive(false);
        CommuteSceneSetup.Configure(scene, journey, player, flow, canvas, modal, hall);
        EditorUtility.SetDirty(journey); EditorUtility.SetDirty(modal); EditorUtility.SetDirty(player.CharacterVisual);
    }
    private static void ConfigureRailing(SpriteRenderer background)
    {
        const string path = "Assets/Room404Layout/HallwayRailing.asset";
        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        Rect rect = new Rect(0,0,background.sprite.rect.width,background.sprite.rect.height * .38f);
        if (sprite == null) { sprite = Sprite.Create(background.sprite.texture,rect,new Vector2(.5f,.5f),100f,0,SpriteMeshType.FullRect); AssetDatabase.CreateAsset(sprite,path); }
        var child = Child(background.transform,"Balcony_Foreground");
        child.localPosition = new Vector3(0,(rect.center.y-background.sprite.pivot.y)/100f,0);
        var renderer = Component<SpriteRenderer>(child.gameObject); renderer.sprite = sprite; renderer.sortingOrder = 20;
    }
    private static void Restroom(Transform hall, string name, Vector3 position, RestroomInteractable.Gender gender, DormJourneyController journey, string file)
    {
        var door = Child(hall,name); door.localPosition = position;
        var sr = Component<SpriteRenderer>(door.gameObject); sr.sprite = Load(file,new Vector2(.5f,0f)); sr.sortingOrder = -80;
        Trigger(door,new Vector2(1.1f,.5f));
        var exit = Child(door,"ExitPivot"); exit.localPosition = new Vector3(0f,.35f,0f);
        var restroom=Component<RestroomInteractable>(door.gameObject);restroom.Configure(gender,journey,exit);
        if(gender==RestroomInteractable.Gender.Male)
        {
            var bounds=sr.sprite.bounds;
            var leaf=Child(door,"Door_Leaf_Pivot");leaf.localPosition=new Vector3(-bounds.extents.x,0);leaf.localScale=Vector3.one;
            var art=Child(leaf,"Door_Art");art.localPosition=new Vector3(bounds.extents.x,0);
            var leafRenderer=Component<SpriteRenderer>(art.gameObject);leafRenderer.sprite=sr.sprite;leafRenderer.sortingOrder=-80;
            var interior=Child(door,"Door_Opening");interior.localPosition=bounds.center;
            var interiorRenderer=Component<SpriteRenderer>(interior.gameObject);
            interiorRenderer.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Room404Layout/DoorOpeningSprite.asset");
            interior.localScale=new Vector3(bounds.size.x/interiorRenderer.sprite.bounds.size.x,bounds.size.y/interiorRenderer.sprite.bounds.size.y,1);
            interior.localPosition=bounds.center-Vector3.Scale(interiorRenderer.sprite.bounds.center,interior.localScale);
            interiorRenderer.color=new Color(.12f,.15f,.14f);interiorRenderer.sortingOrder=-81;interiorRenderer.enabled=false;
            sr.enabled=false;restroom.ConfigureDoor(leaf,interiorRenderer);
        }
    }
    private static void Portal(Transform parent,string name,Vector3 position,bool outward,DormJourneyController journey,HingedRoomDoor door)
    {
        var trigger = Child(parent,name); trigger.localPosition = position; Trigger(trigger,new Vector2(1.1f,.5f));
        Component<DormDoorwayTrigger>(trigger.gameObject).Configure(journey,door,outward);
    }
    internal static void Wall(Transform parent,string name,Vector2 position,Vector2 size)
    { var t=Child(parent,name);t.localPosition=position;Component<BoxCollider2D>(t.gameObject).size=size; }
    internal static void Trigger(Transform transform,Vector2 size)
    { var collider=Component<BoxCollider2D>(transform.gameObject);collider.isTrigger=true;collider.size=size; }
    internal static void WorldLabel(Transform parent,string name,string value,Vector3 position,Color color)
    {
        var t=Child(parent,name); t.localPosition=position;
        var label=Component<TextMeshPro>(t.gameObject);label.text=value;label.fontSize=2f;label.alignment=TextAlignmentOptions.Center;
        label.color=color;label.rectTransform.sizeDelta=new Vector2(2.8f,.65f);label.GetComponent<MeshRenderer>().sortingOrder=5;
    }
    private static Sprite Load(string name,Vector2 pivot)
    {
        return LoadSprite(Folder+name,pivot);
    }
    internal static Sprite LoadSprite(string path, Vector2 pivot)
    {
        var importer=AssetImporter.GetAtPath(path) as TextureImporter;
        if(importer==null)throw new InvalidOperationException("Missing journey batch sprite: "+path);
        importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=SpriteImportMode.Single;importer.spritePixelsPerUnit=100;
        importer.filterMode=FilterMode.Point;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.mipmapEnabled=false;importer.alphaIsTransparency=true;
        var settings=new TextureImporterSettings();importer.ReadTextureSettings(settings);settings.spriteAlignment=(int)SpriteAlignment.Custom;settings.spritePivot=pivot;importer.SetTextureSettings(settings);importer.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }
    internal static T Component<T>(GameObject obj) where T:Component { var c=obj.GetComponent<T>();return c!=null?c:obj.AddComponent<T>(); }
    internal static Transform Child(Transform parent,string name)
    { var t=parent.Find(name);if(t!=null)return t;t=(parent is RectTransform ? new GameObject(name, typeof(RectTransform)) : new GameObject(name)).transform;t.SetParent(parent,false);return t; }
    internal static Transform Root(Scene scene,string name)
    { foreach(var go in scene.GetRootGameObjects())if(go.name==name)return go.transform;return (name.StartsWith("Canvas_") ? new GameObject(name, typeof(RectTransform)) : new GameObject(name)).transform; }
    internal static GameObject Find(Scene scene,string name)
    { foreach(var go in scene.GetRootGameObjects())foreach(var t in go.GetComponentsInChildren<Transform>(true))if(t.name==name)return t.gameObject;throw new InvalidOperationException("Missing "+name); }
    internal static RectTransform Panel(Transform parent,string name,Vector2 anchor,Vector2 pos,Vector2 size,Color color)
    {
        var t=Child(parent,name);var rect=Component<RectTransform>(t.gameObject);rect.anchorMin=rect.anchorMax=anchor;rect.pivot=new Vector2(.5f,.5f);rect.anchoredPosition=pos;rect.sizeDelta=size;
        var image=Component<Image>(t.gameObject);image.color=color;image.raycastTarget=false;return rect;
    }
    internal static TMP_Text Text(Transform parent,string name,string value,float size)
    { var t=Child(parent,name);var rect=Component<RectTransform>(t.gameObject);rect.anchorMin=Vector2.zero;rect.anchorMax=Vector2.one;rect.offsetMin=rect.offsetMax=Vector2.zero;var text=Component<TextMeshProUGUI>(t.gameObject);text.text=value;text.fontSize=size;text.color=Color.white;text.alignment=TextAlignmentOptions.Center;text.raycastTarget=false;return text; }
    internal static TMP_Text LabelAt(Transform parent,string name,string value,Vector2 pos,Vector2 size,float font)
    { var text=Text(parent,name,value,font);text.rectTransform.anchorMin=text.rectTransform.anchorMax=new Vector2(.5f,.5f);text.rectTransform.anchoredPosition=pos;text.rectTransform.sizeDelta=size;return text; }
    internal static Button Button(Transform parent,string name,string label,Vector2 pos)
    { var rect=Panel(parent,name,new Vector2(.5f,.5f),pos,new Vector2(350,44),new Color(.16f,.32f,.4f));var button=Component<Button>(rect.gameObject);button.targetGraphic=rect.GetComponent<Image>();rect.GetComponent<Image>().raycastTarget=true;Text(rect,"Label",label,21);return button; }
}
