using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Small local journey flags; the existing four-stage story remains owned by MVPFlowController.
public class DormJourneyController : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInteractionStateController player;
    [SerializeField] private Camera view;
    [SerializeField] private Transform hallwayArrival, roomArrival;
    [SerializeField] private SpriteRenderer hallwayBackdrop;
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private TMP_Text missionText, noticeText;
    [SerializeField] private GameObject roomHud, roomInfo;
    [SerializeField] private MVPFlowController story;
    [SerializeField] private AudioSource waterAudio;
    [SerializeField] private Vector3 roomCameraPosition;
    [SerializeField] private float roomCameraSize;
    [SerializeField] private bool explicitRoomView;
    private float noticeUntil, portalCooldown, rejectedUntil;
    private AudioClip waterClip;
    private bool busy;
    public System.Func<bool> CampaignCanTravel;
    // Production may start at the station; its room camera must be supplied explicitly.
    public void ConfigureRoomView(Vector3 position,float orthographicSize)
    {roomCameraPosition=position;roomCameraSize=orthographicSize;explicitRoomView=true;}
    public System.Func<bool> CommitWashCompletion;
    public void RestoreDailyFlags(bool bag,bool washed)
    {HasBackpack=bag;HasRefreshed=washed;if(player!=null)player.CharacterVisual.SetBackpackEquipped(bag);}
    private SpriteRenderer[] washRenderers;
    private bool[] washVisibility;
    private RestroomInteractable activeRestroom;
    [SerializeField] private Slider washProgress;
    [SerializeField] private TMP_Text washLabel;
    public bool WashProgressVisible=>washProgress!=null && washProgress.transform.parent.gameObject.activeSelf;
    public float WashProgress=>washProgress!=null?washProgress.value:0;
    public void ConfigureWashProgress(Slider bar,TMP_Text label){washProgress=bar;washLabel=label;}
    public bool HasBackpack { get; private set; }
    public bool HasRefreshed { get; private set; }
    public enum JourneyArea { InRoom, InHallway, Downstairs_Campus, BusStop, BoardedBus, University }
    public JourneyArea CurrentArea { get; private set; } = JourneyArea.InRoom;
    public bool IsInHallway => CurrentArea == JourneyArea.InHallway;
    [SerializeField] private CampusCommuteController commute;
    private SpriteRenderer activeBackdrop;
    [SerializeField] private YearOneMilestoneController yearOne;
    public void ConfigureYearOne(YearOneMilestoneController controller) => yearOne=controller;
    public void ConfigureCommute(CampusCommuteController controller) => commute = controller;
    public bool BeginJourneyTransition() => Begin();
    public void FinishJourneyTransition() => FinishBusy();
    public void RevealCompletion() { if (fade != null) { fade.alpha = 0f; fade.blocksRaycasts = false; } }
    public void MovePlayer(Vector3 position) => player.UpdatePosePosition(position, this);
    public void UpdateCampusArea(bool atStop)
    {
        if (CurrentArea == JourneyArea.Downstairs_Campus || CurrentArea == JourneyArea.BusStop)
            CurrentArea = atStop ? JourneyArea.BusStop : JourneyArea.Downstairs_Campus;
    }
    public void ArriveInArea(JourneyArea area, Transform arrival, SpriteRenderer backdrop)
    {
        CurrentArea = area; activeBackdrop = backdrop;
        if (arrival != null) MovePlayer(arrival.position);
        bool inRoom = area == JourneyArea.InRoom;
        if (roomHud != null) roomHud.SetActive(inRoom);
        if (roomInfo != null) roomInfo.SetActive(inRoom);
        if (view != null)
        {
            view.orthographicSize = area==JourneyArea.University && backdrop!=null ?
                Mathf.Max(backdrop.bounds.extents.y,backdrop.bounds.extents.x/view.aspect)+.1f : roomCameraSize;
            view.transform.position = inRoom ? roomCameraPosition : HallwayCameraPosition();
        }
    }
    public void RestoreRoom() => ArriveInArea(JourneyArea.InRoom, roomArrival, null);
    public bool IsBusy => busy;
    public bool ReadyForBus { get; private set; }
    public float StudyProgress { get; private set; }
    public PlayerInteractionStateController Player => player;
    public string LastNotice { get; private set; } = "";
    public bool CanInteract => isActiveAndEnabled && !busy;
    public string InteractionPrompt => "";
    public Transform GetTransform() => transform;
    public void Interact() { }

    public void Configure(PlayerInteractionStateController actor, Camera camera, Transform hallSpawn, Transform roomSpawn,
        SpriteRenderer background, CanvasGroup overlay, TMP_Text objective, TMP_Text notice, GameObject existingHud, GameObject info, AudioSource audio, MVPFlowController flow)
    {
        player = actor; view = camera; hallwayArrival = hallSpawn; roomArrival = roomSpawn;
        hallwayBackdrop = background; fade = overlay; missionText = objective; noticeText = notice; roomHud = existingHud; roomInfo = info; waterAudio = audio; story = flow;
    }
    private void Awake()
    {
        if (view != null && !explicitRoomView) { roomCameraPosition = view.transform.position; roomCameraSize = view.orthographicSize; }
        waterClip = DormSound.CreateWater();
        if (waterAudio != null) { waterAudio.clip = waterClip; waterAudio.loop = true; waterAudio.volume = .28f; }
        if (fade != null) { fade.alpha = 0f; fade.blocksRaycasts = false; }
    }
    public void Notify(string message, float duration = 2.5f)
    {
        LastNotice = message; noticeUntil = Time.time + duration;
        if (noticeText != null) { noticeText.text = message; noticeText.transform.parent.gameObject.SetActive(true); noticeText.gameObject.SetActive(true); }
    }
    public bool EquipBackpack(PlayerInteractionStateController actor)
    {
        if (actor != player || !player.CanInteract || HasBackpack || busy) return false;
        HasBackpack = true;
        player.CharacterVisual?.SetBackpackEquipped(true);
        Notify("Đã đeo balo. Sẵn sàng ra ngoài!");
        return true;
    }
    public void AddStudyProgress(float amount) => StudyProgress = Mathf.Clamp01(StudyProgress + Mathf.Max(0f, amount));
    private bool Begin()
    {
        if (busy || player == null || !player.CanInteract || !player.TryEnterState(PlayerInteractionState.Transitioning, null, this)) return false;
        busy = true;
        return true;
    }
    public bool TryPortal(bool toHallway, HingedRoomDoor door)
    {
        if (Time.time < portalCooldown || IsInHallway == toHallway || busy) return false;
        if (toHallway && (door == null || door.CurrentAngle < 60f)) return false;
        if (toHallway && !HasBackpack)
        {
            if (Time.time > rejectedUntil) { Notify("Chưa đeo balo, lấy balo trước khi đi học!"); rejectedUntil = Time.time + 2f; }
            return false;
        }
        if (!Begin()) return false;
        if (!toHallway && door != null) door.SetPreviewAngle(70f);
        StartCoroutine(PortalRoutine(toHallway));
        return true;
    }
    public IEnumerator FadeTo(float target, float duration)
    {
        float start = fade != null ? fade.alpha : 0f;
        float elapsed = 0f;
        if (fade != null) fade.blocksRaycasts = true;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (fade != null) fade.alpha = Mathf.Lerp(start, target, elapsed / duration);
            yield return null;
        }
        if (fade != null) fade.alpha = target;
    }
    private IEnumerator PortalRoutine(bool toHallway)
    {
        yield return FadeTo(1f, .2f);
        CurrentArea = toHallway ? JourneyArea.InHallway : JourneyArea.InRoom;
        activeBackdrop = toHallway ? hallwayBackdrop : null;
        Transform arrival = toHallway ? hallwayArrival : roomArrival;
        player.UpdatePosePosition(arrival.position, this);
        if (roomHud != null) roomHud.SetActive(!toHallway);
        if (roomInfo != null) roomInfo.SetActive(!toHallway);
        if (view != null)
        {
            view.orthographicSize = roomCameraSize;
            view.transform.position = toHallway ? HallwayCameraPosition() : roomCameraPosition;
        }
        yield return FadeTo(0f, .2f);
        FinishBusy();
        portalCooldown = Time.time + .7f;
        Notify(toHallway ? "Tầng 4 • KTX Khu B — WC Nam ở cuối hành lang." : "Đã trở về phòng 404.");
    }
    public bool UseRestroom(PlayerInteractionStateController actor, RestroomInteractable.Gender gender, Transform exit,RestroomInteractable restroom=null)
    {
        if (actor != player || !IsInHallway || exit == null || busy || !player.CanInteract) return false;
        if (gender == RestroomInteractable.Gender.Female)
        {
            Notify("Đây là WC Nữ, không được vào!");
            return false;
        }
        if (!Begin()) return false;
        Notify("Đang vào WC Nam…",.5f);
        StartCoroutine(WashRoutine(exit,restroom));
        return true;
    }
    private IEnumerator WashRoutine(Transform exit,RestroomInteractable restroom)
    {
        activeRestroom=restroom;
        if (waterAudio != null) waterAudio.Play();
        if(restroom!=null)yield return restroom.AnimateDoor(true);
        Vector3 start=player.transform.position;
        Vector3 inside=restroom!=null?restroom.transform.position+Vector3.up*.4f:exit.position;
        for(float t=0;t<.25f;t+=Time.deltaTime){MovePlayer(Vector3.Lerp(start,inside,t/.25f));yield return null;}
        MovePlayer(inside);
        var renderers=player.GetComponentsInChildren<SpriteRenderer>();
        var visible=new bool[renderers.Length];
        washRenderers=renderers;washVisibility=visible;
        for(int i=0;i<renderers.Length;i++){visible[i]=renderers[i].enabled;renderers[i].enabled=false;}
        if(restroom!=null)yield return restroom.AnimateDoor(false);
        if(washProgress!=null){washProgress.value=0;washProgress.transform.parent.gameObject.SetActive(true);}
        for(float t=0;t<3f;t+=Time.deltaTime)
        {
            if(washProgress!=null)washProgress.value=t/3f;
            if(washLabel!=null)washLabel.text="Đánh răng, rửa mặt… "+Mathf.RoundToInt(t/3f*100)+"%";
            yield return null;
        }
        if(washProgress!=null){washProgress.value=1;washProgress.transform.parent.gameObject.SetActive(false);}
        if (waterAudio != null) waterAudio.Stop();
        if(restroom!=null)yield return restroom.AnimateDoor(true);
        RestoreWashActor();
        for(float t=0;t<.25f;t+=Time.deltaTime){MovePlayer(Vector3.Lerp(inside,exit.position,t/.25f));yield return null;}
        MovePlayer(exit.position);
        if(restroom!=null)yield return restroom.AnimateDoor(false);
        activeRestroom=null;
        bool committed=CommitWashCompletion==null || CommitWashCompletion();
        HasRefreshed = committed;
        FinishBusy();
        Notify(committed?"Đã vệ sinh cá nhân, tỉnh táo. Đến cầu thang để đi học!":"Chưa lưu được bước vệ sinh. Thử lại tại WC; tiến độ chưa thay đổi.");
    }
    public bool TryStairs(PlayerInteractionStateController actor)
    {
        if (actor != player || !IsInHallway || busy || !player.CanInteract) return false;
        if(CampaignCanTravel!=null && !CampaignCanTravel()){Notify("Hoàn thành mục tiêu hiện tại trước khi đi nhé.");return false;}
        if (!HasBackpack || !HasRefreshed)
        {
            Notify(!HasBackpack ? "Cần đeo balo trước khi xuống sảnh." : "Ghé WC Nam rửa mặt trước khi đi học nhé!");
            return false;
        }
        ReadyForBus = true;
        if (commute != null) return commute.TryDescend(actor);
        Notify("Cầu thang chưa được kết nối.");
        return false;
    }
    private Vector3 HallwayCameraPosition()
    {
        Bounds bounds = (activeBackdrop != null ? activeBackdrop : hallwayBackdrop).bounds;
        float half = view.orthographicSize * view.aspect;
        float x = bounds.size.x <= half * 2f ? bounds.center.x : Mathf.Clamp(player.transform.position.x, bounds.min.x + half, bounds.max.x - half);
        return new Vector3(x, bounds.center.y, roomCameraPosition.z);
    }
    private void LateUpdate()
    {
        if (CurrentArea != JourneyArea.InRoom && CurrentArea != JourneyArea.BoardedBus && view != null && activeBackdrop != null && player != null)
            view.transform.position = Vector3.Lerp(view.transform.position, HallwayCameraPosition(), 1f - Mathf.Exp(-8f * Time.deltaTime));
        bool ending = story != null && story.endingPanel != null && story.endingPanel.activeSelf;
        if (noticeText != null && (Time.time > noticeUntil || ending || (yearOne!=null && yearOne.SuppressNotice))) noticeText.transform.parent.gameObject.SetActive(false);
        if(yearOne!=null && yearOne.OwnsPresentation)
        {
            if(roomHud!=null)roomHud.SetActive(false);
            if(roomInfo!=null)roomInfo.SetActive(false);
        }
        if (missionText != null) missionText.transform.parent.gameObject.SetActive(!ending && (yearOne==null || (!yearOne.OwnsPresentation && yearOne.CurrentPhase!=YearOneMilestoneController.Phase.YearTwo)));
        if (missionText != null)
        {
            string objective = !HasBackpack ? "Lấy balo trên móc cạnh cửa [E]" :
                CurrentArea == JourneyArea.BoardedBus ? "Xe buýt 33 • Hoàn thành hành trình đi học" :
                CurrentArea == JourneyArea.BusStop ? "Trạm 33 • Chờ xe rồi bấm [E] lên xe" :
                CurrentArea == JourneyArea.Downstairs_Campus ? "Đi theo vỉa hè đến trạm xe buýt 33" :
                CurrentArea == JourneyArea.InRoom ? (commute != null && commute.HasCompletedTrip ? "Đã hoàn thành chuyến đi học đầu tiên" : "Mở cửa và bước ra hành lang") :
                !HasRefreshed ? "Ghé WC Nam rửa mặt" : "[E] Xuống cầu thang ra sảnh KTX";
            missionText.text = "Đi học: " + objective + (commute != null ? " • " + commute.BalanceText : "");
        }
    }
    private void FinishBusy()
    {
        RestoreWashActor();
        if(activeRestroom!=null){activeRestroom.ResetDoor();activeRestroom=null;}
        if(washProgress!=null)washProgress.transform.parent.gameObject.SetActive(false);
        if (player != null && player.IsInteractingWith(this)) player.ExitInteraction();
        busy = false;
        if (fade != null) { fade.alpha = 0f; fade.blocksRaycasts = false; }
    }
    private void RestoreWashActor()
    {
        if(washRenderers==null)return;
        for(int i=0;i<washRenderers.Length;i++)if(washRenderers[i]!=null)washRenderers[i].enabled=washVisibility[i];
        washRenderers=null;washVisibility=null;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        if (waterAudio != null) waterAudio.Stop();
        FinishBusy();
    }
    private void OnDestroy() { if (waterClip != null) Destroy(waterClip); }
}
