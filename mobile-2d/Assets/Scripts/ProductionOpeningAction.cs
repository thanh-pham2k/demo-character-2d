using UnityEngine;
public sealed class ProductionOpeningAction : MonoBehaviour,IPlayerInteractable
{
    public ProductionOpeningController controller;
    public int mission;
    public string step;
    public TMPro.TMP_Text marker;
    public string InteractionPrompt=>"E — "+controller.CurrentActionLabel;
    public bool CanInteract=>isActiveAndEnabled && controller!=null && controller.CanUse(this);
    public Transform GetTransform()=>transform;
    public bool CanInteractWith(PlayerInteractionStateController actor)=>CanInteract && actor==controller.player && actor.CanInteract && Vector2.Distance(actor.transform.position,transform.position)<=1.8f;
    private void Update(){if(marker!=null){marker.gameObject.SetActive(CanInteract);if(CanInteract)marker.text=controller.CurrentActionLabel;}}
    public void Interact()=>controller.Open(this);
    public void Interact(PlayerInteractionStateController actor){if(CanInteractWith(actor))controller.Open(this);}
}
