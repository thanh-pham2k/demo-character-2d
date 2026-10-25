using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static DormJourneySceneSetup;

public static partial class ProductionOpeningSceneBuilder
{
    private static Sprite Legacy(string path)=>AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/MVP/"+path+".png");
    private static SpriteRenderer Item(Transform parent,string name,Sprite sprite,Vector2 point,float height,int order)
    {var renderer=Render(parent,name,sprite,point,1,order);renderer.transform.localScale=Vector3.one*(height/sprite.bounds.size.y);return renderer;}
    private static void BuildHousekeepingWorld(ProductionOpeningController c)
    {
        var room=c.zones[3];var canteen=c.zones[4];
        c.bucket=Item(room,"CleaningBucket",Art("P00","cleaning_bucket"),new Vector2(-4.8f,-2.85f),.6f,12);c.bucket.enabled=false;
        c.rag=Item(c.player.transform,"CleaningRag",Art("P00","cleaning_rag"),new Vector2(.25f,.55f),.22f,25);c.rag.enabled=false;
        c.cleaningSpots=new ProductionCleaningSpot[4];
        string[] rubbish={"trash_noodles","trash_cans","trash_bags","trash_small_0"};
        for(int i=0;i<4;i++)
        {
            var point=Node("CleanSpot"+i,room,new Vector2(-2.7f+i*1.8f,-2.75f));point.AddComponent<CircleCollider2D>().isTrigger=true;
            var spot=point.AddComponent<ProductionCleaningSpot>();spot.controller=c;spot.index=i;
            spot.dirt=Item(point.transform,"Dirt",Legacy("props/"+rubbish[i]),Vector2.zero,.35f,10);
            var label=Node("SpotLabel",point.transform,new Vector2(0,.55f)).AddComponent<TextMeshPro>();label.text="Lau điểm "+(i+1);label.fontSize=1.6f;label.rectTransform.sizeDelta=new Vector2(3,1);label.alignment=TextAlignmentOptions.Center;label.GetComponent<MeshRenderer>().sortingOrder=30;spot.marker=label;c.cleaningSpots[i]=spot;
        }
        c.roomFriends=new GameObject[3];string[] friends={"long","to","dam"};
        for(int i=0;i<3;i++)c.roomFriends[i]=Render(room,"RoomFriend_"+friends[i],Art("DerivedExisting",friends[i]+"_right_idle"),new Vector2(-3+i*3,-1.9f),.65f,15).gameObject;
        Render(canteen,"Vendor",Art("C00","npc_canteen_vendor"),new Vector2(-3,.2f),1,0);
        var bg=Art("E01","canteen_background");var texture=bg.texture;
        const string counterPath="Assets/AssetProduction/CanteenCounterV2.asset";
        var counter=AssetDatabase.LoadAssetAtPath<Sprite>(counterPath);
        if(counter==null){counter=Sprite.Create(texture,new Rect(texture.width*.08f,texture.height*.435f,texture.width*.36f,texture.height*.14f),new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect);counter.name="CanteenCounterForeground";AssetDatabase.CreateAsset(counter,counterPath);}
        Render(canteen,"ServiceCounterForeground",counter,new Vector2(-3.84f,.04f),16/bg.bounds.size.x,5);
        c.seatedFriends=new GameObject[4];string[] poses={"luon_lake_sit","dam_lake_sit","long_lake_sit","to_lake_sit"};
        for(int i=0;i<4;i++)c.seatedFriends[i]=Render(canteen,poses[i],Art("C02",poses[i]),new Vector2(i*1.58f,-1.3f),1.2f,3).gameObject;
        var tabletop=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/AssetProduction/CanteenTableTop.asset");
        if(tabletop!=null)Render(canteen,"TableForeground",tabletop,new Vector2((.417f+.502f*.5f-.5f)*16,(.39f+.095f*.5f-.5f)*8),16/bg.bounds.size.x,5);
        c.servedFood=new GameObject[5];
        var tray=Art("P00","meal_tray");var plate=Render(canteen,"LuonMealTray",tray,new Vector2(0,-.48f),1,8);plate.transform.localScale=Vector3.one*(.85f/tray.bounds.size.x);plate.transform.localRotation=Quaternion.Euler(0,0,13);plate.transform.localPosition=new Vector3(0,-.35f,0)-plate.transform.localRotation*Vector3.Scale(tray.bounds.center,plate.transform.localScale);c.servedFood[0]=plate.gameObject;
        c.servedFood[1]=Item(canteen,"MealRice",Art("P00","food_rice"),new Vector2(-.3f,-.35f),.2f,9).gameObject;
        c.servedFood[2]=Item(canteen,"MealMeat",Art("P00","food_meat"),new Vector2(.2f,-.35f),.15f,9).gameObject;
        c.servedFood[3]=Item(canteen,"MealVeg",Art("P00","food_vegetables"),new Vector2(.05f,-.2f),.18f,9).gameObject;
        c.servedFood[4]=Item(canteen,"MealTea",Art("P03","peach_tea"),new Vector2(.6f,-.35f),.3f,9).gameObject;
    }
    private static void BuildHousekeepingUI(ProductionOpeningController c,RectTransform modal)
    {
        var panel=Panel(modal,"MealSelection",new Vector2(.5f,.5f),new Vector2(0,-55),new Vector2(960,140),Color.clear);c.mealPanel=panel.gameObject;
        c.mealButtons=new Button[4];string[] ids={"food_rice","food_meat","food_vegetables","peach_tea"};
        for(int i=0;i<4;i++)
        {
            var button=Button(panel,"MealItem"+i,"",new Vector2(-345+i*230,0));button.GetComponent<RectTransform>().sizeDelta=new Vector2(220,44);
            var text=button.GetComponentInChildren<TMP_Text>();text.fontSize=17;text.rectTransform.offsetMin=new Vector2(32,0);
            var icon=Panel(button.transform,"Icon",new Vector2(0,.5f),new Vector2(18,0),new Vector2(28,28),Color.white).GetComponent<Image>();icon.sprite=Art(i==3?"P03":"P00",ids[i]);icon.preserveAspect=true;
            c.mealButtons[i]=button;
        }
        c.extraMeatButton=Button(panel,"ExtraMeat","Thêm thịt",new Vector2(-300,-50));c.extraMeatButton.GetComponent<RectTransform>().sizeDelta=new Vector2(310,36);c.extraMeatButton.GetComponentInChildren<TMP_Text>().fontSize=17;
        c.mealTotal=LabelAt(panel,"MealTotal","",new Vector2(185,-50),new Vector2(630,42),19);
    }
}
