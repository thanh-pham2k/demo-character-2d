using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public sealed partial class ProductionOpeningController
{
    public GameObject registrationPanel;
    public TMP_Text registrationStatus,registrationSummary;
    public Button registrationRefresh,registrationFullClass;
    public Button[] registrationClasses;
    public SpriteRenderer registrationTyping,registrationLaptop;
    public Sprite[] registrationTypingFrames;
    public float registrationOpeningDelay=5;
    private float registrationRemaining=-1;
    private bool registrationHidPlayer,registrationPreviousVisible;
    private readonly bool[] registeredClasses=new bool[3];
    private static readonly string[] RegistrationNames={"Giải Tích 2 • GT2-0645 • T2 06:45–08:15","Vật Lý 1 • VL1-0645 • T4 06:45–08:15","Kỹ Thuật Lập Trình • KTLT-1300 • T6 13:00–15:15"};
    public int RegistrationSelectedCount {get{int count=0;foreach(bool value in registeredClasses)if(value)count++;return count;}}
    public float RegistrationRemaining=>Mathf.Max(0,registrationRemaining);
    public bool RegistrationVisible=>registrationPanel!=null && registrationPanel.activeInHierarchy;
    private void SetupRegistration()
    {
        if(registrationPanel==null)return;
        registrationPanel.SetActive(false);registrationRefresh.onClick.AddListener(()=>TryRefreshRegistration());
        for(int i=0;i<registrationClasses.Length;i++){int index=i;registrationClasses[i].onClick.AddListener(()=>ToggleRegistrationClass(index));}
        registrationFullClass.interactable=false;
    }
    private void ResetRegistration()
    {
        registrationRemaining=-1;
        for(int i=0;i<registeredClasses.Length;i++)registeredClasses[i]=Progress.Has("action:9:select_classes");
        RestoreRegistrationActor();
    }
    private void OpenRegistration()
    {
        if(Progress.CurrentMission!=9 || registrationPanel==null)return;
        if(registrationRemaining<0)registrationRemaining=Progress.Has("action:9:refresh_registration")?0:registrationOpeningDelay;
        for(int i=0;i<registeredClasses.Length;i++)registeredClasses[i]=Progress.Has("action:9:select_classes");
        body.gameObject.SetActive(false);registrationPanel.SetActive(true);
        var main=player.CharacterVisual.GetComponent<SpriteRenderer>();registrationPreviousVisible=main.enabled;main.enabled=false;registrationHidPlayer=true;registrationTyping.enabled=true;
        UpdateRegistrationUI();
    }
    public bool TryRefreshRegistration()
    {
        if(!RegistrationVisible || Step!="refresh_registration" || travelling)return false;
        if(registrationRemaining>0){registrationStatus.text="08:59 • Cổng chưa mở. Đợi tới 09:00 rồi F5 lại nhé.";return false;}
        return CommitRegistrationStep("refresh_registration");
    }
    public bool ToggleRegistrationClass(int index)
    {
        if(!RegistrationVisible || Step!="select_classes" || index<0 || index>=registeredClasses.Length || travelling)return false;
        registeredClasses[index]=!registeredClasses[index];UpdateRegistrationUI();return true;
    }
    public bool TrySelectFullRegistrationClass()=>false;
    private bool CommitRegistrationStep(string step)
    {
        if(Progress.CurrentMission!=9 || Step!=step || !RegistrationVisible)return false;
        if(!session.Commit(9,step,"",out string error)){registrationStatus.text=error;return false;}
        // Keep the same computer interaction owner while its UI advances. The
        // current model step is committed explicitly; the old action token only owns the actor lock.
        openedFrame=Time.frameCount;title.text=CurrentActionLabel;RefreshStage(false);UpdateRegistrationUI();return true;
    }
    private bool HandleRegistrationConfirm()
    {
        if(Progress.CurrentMission!=9)return false;
        if(Step=="refresh_registration")return true;
        if(Step=="select_classes")
        {
            if(RegistrationSelectedCount!=3){registrationStatus.text="Chọn đủ 3 môn trước khi chốt lịch.";return true;}
            CommitRegistrationStep("select_classes");return true;
        }
        if(Step=="confirm_schedule")
        {
            if(RegistrationSelectedCount!=3){registrationStatus.text="Lịch chưa đủ môn.";return true;}
            StartCoroutine(RegistrationWeekBridge());return true;
        }
        return true;
    }
    private IEnumerator RegistrationWeekBridge()
    {
        travelling=true;
        if(!session.Commit(9,"confirm_schedule","",out string error)){travelling=false;registrationStatus.text=error;yield break;}
        registrationPanel.SetActive(false);body.gameObject.SetActive(true);confirmButton.gameObject.SetActive(false);cancelButton.gameObject.SetActive(false);
        title.text="HỌC KỲ 3 • NHỮNG TUẦN ĐẦU";
        body.text="Lịch đã lưu: T2 và T4 bắt đầu 06:45. Lươn trải qua những tuần học đầu kỳ.\n\nMột tối 23:40, Long rủ: Làm thêm một ván nữa không?\nSáng mai vẫn có lớp sớm.";
        for(float elapsed=0;elapsed<1.8f;){if(!appPaused)elapsed+=Time.deltaTime;yield return null;}
        yield return FadeTo(1);RefreshStage(false);yield return FadeTo(0);travelling=false;Close();RefreshRegistration();Notify(Guide());
    }
    private void UpdateRegistrationUI()
    {
        if(!RegistrationVisible || Progress.CurrentMission!=9)return;
        bool selecting=Step=="select_classes",waiting=Step=="refresh_registration";
        registrationRefresh.gameObject.SetActive(waiting);
        registrationFullClass.gameObject.SetActive(selecting);registrationFullClass.interactable=false;
        for(int i=0;i<registrationClasses.Length;i++)
        {
            var button=registrationClasses[i];button.gameObject.SetActive(selecting);button.interactable=selecting;
            button.GetComponentInChildren<TMP_Text>(true).text=(registeredClasses[i]?"[x] ":"[ ] ")+RegistrationNames[i];
        }
        registrationStatus.text=waiting?(registrationRemaining>0?"08:59 • Còn "+Mathf.CeilToInt(registrationRemaining)+"s tới giờ mở cổng":"09:00 • Cổng đã mở. Bấm F5 hoặc Tải lại."):selecting?"09:00 • Đã chọn "+RegistrationSelectedCount+"/3 môn. Kiểm tra ngày và giờ học.":"LỊCH ĐÃ CHỌN • Kiểm tra lần cuối trước khi xác nhận.";
        registrationSummary.rectTransform.anchoredPosition=selecting?new Vector2(0,-125):waiting?new Vector2(0,40):new Vector2(0,10);
        registrationSummary.rectTransform.sizeDelta=selecting?new Vector2(900,52):waiting?new Vector2(900,90):new Vector2(900,190);
        registrationSummary.fontSize=selecting?18:waiting?22:22;
        registrationSummary.text=waiting?"Cổng đăng ký mở lúc09:00. Bấm trước giờ không giữ chỗ.\nLớp muộn đang rất đông, cần chuẩn bị cả phương án 06:45.":selecting?"Lớp 09:00 đã hết chỗ; các nhóm trên còn chỗ và không trùng lịch.\nLựa chọn chưa chốt sẽ không tự lưu khi đóng cửa sổ.":"T2 • Giải Tích 2 • 06:45\nT4 • Vật Lý 1 • 06:45\nT6 • Kỹ Thuật Lập Trình • 13:00\nĐặt báo thức để kịp xe và lớp sớm.";
        confirmButton.gameObject.SetActive(!waiting);confirmButton.interactable=!selecting || RegistrationSelectedCount==3;
        confirmButton.GetComponentInChildren<TMP_Text>(true).text=selecting?"[Space] Chốt 3 môn":"[Space] Xác nhận lịch 06:45";
    }
    private void RestoreRegistrationActor()
    {
        var visual=player!=null?player.CharacterVisual:null;var renderer=visual!=null?visual.GetComponent<SpriteRenderer>():null;
        if(registrationHidPlayer && renderer!=null)renderer.enabled=registrationPreviousVisible;registrationHidPlayer=false;
        if(registrationTyping!=null)registrationTyping.enabled=false;
    }
    private void CloseRegistration()
    {
        RestoreRegistrationActor();if(registrationPanel!=null)registrationPanel.SetActive(false);
        if(body!=null)body.gameObject.SetActive(true);
        if(confirmButton!=null){confirmButton.gameObject.SetActive(true);confirmButton.interactable=true;confirmButton.GetComponentInChildren<TMP_Text>(true).text="[Space] Xác nhận";}
        if(cancelButton!=null)cancelButton.gameObject.SetActive(true);
    }
    private void RefreshRegistration()
    {
        if(!Ready)return;
        if(Progress.CurrentMission==9 && Step=="refresh_registration" && registrationRemaining<0)registrationRemaining=registrationOpeningDelay;
        if(registrationLaptop!=null)registrationLaptop.enabled=Progress.Has("action:3:place_backpack") && !Progress.Has("room:luon_packed");
        bool night=Progress.CurrentMission==10;
        foreach(var sprite in zones[3].GetComponentsInChildren<SpriteRenderer>(true))sprite.color=night?new Color(.68f,.72f,.86f):Color.white;
        var visual=player!=null?player.CharacterVisual:null;var main=visual!=null?visual.GetComponent<SpriteRenderer>():null;
        if(main!=null)main.color=night?new Color(.68f,.72f,.86f):Color.white;
        if(activeZone==3)view.backgroundColor=night?new Color(.035f,.045f,.07f):Color.white;
    }
    private void UpdateRegistration()
    {
        if(Ready && Progress.CurrentMission==9)
        {
            if(!appPaused && !startPanel.activeSelf && Step=="refresh_registration")registrationRemaining=Mathf.Max(0,registrationRemaining-Time.deltaTime);
            if(RegistrationVisible)
            {
                if(Step=="refresh_registration")UpdateRegistrationUI();
                if(Keyboard.current!=null && Keyboard.current.f5Key.wasPressedThisFrame)TryRefreshRegistration();
            }
        }
        if(registrationHidPlayer && registrationTyping!=null && registrationTypingFrames.Length>0)registrationTyping.sprite=registrationTypingFrames[(int)(Time.time*5)%registrationTypingFrames.Length];
    }
}
