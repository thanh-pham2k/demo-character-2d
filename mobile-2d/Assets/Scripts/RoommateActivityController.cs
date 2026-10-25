using System.Collections;
using UnityEngine;

// One authored roommate following room markers. No player input or global AI.
public sealed class RoommateActivityController : MonoBehaviour
{
    public enum Activity { Idle, Walking, Gaming, EnteringBed, Sleeping, LeavingBed }
    [SerializeField] private SpriteRenderer body,upper;
    [SerializeField] private Sprite idle;
    [SerializeField] private Sprite[] gamingLower,gamingUpper,bedEnter,sleep,bedExit;
    [SerializeField] private Transform home;
    [SerializeField] private AudioSource keyboardAudio;
    [SerializeField] private Vector3 poseOffset;
    private PropReservation reservation;
    private Coroutine routine;
    private float timer;
    private int frame;
    private int originalOrder;
    private int originalLayer;
    private Quaternion originalRotation;
    private SpriteRenderer foreground;
    private bool foregroundEnabled;
    private Vector3 originalScale,originalUpperPosition;
    private bool initialized;
    private AudioClip typingClip;
    [SerializeField] private Sprite[] walking,rising;
    private Transform chairExit;
    [SerializeField] private Transform deskPose,bedPose;
    public void ConfigurePresentation(Transform desk,Transform sleeping){deskPose=desk;bedPose=sleeping;}
    public void ConfigureLocomotion(Sprite[] steps,Sprite[] standUp){walking=steps;rising=standUp;}
    public Activity Current { get; private set; }
    public bool IsSleeping=>Current==Activity.Sleeping;
    public void Configure(SpriteRenderer renderer,SpriteRenderer front,Sprite standing,Sprite[] lowerFrames,Sprite[] upperFrames,
        Sprite[] entering,Sprite[] sleeping,Sprite[] exiting,Transform idleMarker,AudioSource audio,Vector3 markerOffset)
    {body=renderer;upper=front;idle=standing;gamingLower=lowerFrames;gamingUpper=upperFrames;bedEnter=entering;sleep=sleeping;bedExit=exiting;home=idleMarker;keyboardAudio=audio;poseOffset=markerOffset;}
    private void Awake()=>Initialize();
    private void Initialize()
    {
        if(initialized || body==null)return;initialized=true;originalOrder=body.sortingOrder;originalLayer=body.sortingLayerID;originalRotation=transform.rotation;originalScale=transform.localScale;
        if(upper!=null){originalUpperPosition=upper.transform.localPosition;upper.enabled=false;}
        if(keyboardAudio!=null){typingClip=DormSound.CreateTyping();keyboardAudio.clip=typingClip;keyboardAudio.loop=true;}
    }
    public bool StartGaming(ChairInteractable chair,Transform[] path)
    {
        if(!isActiveAndEnabled || Current!=Activity.Idle || chair==null || !chair.Reservation.TryReserve(this))return false;
        reservation=chair.Reservation;routine=StartCoroutine(GamingArrival(chair,path));return true;
    }
    private IEnumerator GamingArrival(ChairInteractable chair,Transform[] path)
    {
        chairExit=chair.ExitPivot;
        if(path!=null)foreach(var marker in path)if(marker!=null)yield return WalkTo(marker.position);
        yield return WalkTo(chair.ExitPivot.position);
        transform.position=chair.SeatPivot.position+poseOffset;transform.localScale=deskPose!=null?deskPose.localScale:originalScale;body.flipX=false;Current=Activity.Gaming;frame=0;timer=.31f;
        ApplySorting(chair.BackRenderer,chair.ForegroundRenderer);if(upper!=null){upper.transform.localPosition=originalUpperPosition;if(deskPose!=null)upper.transform.position+=deskPose.localPosition;upper.enabled=true;}
        if(keyboardAudio!=null)keyboardAudio.Play();routine=null;
    }
    public bool StartSleeping(BedInteractable bed,Transform[] path)
    {
        if(!isActiveAndEnabled || Current!=Activity.Idle || bed==null || !bed.Reservation.TryReserve(this))return false;
        reservation=bed.Reservation;routine=StartCoroutine(SleepArrival(bed,path));return true;
    }
    private IEnumerator SleepArrival(BedInteractable bed,Transform[] path)
    {
        if(path!=null)foreach(var marker in path)if(marker!=null)yield return WalkTo(marker.position);
        yield return WalkTo(bed.ExitPivot.position);Current=Activity.EnteringBed;
        Vector3 start=transform.position;Vector3 target=bed.BedPivot.position+poseOffset+(bedPose!=null?bedPose.localPosition:Vector3.zero);ApplySorting(bed.BackRenderer,bed.ForegroundRenderer);transform.rotation=bed.BedPivot.rotation*(bedPose!=null?bedPose.localRotation:Quaternion.identity);
        transform.localScale=bedPose!=null?bedPose.localScale:originalScale;body.flipX=false;
        for(int i=0;i<bedEnter.Length;i++)
        {body.sprite=bedEnter[i];transform.position=Vector3.Lerp(start,target,(i+1f)/bedEnter.Length);yield return new WaitForSeconds(.22f);}
        transform.position=target;Current=Activity.Sleeping;frame=0;timer=0;routine=null;
    }
    public void Wake(Transform exit)
    {
        if(Current!=Activity.Sleeping)return;
        routine=StartCoroutine(WakeRoutine(exit));
    }
    private IEnumerator WakeRoutine(Transform exit)
    {
        Current=Activity.LeavingBed;
        foreach(var sprite in bedExit){body.sprite=sprite;yield return new WaitForSeconds(.22f);}
        if(exit!=null)transform.position=exit.position;Release();Current=Activity.Idle;Restore();routine=null;
    }
    private IEnumerator WalkTo(Vector3 destination)
    {
        Current=Activity.Walking;body.sprite=idle;body.sortingOrder=originalOrder;
        float elapsed=0;
        while(Vector3.Distance(transform.position,destination)>.025f)
        {elapsed+=Time.deltaTime;if(walking!=null && walking.Length>0)body.sprite=walking[(int)(elapsed/.13f)%walking.Length];body.flipX=destination.x<transform.position.x;transform.position=Vector3.MoveTowards(transform.position,destination,3f*Time.deltaTime);yield return null;}
        transform.position=destination;
    }
    public void LeaveGaming(){if(Current==Activity.Gaming)routine=StartCoroutine(LeaveGamingRoutine());}
    private IEnumerator LeaveGamingRoutine()
    {
        Current=Activity.Walking;transform.localScale=originalScale;if(keyboardAudio!=null)keyboardAudio.Stop();if(upper!=null)upper.enabled=false;
        if(rising!=null)foreach(var sprite in rising){body.sprite=sprite;yield return new WaitForSeconds(.16f);}
        if(chairExit!=null)yield return WalkTo(chairExit.position);
        Release();Current=Activity.Idle;Restore();routine=null;
    }
    public void StopActivity()
    {
        Initialize();
        if(routine!=null)StopCoroutine(routine);routine=null;Release();Current=Activity.Idle;Restore();
        if(home!=null)transform.position=home.position;
    }
    private void ApplySorting(SpriteRenderer back,SpriteRenderer front)
    {
        if(back!=null){body.sortingLayerID=back.sortingLayerID;body.sortingOrder=back.sortingOrder+1;}
        if(front==null)return;
        foreground=front;foregroundEnabled=front.enabled;front.enabled=true;
        body.sortingLayerID=front.sortingLayerID;body.sortingOrder=Mathf.Min(body.sortingOrder,front.sortingOrder-1);
        if(upper!=null){upper.sortingLayerID=front.sortingLayerID;upper.sortingOrder=front.sortingOrder+1;}
    }
    private void Release(){if(reservation!=null)reservation.Release(this);reservation=null;if(foreground!=null)foreground.enabled=foregroundEnabled;foreground=null;}
    private void Restore(){if(body!=null){body.sprite=idle;body.sortingOrder=originalOrder;body.sortingLayerID=originalLayer;}transform.rotation=originalRotation;transform.localScale=originalScale;if(upper!=null){upper.transform.localPosition=originalUpperPosition;upper.enabled=false;}if(keyboardAudio!=null)keyboardAudio.Stop();}
    private void Update()
    {
        if(reservation!=null && reservation.Owner!=this){StopActivity();return;}
        if(Current!=Activity.Gaming && Current!=Activity.Sleeping)return;
        timer+=Time.deltaTime;float hold=Current==Activity.Gaming?DeskPoseTiming.Hold(frame):.65f;
        var frames=Current==Activity.Gaming?gamingLower:sleep;if(frames==null || frames.Length==0)return;
        if(timer>=hold){timer-=hold;frame=(frame+1)%frames.Length;}
        body.sprite=frames[frame];
        if(Current==Activity.Gaming)
        {
            if(upper!=null && gamingUpper.Length>0){upper.enabled=true;upper.sprite=gamingUpper[frame%gamingUpper.Length];upper.flipX=body.flipX;}
            if(keyboardAudio!=null)keyboardAudio.volume=DeskPoseTiming.IsKeyOrClick(frame)?.18f:0;
        }
    }
    private void OnDisable(){StopActivity();}
    private void OnDestroy(){if(typingClip!=null)Destroy(typingClip);}
}
