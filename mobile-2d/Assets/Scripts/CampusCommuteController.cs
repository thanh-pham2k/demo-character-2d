using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

// Concrete travel and bus fare; does not advance the legacy four-stage story.
public class CampusCommuteController : MonoBehaviour
{
    public const float ApproachSeconds=2.4f,DepartSeconds=2.5f,BoardSeconds=.4f;
    public float EstimatedTripSeconds=>(BusAtStop?0:ApproachSeconds)+(MissedBus?1.2f:0)+BoardSeconds+DepartSeconds+1.2f;
    public const int TicketPrice = 7000;
    [SerializeField] private DormJourneyController journey;
    [SerializeField] private Transform campusArrival, hallwayArrival, stopAnchor, busPark, busOffscreen, boardingPoint;
    [SerializeField] private Transform busEntry;
    public void ConfigureEntry(Transform entry)=>busEntry=entry;
    [SerializeField] private Transform returnEntry,returnPark,returnExit,returnStop,returnBoard;
    [SerializeField] private SpriteRenderer returnBackdrop;
    [SerializeField] private YearOneCampaignController campaign;
    private bool returning;
    private ICommuteProgress tripProgress;
    public void BindProgress(ICommuteProgress progress)
    {
        if(ownsTransition || (journey!=null && journey.IsBusy))throw new InvalidOperationException("Cannot replace a trip host during travel.");
        if(progress!=null && campaign!=null)throw new InvalidOperationException("A trip cannot have both legacy and production progress owners.");
        tripProgress=progress;
    }
    public bool IsReturning=>returning;
    public Func<bool,IEnumerator> CampaignArrival;
    public void ConfigureCampaign(YearOneCampaignController controller,Transform entry,Transform park,Transform exit,Transform stop,Transform board,SpriteRenderer universityBackdrop)
    {if(controller!=null && tripProgress!=null)throw new InvalidOperationException("A trip already has a production progress owner.");campaign=controller;returnEntry=entry;returnPark=park;returnExit=exit;returnStop=stop;returnBoard=board;returnBackdrop=universityBackdrop;}
    private bool CampaignActive=>campaign!=null && campaign.Initialized;
    public bool MissedBus=>tripProgress!=null?tripProgress.MissedBus:CampaignActive && !returning && campaign.Progress.Milestone.MissableBus && campaign.Progress.Has("day:4:missed");
    private Transform ActivePark=>returning?returnPark:busPark;
    private Transform ActiveEntry=>returning?returnEntry:busEntry;
    private Transform ActiveExit=>returning?returnExit:busOffscreen;
    private Transform ActiveBoard=>returning?returnBoard:boardingPoint;
    public void PrepareTrip(bool homebound)
    {
        returning=homebound;recoveryPresented=false;arrivalStarted=false;HasCompletedTrip=false;BusAtStop=false;
        if(bus!=null){bus.enabled=false;bus.flipX=homebound;if(ActiveEntry!=null)bus.transform.position=ActiveEntry.position;}
        if(travelAudio!=null)travelAudio.Stop();
        if(CampaignActive)MoneyVnd=campaign.Progress.money;
    }
    [SerializeField] private SpriteRenderer campusBackdrop, hallwayBackdrop, bus;
    [SerializeField] private AudioSource travelAudio;
    [SerializeField] private GameObject completionPanel;
    [SerializeField] private TMP_Text receipt;
    [SerializeField] private Button returnButton;
    [SerializeField] private int initialMoney = 35000;
    [SerializeField] private YearOneMilestoneController yearOne;
    public void ConfigureYearOne(YearOneMilestoneController controller) => yearOne = controller;
    public void RestoreBoardedActor() => RestoreActor();
    private AudioClip stepsClip, engineClip;
    private bool arrivalStarted, ownsTransition,recoveryPresented;
    private SpriteRenderer[] actorRenderers;
    private bool[] actorVisibility;
    private int localMoneyVnd;
    public int MoneyVnd { get=>tripProgress!=null?tripProgress.Money:localMoneyVnd; private set=>localMoneyVnd=value; }
    public int TicketsPurchased { get; private set; }
    public bool BusAtStop { get; private set; }
    public bool HasCompletedTrip { get; private set; }
    public bool CompletionVisible => completionPanel != null && completionPanel.activeSelf;
    public Transform OutboundStopAnchor=>stopAnchor;
    public Transform StopAnchor => returning?returnStop:stopAnchor;
    public Transform CampusArrival => campusArrival;
    public SpriteRenderer BusRenderer => bus;
    public Button ReturnButton => returnButton;
    public bool IsOutdoors => journey != null && (journey.CurrentArea == DormJourneyController.JourneyArea.Downstairs_Campus || journey.CurrentArea == DormJourneyController.JourneyArea.BusStop || (returning && journey.CurrentArea==DormJourneyController.JourneyArea.University));
    public string BalanceText => MoneyVnd.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + "đ";
    public DormJourneyController Journey => journey;

