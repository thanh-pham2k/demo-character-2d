using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static DormJourneySceneSetup;

public static class YearOneCampaignSceneSetup
{
    private const string Folder="Assets/Sprites/MVP/year1_campaign/";
    private static Sprite Art(string name,Vector2 pivot)=>LoadSprite(Folder+name+".png",pivot);
    private static Sprite[] Frames(string prefix,int count,Vector2 pivot)
    {var frames=new Sprite[count];for(int i=0;i<count;i++)frames[i]=Art(prefix+i,pivot);return frames;}
    private static Sprite[] Halves(Sprite[] frames,string prefix,bool upper)
    {
        var result=new Sprite[frames.Length];
        for(int i=0;i<frames.Length;i++)
        {
            string path="Assets/Room404Layout/Campaign_"+prefix+(upper?"_Upper_":"_Lower_")+i+".asset";
            result[i]=AssetDatabase.LoadAssetAtPath<Sprite>(path);if(result[i]!=null)continue;
            var f=frames[i];var rect=new Rect(0,upper?f.rect.height*.5f:0,f.rect.width,f.rect.height*.5f);
            result[i]=Sprite.Create(f.texture,rect,new Vector2(.5f,upper?0:1),100,0,SpriteMeshType.FullRect);AssetDatabase.CreateAsset(result[i],path);
        }
        return result;
    }
    private static Sprite Foreground(Sprite backdrop,string name,int height)
    {
        string path="Assets/Room404Layout/Campaign_"+name+"_Foreground.asset";
        var sprite=AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if(sprite==null){sprite=Sprite.Create(backdrop.texture,new Rect(0,0,backdrop.rect.width,height),new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect);AssetDatabase.CreateAsset(sprite,path);}
        return sprite;
    }
    private static void Entry(Transform parent,string name,Vector3 position,YearOneMilestoneController controller,YearOneInteractable.Action action)
    {var t=Child(parent,name);t.localPosition=position;Trigger(t,new Vector2(.7f,.6f));Component<YearOneInteractable>(t.gameObject).Configure(controller,action);}
    public static void Configure(Scene scene,DormJourneyController journey,PlayerInteractionStateController player,MVPFlowController flow,
        CampusCommuteController commute,YearOneMilestoneController milestone)
    {
        var campaign=Component<YearOneCampaignController>(milestone.gameObject);campaign.Configure(journey);
        foreach(var collider in flow.roommatesGroup.GetComponentsInChildren<Collider2D>(true))collider.isTrigger=true;
        var lightRoot=Child(Find(scene,"--- ENVIRONMENT ---").transform,"LightSwitch_Floor_Interaction");
        var switchVisual=Find(scene,"Light_Switch").transform;
        lightRoot.position=new Vector3(switchVisual.position.x,-.7f,switchVisual.position.z);
        Trigger(lightRoot,new Vector2(.6f,.5f));var light=Component<InteractableProp>(lightRoot.gameObject);
        if(flow.lightSwitchInteractable!=null && flow.lightSwitchInteractable!=light)Object.DestroyImmediate(flow.lightSwitchInteractable);
        flow.lightSwitchInteractable=light;light.SetInteractable(false);
        var canvas=Root(scene,"Canvas_YearOneCampaign");Component<RectTransform>(canvas.gameObject);
        var uiCanvas=Component<Canvas>(canvas.gameObject);uiCanvas.renderMode=RenderMode.ScreenSpaceOverlay;uiCanvas.sortingOrder=35;
        var scaler=Component<CanvasScaler>(canvas.gameObject);scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1280,720);scaler.matchWidthOrHeight=.5f;
        Component<GraphicRaycaster>(canvas.gameObject);
        var hud=Panel(canvas,"Campaign_HUD",new Vector2(.5f,1),new Vector2(0,-40),new Vector2(1200,70),new Color(.035f,.08f,.12f,.95f));
        var objective=Text(hud,"Objective","Năm 1 • Chọn Tiếp tục hoặc Chơi lại",18);
        var welcome=Panel(canvas,"Welcome",new Vector2(.5f,.5f),Vector2.zero,new Vector2(900,350),new Color(.035f,.08f,.12f,.98f));
        LabelAt(welcome,"Title","NĂM 1 • PHÒNG 404",new Vector2(0,120),new Vector2(850,50),30);
        LabelAt(welcome,"Summary","10 ngày đại diện • 6 môn • 32 tín chỉ mục tiêu\nTiến độ tự lưu sau mỗi hoạt động.",new Vector2(0,40),new Vector2(850,100),23);
        var resume=Button(welcome,"Continue","Tiếp tục checkpoint",new Vector2(-210,-100));
        var fresh=Button(welcome,"New_Game","Chơi lại Năm 1",new Vector2(210,-100));
        var study=Panel(canvas,"Course_List",new Vector2(.5f,.5f),Vector2.zero,new Vector2(1080,570),new Color(.035f,.08f,.12f,.99f));
        LabelAt(study,"Title","HỌC TẬP • BÀI TẬP / ÔN THI / TIẾN ĐỘ",new Vector2(0,240),new Vector2(1030,45),26);
        var courses=new Button[6];
        for(int i=0;i<courses.Length;i++)
        {courses[i]=Button(study,"Course_"+i,YearOneSchedule.Courses[i].Name,new Vector2(-300,170-i*62));courses[i].GetComponent<RectTransform>().sizeDelta=new Vector2(400,48);}
        var detail=LabelAt(study,"Course_Progress","",new Vector2(220,105),new Vector2(560,230),22);
        var work=new Button[3];string[] labels={"Làm bài tập","Ôn thi","Kiểm tra môn"};
        for(int i=0;i<3;i++)work[i]=Button(study,"Work_"+i,labels[i],new Vector2(220,-55-i*55));
        var back=Button(study,"Back","Về máy tính",new Vector2(0,-245));
        var ui=Component<YearOneCampaignUI>(canvas.gameObject);ui.Configure(campaign,welcome.gameObject,study.gameObject,detail,objective,fresh,resume,back,courses,work);
        var dinner=Panel(canvas,"Dinner_Menu",new Vector2(.5f,.5f),Vector2.zero,new Vector2(1000,350),new Color(.035f,.08f,.12f,.98f));
        LabelAt(dinner,"Title","BỮA CƠM SINH VIÊN ĐẦU TIÊN",new Vector2(0,120),new Vector2(950,50),29);
        LabelAt(dinner,"Budget","Chọn một phần cơm: 1 mặn + 1 xào • 25.000đ\nSau bữa cơm nhận 120.000đ sinh hoạt cho năm học.",new Vector2(0,30),new Vector2(950,110),22);
        var dish1=Button(dinner,"Meal_0","Cơm thịt kho + rau xào",new Vector2(-220,-100));
        var dish2=Button(dinner,"Meal_1","Cơm trứng + đậu xào",new Vector2(220,-100));
        ui.ConfigureDinner(dinner.gameObject,new[]{dish1,dish2});welcome.gameObject.SetActive(false);study.gameObject.SetActive(false);dinner.gameObject.SetActive(false);
        var dayCard=Panel(canvas,"Day_Transition",new Vector2(.5f,.5f),Vector2.zero,new Vector2(1000,260),new Color(.035f,.08f,.12f,.99f));
        var dayTitle=LabelAt(dayCard,"Title","",Vector2.zero,new Vector2(950,210),30);ui.ConfigureDayCard(dayCard.gameObject,dayTitle);dayCard.gameObject.SetActive(false);
        var desk=Find(scene,"DeskActionModal").GetComponent<DeskActionModal>();desk.ConfigureCampaign(ui);
        Text(Find(scene,"DeskActionModal").transform.Find("Panel/Computer"),"Label","[1] Game",21);
        Text(Find(scene,"DeskActionModal").transform.Find("Panel/Study"),"Label","[2] Học tập",21);

