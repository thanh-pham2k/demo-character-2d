using UnityEngine;

[DisallowMultipleComponent]
public class ChairInteractable : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private Transform seatPivot;
    [SerializeField] private Transform exitPivot;
    [SerializeField] private PlayerInteractionStateController player;
    [SerializeField] private SpriteRenderer deskForeground;
    [SerializeField] private SpriteRenderer deskBack;
    [SerializeField] private bool faceLeft;
    [SerializeField] private DeskActionModal actionModal;
    private PlayerInteractionStateController occupant;
    public PropReservation Reservation=>PropReservation.For(gameObject);
    public void ConfigureMenu(DeskActionModal modal) => actionModal = modal;

    public Transform SeatPivot => seatPivot;
    public SpriteRenderer BackRenderer=>deskBack;
    public SpriteRenderer ForegroundRenderer=>deskForeground;
    public Transform ExitPivot => exitPivot;
    public PlayerInteractionStateController Occupant => occupant != null && occupant.IsInteractingWith(this) ? occupant : null;
    public bool CanInteract => isActiveAndEnabled && seatPivot != null && exitPivot != null;
    public string InteractionPrompt => Occupant != null ? "E - Đứng lên" : "E - Ngồi xuống";
    public Transform GetTransform() => transform;

    public void Configure(Transform seat, Transform exit, PlayerInteractionStateController defaultPlayer,
        SpriteRenderer foreground, bool facingLeft, SpriteRenderer back = null)
    {
        seatPivot = seat; exitPivot = exit; player = defaultPlayer;
        deskForeground = foreground; deskBack = back; faceLeft = facingLeft;
    }

    public bool CanInteractWith(PlayerInteractionStateController interactor)
    {
        if (!CanInteract || interactor == null) return false;
        return Occupant == interactor || (Occupant == null && interactor.CanInteract && Reservation.IsAvailableFor(interactor));
    }

    public void Interact() => Interact(player);

    public void Interact(PlayerInteractionStateController interactor)
    {
        if (!CanInteractWith(interactor)) return;
        if (Occupant == interactor)
        {
            if (actionModal != null) actionModal.Close(this);
            interactor.ExitInteraction(exitPivot);
            Reservation.Release(interactor);
            occupant = null;
            return;
        }
        if (!Reservation.TryReserve(interactor))return;
        if (!interactor.TryEnterState(PlayerInteractionState.Sitting, seatPivot, this)){Reservation.Release(interactor);return;}
        occupant = interactor;
        if (interactor.CharacterVisual != null)
            interactor.CharacterVisual.ApplyPosePresentation(deskBack, deskForeground, seatPivot, faceLeft);
        if (actionModal != null) actionModal.Open(this, interactor);
    }

    private void OnDisable()
    {
        if (actionModal != null) actionModal.Close(this);
        if (Occupant != null) occupant.ExitInteraction(exitPivot);
        if(occupant!=null)Reservation.Release(occupant);
        occupant = null;
    }
    private void Update(){if(occupant!=null && Occupant==null){Reservation.Release(occupant);occupant=null;}}
}
