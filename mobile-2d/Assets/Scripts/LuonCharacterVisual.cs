using UnityEngine;

public class LuonCharacterVisual : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite[] walkSprites;
    public Sprite sitLaptopSprite;
    public Sprite lyingSprite;
    [SerializeField] private SpriteRenderer poseShadow;
    [SerializeField] private Sprite backpackIdleSprite;
    [SerializeField] private Sprite[] backpackWalkSprites;
    private bool backpackEquipped;
    public bool BackpackEquipped => backpackEquipped;
    public void ConfigureBackpackSprites(Sprite idle, Sprite[] walk) { backpackIdleSprite = idle; backpackWalkSprites = walk; }
    public void SetBackpackEquipped(bool equipped) { backpackEquipped = equipped; ResolveComponents(); if (interactionState == PlayerInteractionState.Standing && sr != null) sr.sprite = StandingIdle; }
    public Sprite CurrentStandingSprite => StandingIdle;
    private Sprite StandingIdle
    {
        get
        {
            if(productionCatalog!=null && TryProductionIdle(out var sprite))return sprite;
            return backpackEquipped && backpackIdleSprite!=null?backpackIdleSprite:idleSprite;
        }
    }
    public bool OwnsDirectionalFacing=>productionCatalog!=null;
    private bool TryProductionIdle(out Sprite sprite)
    {
        if(productionFacing=="right" && productionOutfit==ProductionOutfit.Casual)
        {sprite=backpackEquipped?productionBackpackSideIdle:productionSideIdle;return sprite!=null;}
        string outfit=productionOutfit==ProductionOutfit.Shirt?"shirt":productionOutfit==ProductionOutfit.Robe?"robe":backpackEquipped?"backpack":"casual";
        return productionCatalog.TryGet("luon_"+outfit+"_"+productionFacing+"_idle",out sprite);
    }
    private Sprite[] StandingWalk => backpackEquipped && backpackWalkSprites != null && backpackWalkSprites.Length > 0 ? backpackWalkSprites : walkSprites;

    public enum ProductionOutfit { Casual, Shirt, Robe }
    [SerializeField] private RemainingAssetCatalog productionCatalog;
    [SerializeField] private Sprite productionSideIdle, productionBackpackSideIdle;
    [SerializeField] private Sprite[] productionSideWalk, productionBackpackSideWalk;
    private ProductionOutfit productionOutfit;
    private string productionFacing="right", productionCycle="";
    public void ConfigureProductionSprites(RemainingAssetCatalog catalog,Sprite sideIdle,Sprite[] sideWalk,Sprite packIdle,Sprite[] packWalk)
    {productionCatalog=catalog;productionSideIdle=sideIdle;productionSideWalk=sideWalk;productionBackpackSideIdle=packIdle;productionBackpackSideWalk=packWalk;}
    public void SetProductionOutfit(ProductionOutfit outfit){productionOutfit=outfit;productionCycle="";}
    private bool UpdateProductionStanding(Vector2 velocity)
    {
        if(productionCatalog==null)return false;
        bool moving=velocity.sqrMagnitude>.01f;
        if(moving)
        {
            productionFacing=Mathf.Abs(velocity.y)>Mathf.Abs(velocity.x)?(velocity.y>0?"back":"front"):"right";
            sr.flipX=productionFacing=="right" && velocity.x<0;
        }
        string outfit=productionOutfit==ProductionOutfit.Shirt?"shirt":productionOutfit==ProductionOutfit.Robe?"robe":backpackEquipped?"backpack":"casual";
        string prefix="luon_"+outfit+"_"+productionFacing;
        Sprite idle=null;Sprite[] walk=null;
        if(productionFacing=="right" && productionOutfit==ProductionOutfit.Casual)
        {idle=backpackEquipped?productionBackpackSideIdle:productionSideIdle;walk=backpackEquipped?productionBackpackSideWalk:productionSideWalk;}
        else
        {
            productionCatalog.TryGet(prefix+"_idle",out idle);
            if(productionCatalog.TryGetAnimation(prefix+"_walk",out var clip))walk=clip.frames;
        }
        if(idle==null || walk==null || walk.Length==0)return false;
        if(productionCycle!=prefix){productionCycle=prefix;currentWalkFrame=0;animTimer=0;}
        if(moving)
        {
            animTimer+=Time.deltaTime;
            float interval=1f/Mathf.Max(1f,walkFrameRate);
            while(animTimer>=interval){animTimer-=interval;currentWalkFrame=(currentWalkFrame+1)%walk.Length;}
            sr.sprite=walk[currentWalkFrame];
        }
        else{animTimer=0;currentWalkFrame=0;sr.sprite=idle;}
        return true;
    }

    [Header("Bed Animation Sprites")]
    public Sprite[] bedEnterSprites;
    public Sprite[] bedIdleSprites;
    public Sprite bedSleepSprite;
    public Sprite[] bedExitSprites;

    public enum DeskPoseState { None, SittingStudying, SittingTyping }
    [SerializeField] private Sprite[] studyingSprites, typingSprites, studyingForeground, typingForeground, studyingLower, typingLower;
    [SerializeField] private SpriteRenderer deskUpperBody;
    private float deskTimer;
    private int deskFrame;
    private Color deskBaseColor;
    [SerializeField] private Vector3 typingOffset;
    private Vector3 deskPosition;
    private bool deskFacing;
    public void ConfigureTypingOffset(Vector3 offset)=>typingOffset=offset;
    public DeskPoseState DeskPose { get; private set; }
    public int DeskAnimationFrame=>deskFrame;
    public void ConfigureDeskSprites(Sprite[] study, Sprite[] typing, Sprite[] studyFront, Sprite[] typingFront, Sprite[] studyBase, Sprite[] typingBase, SpriteRenderer upper)
    { studyingSprites = study; typingSprites = typing; studyingForeground = studyFront; typingForeground = typingFront; studyingLower = studyBase; typingLower = typingBase; deskUpperBody = upper; }
    public Sprite CurrentDeskActionFrame => deskUpperBody != null ? deskUpperBody.sprite : null;
    private Sprite DeskBaseFrame(Sprite fallback)
    {
        var frames = DeskPose == DeskPoseState.SittingStudying ? studyingLower : typingLower;
        return frames != null && frames.Length > 0 ? frames[deskFrame % frames.Length] : fallback;
    }
    private void UpdateDeskForeground()
    {
        if (deskUpperBody == null) return;
        var frames = DeskPose == DeskPoseState.SittingStudying ? studyingForeground : typingForeground;
        deskUpperBody.enabled = DeskPose != DeskPoseState.None && frames != null && frames.Length > 0;
        if (!deskUpperBody.enabled) return;
        deskUpperBody.sprite = frames[deskFrame % frames.Length];
        deskUpperBody.flipX = sr.flipX; deskUpperBody.color = sr.color;
    }
    public void SetDeskPose(DeskPoseState pose)
    {
        ResolveComponents();
        if (interactionState != PlayerInteractionState.Sitting) pose = DeskPoseState.None;
        if (sr != null && DeskPose == DeskPoseState.None && pose != DeskPoseState.None) {deskBaseColor = sr.color;deskPosition=deskUpperBody!=null?deskUpperBody.transform.localPosition:Vector3.zero;deskFacing=sr.flipX;}
        if(DeskPose!=DeskPoseState.None){if(deskUpperBody!=null)deskUpperBody.transform.localPosition=deskPosition;sr.flipX=deskFacing;}
        if(pose==DeskPoseState.SittingTyping){if(deskUpperBody!=null)deskUpperBody.transform.position+=typingOffset;sr.flipX=false;}
        if (sr != null && DeskPose != DeskPoseState.None && pose == DeskPoseState.None) sr.color = deskBaseColor;
        DeskPose = pose; deskTimer = 0f; deskFrame = 0;
        if (sr == null) return;
        Sprite[] frames = pose == DeskPoseState.SittingStudying ? studyingSprites : typingSprites;
        if (pose != DeskPoseState.None && frames != null && frames.Length > 0)
        {
            sr.sprite = DeskBaseFrame(frames[0]);
            sr.color = pose == DeskPoseState.SittingTyping ? deskBaseColor * new Color(.9f,.97f,1f,1f) : deskBaseColor;
        }
        else if (interactionState == PlayerInteractionState.Sitting) sr.sprite = sitLaptopSprite;
        UpdateDeskForeground();
    }

    [Header("Animation Settings")]
    public float walkFrameRate = 8f; // frames per second
    public float bedAnimFrameRate = 6f;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float animTimer;
    private int currentWalkFrame;
    private int currentBedFrame;
    private PlayerInteractionState interactionState;
    private bool presentationCaptured;
    private Quaternion standingRotation;
    private Vector3 standingScale;
    private int standingOrder, standingLayer;
    private bool standingFacing, shadowEnabled;

    private void Awake()
    {
        ResolveComponents();
    }

    private void ResolveComponents()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (rb == null) rb = GetComponentInParent<Rigidbody2D>();
    }

    public void SetSitting(bool sit)
    {
        var controller = GetComponentInParent<PlayerInteractionStateController>();
        if (controller != null)
        {
            if (sit) controller.TryEnterState(PlayerInteractionState.Sitting);
            else if (controller.CurrentState == PlayerInteractionState.Sitting) controller.ExitInteraction();
            return;
        }
        // Keep visual-only use working outside the interactive player hierarchy.
        ApplyInteractionState(sit ? PlayerInteractionState.Sitting : PlayerInteractionState.Standing);
    }

    public void SetLying(bool lie)
    {
        var controller = GetComponentInParent<PlayerInteractionStateController>();
        if (controller != null)
        {
            if (lie) controller.TryEnterState(PlayerInteractionState.Lying);
            else if (controller.CurrentState == PlayerInteractionState.Lying) controller.ExitInteraction();
            return;
        }
        ApplyInteractionState(lie ? PlayerInteractionState.Lying : PlayerInteractionState.Standing);
    }

    public void SetSleeping(bool sleep)
    {
        var controller = GetComponentInParent<PlayerInteractionStateController>();
        if (controller != null)
        {
            if (sleep && controller.CurrentState == PlayerInteractionState.Lying)
                controller.TryEnterState(PlayerInteractionState.Sleeping, null, controller.ActiveInteractable);
            else if (!sleep && controller.CurrentState == PlayerInteractionState.Sleeping)
                controller.TryEnterState(PlayerInteractionState.Lying, null, controller.ActiveInteractable);
            return;
        }
        ApplyInteractionState(sleep ? PlayerInteractionState.Sleeping : PlayerInteractionState.Lying);
    }

    public System.Collections.IEnumerator PlayBedEnterRoutine(System.Action onComplete, System.Action<float> onProgress = null)
    {
        yield return PlayBedFrames(bedEnterSprites, onComplete, onProgress);
    }

    public System.Collections.IEnumerator PlayBedExitRoutine(System.Action onComplete, System.Action<float> onProgress = null)
    {
        yield return PlayBedFrames(bedExitSprites, onComplete, onProgress);
    }

    private System.Collections.IEnumerator PlayBedFrames(Sprite[] frames, System.Action onComplete, System.Action<float> onProgress)
    {
        ResolveComponents();
        currentBedFrame = 0;
        animTimer = 0f;
        if (frames != null && frames.Length > 0 && sr != null)
        {
            bool bedFacing = sr.flipX;
            float frameDuration = 1f / Mathf.Max(1f, bedAnimFrameRate);
            for (int frame = 0; frame < frames.Length; frame++)
            {
                if (frames[frame] != null) sr.sprite = frames[frame];
                // The first climbing pose braces toward the bed; the other poses face along its axis.
                sr.flipX = bedEnterSprites != null && bedEnterSprites.Length > 0 && frames[frame] == bedEnterSprites[0]
                    ? !bedFacing : bedFacing;
                float elapsed = 0f;
                while (elapsed < frameDuration)
                {
                    float progress = frames.Length == 1 ? 1f :
                        Mathf.Clamp01((frame + elapsed / frameDuration) / (frames.Length - 1));
                    onProgress?.Invoke(progress);
                    elapsed += Time.deltaTime;
                    yield return null;
                }
            }
            sr.flipX = bedFacing;
            onProgress?.Invoke(1f);
        }
        else onProgress?.Invoke(1f);
        onComplete?.Invoke();
    }

    public void ConfigurePoseShadow(SpriteRenderer shadow) => poseShadow = shadow;

    private void CapturePresentation()
    {
        if (presentationCaptured || sr == null) return;
        presentationCaptured = true;
        standingRotation = transform.localRotation;
        standingScale = transform.localScale;
        standingOrder = sr.sortingOrder;
        standingLayer = sr.sortingLayerID;
        standingFacing = sr.flipX;
        if (poseShadow != null) { shadowEnabled = poseShadow.enabled; poseShadow.enabled = false; }
    }

    public void ApplyPosePresentation(SpriteRenderer back, SpriteRenderer foreground, Transform orientation, bool faceLeft)
    {
        ResolveComponents();
        if (sr == null) return;
        CapturePresentation();
        if (orientation != null) transform.rotation = orientation.rotation;
        sr.flipX = faceLeft;
        if (back != null)
        {
            sr.sortingLayerID = back.sortingLayerID;
            sr.sortingOrder = back.sortingOrder + 1;
        }
        if (foreground != null)
        {
            sr.sortingLayerID = foreground.sortingLayerID;
            sr.sortingOrder = back == null ? foreground.sortingOrder - 1 :
                Mathf.Min(sr.sortingOrder, foreground.sortingOrder - 1);
        }
    }

    internal void ApplyInteractionState(PlayerInteractionState state)
    {
        ResolveComponents();
        if (state != PlayerInteractionState.Sitting) SetDeskPose(DeskPoseState.None);
        interactionState = state;
        animTimer = 0f;
        currentWalkFrame = 0;
        if (sr == null) return;
        if (state != PlayerInteractionState.Standing) CapturePresentation();
        else if (presentationCaptured)
        {
            transform.localRotation = standingRotation;
            transform.localScale = standingScale;
            sr.sortingOrder = standingOrder;
            sr.sortingLayerID = standingLayer;
            sr.flipX = standingFacing;
            if (poseShadow != null) poseShadow.enabled = shadowEnabled;
            presentationCaptured = false;
        }
        Sprite pose = state == PlayerInteractionState.Sitting && sitLaptopSprite != null ? sitLaptopSprite :
            state == PlayerInteractionState.Lying && bedSleepSprite != null ? bedSleepSprite :
            state == PlayerInteractionState.Lying && lyingSprite != null ? lyingSprite :
            state == PlayerInteractionState.Sleeping && bedSleepSprite != null ? bedSleepSprite : StandingIdle;
        if (pose != null) sr.sprite = pose;
    }

    private void Update()
    {
        if (sr == null) return;

        if (interactionState == PlayerInteractionState.Sitting && DeskPose != DeskPoseState.None)
        {
            Sprite[] frames = DeskPose == DeskPoseState.SittingStudying ? studyingSprites : typingSprites;
            deskTimer += Time.deltaTime;
            float interval = DeskPose == DeskPoseState.SittingTyping ? DeskPoseTiming.Hold(deskFrame) : .35f;
            if (frames != null && frames.Length > 0 && deskTimer >= interval)
            {
                deskTimer = 0f; deskFrame = (deskFrame + 1) % frames.Length; sr.sprite = DeskBaseFrame(frames[deskFrame]);
                UpdateDeskForeground();
            }
            return;
        }
        if (interactionState == PlayerInteractionState.Lying && bedSleepSprite == null && bedIdleSprites != null && bedIdleSprites.Length > 0)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= 0.9f) // registered breathing frames; no whole-body pose changes
            {
                animTimer = 0f;
                currentBedFrame = (currentBedFrame + 1) % bedIdleSprites.Length;
                sr.sprite = bedIdleSprites[currentBedFrame];
            }
            return;
        }

        if(interactionState==PlayerInteractionState.Sleeping ||
            (interactionState==PlayerInteractionState.Lying && bedSleepSprite!=null))
        {
            animTimer+=Time.deltaTime;
            transform.localScale=new Vector3(standingScale.x,standingScale.y*(1f+.006f*Mathf.Sin(animTimer*2.2f)),standingScale.z);
            return;
        }
        if (interactionState != PlayerInteractionState.Standing) return;

        Vector2 vel = rb != null ? rb.linearVelocity : Vector2.zero;
        if(UpdateProductionStanding(vel))return;
        bool isMoving = vel.magnitude > 0.1f;

        if (isMoving)
        {
            // flip sprite based on movement
            if (vel.x > 0.05f) sr.flipX = false;
            else if (vel.x < -0.05f) sr.flipX = true;

            Sprite[] activeWalk = StandingWalk;
            if (activeWalk != null && activeWalk.Length > 0)
            {
                animTimer += Time.deltaTime;
                if (animTimer >= 1f / Mathf.Max(0.01f, walkFrameRate))
                {
                    animTimer = 0f;
                    currentWalkFrame = (currentWalkFrame + 1) % activeWalk.Length;
                    sr.sprite = activeWalk[currentWalkFrame];
                }
            }
        }
        else
        {
            currentWalkFrame = 0;
            animTimer = 0f;
            if (StandingIdle != null)
            {
                sr.sprite = StandingIdle;
            }
        }
    }
}
