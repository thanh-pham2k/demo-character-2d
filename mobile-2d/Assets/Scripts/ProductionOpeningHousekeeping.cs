using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed partial class ProductionOpeningController
{
    public ProductionCleaningSpot[] cleaningSpots;
    public SpriteRenderer bucket,rag;
    public GameObject[] roomFriends,seatedFriends,servedFood;
    public GameObject mealPanel;
    public Button[] mealButtons;
    public Button extraMeatButton;
    public TMP_Text mealTotal;
    public float cleaningTimeLimit=60;
    private bool visitingCanteen,cleaningActive,cleaningValidated,dinnerHidPlayer,previousPlayerVisible;
    private readonly bool[] cleaned=new bool[4];
    private readonly bool[] mealItems={true,false,true,false};
    private bool extraMeat;
    private ProductionCleaningSpot scrubOwner;
    public bool CleaningActive=>cleaningActive;
    public float CleaningRemaining {get;private set;}
    public int CleanedCount {get{int n=0;foreach(bool value in cleaned)if(value)n++;return n;}}
    public int MealPrice=>(mealItems[0]?5000:0)+(mealItems[1]?10000:0)+(mealItems[2]?5000:0)+(mealItems[3]?3000:0)+(extraMeat?10000:0);
    public bool MealItemSelected(int index)=>index>=0 && index<mealItems.Length && mealItems[index];
    public string MealChoice
    {
        get
        {
            if(!mealItems[0] || !mealItems[1] && !mealItems[2] || extraMeat)return "";
            if(mealItems[1] && mealItems[2])return mealItems[3]?"tea":"standard";
            if(mealItems[1])return mealItems[3]?"meat_tea":"meat";
            return mealItems[3]?"vegetarian_tea":"vegetarian";
        }
    }
    private void ApplyZoneActorScale(){if(player!=null && activeZone>=0)player.CharacterVisual.transform.localScale=Vector3.one*(activeZone==4?1f:activeZone==7?.45f:.65f);}
    private void SetupHousekeeping()
    {
        mealPanel.SetActive(false);
        for(int i=0;i<mealButtons.Length;i++){int index=i;mealButtons[i].onClick.AddListener(()=>ToggleMealItem(index));}
        extraMeatButton.onClick.AddListener(()=>{extraMeat=!extraMeat;if(extraMeat)mealItems[1]=true;UpdateMealCart();});
    }
    private void ResetHousekeeping()
    {
        visitingCanteen=false;cleaningActive=false;cleaningValidated=false;
        for(int i=0;i<cleaned.Length;i++)cleaned[i]=false;
    }
    private int HousekeepingZone(int fallback)
    {
        if(Progress.CurrentMission==4 && Step=="buy_cleaning_kit")return visitingCanteen?4:3;
        if(Progress.CurrentMission==5)return Step=="choose_meal" && !visitingCanteen?3:4;
        return fallback;
    }
    private string HousekeepingDescription()
    {
        switch(Step)
        {
            case "buy_cleaning_kit":return activeZone==3?"Phòng còn bụi. Xuống quầy căn tin mua xô và giẻ rồi về dọn nhé.":"Bộ vệ sinh gồm xô và giẻ: 5.000đ. Mua một lần, có thể dùng lại khi thử dọn phòng.";
            case "clean_four_spots":return "Lau đủ bốn điểm bụi trong 60 giây. Tới gần từng điểm rồi bấm E hoặc Tương tác. Hết giờ chỉ thử lại phần dọn, không mua lại đồ.";
            case "meet_roommates":return RoommateLine();
            case "choose_meal":return activeZone==3?"Long, Tộ và Đạm rủ xuống căn tin ăn cùng. Đi tới cửa để cùng nhóm xuống quầy.":"Chọn cơm và ít nhất một món ăn; trà tuỳ chọn. Kiểm tra tổng tiền trước khi thanh toán.";
            case "eat_together":return DinnerLine();
            case "receive_schedule":return "Điện thoại báo lịch học ngày mai. Mẹ nhắn đã gửi 100.000đ sinh hoạt đầu kỳ. Xem lịch, kiểm tra ví rồi về nghỉ.";
            case "return_sleep":return "Cả nhóm về phòng, chuẩn bị cho buổi học đầu tiên. Chuyến xe tới trường là hành trình riêng vào sáng mai.";
            default:return null;
        }
    }
    private string RoommateLine()
    {
        string[] lines={"Long: Tui là Long. Cùng phòng rồi, có gì cứ gọi nhau nha!","Tộ: Tui mang ít đồ quê lên. Dọn phòng xong rồi đi ăn chung cho vui!","Đạm: Tui học bên HCMUS. Mấy ông học khác trường nhưng về 404 vẫn là anh em nhé."};
        return lines[Mathf.Clamp(dialogueLine,0,2)];
    }
    private string DinnerLine()
    {
        string[] lines={"Long: Mỗi người chọn món vừa túi tiền. Ngồi đây ăn chung nè!","Tộ: No bụng rồi mai mới đi học được. Có đồ quê tui sẽ chia cho cả phòng.","Đạm: Mai tui qua trường bên kia. Tối về kể nhau nghe buổi học đầu nha.","Lươn: Vậy chốt nhé. Ăn xong coi lịch rồi về chuẩn bị balo!"};
        return lines[Mathf.Clamp(dialogueLine,0,3)];
    }
    private void OpenHousekeeping()
    {
        bool choosing=Step=="choose_meal" && activeZone==4;
        mealPanel.SetActive(choosing);
        if(choosing){mealItems[0]=true;mealItems[1]=false;mealItems[2]=true;mealItems[3]=false;extraMeat=false;UpdateMealCart();}
        if(Step=="eat_together")
        {
            var renderer=player.CharacterVisual.GetComponent<SpriteRenderer>();previousPlayerVisible=renderer.enabled;renderer.enabled=false;dinnerHidPlayer=true;
        }
        RefreshHousekeepingVisuals();
    }
    private bool HandleHousekeepingConfirm()
    {
        if((Step=="buy_cleaning_kit" || Step=="choose_meal") && activeZone==3){StartCoroutine(VisitCanteen());return true;}
        if(Step=="buy_cleaning_kit"){StartCoroutine(Travel());return true;}
        if(Step=="clean_four_spots")
        {
            Close();cleaningActive=true;cleaningValidated=false;CleaningRemaining=Mathf.Max(.1f,cleaningTimeLimit);
            for(int i=0;i<cleaned.Length;i++)cleaned[i]=false;
            Notify("Dọn lần lượt bốn điểm bụi. Tới gần từng điểm rồi bấm E hoặc Tương tác.");RefreshHousekeepingVisuals();return true;
        }
        if(Step=="meet_roommates" && dialogueLine<2){dialogueLine++;body.text=RoommateLine();RefreshHousekeepingVisuals();return true;}
        if(Step=="eat_together" && dialogueLine<3){dialogueLine++;body.text=DinnerLine();return true;}
        if(Step=="choose_meal"){Commit(MealChoice);return true;}
        if(Step=="return_sleep"){StartCoroutine(Travel());return true;}
        return false;
    }
    private bool CanCommitHousekeeping(string choice)
    {
        if(Step=="buy_cleaning_kit" && activeZone!=4)return false;
        if(Step=="clean_four_spots" && !cleaningValidated)return false;
        if(Step=="meet_roommates" && dialogueLine<2 || Step=="eat_together" && dialogueLine<3)return false;
        if(Step=="choose_meal")
        {
            if(activeZone!=4 || choice=="" || choice!=MealChoice || MealPrice>Progress.Money)return false;
            bool priced=false;foreach(var option in Progress.CurrentStep.choices)if(option.id==choice && option.cost+Progress.CurrentStep.cost==MealPrice)priced=true;
            if(!priced)return false;
        }
        return true;
    }
    private IEnumerator VisitCanteen()
    {
        travelling=true;modal.SetActive(false);yield return FadeTo(1);visitingCanteen=true;RefreshStage(true);yield return FadeTo(0);travelling=false;Close();
    }
    private void CloseHousekeeping()
    {
        mealPanel.SetActive(false);confirmButton.interactable=true;
        if(dinnerHidPlayer){player.CharacterVisual.GetComponent<SpriteRenderer>().enabled=previousPlayerVisible;dinnerHidPlayer=false;}
        if(Ready)RefreshHousekeepingVisuals();
    }
    public void ToggleMealItem(int index)
    {
        if(!ModalVisible || Step!="choose_meal" || activeZone!=4 || index<0 || index>=mealItems.Length)return;
        mealItems[index]=!mealItems[index];if(index==1 && !mealItems[index])extraMeat=false;UpdateMealCart();
    }
    private void UpdateMealCart()
    {
        string[] labels={"Cơm • 5.000đ","Thịt • 10.000đ","Rau • 5.000đ","Trà • 3.000đ"};
        for(int i=0;i<mealButtons.Length;i++)mealButtons[i].GetComponentInChildren<TMP_Text>().text=(mealItems[i]?"[x] ":"[ ] ")+labels[i];
        extraMeatButton.GetComponentInChildren<TMP_Text>().text=(extraMeat?"[x] ":"[ ] ")+"Thêm thịt • 10.000đ";
        string total=MealPrice.ToString("N0",System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
        mealTotal.text="Tổng: "+total+"đ / Ví: "+Progress.Money.ToString("N0",System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))+"đ";
        bool valid=MealChoice!="" && MealPrice<=Progress.Money;confirmButton.interactable=valid;
        if(MealPrice>Progress.Money)mealTotal.text+=" • Vượt ngân sách";
        else if(MealChoice=="")mealTotal.text+=" • Cần cơm và một món ăn";
        aidButton.gameObject.SetActive(Progress.Money<10000 && MealChoice=="vegetarian");
    }
    public bool CanClean(int index)=>Ready && cleaningActive && !Busy && !appPaused && !startPanel.activeSelf && Step=="clean_four_spots" && index>=0 && index<4 && !cleaned[index];
    public void Scrub(ProductionCleaningSpot spot,PlayerInteractionStateController actor)
    {
        if(!CanClean(spot.index) || actor!=player || Vector2.Distance(actor.transform.position,spot.transform.position)>1.8f)return;
        if(!player.TryEnterState(PlayerInteractionState.Transitioning,null,spot))return;
        scrubOwner=spot;StartCoroutine(ScrubRoutine(spot));
    }
    private IEnumerator ScrubRoutine(ProductionCleaningSpot spot)
    {
        float elapsed=0;
        while(cleaningActive && elapsed<.8f){if(!appPaused)elapsed+=Time.deltaTime;yield return null;}
        if(cleaningActive)
        {
            cleaned[spot.index]=true;
            if(CleanedCount==4)
            {
                cleaningValidated=true;
                if(session.Commit(4,"clean_four_spots","",out string error)){cleaningActive=false;RefreshStage(false);}
                else{cleaningActive=false;for(int i=0;i<4;i++)cleaned[i]=false;Notify(error);}
            }
        }
        if(scrubOwner==spot){player.ExitInteraction();scrubOwner=null;}
        RefreshHousekeepingVisuals();
    }
    private void UpdateHousekeeping()
    {
        if(cleaningActive && !appPaused && !startPanel.activeSelf)
        {
            CleaningRemaining=Mathf.Max(0,CleaningRemaining-Time.deltaTime);
            if(CleaningRemaining<=0)
            {
                cleaningActive=false;for(int i=0;i<4;i++)cleaned[i]=false;
                RefreshStage(false);Notify("Hết giờ. Dụng cụ vẫn còn; tới điểm dọn để thử lại, không mất thêm tiền.");
            }
            else hud.text="M04 • Dọn phòng: "+CleanedCount+"/4 • Còn "+Mathf.CeilToInt(CleaningRemaining)+" giây\nVí: "+Progress.Money.ToString("N0",System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))+"đ";
        }
        rag.enabled=scrubOwner!=null;
        if(rag.enabled)rag.transform.position=player.transform.position+new Vector3(.25f,.55f,0);
    }
    private void RefreshHousekeepingVisuals()
    {
        bool done=Progress.Has("action:4:clean_four_spots");
        foreach(var spot in cleaningSpots)spot.dirt.enabled=!done && !cleaned[spot.index];
        bucket.enabled=Progress.Has("item:cleaning_kit");
        bool met=Progress.Has("action:4:meet_roommates");
        for(int i=0;i<roomFriends.Length;i++)roomFriends[i].SetActive(met || Step=="meet_roommates" && ModalVisible && dialogueLine>=i);
        for(int i=0;i<seatedFriends.Length;i++)seatedFriends[i].SetActive(Progress.CurrentMission==5 && met && (i!=0 || dinnerHidPlayer));
        bool paid=Progress.Has("action:5:choose_meal");
        bool meat=Progress.Has("meal:meat") || Progress.Has("meal:standard") || Progress.Has("meal:meat_tea") || Progress.Has("meal:tea");
        bool veg=!Progress.Has("meal:meat") && !Progress.Has("meal:meat_tea");
        bool tea=Progress.Has("meal:vegetarian_tea") || Progress.Has("meal:meat_tea") || Progress.Has("meal:tea");
        bool[] food={paid,paid,paid && meat,paid && veg,paid && tea};for(int i=0;i<servedFood.Length;i++)servedFood[i].SetActive(food[i]);
    }
    private void DisableHousekeeping()
    {
        cleaningActive=false;if(scrubOwner!=null && player!=null)player.ExitInteraction();scrubOwner=null;
        if(rag!=null)rag.enabled=false;
        var visual=player!=null?player.CharacterVisual:null;
        var renderer=visual!=null?visual.GetComponent<SpriteRenderer>():null;
        if(dinnerHidPlayer && renderer!=null)renderer.enabled=previousPlayerVisible;
        dinnerHidPlayer=false;
    }
}
