using UnityEngine;

[DisallowMultipleComponent]
public class ChairInteractable : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private Transform seatPivot;
    [SerializeField] private Transform exitPivot;
    [SerializeField] private PlayerInteractionStateController player;
    [SerializeField] private SpriteRenderer deskForeground;
    [SerializeField] private bool faceLeft;
    private PlayerInteractionStateController occupant;

    public Transform SeatPivot => seatPivot;
    public Transform ExitPivot => exitPivot;
    public PlayerInteractionStateController Occupant => occupant != null && occupant.IsInteractingWith(this) ? occupant : null;
    public bool CanInteract => isActiveAndEnabled && seatPivot != null && exitPivot != null;
    public string InteractionPrompt => Occupant != null ? "E - Đứng lên" : "E - Ngồi xuống";
    public Transform GetTransform() => transform;

    public void Configure(Transform seat, Transform exit, PlayerInteractionStateController defaultPlayer,
        SpriteRenderer foreground, bool facingLeft)
    {
        seatPivot = seat; exitPivot = exit; player = defaultPlayer;
        deskForeground = foreground; faceLeft = facingLeft;
    }

    public bool CanInteractWith(PlayerInteractionStateController interactor)
    {
        if (!CanInteract || interactor == null) return false;
        return Occupant == interactor || (Occupant == null && interactor.CanInteract);
    }

    public void Interact() => Interact(player);

    public void Interact(PlayerInteractionStateController interactor)
    {
        if (!CanInteractWith(interactor)) return;
        if (Occupant == interactor)
        {
            interactor.ExitInteraction(exitPivot);
            occupant = null;
            return;
        }
        if (!interactor.TryEnterState(PlayerInteractionState.Sitting, seatPivot, this)) return;
        occupant = interactor;
        if (interactor.CharacterVisual != null)
            interactor.CharacterVisual.ApplyPosePresentation(null, deskForeground, seatPivot, faceLeft);
    }

    private void OnDisable()
    {
        if (Occupant != null) occupant.ExitInteraction(exitPivot);
        occupant = null;
    }
}