        var luon=Frames("luon_typing_",6,new Vector2(.5f,.5f));
        player.CharacterVisual.ConfigureTypingOffset(Vector3.up*.45f);
        var studyFrames=new Sprite[3];var studyUpper=new Sprite[3];var studyLower=new Sprite[3];
        for(int i=0;i<3;i++)
        {
            studyFrames[i]=LoadSprite("Assets/Sprites/MVP/commute/study_"+i+".png",new Vector2(.5f,.5f));
            studyUpper[i]=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Room404Layout/Desk_Study_Upper_"+i+".asset");
            studyLower[i]=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Room404Layout/Desk_Study_Lower_"+i+".asset");
        }
        player.CharacterVisual.ConfigureDeskSprites(studyFrames,luon,studyUpper,Halves(luon,"Luon",true),studyLower,Halves(luon,"Luon",false),player.CharacterVisual.transform.Find("Desk_UpperBody").GetComponent<SpriteRenderer>());
        var longObj=Find(scene,"Long_NPC");var longRenderer=longObj.GetComponent<SpriteRenderer>();
        var longHome=Child(flow.roommatesGroup.transform,"Long_Home");longHome.position=longObj.transform.position;
        var longFront=Child(longObj.transform,"Desk_UpperBody");longFront.localPosition=new Vector3(0,.2667f);var longUpper=Component<SpriteRenderer>(longFront.gameObject);longUpper.enabled=false;
        var audio=Component<AudioSource>(longObj);audio.playOnAwake=false;audio.loop=true;
        var actor=Component<RoommateActivityController>(longObj);var longFrames=Frames("long_typing_",6,new Vector2(.5f,.5f));
        actor.Configure(longRenderer,longUpper,longRenderer.sprite,Halves(longFrames,"Long",false),Halves(longFrames,"Long",true),Frames("long_enter_",3,new Vector2(.5f,.5f)),
            Frames("long_sleep_",3,new Vector2(.5f,.5f)),Frames("long_exit_",3,new Vector2(.5f,.5f)),longHome,audio,player.CharacterVisual.transform.localPosition);
        var deskPose=Child(longObj.transform,"Gaming_Pose_Registration");deskPose.localPosition=Vector3.up*.45f;deskPose.localScale=player.CharacterVisual.transform.localScale;
        var bedPose=Child(longObj.transform,"Sleeping_Pose_Registration");bedPose.localRotation=Quaternion.identity;bedPose.localScale=Vector3.one*.30f;bedPose.localPosition=Vector3.up*.14f;
        actor.ConfigurePresentation(deskPose,bedPose);
        // Match the centred origin of existing NPC sprites and room actor markers.
        actor.ConfigureLocomotion(Frames("long_walk_",4,new Vector2(.5f,.5f)),Frames("long_rise_",2,new Vector2(.5f,.5f)));
        var laptop=Find(scene,"Laptop_Prop").GetComponent<SpriteRenderer>();var second=Child(laptop.transform.parent,"Laptop_Second");
        second.position=laptop.transform.position+Vector3.right*1.2f;second.localScale=laptop.transform.localScale;
        var secondRenderer=Component<SpriteRenderer>(second.gameObject);secondRenderer.sprite=laptop.sprite;secondRenderer.sortingLayerID=laptop.sortingLayerID;secondRenderer.sortingOrder=laptop.sortingOrder;
        StudyDeskLayoutSetup.Configure(scene);

