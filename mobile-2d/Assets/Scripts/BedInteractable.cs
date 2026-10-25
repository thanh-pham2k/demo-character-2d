using UnityEngine;

[DisallowMultipleComponent]
public class BedInteractable : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private Transform bedPivot;
    [SerializeField] private Transform exitPivot;
    [SerializeField] private PlayerInteractionStateController player;
    [SerializeField] private SpriteRenderer bedBack;
    [SerializeField] private SpriteRenderer bedForeground;
    [SerializeField] private InteractableProp prerequisiteInteraction;
    [SerializeField] private bool faceLeft;
    private PlayerInteractionStateController occupant;
    private bool occupied;
    private bool previousForegroundEnabled;
    private bool isTransitioning;
    public PropReservation Reservation=>PropReservation.For(gameObject);

    public Transform BedPivot => bedPivot;
    public SpriteRenderer BackRenderer=>bedBack;
    public SpriteRenderer ForegroundRenderer=>bedForeground;
    public Transform ExitPivot => exitPivot;
    public PlayerInteractionStateController Occupant => occupant != null && occupant.IsInteractingWith(this) ? occupant : null;
    public bool CanInteract => isActiveAndEnabled && !isTransitioning && bedPivot != null && exitPivot != null &&
        (prerequisiteInteraction == null || !prerequisiteInteraction.IsActive);
    public string InteractionPrompt => isTransitioning ? string.Empty : (Occupant != null ? "E - Rời giường" : "E - Nằm nghỉ");
    public Transform GetTransform() => transform;

    public void Configure(Transform bed, Transform exit, PlayerInteractionStateController defaultPlayer,
        SpriteRenderer back, SpriteRenderer foreground, InteractableProp prerequisite, bool facingLeft)
    {
        bedPivot = bed; exitPivot = exit; player = defaultPlayer; bedBack = back;
        bedForeground = foreground; prerequisiteInteraction = prerequisite; faceLeft = facingLeft;
    }

    public bool CanInteractWith(PlayerInteractionStateController interactor)
    {
        if (!CanInteract || interactor == null || isTransitioning) return false;
        return Occupant == interactor || (Occupant == null && interactor.CanInteract && Reservation.IsAvailableFor(interactor));
    }

    public void Interact() => Interact(player);

    public void Interact(PlayerInteractionStateController interactor)
    {
        if (!CanInteractWith(interactor) || isTransitioning) return;
        if (Occupant == interactor)
        {
            if (Application.isPlaying) StartCoroutine(ExitBedRoutine(interactor));
            else ExecuteImmediateExit(interactor);
            return;
        }
        if(!Reservation.TryReserve(interactor))return;
        if (Application.isPlaying) StartCoroutine(EnterBedRoutine(interactor));
        else ExecuteImmediateEnter(interactor);
    }

    public void ExecuteImmediateEnter(PlayerInteractionStateController interactor)
    {
        if(!Reservation.TryReserve(interactor))return;
        if (!interactor.TryEnterState(PlayerInteractionState.Lying, bedPivot, this)){Reservation.Release(interactor);return;}
        occupant = interactor;
        occupied = true;
        if (bedForeground != null)
        {
            previousForegroundEnabled = bedForeground.enabled;
            bedForeground.enabled = true;
        }
        if (interactor.CharacterVisual != null)
            interactor.CharacterVisual.ApplyPosePresentation(bedBack, bedForeground, bedPivot, faceLeft);
    }

    public void ExecuteImmediateExit(PlayerInteractionStateController interactor)
    {
        interactor.ExitInteraction(exitPivot);
        ClearOccupant();
    }

    private System.Collections.IEnumerator EnterBedRoutine(PlayerInteractionStateController interactor)
    {
        if (!interactor.TryEnterState(PlayerInteractionState.Transitioning, bedPivot, this, false)){Reservation.Release(interactor);yield break;}
        isTransitioning = true;
        occupant = interactor;
        occupied = true;
        if (bedForeground != null)
        {
            previousForegroundEnabled = bedForeground.enabled;
            bedForeground.enabled = true;
        }
        Vector3 start = interactor.transform.position;
        if (interactor.CharacterVisual != null)
        {
            interactor.CharacterVisual.ApplyPosePresentation(bedBack, bedForeground, bedPivot, faceLeft);
            yield return interactor.CharacterVisual.PlayBedEnterRoutine(null, progress =>
                interactor.UpdatePosePosition(Vector3.Lerp(start, bedPivot.position, Mathf.SmoothStep(0f, 1f, progress)), this));
        }
        if (interactor != null && interactor.IsInteractingWith(this))
            interactor.TryEnterState(PlayerInteractionState.Lying, bedPivot, this);
        else ClearOccupant();
        isTransitioning = false;
    }

    private System.Collections.IEnumerator ExitBedRoutine(PlayerInteractionStateController interactor)
    {
        if (!interactor.TryEnterState(PlayerInteractionState.Transitioning, null, this)) yield break;
        isTransitioning = true;
        Vector3 start = interactor.transform.position;
        if (interactor.CharacterVisual != null)
            yield return interactor.CharacterVisual.PlayBedExitRoutine(null, progress =>
                interactor.UpdatePosePosition(Vector3.Lerp(start, exitPivot.position, Mathf.SmoothStep(0f, 1f, progress)), this));
        if (interactor != null && interactor.IsInteractingWith(this)) interactor.ExitInteraction(exitPivot);
        ClearOccupant();
    }

    private void ClearOccupant()
    {
        if(occupant!=null)Reservation.Release(occupant);
        if (occupied && bedForeground != null) bedForeground.enabled = previousForegroundEnabled;
        occupant = null;
        occupied = false;
        isTransitioning = false;
    }

    private void Update()
    {
        if (occupied && Occupant == null) ClearOccupant();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (Occupant != null) occupant.ExitInteraction(exitPivot);
        ClearOccupant();
    }
}
