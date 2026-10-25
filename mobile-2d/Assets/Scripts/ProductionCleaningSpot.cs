using UnityEngine;
public sealed class ProductionCleaningSpot : MonoBehaviour,IPlayerInteractable
{
    public ProductionOpeningController controller;
    public int index;
    public SpriteRenderer dirt;
    public TMPro.TMP_Text marker;
    public bool CanInteract=>isActiveAndEnabled && controller!=null && controller.CanClean(index);
    public string InteractionPrompt=>"E — Lau điểm bụi "+(index+1);
    public Transform GetTransform()=>transform;
    public bool CanInteractWith(PlayerInteractionStateController actor)=>CanInteract && actor==controller.player && actor.CanInteract && Vector2.Distance(actor.transform.position,transform.position)<=1.8f;
    public void Interact()=>controller.Scrub(this,controller.player);
    public void Interact(PlayerInteractionStateController actor){if(CanInteractWith(actor))controller.Scrub(this,actor);}
    private void Update(){if(marker!=null)marker.gameObject.SetActive(CanInteract);}
}
