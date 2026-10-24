using UnityEngine;

public class LuonCharacterVisual : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite[] walkSprites;
    public Sprite sitLaptopSprite;
    public Sprite lyingSprite;
    [SerializeField] private SpriteRenderer poseShadow;

    [Header("Bed Animation Sprites")]
    public Sprite[] bedEnterSprites;
    public Sprite[] bedIdleSprites;
    public Sprite bedSleepSprite;
    public Sprite[] bedExitSprites;

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

    public System.Collections.IEnumerator PlayBedEnterRoutine(System.Action onComplete)
    {
        ResolveComponents();
        currentBedFrame = 0;
        animTimer = 0f;
        if (bedEnterSprites != null && bedEnterSprites.Length > 0 && sr != null)
        {
            while (currentBedFrame < bedEnterSprites.Length)
            {
                sr.sprite = bedEnterSprites[currentBedFrame];
                float waitTime = 1f / Mathf.Max(1f, bedAnimFrameRate);
                yield return new WaitForSeconds(waitTime);
                currentBedFrame++;
            }
        }
        onComplete?.Invoke();
    }

    public System.Collections.IEnumerator PlayBedExitRoutine(System.Action onComplete)
    {
        ResolveComponents();
        currentBedFrame = 0;
        animTimer = 0f;
        if (bedExitSprites != null && bedExitSprites.Length > 0 && sr != null)
        {
            while (currentBedFrame < bedExitSprites.Length)
            {
                sr.sprite = bedExitSprites[currentBedFrame];
                float waitTime = 1f / Mathf.Max(1f, bedAnimFrameRate);
                yield return new WaitForSeconds(waitTime);
                currentBedFrame++;
            }
        }
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
            state == PlayerInteractionState.Lying && lyingSprite != null ? lyingSprite :
            state == PlayerInteractionState.Sleeping && bedSleepSprite != null ? bedSleepSprite : idleSprite;
        if (pose != null) sr.sprite = pose;
    }

    private void Update()
    {
        if (sr == null) return;

        if (interactionState == PlayerInteractionState.Lying && bedIdleSprites != null && bedIdleSprites.Length > 0)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= 1.5f) // slow resting breathing / gentle pose shift
            {
                animTimer = 0f;
                currentBedFrame = (currentBedFrame + 1) % bedIdleSprites.Length;
                sr.sprite = bedIdleSprites[currentBedFrame];
            }
            return;
        }

        if (interactionState != PlayerInteractionState.Standing) return;

        Vector2 vel = rb != null ? rb.linearVelocity : Vector2.zero;
        bool isMoving = vel.magnitude > 0.1f;

        if (isMoving)
        {
            // flip sprite based on movement
            if (vel.x > 0.05f) sr.flipX = false;
            else if (vel.x < -0.05f) sr.flipX = true;

            if (walkSprites != null && walkSprites.Length > 0)
            {
                animTimer += Time.deltaTime;
                if (animTimer >= 1f / Mathf.Max(0.01f, walkFrameRate))
                {
                    animTimer = 0f;
                    currentWalkFrame = (currentWalkFrame + 1) % walkSprites.Length;
                    sr.sprite = walkSprites[currentWalkFrame];
                }
            }
        }
        else
        {
            currentWalkFrame = 0;
            animTimer = 0f;
            if (idleSprite != null)
            {
                sr.sprite = idleSprite;
            }
        }
    }
}
