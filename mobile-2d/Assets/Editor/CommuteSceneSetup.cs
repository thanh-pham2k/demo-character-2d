using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using static DormJourneySceneSetup;

// Additive authoring for hook, desk activities and campus. Existing room layout is retained.
public static class CommuteSceneSetup
{
    private const string Folder="Assets/Sprites/MVP/commute/";
    private static Sprite Sprite(string name,Vector2 pivot)=>LoadSprite(Folder+name+".png",pivot);
    public static void Configure(Scene scene,DormJourneyController journey,PlayerInteractionStateController player,
        MVPFlowController flow,Transform canvas,DeskActionModal modal,Transform hall)
    {
        var old=flow.backpackProp.GetComponent<BackpackInteractable>();
        if(old!=null)Object.DestroyImmediate(old);
        var oldCollider=flow.backpackProp.GetComponent<CircleCollider2D>();if(oldCollider!=null)Object.DestroyImmediate(oldCollider);
        flow.backpackProp.GetComponent<SpriteRenderer>().enabled=false; // Story activation remains intact; the wall hook is the physical pickup.
        var hook=Child(Find(scene,"--- ENVIRONMENT ---").transform,"Backpack_Hook_Prop");
        hook.localPosition=new Vector3(1.05f,.1f,0f);
        var hookArt=Child(hook,"Hook_Visual");hookArt.localPosition=new Vector3(0,.9f,0);hookArt.localScale=new Vector3(.5f,.5f,1f);
        var hookRenderer=Component<SpriteRenderer>(hookArt.gameObject);hookRenderer.sprite=Sprite("hook_full",new Vector2(.5f,.5f));hookRenderer.enabled=true;hookRenderer.sortingOrder=-75;
        var pickup=Component<BackpackInteractable>(hook.gameObject);
        pickup.Configure(journey,hookRenderer,hookRenderer.sprite,Sprite("hook_empty",new Vector2(.5f,.5f)));
        var hookTrigger=Component<CircleCollider2D>(hook.gameObject);hookTrigger.radius=.45f;hookTrigger.isTrigger=true;
        var study=new[]{Sprite("study_0",new Vector2(.5f,.5f)),Sprite("study_1",new Vector2(.5f,.5f)),Sprite("study_2",new Vector2(.5f,.5f))};
        var typing=new[]{Sprite("typing_0",new Vector2(.5f,.5f)),Sprite("typing_1",new Vector2(.5f,.5f))};
        var upper=Child(player.CharacterVisual.transform,"Desk_UpperBody");upper.localPosition=new Vector3(0,.2667f,0);
        var upperRenderer=Component<SpriteRenderer>(upper.gameObject);upperRenderer.enabled=false;
        var laptop=Find(scene,"Laptop_Prop").GetComponent<SpriteRenderer>();
        upperRenderer.sortingLayerID=laptop.sortingLayerID;upperRenderer.sortingOrder=laptop.sortingOrder+2;
        player.CharacterVisual.ConfigureDeskSprites(study,typing,FrontSprites(study,"Study",true),FrontSprites(typing,"Typing",true),FrontSprites(study,"Study",false),FrontSprites(typing,"Typing",false),upperRenderer);
        var glow=Child(laptop.transform,"Activity_Screen_Glow");
        glow.localPosition=new Vector3(-1.38f,-.07f,0);glow.localRotation=Quaternion.Euler(0,0,-7f);glow.localScale=new Vector3(3.7f,2.2f,1);
        var glowRenderer=Component<SpriteRenderer>(glow.gameObject);
        glowRenderer.sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Room404Layout/DoorOpeningSprite.asset");
        glowRenderer.color=new Color(.1f,.65f,1f,.32f);glowRenderer.sortingLayerID=laptop.sortingLayerID;glowRenderer.sortingOrder=laptop.sortingOrder+1;glowRenderer.enabled=false;
        modal.ConfigureScreenGlow(glowRenderer);
        var book=canvas.Find("DeskActionModal/Panel/Book_Icon");
        foreach(Transform child in book)child.gameObject.SetActive(false);
        var bookImage=book.GetComponent<Image>();bookImage.sprite=Sprite("notebook",new Vector2(.5f,.5f));bookImage.color=Color.white;bookImage.preserveAspect=true;
        var hud=canvas.Find("Journey_HUD").GetComponent<RectTransform>();hud.sizeDelta=new Vector2(650,54);hud.anchoredPosition=new Vector2(0,-34);
        canvas.Find("Journey_HUD/Mission").GetComponent<TMP_Text>().fontSize=18;

        var campus=Root(scene,"Campus_Zone");campus.position=new Vector3(48f,0f,0f);
        var backdrop=Component<SpriteRenderer>(Child(campus,"Campus_Backdrop").gameObject);
        backdrop.sprite=Sprite("campus",new Vector2(.5f,.5f));backdrop.sortingOrder=-100;
        float scale=20f/backdrop.sprite.bounds.size.x;backdrop.transform.localScale=new Vector3(scale,scale,1f);
        Wall(campus,"Campus_Back_Wall",new Vector2(0,-.08f),new Vector2(20,.15f));
        Wall(campus,"Campus_Curb",new Vector2(0,-1.5f),new Vector2(20,.15f));
        Wall(campus,"Campus_West",new Vector2(-10,0),new Vector2(.2f,3));
        Wall(campus,"Campus_East",new Vector2(10,0),new Vector2(.2f,3));
        var arrival=Child(campus,"Lobby_ExitPivot");arrival.localPosition=new Vector3(-7.4f,-.4f,0);
        var hallArrival=Child(hall,"Stairs_ReturnPivot");hallArrival.localPosition=new Vector3(7.5f,.1f,0);
        var stop=Child(campus,"Bus_Stop_33");stop.localPosition=new Vector3(7.7f,-.4f,0);Trigger(stop,new Vector2(.8f,.8f));
        var sign=Child(stop,"Stop_Sign");sign.localPosition=new Vector3(.5f,-.7f,0);
        var signRenderer=Component<SpriteRenderer>(sign.gameObject);signRenderer.sprite=Sprite("bus_sign",new Vector2(.5f,0));signRenderer.sortingOrder=-20;
        var park=Child(campus,"Bus_Park");park.localPosition=new Vector3(3.2f,-3f,0);
        var offscreen=Child(campus,"Bus_Offscreen");offscreen.localPosition=new Vector3(20,-3f,0);
        var busEntry=Child(campus,"Bus_Entry");busEntry.localPosition=new Vector3(-20,-3f,0);
        var boarding=Child(campus,"Bus_BoardingPivot");boarding.localPosition=new Vector3(5.4f,-.95f,0);
        var vehicle=Child(campus,"Bus_33");vehicle.position=offscreen.position;
        var bus=Component<SpriteRenderer>(vehicle.gameObject);bus.sprite=Sprite("bus_33",new Vector2(.5f,0));bus.sortingOrder=-30;bus.enabled=false;
        WorldLabel(campus,"Campus_Lobby_Label","SẢNH KTX KHU B",new Vector3(-7.4f,2.5f),new Color(.08f,.17f,.18f));
        WorldLabel(campus,"Bus_Stop_Label","TRẠM XE BUÝT 33",new Vector3(7.4f,2.5f),new Color(.08f,.17f,.18f));
        var commute=Component<CampusCommuteController>(journey.gameObject);
        var audio=Component<AudioSource>(vehicle.gameObject);audio.playOnAwake=false;
        var card=Panel(canvas,"Commute_Completed",new Vector2(.5f,.5f),Vector2.zero,new Vector2(0,350),new Color(.04f,.1f,.14f,.98f));
        card.anchorMin=new Vector2(.08f,.5f);card.anchorMax=new Vector2(.92f,.5f);
        LabelAt(card,"Heading","HOÀN THÀNH HÀNH TRÌNH ĐI HỌC",new Vector2(0,125),new Vector2(500,60),26);
        var receipt=LabelAt(card,"Receipt","",new Vector2(0,10),new Vector2(500,160),20);
        var goBack=Button(card,"Return_Room","Trở về phòng 404",new Vector2(0,-125));
        card.gameObject.SetActive(false);
        commute.Configure(journey,arrival,hallArrival,stop,park,offscreen,boarding,backdrop,
            hall.Find("Hallway_Backdrop").GetComponent<SpriteRenderer>(),bus,audio,card.gameObject,receipt,goBack);
        journey.ConfigureCommute(commute);
        commute.ConfigureEntry(busEntry);
        Component<BusStopInteractable>(stop.gameObject).Configure(commute);
        var lobby=Child(campus,"Lobby_Stairs_Entry");lobby.localPosition=arrival.localPosition;Trigger(lobby,new Vector2(.8f,.8f));
        Component<CampusLobbyInteractable>(lobby.gameObject).Configure(commute);
        YearOneSceneSetup.Configure(scene,journey,player,flow,commute);
        EditorUtility.SetDirty(commute);EditorUtility.SetDirty(journey);EditorUtility.SetDirty(modal);EditorUtility.SetDirty(player.CharacterVisual);
    }
    private static Sprite[] FrontSprites(Sprite[] frames,string activity,bool upper)
    {
        var result=new Sprite[frames.Length];
        for(int i=0;i<frames.Length;i++)
        {
            string path="Assets/Room404Layout/Desk_"+activity+(upper?"_Upper_":"_Lower_")+i+".asset";
            result[i]=AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if(result[i]!=null)continue;
            var frame=frames[i];Rect rect=new Rect(0,upper?frame.rect.height*.5f:0,frame.rect.width,frame.rect.height*.5f);
            result[i]=UnityEngine.Sprite.Create(frame.texture,rect,new Vector2(.5f,upper?0:1),100,0,SpriteMeshType.FullRect);
            AssetDatabase.CreateAsset(result[i],path);
        }
        return result;
    }
}
