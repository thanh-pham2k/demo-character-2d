using UnityEngine;

public class BackpackInteractable : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private DormJourneyController journey;
    [SerializeField] private SpriteRenderer backpack;
    [SerializeField] private Sprite hangingSprite, emptyHookSprite;
    public bool IsHookEmpty => journey != null && journey.HasBackpack && backpack != null && backpack.sprite == emptyHookSprite;
    public SpriteRenderer Visual => backpack;
    public bool CanInteract => isActiveAndEnabled && journey != null && !journey.HasBackpack && !journey.IsBusy;
    public string InteractionPrompt => "E - Lấy balo đeo vào";
    public Transform GetTransform() => transform;
    public void Configure(DormJourneyController controller, SpriteRenderer sprite, Sprite full = null, Sprite empty = null)
    { journey = controller; backpack = sprite; hangingSprite = full; emptyHookSprite = empty; }
    public void RefreshVisual()
    {
        if (journey == null || backpack == null) return;
        backpack.enabled = true;
        if (journey.HasBackpack && emptyHookSprite != null) backpack.sprite = emptyHookSprite;
        else if (hangingSprite != null) backpack.sprite = hangingSprite;
    }
    public bool CanInteractWith(PlayerInteractionStateController actor) => CanInteract && actor == journey.Player && actor.CanInteract;
    public void Interact() => Interact(journey != null ? journey.Player : null);
    public void Interact(PlayerInteractionStateController actor)
    {
        if (CanInteractWith(actor) && journey.EquipBackpack(actor)) RefreshVisual();
    }
    private void OnEnable() => RefreshVisual();
}
