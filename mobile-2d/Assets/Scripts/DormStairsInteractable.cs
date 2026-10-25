using UnityEngine;

public class DormStairsInteractable : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private DormJourneyController journey;
    public bool CanInteract => isActiveAndEnabled && journey != null && journey.IsInHallway && !journey.IsBusy;
    public string InteractionPrompt => "E - Xuống cầu thang";
    public Transform GetTransform() => transform;
    public void Configure(DormJourneyController controller) => journey = controller;
    public bool CanInteractWith(PlayerInteractionStateController actor) => CanInteract && actor == journey.Player && actor.CanInteract;
    public void Interact() => Interact(journey != null ? journey.Player : null);
    public void Interact(PlayerInteractionStateController actor) { if (CanInteractWith(actor)) journey.TryStairs(actor); }
}