        var gate=Find(scene,"H6_Gate_Zone").transform;var classroom=Find(scene,"H6_Classroom_Zone").transform;
        var lecture=Art("classroom",new Vector2(.5f,.5f));var lab=Art("lab",new Vector2(.5f,.5f));
        classroom.Find("Seat_Label").gameObject.SetActive(false);gate.Find("Gate_Objective").gameObject.SetActive(false);
        var classBackground=classroom.Find("H6_Classroom_Backdrop").GetComponent<SpriteRenderer>();classBackground.sprite=lecture;classBackground.transform.localScale=Vector3.one*(16/lecture.bounds.size.x);
        var fgClass=Foreground(lecture,"Classroom",286);var fgLab=Foreground(lab,"Lab",286);
        var front=classBackground.transform.Find("H6_DeskForeground");front.GetComponent<SpriteRenderer>().sprite=fgClass;
        front.localPosition=new Vector3(0,(fgClass.rect.center.y-lecture.pivot.y)/100);
        classroom.Find("Luon_SeatPivot").localPosition=new Vector3(-1,.1f);classroom.Find("Seat_Beside_Dam").localPosition=new Vector3(-1,-.3f);
        classroom.Find("Classroom_Arrival").localPosition=new Vector3(0,-.3f);
        classroom.Find("Dam_Writing").localPosition=new Vector3(1,-.15f);classroom.Find("Dam_Writing").localScale=Vector3.one*.38f;
        classroom.Find("Dam_Writing").GetComponent<SpriteRenderer>().sprite=Art("dam_write_0",new Vector2(.5f,0));
        classroom.Find("Teacher").GetComponent<SpriteRenderer>().sprite=Art("teacher_0",new Vector2(.5f,0));
        Component<ClassroomNpcAnimation>(classroom.gameObject).Configure(milestone,classroom.Find("Dam_Writing").GetComponent<SpriteRenderer>(),
            classroom.Find("Teacher").GetComponent<SpriteRenderer>(),Frames("dam_write_",3,new Vector2(.5f,0)),Art("dam_surprise",new Vector2(.5f,0)),Frames("teacher_",3,new Vector2(.5f,0)));
        ProductionClassmateIntegration.Configure(classroom);
        Wall(classroom,"North_Wall",new Vector2(0,.2f),new Vector2(16,.15f));Wall(classroom,"South_Wall",new Vector2(0,-.9f),new Vector2(16,.15f));
        Entry(classroom,"Classroom_Exit",new Vector3(0,-.3f),milestone,YearOneInteractable.Action.ExitClass);
        Entry(gate,"Return_Bus_Entry",new Vector3(.55f,-3.1f),milestone,YearOneInteractable.Action.ReturnBus);
        var returnStop=Child(gate,"Return_Bus_Stop");returnStop.localPosition=new Vector3(.55f,-3.1f);Trigger(returnStop,new Vector2(.8f,.6f));Component<BusStopInteractable>(returnStop.gameObject).Configure(commute);
        var re=Child(gate,"Bus_Return_Entry");re.localPosition=new Vector3(20,-4.5f);
        var rp=Child(gate,"Bus_Return_Park");rp.localPosition=new Vector3(3,-4.5f);
        var rx=Child(gate,"Bus_Return_Exit");rx.localPosition=new Vector3(-20,-4.5f);
        var rb=Child(gate,"Bus_Return_Board");rb.localPosition=new Vector3(1,-3.5f);
        commute.ConfigureCampaign(campaign,re,rp,rx,returnStop,rb,gate.Find("H6_Gate_Backdrop").GetComponent<SpriteRenderer>());