    public void Configure(DormJourneyController controller, Transform campusSpawn, Transform hallSpawn, Transform stop,
        Transform park, Transform offscreen, Transform boarding, SpriteRenderer background, SpriteRenderer hall, SpriteRenderer vehicle,
        AudioSource audio, GameObject completed, TMP_Text ticketReceipt, Button goBack)
    {
        journey=controller;campusArrival=campusSpawn;hallwayArrival=hallSpawn;stopAnchor=stop;busPark=park;
        busOffscreen=offscreen;boardingPoint=boarding;campusBackdrop=background;hallwayBackdrop=hall;bus=vehicle;
        travelAudio=audio;completionPanel=completed;receipt=ticketReceipt;returnButton=goBack;
    }
    private void Awake()
    {
        MoneyVnd=initialMoney;
        stepsClip=DormSound.CreateFootsteps();engineClip=DormSound.CreateBusEngine();
        if(bus!=null){bus.transform.position=busOffscreen.position;bus.enabled=false;}
        if(completionPanel!=null)completionPanel.SetActive(false);
        if(returnButton!=null)returnButton.onClick.AddListener(ReturnFromClass);
    }
    public bool TryDescend(PlayerInteractionStateController actor)
    {
        if (actor != journey.Player || HasCompletedTrip || !journey.HasBackpack || !journey.HasRefreshed || !journey.BeginJourneyTransition()) return false;
        ownsTransition=true;StartCoroutine(StairRoutine(true)); return true;
    }
    public bool TryReturnUpstairs(PlayerInteractionStateController actor)
    {
        if(actor!=journey.Player || !IsOutdoors || !journey.BeginJourneyTransition())return false;
        ownsTransition=true;StartCoroutine(StairRoutine(false));return true;
    }
    private IEnumerator StairRoutine(bool downstairs)
    {
        travelAudio.clip=stepsClip;travelAudio.loop=true;travelAudio.volume=.4f;travelAudio.pitch=1f;travelAudio.Play();
        yield return journey.FadeTo(1f,.2f);
        yield return new WaitForSeconds(.6f);
        journey.ArriveInArea(downstairs ? DormJourneyController.JourneyArea.Downstairs_Campus : DormJourneyController.JourneyArea.InHallway,
            downstairs ? campusArrival : hallwayArrival, downstairs ? campusBackdrop : hallwayBackdrop);
        yield return journey.FadeTo(0f,.2f);
        travelAudio.Stop();journey.FinishJourneyTransition();ownsTransition=false;
        journey.Notify(downstairs ? "Sảnh KTX Khu B • Đi theo vỉa hè ra trạm xe buýt 33." : "Đã trở lại hành lang tầng 4.");
    }
    private void Update()
    {
        if(!IsOutdoors || journey.IsBusy)return;
        bool atStop=Mathf.Abs(journey.Player.transform.position.x-StopAnchor.position.x)<1.8f;
        if(atStop && CampaignActive && !returning)campaign.ReachedBusStop();
        journey.UpdateCampusArea(atStop);
        if(BusAtStop && MissedBus && !recoveryPresented)
        {BusAtStop=false;StartCoroutine(ReplaceMissedBus());}
        if(atStop && !arrivalStarted && !HasCompletedTrip)
        { arrivalStarted=true;StartCoroutine(ArrivalRoutine()); }
    }
    private IEnumerator ReplaceMissedBus()
    {
        recoveryPresented=true;journey.Notify("Chuyến này đã hết giờ đón. Đợi xe 33 tiếp theo nhé.",4);
        Vector3 start=bus.transform.position;
        for(float t=0;t<1.5f;t+=Time.deltaTime){bus.transform.position=Vector3.Lerp(start,ActiveExit.position,t/1.5f);yield return null;}
        bus.enabled=false;yield return ArrivalRoutine();
    }
    private IEnumerator ArrivalRoutine()
    {
        recoveryPresented=MissedBus;
        if(MissedBus)
        {
            journey.Notify("Chuyến trước đã đi. Đợi chuyến xe 33 tiếp theo ở trạm nhé.",4f);
            yield return new WaitForSeconds(1.2f);
        }
        bus.enabled=true;bus.flipX=returning;bus.transform.position=ActiveEntry!=null?ActiveEntry.position:ActiveExit.position;
        Vector3 approachStart=bus.transform.position;
        travelAudio.clip=engineClip;travelAudio.loop=true;travelAudio.volume=.35f;travelAudio.pitch=1f;travelAudio.Play();
        float elapsed=0f;
        while(elapsed<ApproachSeconds)
        {
            elapsed+=Time.deltaTime;
            bus.transform.position=Vector3.Lerp(approachStart,ActivePark.position,Mathf.SmoothStep(0f,1f,elapsed/ApproachSeconds));
            yield return null;
        }
        bus.transform.position=ActivePark.position;BusAtStop=true;travelAudio.pitch=.8f;
        journey.Notify("Xe buýt 33 đã ghé trạm. [E] Lên xe • Vé 7.000đ");
    }
    public bool TryBoard(PlayerInteractionStateController actor)
    {
        if(actor!=journey.Player || !IsOutdoors || journey.IsBusy || HasCompletedTrip)return false;
        if(MissedBus && !recoveryPresented){journey.Notify("Đợi chuyến xe 33 tiếp theo ghé trạm nhé.");return false;}
        if(!BusAtStop){journey.Notify("Xe 33 đang ghé trạm, chờ một chút nhé!");return false;}
        if(tripProgress!=null && CampaignArrival==null){journey.Notify("Chuyến xe chưa có điểm đến.");return false;}
        if(tripProgress!=null && !tripProgress.CanBoard(returning,out string tripError)){journey.Notify(tripError);return false;}
        if(MoneyVnd<TicketPrice){journey.Notify("Chưa đủ 7.000đ mua vé xe buýt.");return false;}
        if(!journey.HasBackpack || !journey.HasRefreshed || !journey.BeginJourneyTransition())return false;
        if(CampaignActive && !campaign.CompleteTrip(returning,MissedBus))
        {journey.FinishJourneyTransition();journey.Notify(string.IsNullOrEmpty(campaign.SaveError)?"Chuyến xe này đã được ghi nhận hoặc vị trí hiện tại không cho phép lên xe.":"Chưa lưu được vé: "+campaign.SaveError);return false;}
        ownsTransition=true;
        if(CampaignActive){MoneyVnd=campaign.Progress.money;TicketsPurchased++;}
        else if(tripProgress==null){MoneyVnd-=TicketPrice;TicketsPurchased++;}
        StartCoroutine(BoardAndDepart());return true;
    }
    private IEnumerator BoardAndDepart()
    {
        journey.Notify(tripProgress==null?"Đã mua vé 7.000đ • Đang lên xe buýt 33...":"Đang lên xe buýt 33 • Vé 7.000đ...");
        Vector3 start=journey.Player.transform.position;
        float elapsed=0f;
        while(elapsed<BoardSeconds)
        {
            elapsed+=Time.deltaTime;
            journey.MovePlayer(Vector3.Lerp(start,ActiveBoard.position,Mathf.SmoothStep(0f,1f,elapsed/BoardSeconds)));
            yield return null;
        }
        yield return journey.FadeTo(.95f,.15f);
        actorRenderers=journey.Player.GetComponentsInChildren<SpriteRenderer>();actorVisibility=new bool[actorRenderers.Length];
        for(int i=0;i<actorRenderers.Length;i++){actorVisibility[i]=actorRenderers[i].enabled;actorRenderers[i].enabled=false;}
        journey.ArriveInArea(DormJourneyController.JourneyArea.BoardedBus,null,returning?returnBackdrop:campusBackdrop);
        yield return journey.FadeTo(0f,.15f);
        Vector3 busStart=bus.transform.position;
        elapsed=0f;
        while(elapsed<DepartSeconds)
        {
            elapsed+=Time.deltaTime;
            float t=elapsed/DepartSeconds;
            bus.transform.position=Vector3.Lerp(busStart,ActiveExit.position,t*t);
            travelAudio.pitch=Mathf.Lerp(.8f,1.3f,t);
            yield return null;
        }
        bus.transform.position=ActiveExit.position;bus.enabled=false;BusAtStop=false;travelAudio.Stop();HasCompletedTrip=true;
        if(tripProgress!=null)
        {
            // Durable destination is published before any arrival callback releases the actor.
            yield return journey.FadeTo(1f,.15f);
            if(!tripProgress.CommitArrival(returning,out string error))
            {
                RestoreActor();
                journey.ArriveInArea(returning?DormJourneyController.JourneyArea.University:DormJourneyController.JourneyArea.BusStop,
                    StopAnchor,returning?returnBackdrop:campusBackdrop);
                HasCompletedTrip=false;BusAtStop=true;bus.enabled=true;bus.transform.position=ActivePark.position;
                travelAudio.clip=engineClip;travelAudio.loop=true;travelAudio.pitch=.8f;travelAudio.Play();
                yield return journey.FadeTo(0f,.15f);
                journey.FinishJourneyTransition();ownsTransition=false;
                journey.Notify("Chưa lưu được chuyến xe. Vé chưa bị trừ; thử lại tại trạm. "+error,5f);
                yield break;
            }
            TicketsPurchased++;
            if(CampaignArrival!=null)yield return CampaignArrival(returning);
            else
            {
                // A production host requires a destination presenter; never fall into the legacy award path.
                RestoreActor();journey.FinishJourneyTransition();ownsTransition=false;
                throw new InvalidOperationException("A production trip requires a destination arrival presenter.");
            }
            ownsTransition=false;yield break;
        }
        if(CampaignActive && CampaignArrival!=null)
        {
            yield return CampaignArrival(returning);
            MoneyVnd=campaign.Progress.money;ownsTransition=false;yield break;
        }
        if(yearOne!=null)
        {
            yield return yearOne.ArriveFromBus();
            ownsTransition=false;
            yield break;
        }
        receipt.text="Xe buýt 33 đã khởi hành!\nHoàn thành hành trình đến tiết Giải Tích đầu tiên.\n\nVé: 7.000đ  •  Số dư: "+BalanceText;
        completionPanel.SetActive(true);journey.RevealCompletion();
    }
    public void ReturnFromClass()
    {
        if(!HasCompletedTrip || !CompletionVisible)return;
        completionPanel.SetActive(false);StartCoroutine(ReturnRoutine());
    }
    private IEnumerator ReturnRoutine()
    {
        yield return journey.FadeTo(1f,.2f);
        RestoreActor();journey.RestoreRoom();
        yield return journey.FadeTo(0f,.2f);
        journey.FinishJourneyTransition();ownsTransition=false;journey.Notify("Đã hoàn thành chuyến đi học đầu tiên!");
    }
    private void RestoreActor()
    {
        if(actorRenderers==null)return;
        for(int i=0;i<actorRenderers.Length;i++)if(actorRenderers[i]!=null)actorRenderers[i].enabled=actorVisibility[i];
        actorRenderers=null;actorVisibility=null;
    }
    private void OnDisable()
    {
        StopAllCoroutines();if(travelAudio!=null)travelAudio.Stop();RestoreActor();
        if(ownsTransition && journey!=null && journey.IsBusy)journey.FinishJourneyTransition();
        ownsTransition=false;
    }
    private void OnDestroy(){if(stepsClip!=null)Destroy(stepsClip);if(engineClip!=null)Destroy(engineClip);}
}
