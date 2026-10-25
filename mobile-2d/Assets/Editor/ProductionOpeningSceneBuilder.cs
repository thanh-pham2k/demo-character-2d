using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DormJourneySceneSetup;

[InitializeOnLoad]
public static partial class ProductionOpeningSceneBuilder
{
    public const string ScenePath="Assets/Scenes/Production_Opening.unity";
    private static string Request=>Path.Combine(Application.dataPath,"../Temp/BuildProductionOpening.run");
    private static double allowedAfter;
    private static Scene target;
    static ProductionOpeningSceneBuilder(){allowedAfter=EditorApplication.timeSinceStartup+3;EditorApplication.update+=Poll;}
    private static void Poll()
    {
        if(!File.Exists(Request)||EditorApplication.timeSinceStartup<allowedAfter||EditorApplication.isCompiling||EditorApplication.isUpdating||EditorApplication.isPlayingOrWillChangePlaymode)return;
        File.Delete(Request);
        try{Build();}catch(Exception ex){Debug.LogException(ex);File.WriteAllText(Output("opening-build.txt"),"FAIL "+ex);}
    }
    private static string Output(string name)=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../assets/mvp_sprites/remaining/review/"+name));
    private static GameObject Node(string name,Transform parent,Vector2 local)
    {
        var go=new GameObject(name);SceneManager.MoveGameObjectToScene(go,target);
        if(parent!=null)go.transform.SetParent(parent,false);go.transform.localPosition=local;return go;
    }
    private static Sprite Art(string batch,string id)
    {
        var sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/remaining/"+batch+"/"+id+".png");
        if(sprite==null)throw new InvalidOperationException("Missing "+id);return sprite;
    }
    private static SpriteRenderer Render(Transform parent,string name,Sprite sprite,Vector2 pos,float scale,int order)
    {
        var go=Node(name,parent,pos);go.transform.localScale=Vector3.one*scale;
        var r=go.AddComponent<SpriteRenderer>();r.sprite=sprite;r.sortingOrder=order;return r;
    }
    private static void Bound(Transform zone,Vector2 pos,Vector2 size)
    {var go=Node("WalkingBoundary",zone,pos);go.AddComponent<BoxCollider2D>().size=size;}
    public static void Build()
    {
        var original=SceneManager.GetActiveScene();
        if(original.path!="Assets/Scenes/Room404_MVP.unity" || original.isDirty)throw new InvalidOperationException("Keep a clean saved Room404_MVP open as room-art source.");
        target=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);SceneManager.SetActiveScene(target);
        try
        {
            var controller=Node("Production_Opening",null,Vector2.zero).AddComponent<ProductionOpeningController>();
            controller.definition=AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Resources/ProductionCampaign/missions.json");
            var camera=Node("Main Camera",null,Vector2.zero).AddComponent<Camera>();camera.tag="MainCamera";camera.orthographic=true;camera.orthographicSize=4.5f;camera.transform.position=new Vector3(0,0,-10);camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.08f,.1f,.14f);controller.view=camera;
            controller.zones=new Transform[8];controller.spawns=new Transform[8];
            string[] names={"Station","Reception","Hallway404","Room404","Canteen"};
            for(int i=0;i<5;i++)
            {
                var zone=Node(names[i],null,new Vector2(i*24,0)).transform;controller.zones[i]=zone;
                controller.spawns[i]=Node("SafeEntry",zone,(i==3?new Vector2(-2.6f,-2.85f):new Vector2(-5.8f,i==2?-.75f:-2.7f))).transform;
                float top=i==2?-.35f:-1.25f,bottom=i==2?-1.15f:-3.6f;
                Bound(zone,new Vector2(0,bottom),new Vector2(16,.15f));Bound(zone,new Vector2(0,top),new Vector2(16,.15f));Bound(zone,new Vector2(i==3?-3.4f:-7.5f,(top+bottom)*.5f),new Vector2(.2f,top-bottom));Bound(zone,new Vector2(i==3?3.4f:7.5f,(top+bottom)*.5f),new Vector2(.2f,top-bottom));
                if(i<2)
                {
                    var bg=Art("E00",i==0?"bus_station_background":"dorm_reception_background");Render(zone,"Backdrop",bg,Vector2.zero,8/bg.bounds.size.y,-100);
                }
                else if(i==2)
                {
                    var bg=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/dorm_journey/hallway.png");Render(zone,"Backdrop",bg,Vector2.zero,16/bg.bounds.size.x,-100);
                }
                else if(i==4)
                {
                    var bg=Art("E01","canteen_background");Render(zone,"Backdrop",bg,Vector2.zero,8/bg.bounds.size.y,-100);
                }
                else
                {
                    foreach(string sourceName in new[]{"Room_Backdrop","BunkBed_Left","BunkBed_Right","Study_Desk"})
                    {
                        var source=Find(original,sourceName).GetComponent<SpriteRenderer>();
                        var clone=Render(zone,sourceName,source.sprite,source.transform.position,1,sourceName=="Room_Backdrop"?-100:-10);
                        clone.transform.localScale=source.transform.lossyScale;clone.transform.localRotation=source.transform.rotation;
                    }
                }
            }
            Render(controller.zones[0],"Tout",Art("C00","npc_station_tout"),new Vector2(-2,-2.2f),.65f,10);
            var bus=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/commute/bus_33.png");Render(controller.zones[0],"DormBus",bus,new Vector2(4,-2.2f),.65f,-5);
            Render(controller.zones[1],"Manager",Art("C00","npc_dorm_manager"),new Vector2(3,.4f),.75f,0);
            var counter=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/AssetProduction/ReceptionCounter.asset");
            if(counter!=null)Render(controller.zones[1],"CounterForeground",counter,new Vector2(3.72f,.36f),16/Art("E00","dorm_reception_background").bounds.size.x,5);
            controller.leftBag=Node("LeftBedBagAnchor",controller.zones[3],new Vector2(-4,-1.5f)).transform;
            controller.rightBag=Node("RightBedBagAnchor",controller.zones[3],new Vector2(4,-1.5f)).transform;
            controller.placedBag=Render(controller.zones[3],"PlacedBackpack",AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/props/luon_backpack.png"),new Vector2(-4,-1.5f),.3f,15);controller.placedBag.transform.localScale=Vector3.one*(.7f/controller.placedBag.sprite.bounds.size.y);controller.placedBag.enabled=false;
            controller.fan=Render(controller.zones[3],"MiniFan",Art("P00","mini_fan"),new Vector2(0,-1.5f),.5f,15);
            controller.fan.transform.localScale=Vector3.one*(.65f/controller.fan.sprite.bounds.size.y);
            var player=Node("Player_Luon",null,new Vector2(-5.8f,-2.7f));var rb=player.AddComponent<Rigidbody2D>();rb.gravityScale=0;rb.freezeRotation=true;
            var collider=player.AddComponent<CircleCollider2D>();collider.radius=.16f;
            player.AddComponent<PlayerMovement>();controller.player=player.AddComponent<PlayerInteractionStateController>();
            var sr=Render(player.transform,"Visual",Art("DerivedExisting","casual_right_idle"),Vector2.zero,.65f,20);
            var visual=sr.gameObject.AddComponent<LuonCharacterVisual>();visual.idleSprite=sr.sprite;
            Sprite[] Side(string outfit,int count){var frames=new Sprite[count];for(int i=0;i<count;i++)frames[i]=Art("DerivedExisting",outfit+"_right_walk_"+i);return frames;}
            visual.walkSprites=Side("casual",4);
            visual.ConfigureProductionSprites(AssetDatabase.LoadAssetAtPath<RemainingAssetCatalog>("Assets/AssetProduction/RemainingAssetCatalog.asset"),sr.sprite,visual.walkSprites,Art("DerivedExisting","backpack_right_idle"),Side("backpack",3));
            visual.SetBackpackEquipped(true);
            var detector=player.AddComponent<PlayerInteractionDetector>();
            BuildHousekeepingWorld(controller);BuildSchoolWorld(controller,original);BuildSchoolLinks(controller);BuildYearOneWorld(controller);BuildRegistrationWorld(controller);
            var entries=new List<ProductionOpeningAction>();
            void Action(int zone,int mission,string id,Vector2 pos)
            {
                var go=Node(id,controller.zones[zone],pos);go.AddComponent<CircleCollider2D>().isTrigger=true;
                var action=go.AddComponent<ProductionOpeningAction>();action.controller=controller;action.mission=mission;action.step=id;entries.Add(action);
                var label=Node("ActionLabel",go.transform,new Vector2(0,.7f)).AddComponent<TextMeshPro>();label.fontSize=2.1f;label.alignment=TextAlignmentOptions.Center;label.rectTransform.sizeDelta=new Vector2(4,1);label.text=id;label.color=new Color(1,.9f,.4f);label.GetComponent<MeshRenderer>().sortingOrder=30;action.marker=label;
            }
            Action(0,1,"decline_tout",new Vector2(-2,-2.5f));Action(0,1,"buy_ticket",new Vector2(1,-2.5f));Action(0,1,"arrive_dorm",new Vector2(5,-2.5f));
            Action(1,2,"present_documents",new Vector2(3,-2));Action(1,2,"receive_room_key",new Vector2(3,-2));Action(1,2,"reach_room",new Vector2(-5.5f,-2.5f));
            Action(2,3,"unlock_room",new Vector2(4,-.75f));Action(3,3,"claim_bed",new Vector2(-2.8f,-2.5f));Action(3,3,"place_backpack",new Vector2(-4,-2.1f));Action(3,3,"repair_fan",new Vector2(0,-2.5f));
            Action(3,4,"buy_cleaning_kit",new Vector2(2.6f,-2.5f));Action(4,4,"buy_cleaning_kit",new Vector2(-4,-1.65f));
            Action(3,4,"clean_four_spots",new Vector2(-2.7f,-2.5f));Action(3,4,"meet_roommates",new Vector2(0,-2.8f));
            Action(3,5,"choose_meal",new Vector2(2.6f,-2.5f));Action(4,5,"choose_meal",new Vector2(-4,-1.65f));
            Action(4,5,"eat_together",new Vector2(-.2f,-1.55f));Action(4,5,"receive_schedule",new Vector2(-4,-2.6f));Action(4,5,"return_sleep",new Vector2(-6.5f,-2.6f));
            Action(3,6,"equip_backpack",new Vector2(-2.8f,-2.2f));Action(6,6,"enter_class",new Vector2(-3,-2.5f));
            Action(7,6,"solve_warmup",new Vector2(-.95f,-.75f));Action(7,6,"finish_lesson",new Vector2(-.95f,-.75f));Action(3,6,"tell_dam",new Vector2(2.6f,-2.5f));
            Action(2,7,"join_wc_queue",new Vector2(-2.6f,-.75f));Action(6,7,"attend_class",new Vector2(-3,-2.5f));
            Action(7,7,"review_commute_ledger",new Vector2(-.95f,-.75f));
            foreach(var course in ProductionYearOneCurriculum.Load().courses)
            {Action(7,7,"study_y1_"+course.id,new Vector2(-.95f,-.75f));Action(7,7,"exam_y1_"+course.id,new Vector2(-.95f,-.75f));}
            Action(7,7,"finish_year_exams",new Vector2(-.95f,-.75f));Action(7,7,"read_year_results",new Vector2(-.95f,-.75f));
            Action(3,8,"get_cooker",new Vector2(-2.5f,-2.5f));Action(3,8,"plug_cooker",new Vector2(2.6f,-1.7f));Action(3,8,"add_ingredients",new Vector2(0,-2.5f));
            Action(3,8,"share_feast",new Vector2(0,-2.8f));Action(3,8,"toast",new Vector2(0,-2.8f));Action(3,8,"sleep_new_term",new Vector2(-2.8f,-2.2f));
            Action(3,9,"refresh_registration",new Vector2(-1,-1.8f));Action(3,9,"select_classes",new Vector2(-1,-1.8f));Action(3,9,"confirm_schedule",new Vector2(-1,-1.8f));
            controller.actions=entries.ToArray();
            var canvas=Node("Canvas_Opening",null,Vector2.zero).AddComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.sortingOrder=1000;
            var scaler=canvas.gameObject.AddComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1280,720);canvas.gameObject.AddComponent<GraphicRaycaster>();
            var events=Node("EventSystem",null,Vector2.zero);events.AddComponent<EventSystem>();events.AddComponent<InputSystemUIInputModule>();
            Panel(canvas.transform,"HUD_Backplate",new Vector2(.5f,.5f),new Vector2(0,300),new Vector2(1180,105),new Color(.02f,.05f,.09f,.85f));
            controller.hud=LabelAt(canvas.transform,"HUD","",new Vector2(0,300),new Vector2(1150,100),25);
            Panel(canvas.transform,"Notice_Backplate",new Vector2(.5f,.5f),new Vector2(0,-210),new Vector2(1120,70),new Color(.02f,.05f,.09f,.8f));
            controller.notice=LabelAt(canvas.transform,"Notice","",new Vector2(0,-210),new Vector2(1150,70),21);
            var prompt=Panel(canvas.transform,"InteractionPrompt",new Vector2(.5f,.5f),new Vector2(0,-285),new Vector2(750,50),new Color(.03f,.08f,.12f,.9f));
            detector.ConfigurePrompt(prompt.gameObject,(TextMeshProUGUI)Text(prompt,"Label","",23));
            controller.interactButton=Button(canvas.transform,"Interact","Tương tác",new Vector2(450,-285));controller.interactButton.GetComponent<RectTransform>().sizeDelta=new Vector2(190,70);
            var joy=Panel(canvas.transform,"Joystick",new Vector2(.5f,.5f),new Vector2(-500,-250),new Vector2(120,120),new Color(.2f,.3f,.4f,.65f));joy.GetComponent<Image>().raycastTarget=true;
            var handle=Panel(joy,"Handle",new Vector2(.5f,.5f),Vector2.zero,new Vector2(45,45),Color.white);
            var knob=AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");joy.GetComponent<Image>().sprite=knob;handle.GetComponent<Image>().sprite=knob;
            var joystick=joy.gameObject.AddComponent<VirtualJoystick>();var serialized=new SerializedObject(joystick);serialized.FindProperty("background").objectReferenceValue=joy;serialized.FindProperty("handle").objectReferenceValue=handle;serialized.ApplyModifiedPropertiesWithoutUndo();
            var modal=Panel(canvas.transform,"ActionModal",new Vector2(.5f,.5f),Vector2.zero,new Vector2(1050,490),new Color(.025f,.07f,.10f,.98f));modal.GetComponent<Image>().raycastTarget=true;controller.modal=modal.gameObject;
            controller.title=LabelAt(modal,"Title","",new Vector2(0,195),new Vector2(970,55),29);
            controller.body=LabelAt(modal,"Body","",new Vector2(0,60),new Vector2(950,175),24);controller.body.textWrappingMode=TextWrappingModes.Normal;controller.body.overflowMode=TextOverflowModes.Overflow;
            controller.confirmButton=Button(modal,"Confirm","[Space] Xác nhận",new Vector2(0,-150));controller.cancelButton=Button(modal,"Cancel","Đóng / Esc",new Vector2(0,-205));
            controller.aidButton=Button(modal,"Aid","Xin hỗ trợ khoản còn thiếu",new Vector2(0,-95));
            controller.leftButton=Button(modal,"LeftBed","Giường bên trái",new Vector2(-225,-150));controller.rightButton=Button(modal,"RightBed","Giường bên phải",new Vector2(225,-150));
            var hold=Button(modal,"Hold","Giữ để siết ốc và cắm quạt",new Vector2(0,-135));controller.holdButton=hold.gameObject.AddComponent<ProductionHoldButton>();
            var meter=Panel(modal,"HoldProgress",new Vector2(.5f,.5f),new Vector2(0,-88),new Vector2(600,20),new Color(.08f,.15f,.18f));controller.holdMeter=meter.gameObject.AddComponent<Slider>();controller.holdMeter.interactable=false;
            var fill=Panel(meter,"Fill",new Vector2(.5f,.5f),Vector2.zero,new Vector2(600,20),new Color(.3f,.8f,.55f));fill.anchorMin=Vector2.zero;fill.anchorMax=Vector2.one;fill.offsetMin=Vector2.zero;fill.offsetMax=Vector2.zero;controller.holdMeter.fillRect=fill;controller.holdMeter.direction=Slider.Direction.LeftToRight;
            var docs=Panel(modal,"InventoryDocuments",new Vector2(.5f,.5f),new Vector2(0,-85),new Vector2(960,100),Color.clear);controller.documents=docs.gameObject;
            void Card(string name,string item,Vector2 position)
            {
                var card=Panel(docs,name,new Vector2(.5f,.5f),position,new Vector2(250,90),new Color(.18f,.3f,.4f));card.GetComponent<Image>().raycastTarget=true;card.gameObject.AddComponent<CanvasGroup>();Text(card,"Label",name,21);card.gameObject.AddComponent<ProductionDocumentCard>().item=item;
            }
            Card("GIẤY NHẬP HỌC","item:admission_documents",new Vector2(-330,0));Card("BALO","item:backpack",new Vector2(-35,0));
            var drop=Panel(docs,"ReceptionDrop",new Vector2(.5f,.5f),new Vector2(295,0),new Vector2(270,100),new Color(.13f,.4f,.28f));drop.GetComponent<Image>().raycastTarget=true;Text(drop,"Label","NỘP GIẤY TẠI ĐÂY",20);drop.gameObject.AddComponent<ProductionDocumentDrop>().controller=controller;
            BuildHousekeepingUI(controller,modal);BuildRegistrationUI(controller,modal);
            var start=Panel(canvas.transform,"Start",new Vector2(.5f,.5f),Vector2.zero,new Vector2(900,380),new Color(.025f,.07f,.1f,.98f));start.GetComponent<Image>().raycastTarget=true;controller.startPanel=start.gameObject;
            LabelAt(start,"Title","PHÒNG404 • NGÀY NHẬP HỌC",new Vector2(0,110),new Vector2(850,60),30);
            LabelAt(start,"Scope","Bến xe → nhận phòng → chuẩn bị chỗ ở\nTự lưu sau mỗi bước. Esc đóng menu; nút dưới tải lại mốc lưu.",new Vector2(0,15),new Vector2(830,110),24);
            controller.beginButton=Button(start,"Begin","Bắt đầu / Tải mốc lưu",new Vector2(0,-115));
            var fadeRect=Panel(canvas.transform,"Fade",new Vector2(.5f,.5f),Vector2.zero,new Vector2(4000,4000),Color.clear);controller.fade=fadeRect.GetComponent<Image>();
            BuildSchoolUI(controller,canvas.transform);camera.gameObject.AddComponent<AudioListener>();
            for(int i=1;i<controller.zones.Length;i++)controller.zones[i].gameObject.SetActive(false);
            // Additive editor build keeps the source scene loaded; isolate its renderers.
            foreach(var root in target.GetRootGameObjects())foreach(var item in root.GetComponentsInChildren<Transform>(true))item.gameObject.layer=29;
            camera.cullingMask=1<<29;
            EditorSceneManager.SaveScene(target,ScenePath);AssetDatabase.SaveAssets();
            MVPSceneSnapshot.SaveCameraFrame(camera,Output("opening-station-staging.png"));
            File.WriteAllText(Output("opening-build.txt"),"PASS "+DateTime.UtcNow.ToString("O")+"\nSeparate opening scene;50authored action anchors,8zones,4cleaning targets,single sprite writer,drag/drop docs,hold,timer and meal budget. Play Mode verification pending.\n");
        }
        finally{EditorSceneManager.CloseScene(target,true);SceneManager.SetActiveScene(original);}
    }
}
