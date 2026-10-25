using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Physical adapters forM01–09. Later missions retain their authored data
// but receive no generic advance/complete button in this scene.
[DefaultExecutionOrder(200)]
public sealed partial class ProductionOpeningController : MonoBehaviour
{
    public TextAsset definition;
    public PlayerInteractionStateController player;
    public Camera view;
    public Transform[] zones,spawns;
    public ProductionOpeningAction[] actions;
    public GameObject startPanel,modal,documents;
    public TMP_Text hud,title,body,notice;
    public Button beginButton,confirmButton,cancelButton,aidButton,leftButton,rightButton,interactButton;
    public ProductionHoldButton holdButton;
    public Slider holdMeter;
    public Image fade;
    public SpriteRenderer placedBag,fan;
    public Transform leftBag,rightBag;
    private ProductionCampaignSession session;
    private string saveDirectory;
    private ProductionOpeningAction owner;
    private int activeZone=-1,dialogueLine,openedFrame;
    private float holdProgress;
    private bool appPaused,travelling,documentValidated;
    public bool Ready=>session!=null;
    public IProductionCampaignView Progress=>session?.Progress;
    public bool Busy=>owner!=null || scrubOwner!=null || travelling || schoolJourney!=null && schoolJourney.IsBusy;
    public bool ModalVisible=>modal!=null && modal.activeSelf;
    public float HoldProgress=>holdProgress;
    public int ActiveZone=>activeZone;
    private string Step=>Progress?.CurrentStep?.id??"";
    public string CurrentActionLabel=>Step=="arrive_dorm"?"Lên xe tới KTX":(Progress?.CurrentStep?.label??"").Replace("khởi động55","khởi động").Replace("Xem32tín chỉ","Xem 32 tín chỉ").Replace("hàngWC","hàng WC").Replace("học kỳ3","học kỳ 3").Replace("lịch06:45","lịch 06:45").Replace("404"," 404").Replace("tầng4","tầng 4").Replace("đủ4điểm","đủ 4 điểm");
    private void Start()
    {
        player.TryEnterState(PlayerInteractionState.Transitioning);
        interactButton.onClick.AddListener(()=>player.GetComponent<PlayerInteractionDetector>().TryInteract());
        startPanel.SetActive(true);modal.SetActive(false);fade.color=Color.clear;SetupHousekeeping();SetupSchool();SetupYearOne();SetupRegistration();
        beginButton.onClick.AddListener(()=>Begin());
        cancelButton.onClick.AddListener(Cancel);
        confirmButton.onClick.AddListener(Confirm);
        aidButton.onClick.AddListener(RequestAid);
        leftButton.onClick.AddListener(()=>Commit("left"));rightButton.onClick.AddListener(()=>Commit("right"));
        hud.text="PHÒNG404 • NHẬP HỌC\nWASD/joystick để đi, E hoặc nút Tương tác để dùng đồ.";
    }
    public void UseSaveDirectoryForTests(string directory)
    {if(Ready)throw new InvalidOperationException("Select save directory before starting");saveDirectory=directory;}
    public void Begin()
    {
        if(Busy)return;
        try
        {
            session=new ProductionCampaignSession(saveDirectory??Path.Combine(Application.persistentDataPath,"production"),definition.text);
            ResetHousekeeping();ResetSchool();ResetYearOne();ResetRegistration();startPanel.SetActive(false);player.ExitInteraction();RefreshStage(true);
        }
        catch(Exception ex){Notify("Không tải được checkpoint. Giữ nguyên file để phục hồi. "+ex.Message);}
    }
    public bool CanUse(ProductionOpeningAction action)=>Ready && !Busy && !startPanel.activeSelf && action.isActiveAndEnabled && !(Step=="clean_four_spots" && cleaningActive) && !(Step=="join_wc_queue" && queueReady) && Progress.CurrentMission==action.mission && Step==action.step;
    public void Open(ProductionOpeningAction action)
    {
        if(!CanUse(action) || Vector2.Distance(player.transform.position,action.transform.position)>1.8f)return;
        if(!player.TryEnterState(PlayerInteractionState.Transitioning,null,action))return;
        owner=action;documentValidated=false;dialogueLine=0;openedFrame=Time.frameCount;holdProgress=0;holdButton.Held=false;
        modal.SetActive(true);title.text=CurrentActionLabel;
        documents.SetActive(Step=="present_documents");holdButton.gameObject.SetActive((Step=="repair_fan" || Step=="plug_cooker"));holdMeter.gameObject.SetActive((Step=="repair_fan" || Step=="plug_cooker"));
        leftButton.gameObject.SetActive(Step=="claim_bed");rightButton.gameObject.SetActive(Step=="claim_bed");
        confirmButton.gameObject.SetActive(Step!="present_documents" && Step!="repair_fan" && Step!="plug_cooker" && Step!="claim_bed");
        aidButton.gameObject.SetActive(Progress.Money<Progress.CurrentStep.cost);
        body.text=Description();OpenHousekeeping();OpenYearOne();OpenRegistration();
        holdButton.GetComponentInChildren<TMP_Text>(true).text=Step=="plug_cooker"?"Giữ để cắm bếp":"Giữ để siết ốc và cắm quạt";
    }
    private string Description()
    {
        string school=SchoolDescription();if(school!=null)return school;
        string housekeeping=HousekeepingDescription();if(housekeeping!=null)return housekeeping;
        switch(Step)
        {
            case "decline_tout":return "Người mời xe: Đi xe riêng không em?\nLươn kiểm tra bản đồ: xe tới KTX có điểm đón phía trước.";
            case "buy_ticket":return "Vé xe tới KTX: 7.000đ\nVí hiện có: "+Progress.Money.ToString("N0",System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))+"đ.\nXác nhận để mua một vé.";
            case "arrive_dorm":return "Lên xe tại điểm đón. Xe sẽ đưa Lươn tới sảnh tiếp nhận KTX.";
            case "present_documents":return "Kéo thẻ GIẤY NHẬP HỌC vào ô tiếp nhận của cô quản lý. Balo không thay cho giấy tờ.";
            case "receive_room_key":return "Cô quản lý: Hồ sơ đủ rồi. Em nhận phòng 404, tầng 4 nhé.\nNhận chìa khóa rồi đi tới cầu thang.";
            case "reach_room":return "Đi lên tầng 4, tìm cửa phòng 404.";
            case "unlock_room":return "Dùng chìa khóa404 để mở cửa phòng.";
            case "claim_bed":return "Chọn một chỗ ngủ còn trống. Lựa chọn sẽ được lưu cùng checkpoint.";
            case "place_backpack":return "Đặt balo lên chỗ ngủ đã chọn. Giấy nhập học đã nộp; tua vít vẫn ở trong ngăn đồ.";
            case "repair_fan":return "Dùng tua vít siết chân quạt, rồi cắm điện.\nGiữ nút hoặc Space trong 3 giây. Thả tay sẽ dừng thao tác.";
            default:return "";
        }
    }
    public void Confirm()
    {
        if(!ModalVisible || travelling || Time.frameCount==openedFrame)return;
        if(HandleHousekeepingConfirm() || HandleSchoolConfirm())return;
        if(Step=="decline_tout" && dialogueLine++==0){body.text="Lươn: Dạ em đi xe tới KTX ở điểm đón kia. Cảm ơn anh!";return;}
        if(Step=="present_documents" || (Step=="repair_fan" || Step=="plug_cooker") || Step=="claim_bed")return;
        if(Step=="arrive_dorm" || Step=="reach_room" || Step=="unlock_room"){StartCoroutine(Travel());return;}
        Commit("");
    }
    public bool PresentDocument(string item)
    {
        if(!ModalVisible || Step!="present_documents")return false;
        if(item!="item:admission_documents" || !Progress.Has(item)){Notify("Cô quản lý cần giấy nhập học, không phải món đồ này.");return false;}
        documentValidated=true;return Commit("");
    }
    public bool Commit(string choice)
    {
        if(owner==null || !ModalVisible || travelling)return false;
        if(Progress.CurrentMission==9 || !CanCommitHousekeeping(choice) || !CanCommitYearOne() || InSchoolMission && (Step=="solve_warmup" || Step=="enter_class"))return false;
        if((Step=="repair_fan" || Step=="plug_cooker") && holdProgress<1 || Step=="present_documents" && !documentValidated)return false;
        if(!session.Commit(owner.mission,owner.step,choice,out string error)){Notify(error);return false;}
        Close();RefreshStage(false);return true;
    }
    private void RequestAid()
    {
        if(owner==null)return;
        if(session.RequestAssistance(Step=="choose_meal"?MealChoice:"",out string error)){body.text=Description();aidButton.gameObject.SetActive(false);Notify("Đã nhận hỗ trợ đúng khoản còn thiếu.");RefreshHudText();if(Step=="choose_meal")UpdateMealCart();}
        else Notify(error);
    }
    private IEnumerator Travel()
    {
        travelling=true;modal.SetActive(false);
        int mission=owner.mission;string step=owner.step;
        yield return FadeTo(1);
        yield return new WaitForSeconds(.7f);
        bool committed=session.Commit(mission,step,"",out string error);
        if(committed && step=="buy_cleaning_kit")visitingCanteen=false;
        RefreshStage(true);if(!committed)Notify(error);
        yield return FadeTo(0);
        travelling=false;Close();
    }
    private IEnumerator FadeTo(float alpha)
    {
        float from=fade.color.a;
        for(float t=0;t<.3f;t+=Time.deltaTime){fade.color=new Color(0,0,0,Mathf.Lerp(from,alpha,t/.3f));yield return null;}
        fade.color=new Color(0,0,0,alpha);
    }
    public void Cancel(){if(!travelling)Close();}
    private void Close()
    {
        modal.SetActive(false);holdProgress=0;holdButton.Held=false;CloseHousekeeping();RestoreSchoolActor();CloseYearOne();CloseRegistration();
        if(owner!=null)player.ExitInteraction();owner=null;ApplyZoneActorScale();
    }
    private void RefreshStage(bool respawn)
    {
        int next=Progress.CurrentMission==1?0:Progress.CurrentMission==2?1:Step=="unlock_room"?2:3;
        next=SchoolZone(HousekeepingZone(next));
        bool changed=next!=activeZone;activeZone=next;
        for(int i=0;i<zones.Length;i++)zones[i].gameObject.SetActive(i==activeZone);
        view.transform.position=zones[next].position+new Vector3(0,0,-10);
        view.backgroundColor=next==3?Color.white:new Color(.08f,.1f,.14f);
        if(changed || respawn)
        {
            player.transform.position=spawns[next].position;
            var rb=player.GetComponent<Rigidbody2D>();rb.position=player.transform.position;rb.linearVelocity=Vector2.zero;
        }
        bool right=Progress.Has("bed:luon_right");
        placedBag.transform.position=(right?rightBag:leftBag).position;
        placedBag.enabled=Progress.Has("room:backpack_stored") && (!Progress.Has("action:6:equip_backpack") || Progress.Has("action:7:read_year_results"));
        player.CharacterVisual.SetBackpackEquipped(!placedBag.enabled);
        foreach(var action in actions)if(action.step=="place_backpack" || action.step=="equip_backpack" || action.step=="sleep_new_term")action.transform.position=zones[3].position+new Vector3(right?2.8f:-2.8f,-2.2f,0);
        RefreshHudText();
        Notify(Guide());RefreshHousekeepingVisuals();ApplyZoneActorScale();RefreshSchool();RefreshQueue();RefreshFeast();RefreshRegistration();
    }
    private void RefreshHudText()
    {
        hud.text="M"+Progress.CurrentMission.ToString("D2")+" • "+(Progress.CurrentStep!=null?CurrentActionLabel:"Hành trình hoàn tất")+"\nVí: "+Progress.Money.ToString("N0",System.Globalization.CultureInfo.GetCultureInfo("vi-VN"))+"đ • Tín chỉ: "+Progress.Credits+"/150";
    }
    private string Guide()
    {
        string school=SchoolGuide();if(school!=null)return school;
        switch(Step)
        {
            case "decline_tout":return "Tới gần người mời xe để trò chuyện.";
            case "buy_ticket":return "Đi tới quầy vé trước điểm đón xe.";
            case "arrive_dorm":return "Tới cửa xe ở phía bên phải để lên xe tới KTX.";
            case "present_documents":return "Đến quầy tiếp nhận, chuẩn bị giấy nhập học.";
            case "receive_room_key":return "Nhận chìa khóa phòng 404 từ cô quản lý.";
            case "reach_room":return "Tới cầu thang bên trái sảnh, lên tầng 4.";
            case "unlock_room":return "Tìm cửa phòng 404 và dùng chìa khóa.";
            case "claim_bed":return "Tới giường để chọn chỗ ngủ còn trống.";
            case "place_backpack":return "Đặt balo lên giường "+(Progress.Has("bed:luon_right")?"bên phải.":"bên trái.");
            case "repair_fan":return "Tới chiếc quạt nhỏ, dùng tua vít để siết ốc và cắm điện.";
            case "buy_cleaning_kit":return activeZone==3?"Tới cửa phòng để xuống quầy căn tin mua xô và giẻ.":"Mua bộ vệ sinh tại quầy rồi mang về phòng.";
            case "clean_four_spots":return "Tới điểm bắt đầu dọn phòng. Có bốn điểm bụi cần lau.";
            case "meet_roommates":return "Các bạn đã tới. Tới giữa phòng để làm quen từng người.";
            case "choose_meal":return activeZone==3?"Cả nhóm rủ đi ăn. Tới cửa để cùng xuống căn tin.":"Tới quầy cơm và chọn món trong ngân sách.";
            case "eat_together":return "Tới bàn nhóm đang ngồi để cùng ăn và trò chuyện.";
            case "receive_schedule":return "Xem tin nhắn và lịch học trên điện thoại.";
            case "return_sleep":return "Tới lối ra căn tin để về phòng nghỉ.";
            case "equip_backpack":return "Sáng hôm sau: chuẩn bị balo cho buổi học đầu tiên.";
            default:return "Chỗ ở đã sẵn sàng. Tiếp theo: mua bộ vệ sinh và dọn phòng cùng bạn.";
        }
    }
    private void Update()
    {
        interactButton.interactable=Ready && !Busy && !startPanel.activeSelf;
        if(!Ready)return;
        if(Keyboard.current!=null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if(ModalVisible)Cancel();
            else if(!Busy)
            {
                bool opening=!startPanel.activeSelf;startPanel.SetActive(opening);FlushMorningClock();
                if(opening)player.TryEnterState(PlayerInteractionState.Transitioning);else player.ExitInteraction();
            }
        }
        if(ModalVisible && (Step=="repair_fan" || Step=="plug_cooker"))
        {
            bool held=!appPaused && (holdButton.Held || Keyboard.current!=null && Keyboard.current.spaceKey.isPressed);
            holdProgress=held?Mathf.Min(1,holdProgress+Time.deltaTime/(Step=="plug_cooker"?1f:3f)):0;holdMeter.value=holdProgress;
            if(holdProgress>=1)Commit("");
        }
        else if(ModalVisible && Time.frameCount>openedFrame && Keyboard.current!=null && Keyboard.current.spaceKey.wasPressedThisFrame)Confirm();
        UpdateHousekeeping();UpdateSchool();UpdateYearOne();UpdateRegistration();
        if(Progress.Has("action:3:repair_fan"))fan.transform.localRotation=Quaternion.Euler(0,0,Mathf.Sin(Time.time*2)*2);
    }
    private void OnApplicationPause(bool paused){appPaused=paused;if(paused)FlushMorningClock();if(paused){holdProgress=0;if(holdButton!=null)holdButton.Held=false;}}
    private void OnDisable()
    {
        FlushMorningClock();StopAllCoroutines();travelling=false;DisableHousekeeping();RestoreRegistrationActor();RestoreFeastActor();RestoreSchoolActor();if(schoolPuzzle!=null)schoolPuzzle.Cancel();
        if(player!=null && owner!=null)player.ExitInteraction();owner=null;
        if(modal!=null)modal.SetActive(false);
        if(fade!=null)fade.color=Color.clear;
    }
    public void Notify(string message){if(notice!=null)notice.text=message;}
}
