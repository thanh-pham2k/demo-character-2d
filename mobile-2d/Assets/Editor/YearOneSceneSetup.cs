using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static DormJourneySceneSetup;

// Additive, idempotent authoring; no extra KTX menu or duplicate room layout.
public static class YearOneSceneSetup
{
    private const string Folder="Assets/Sprites/MVP/year1/";
    private static Sprite Art(string name,bool bottom=true)=>LoadSprite(Folder+name+".png",new Vector2(.5f,bottom?0:.5f));
    private static SpriteRenderer Render(Transform parent,string name,Sprite sprite,Vector3 position,float scale,int order)
    {
        var t=Child(parent,name);t.localPosition=position;t.localScale=Vector3.one*scale;
        var r=Component<SpriteRenderer>(t.gameObject);r.sprite=sprite;r.sortingOrder=order;return r;
    }
    public static void Configure(Scene scene,DormJourneyController journey,PlayerInteractionStateController player,
        MVPFlowController story,CampusCommuteController commute)
    {
        var root=Root(scene,"YearOne_Milestone");var controller=Component<YearOneMilestoneController>(root.gameObject);
        controller.ConfigureRoomBackdrop(Find(scene,"Room_Backdrop").GetComponent<SpriteRenderer>());
        var roomVisuals=new System.Collections.Generic.List<SpriteRenderer>();
        roomVisuals.AddRange(Find(scene,"--- ENVIRONMENT ---").GetComponentsInChildren<SpriteRenderer>(true));
        roomVisuals.AddRange(Find(scene,"--- PROPS ---").GetComponentsInChildren<SpriteRenderer>(true));
        controller.ConfigureRoomVisuals(roomVisuals.ToArray());
        var gate=Root(scene,"H6_Gate_Zone");gate.position=new Vector3(72,0);
        var gateBg=Render(gate,"H6_Gate_Backdrop",Art("h6_gate_v2",false),Vector3.zero,1,-100);
        float gateScale=16/gateBg.sprite.bounds.size.x;gateBg.transform.localScale=Vector3.one*gateScale;
        var gateSpawn=Child(gate,"Bus_AlightPivot");gateSpawn.localPosition=new Vector3(-4,-3.1f);
        var gateDoor=Child(gate,"H6_Door_Interactable");gateDoor.localPosition=new Vector3(.55f,-3.1f);Trigger(gateDoor,new Vector2(1,1));
        Component<YearOneInteractable>(gateDoor.gameObject).Configure(controller,YearOneInteractable.Action.EnterH6);
        BoundsForWalking(gate,16,-2.6f,-3.8f);
        WorldLabel(gate,"Gate_Objective","[E] Vào H6",new Vector3(.55f,-1.8f),new Color(.08f,.15f,.18f));
        var classroom=Root(scene,"H6_Classroom_Zone");classroom.position=new Vector3(96,0);
        var classBg=Render(classroom,"H6_Classroom_Backdrop",Art("h6_classroom_v2",false),Vector3.zero,1,-100);
        classBg.transform.localScale=Vector3.one*(16/classBg.sprite.bounds.size.x);
        const string foregroundPath="Assets/Room404Layout/H6_DeskForegroundV2.asset";
        var foregroundSprite=AssetDatabase.LoadAssetAtPath<Sprite>(foregroundPath);
        var crop=new Rect(0,0,1182,308);
        if(foregroundSprite==null)
        {
            foregroundSprite=Sprite.Create(classBg.sprite.texture,crop,new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect);
            AssetDatabase.CreateAsset(foregroundSprite,foregroundPath);
        }
        var fg=Render(classBg.transform,"H6_DeskForeground",foregroundSprite,
            new Vector3((crop.center.x-classBg.sprite.pivot.x)/100,(crop.center.y-classBg.sprite.pivot.y)/100),1,0);
        controller.ConfigureForeground(fg);
        var spawn=Child(classroom,"Classroom_Arrival");spawn.localPosition=new Vector3(0,-.65f);
        BoundsForWalking(classroom,16,-.15f,-1.25f);
        var seat=Child(classroom,"Luon_SeatPivot");seat.localPosition=new Vector3(-.95f,-.3f);
        var entry=Child(classroom,"Seat_Beside_Dam");entry.localPosition=new Vector3(-.95f,-.65f);Trigger(entry,new Vector2(.9f,.7f));
        Component<YearOneInteractable>(entry.gameObject).Configure(controller,YearOneInteractable.Action.SitBesideClassmate);
        Render(classroom,"Dam_Writing",Art("dam_writing"),new Vector3(.95f,-.55f),.38f,-5);
        WorldLabel(classroom,"Dam_Surprise","!",new Vector3(.95f,.85f),new Color(1f,.55f,.05f));
        var surprise=classroom.Find("Dam_Surprise").gameObject;surprise.SetActive(false);controller.ConfigureSurprise(surprise);
        Render(classroom,"Teacher",Art("teacher"),new Vector3(.1f,-.2f),.32f,1);
        var board=Child(classroom,"Blackboard_PlayerPivot");board.localPosition=new Vector3(-.7f,.2f);
        WorldLabel(classroom,"Seat_Label","[E] Ngồi cạnh bạn cùng lớp",new Vector3(0,.95f),new Color(.08f,.15f,.18f));
        ProductionClassmateIntegration.Configure(classroom);
        var feast=Root(scene,"Room404_Hotpot");feast.position=new Vector3(0,-1.2f);
        var pot0=Art("pot_0");var pot1=Art("pot_1");
        var pot=Render(feast,"Electric_Hotpot",pot0,new Vector3(0,-.7f),.42f,30);
        Render(feast,"Luon_Floor",Art("luon_floor"),new Vector3(-1.2f,-1.15f),.58f,25);
        Render(feast,"Dam_Floor",Art("dam_floor"),new Vector3(1.2f,-1.15f),.58f,25);
        Render(feast,"Long_Floor",Art("long_floor"),new Vector3(-1.1f,.05f),.58f,20);
        Render(feast,"To_Floor",Art("to_floor"),new Vector3(1.1f,.05f),.58f,20);
        var feastArrival=Child(root,"Feast_PlayerPivot");feastArrival.position=new Vector3(0,-2.3f);
        var audio=Component<AudioSource>(feast.gameObject);audio.playOnAwake=false;
        feast.gameObject.SetActive(false);

        var canvas=Root(scene,"Canvas_YearOne");Component<RectTransform>(canvas.gameObject);
        var ui=Component<Canvas>(canvas.gameObject);ui.renderMode=RenderMode.ScreenSpaceOverlay;ui.sortingOrder=40;
        var scaler=Component<CanvasScaler>(canvas.gameObject);scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1280,720);scaler.matchWidthOrHeight=.5f;
        Component<GraphicRaycaster>(canvas.gameObject);
        var dialogue=Panel(canvas,"YearOne_Dialogue",new Vector2(.5f,0),new Vector2(0,115),new Vector2(1050,210),new Color(.035f,.08f,.12f,.97f));
        var name=LabelAt(dialogue,"Speaker","",new Vector2(0,75),new Vector2(950,35),24);name.color=new Color(1,.85f,.45f);
        var body=LabelAt(dialogue,"Body","",new Vector2(0,5),new Vector2(970,105),22);
        var next=Button(dialogue,"Next","[Space] Tiếp tục",new Vector2(0,-77));
        var academic=Panel(canvas,"Academic_Milestone_Card",new Vector2(.5f,.5f),Vector2.zero,new Vector2(1020,460),new Color(.04f,.13f,.18f,.98f));
        LabelAt(academic,"Title","NĂM 1 HOÀN THÀNH",new Vector2(0,165),new Vector2(950,55),32);
        LabelAt(academic,"Summary","Những buổi học và kỳ thi cuối năm lần lượt trôi qua...\nCả phòng cùng vượt qua các môn đại cương.",new Vector2(0,95),new Vector2(950,85),23);
        LabelAt(academic,"Credits","HỌC KỲ: Năm 1 (Kỳ 1 & Kỳ 2)\nTÍN CHỈ TÍCH LŨY: 32 / 150\nTRẠNG THÁI: BÌNH THƯỜNG (KHÔNG NỢ MÔN)",new Vector2(0,-30),new Vector2(950,160),25);
        var academicNext=Button(academic,"Continue","[Space] Về phòng ăn lẩu",new Vector2(0,-175));
        var puzzleRoot=Child(canvas,"CalculusPuzzleModal");var puzzle=Component<CalculusPuzzleModal>(puzzleRoot.gameObject);
        var chalk=Panel(puzzleRoot,"Blackboard",new Vector2(.5f,.5f),Vector2.zero,new Vector2(1060,540),new Color(.035f,.16f,.12f,.99f));
        LabelAt(chalk,"Heading","KHỞI ĐỘNG • ĐỐ VUI TOÁN HỌC",new Vector2(0,220),new Vector2(1000,50),30);
        LabelAt(chalk,"Question","Tính nhanh tổng các số từ 1 đến 10:\n1 + 2 + 3 + ... + 10 = ?\nChọn đáp án đúng:",new Vector2(0,110),new Vector2(1000,160),27);
        var timer=LabelAt(chalk,"Timer","20s",new Vector2(0,10),new Vector2(900,40),23);
        var choices=new Button[3];
        string[] labels={"[1] 45","[2] 55","[3] 65"};
        for(int i=0;i<3;i++){choices[i]=Button(chalk,"Answer_"+i,labels[i],new Vector2(0,-42-i*52));choices[i].GetComponent<RectTransform>().sizeDelta=new Vector2(850,44);}
        var feedback=LabelAt(chalk,"Feedback","",new Vector2(0,-190),new Vector2(990,50),20);
        var retry=Button(chalk,"Retry","[R] Thử lại",new Vector2(0,-240));
        puzzle.Configure(chalk.gameObject,timer,feedback,choices,retry);
        puzzle.ConfigureLabels(chalk.Find("Heading").GetComponent<TMP_Text>(),chalk.Find("Question").GetComponent<TMP_Text>());
        controller.Configure(journey,story,commute,gateSpawn,spawn,seat,board,feastArrival,gateBg,classBg,feast.gameObject,dialogue.gameObject,name,body,next,
            academic.gameObject,academicNext,puzzle,audio,pot,new[]{pot0,pot1});
        var info=Find(scene,"TopInfoPanel").GetComponent<RectTransform>();
        info.anchorMin=info.anchorMax=new Vector2(0,1);info.pivot=new Vector2(0,1);
        info.anchoredPosition=new Vector2(20,-75);info.sizeDelta=new Vector2(450,100);
        var infoBackground=Component<Image>(info.gameObject);infoBackground.color=new Color(.035f,.08f,.12f,.88f);infoBackground.raycastTarget=false;
        ConfigureInfoLabel(story.yearText,new Vector2(12,-8),new Vector2(425,50),19);
        ConfigureInfoLabel(story.timeText,new Vector2(12,-65),new Vector2(425,27),18);
        commute.ConfigureYearOne(controller);journey.ConfigureYearOne(controller);story.ConfigureExtendedYearOne(true);
        dialogue.gameObject.SetActive(false);academic.gameObject.SetActive(false);chalk.gameObject.SetActive(false);
        EditorUtility.SetDirty(controller);EditorUtility.SetDirty(story);EditorUtility.SetDirty(commute);EditorUtility.SetDirty(journey);EditorUtility.SetDirty(puzzle);
        YearOneCampaignSceneSetup.Configure(scene,journey,player,story,commute,controller);
    }
    private static void ConfigureInfoLabel(TMP_Text text,Vector2 position,Vector2 size,float font)
    {
        var rect=text.rectTransform;rect.anchorMin=rect.anchorMax=new Vector2(0,1);rect.pivot=new Vector2(0,1);
        rect.anchoredPosition=position;rect.sizeDelta=size;text.fontSize=font;text.alignment=TextAlignmentOptions.TopLeft;
    }
    private static void BoundsForWalking(Transform parent,float width,float top,float bottom)
    {
        Wall(parent,"North_Wall",new Vector2(0,top),new Vector2(width,.15f));
        Wall(parent,"South_Wall",new Vector2(0,bottom),new Vector2(width,.15f));
        Wall(parent,"West_Wall",new Vector2(-width*.5f,(top+bottom)*.5f),new Vector2(.15f,top-bottom));
        Wall(parent,"East_Wall",new Vector2(width*.5f,(top+bottom)*.5f),new Vector2(.15f,top-bottom));
    }
}
