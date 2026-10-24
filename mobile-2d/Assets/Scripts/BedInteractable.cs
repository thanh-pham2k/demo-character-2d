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

    public Transform BedPivot => bedPivot;
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
        return Occupant == interactor || (Occupant == null && interactor.CanInteract);
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
        if (Application.isPlaying) StartCoroutine(EnterBedRoutine(interactor));
        else ExecuteImmediateEnter(interactor);
    }

    public void ExecuteImmediateEnter(PlayerInteractionStateController interactor)
    {
        if (!interactor.TryEnterState(PlayerInteractionState.Lying, bedPivot, this)) return;
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
        isTransitioning = true;
        if (!interactor.TryEnterState(PlayerInteractionState.Transitioning, bedPivot, this))
        {
            isTransitioning = false;
            yield break;
        }
        occupant = interactor;
        occupied = true;
        if (bedForeground != null)
        {
            previousForegroundEnabled = bedForeground.enabled;
            bedForeground.enabled = true;
        }
        if (interactor.CharacterVisual != null)
            interactor.CharacterVisual.ApplyPosePresentation(bedBack, bedForeground, bedPivot, faceLeft);

        bool animDone = false;
        if (interactor.CharacterVisual != null)
        {
            yield return interactor.CharacterVisual.PlayBedEnterRoutine(() => animDone = true);
        }
        else animDone = true;

        interactor.TryEnterState(PlayerInteractionState.Lying, bedPivot, this);
        isTransitioning = false;
    }

    private System.Collections.IEnumerator ExitBedRoutine(PlayerInteractionStateController interactor)
    {
        isTransitioning = true;
        interactor.TryEnterState(PlayerInteractionState.Transitioning, bedPivot, this);

        bool animDone = false;
        if (interactor.CharacterVisual != null)
        {
            yield return interactor.CharacterVisual.PlayBedExitRoutine(() => animDone = true);
        }
        else animDone = true;

        interactor.ExitInteraction(exitPivot);
        ClearOccupant();
        isTransitioning = false;
    }

    private void ClearOccupant()
    {
        if (occupied && bedForeground != null) bedForeground.enabled = previousForegroundEnabled;
        occupant = null;
        occupied = false;
    }

    private void Update()
    {
        if (occupied && Occupant == null) ClearOccupant();
    }

    private void OnDisable()
    {
        if (Occupant != null) occupant.ExitInteraction(exitPivot);
        ClearOccupant();
    }
}