        var hall=Find(scene,"Hallway_Zone").transform;Entry(hall,"Canteen_Entry",new Vector3(7.4f,.1f),milestone,YearOneInteractable.Action.Canteen);
        var canteen=Root(scene,"Canteen_Zone");canteen.position=new Vector3(120,0);
        var food=Component<SpriteRenderer>(Child(canteen,"Canteen_Backdrop").gameObject);food.sprite=Art("canteen",new Vector2(.5f,.5f));food.sortingOrder=-100;food.transform.localScale=Vector3.one*(16/food.sprite.bounds.size.x);
        var foodSpawn=Child(canteen,"Canteen_Arrival");foodSpawn.localPosition=new Vector3(0,-1.5f);
        string[] friends={"Long_NPC","Dam_NPC","To_NPC"};
        for(int i=0;i<friends.Length;i++)
        {
            var friend=Find(scene,friends[i]);if(friend==null)continue;
            var waiting=Child(canteen,"Dinner_"+friends[i]);waiting.localPosition=new Vector3(-2.1f+i*.8f,-1.35f,0);
            waiting.localScale=player.CharacterVisual.transform.localScale*.55f;
            var art=Component<SpriteRenderer>(waiting.gameObject);art.sprite=friend.GetComponent<SpriteRenderer>().sprite;art.sortingOrder=5;
        }
        Entry(Find(scene,"--- PROPS ---").transform,"Hotpot_Preparation",new Vector3(0,-1.5f),milestone,YearOneInteractable.Action.PrepareFeast);
        var chairs=scene.GetRootGameObjects();var chairList=new System.Collections.Generic.List<ChairInteractable>();var bedList=new System.Collections.Generic.List<BedInteractable>();
        foreach(var root in chairs){chairList.AddRange(root.GetComponentsInChildren<ChairInteractable>(true));bedList.AddRange(root.GetComponentsInChildren<BedInteractable>(true));}
        milestone.ConfigureCampaignFlow(campaign,ui,desk,actor,chairList.ToArray(),bedList.ToArray(),lecture,lab,fgClass,fgLab,food,foodSpawn,
            Find(scene,"Campus_Backdrop").GetComponent<SpriteRenderer>());
        EditorUtility.SetDirty(milestone);EditorUtility.SetDirty(campaign);EditorUtility.SetDirty(ui);EditorUtility.SetDirty(actor);EditorUtility.SetDirty(desk);EditorUtility.SetDirty(commute);EditorUtility.SetDirty(player.CharacterVisual);
    }
}
