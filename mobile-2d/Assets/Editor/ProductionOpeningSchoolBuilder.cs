using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static DormJourneySceneSetup;

public static partial class ProductionOpeningSceneBuilder
{
    private static void BuildSchoolWorld(ProductionOpeningController c,Scene original)
    {
        string[] names={"CampusStop","H6Gate","H6Classroom"};
        string[] paths={"commute/campus","year1/h6_gate_v2","year1/h6_classroom_v2"};
        for(int i=5;i<8;i++)
        {
            var zone=Node(names[i-5],null,new Vector2(i*24,0)).transform;c.zones[i]=zone;
            c.spawns[i]=Node("SafeEntry",zone,new Vector2(i==7?0:-5.8f,i==7?-.9f:-2.7f)).transform;
            var sprite=Legacy(paths[i-5]);Render(zone,"Backdrop",sprite,Vector2.zero,16/sprite.bounds.size.x,-100);
            float top=i==7?-.15f:-1.25f,bottom=i==7?-1.35f:-3.6f;
            Bound(zone,new Vector2(0,top),new Vector2(16,.15f));Bound(zone,new Vector2(0,bottom),new Vector2(16,.15f));
            float side=i==7?1.6f:7.5f;
            Bound(zone,new Vector2(-side,(top+bottom)*.5f),new Vector2(.2f,top-bottom));Bound(zone,new Vector2(side,(top+bottom)*.5f),new Vector2(.2f,top-bottom));
        }
        var services=Node("SchoolTravel",null,Vector2.zero);
        c.schoolJourney=services.AddComponent<DormJourneyController>();c.schoolJourney.enabled=false;
        c.schoolBus=services.AddComponent<CampusCommuteController>();c.schoolBus.enabled=false;
        RestroomInteractable Restroom(string sourceName,float x,RestroomInteractable.Gender gender)
        {
            var go=Object.Instantiate(Find(original,sourceName),c.zones[2]);go.name="School_"+sourceName;go.transform.localPosition=new Vector3(x,-.5f,0);
            go.GetComponent<BoxCollider2D>().offset=new Vector2(0,-.2f);
            var restroom=go.GetComponent<RestroomInteractable>();var exit=go.transform.Find("ExitPivot");exit.localPosition=new Vector3(0,-.25f,0);restroom.Configure(gender,c.schoolJourney,exit);restroom.enabled=false;
            return restroom;
        }
        c.schoolMale=Restroom("WC_Nam",0,RestroomInteractable.Gender.Male);c.schoolFemale=Restroom("WC_Nu",3,RestroomInteractable.Gender.Female);
        Render(c.zones[5],"StopSign",Legacy("commute/bus_sign"),new Vector2(4.5f,-1.7f),.45f,5);
        Render(c.zones[6],"ReturnStopSign",Legacy("commute/bus_sign"),new Vector2(4.5f,-1.7f),.45f,5);
        var vehicle=Render(services.transform,"SchoolBus33",Legacy("commute/bus_33"),Vector2.zero,.65f,5);vehicle.enabled=false;
        Transform Point(int zone,string name,float x,float y)=>Node(name,c.zones[zone],new Vector2(x,y)).transform;
        var stop=Point(5,"BusStop",4.5f,-2.7f);stop.gameObject.AddComponent<CircleCollider2D>().isTrigger=true;stop.gameObject.AddComponent<BusStopInteractable>().Configure(c.schoolBus);
        var returnStop=Point(6,"ReturnBusStop",4.5f,-2.7f);returnStop.gameObject.AddComponent<CircleCollider2D>().isTrigger=true;returnStop.gameObject.AddComponent<BusStopInteractable>().Configure(c.schoolBus);
        var busAudio=services.AddComponent<AudioSource>();busAudio.playOnAwake=false;
        c.schoolBus.Configure(c.schoolJourney,c.spawns[5],c.spawns[2],stop,Point(5,"BusPark",3.5f,-1.7f),Point(5,"BusExit",14,-1.7f),stop,
            c.zones[5].Find("Backdrop").GetComponent<SpriteRenderer>(),c.zones[2].Find("Backdrop").GetComponent<SpriteRenderer>(),vehicle,busAudio,null,null,null);
        c.schoolBus.ConfigureEntry(Point(5,"BusEntry",-14,-1.7f));
        c.schoolBus.ConfigureCampaign(null,Point(6,"ReturnEntry",14,-1.7f),Point(6,"ReturnPark",3.5f,-1.7f),Point(6,"ReturnExit",-14,-1.7f),returnStop,returnStop,c.zones[6].Find("Backdrop").GetComponent<SpriteRenderer>());
        var oldClass=Find(original,"H6_Classroom_Zone").transform;
        var bg=c.zones[7].Find("Backdrop");var oldFg=oldClass.Find("H6_Classroom_Backdrop/H6_DeskForeground");
        var foreground=Render(bg,"DeskForeground",oldFg.GetComponent<SpriteRenderer>().sprite,oldFg.localPosition,1,23);foreground.transform.localScale=oldFg.localScale;
        Render(c.zones[7],"Classmate",Art("C00","npc_h6_classmate_writing"),new Vector2(1,-.15f),.55f,15);
        Render(c.zones[7],"Teacher",Legacy("year1/teacher"),new Vector2(.1f,-.2f),.32f,1);
        c.schoolStudyPose=Render(c.zones[7],"LuonStudy",Legacy("commute/study_0"),new Vector2(-.95f,.1f),.65f,20);c.schoolStudyPose.enabled=false;
    }
    private static void BuildSchoolLinks(ProductionOpeningController c)
    {
        void Link(int zone,ProductionSchoolLink.Route route,Vector2 point,string label)
        {
            var node=Node("SchoolLink_"+route,c.zones[zone],point);node.AddComponent<CircleCollider2D>().isTrigger=true;
            var link=node.AddComponent<ProductionSchoolLink>();link.controller=c;link.route=route;
            var text=Node("Label",node.transform,new Vector2(0,.6f)).AddComponent<TextMeshPro>();text.text=label;text.fontSize=1.8f;text.alignment=TextAlignmentOptions.Center;text.rectTransform.sizeDelta=new Vector2(3,1);text.color=new Color(1,.9f,.4f);text.GetComponent<MeshRenderer>().sortingOrder=30;link.marker=text;
        }
        Link(3,ProductionSchoolLink.Route.Hallway,new Vector2(2.6f,-2.5f),"Hành lang / WC");
        Link(2,ProductionSchoolLink.Route.Lobby,new Vector2(6.2f,-.75f),"Cầu thang ↓ Trạm 33");
        Link(7,ProductionSchoolLink.Route.ReturnStop,new Vector2(0,-1f),"Ra cổng / Xe về KTX");
    }
    private static void BuildSchoolUI(ProductionOpeningController c,Transform canvas)
    {
        var water=c.schoolJourney.gameObject.AddComponent<AudioSource>();water.playOnAwake=false;
        var overlay=Panel(canvas,"SchoolFade",new Vector2(.5f,.5f),Vector2.zero,new Vector2(4000,4000),Color.black);overlay.GetComponent<Image>().raycastTarget=true;
        var group=overlay.gameObject.AddComponent<CanvasGroup>();group.alpha=0;group.blocksRaycasts=false;
        c.schoolJourney.Configure(c.player,c.view,c.spawns[2],c.spawns[3],c.zones[2].Find("Backdrop").GetComponent<SpriteRenderer>(),group,null,null,null,null,water,null);
        c.schoolJourney.ConfigureRoomView(c.zones[3].position+new Vector3(0,0,-10),4.5f);
        var wash=Panel(canvas,"SchoolWash",new Vector2(.5f,.5f),Vector2.zero,new Vector2(600,150),new Color(.04f,.1f,.14f,.97f));
        var washLabel=LabelAt(wash,"Label","Đang vệ sinh cá nhân",new Vector2(0,35),new Vector2(550,55),24);
        var bar=Panel(wash,"Progress",new Vector2(.5f,.5f),new Vector2(0,-25),new Vector2(500,25),new Color(.15f,.2f,.25f));var slider=bar.gameObject.AddComponent<Slider>();slider.interactable=false;
        var fill=Panel(bar,"Fill",new Vector2(.5f,.5f),Vector2.zero,new Vector2(500,25),new Color(.3f,.8f,.55f));fill.anchorMin=Vector2.zero;fill.anchorMax=Vector2.one;fill.offsetMin=fill.offsetMax=Vector2.zero;slider.fillRect=fill;
        washLabel.rectTransform.sizeDelta=new Vector2(420,55);
        for(int i=0;i<2;i++)
        {
            var icon=Panel(wash,i==0?"ToothbrushCup":"FoldedTowel",new Vector2(.5f,.5f),new Vector2(i==0?-260:260,28),new Vector2(48,56),Color.white).GetComponent<Image>();
            icon.sprite=Art("P01",i==0?"toothbrush_cup":"towel_folded");icon.preserveAspect=true;
        }
        c.schoolJourney.ConfigureWashProgress(slider,washLabel);wash.gameObject.SetActive(false);
        var holder=Node("SchoolPuzzle",canvas,Vector2.zero);c.schoolPuzzle=holder.AddComponent<CalculusPuzzleModal>();
        var panel=Panel(canvas,"SchoolBlackboard",new Vector2(.5f,.5f),Vector2.zero,new Vector2(1060,540),new Color(.035f,.16f,.12f,.99f));panel.GetComponent<Image>().raycastTarget=true;
        var heading=LabelAt(panel,"Heading","KHỞI ĐỘNG • LỚP GIẢI TÍCH",new Vector2(0,220),new Vector2(1000,50),30);
        var question=LabelAt(panel,"Question","Tính nhanh: 1 + 2 + ... + 10 = ?",new Vector2(0,125),new Vector2(1000,110),28);
        var timer=LabelAt(panel,"Timer","20s",new Vector2(0,40),new Vector2(900,45),23);
        var answers=new Button[3];string[] labels={"[1] 45","[2] 55","[3] 65"};
        for(int i=0;i<3;i++){answers[i]=Button(panel,"Answer"+i,labels[i],new Vector2(0,-20-i*55));answers[i].GetComponent<RectTransform>().sizeDelta=new Vector2(850,45);}
        var feedback=LabelAt(panel,"Feedback","",new Vector2(0,-190),new Vector2(990,50),20);var retry=Button(panel,"Retry","[R] Thử lại",new Vector2(0,-240));
        c.schoolPuzzle.Configure(panel.gameObject,timer,feedback,answers,retry);c.schoolPuzzle.ConfigureLabels(heading,question);panel.gameObject.SetActive(false);
    }
}
