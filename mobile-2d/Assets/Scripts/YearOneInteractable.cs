using UnityEngine;

// World entry points keep player detection generic.
public class YearOneInteractable : MonoBehaviour, IPlayerInteractable
{
    public enum Action { EnterH6, SitBesideClassmate, ExitClass, Canteen, ReturnBus, PrepareFeast }
    [SerializeField] private YearOneMilestoneController milestone;
    [SerializeField] private Action action;
    public string InteractionPrompt => milestone!=null?milestone.CampaignPrompt(action):"";
    public bool CanInteract => isActiveAndEnabled && milestone!=null && milestone.CanUse(action);
    public Transform GetTransform()=>transform;
    public void Configure(YearOneMilestoneController controller,Action value){milestone=controller;action=value;}
    public bool CanInteractWith(PlayerInteractionStateController actor)=>CanInteract && actor==milestone.Player && actor.CanInteract;
    public void Interact()=>Interact(milestone!=null?milestone.Player:null);
    public void Interact(PlayerInteractionStateController actor){if(CanInteractWith(actor))milestone.Use(action);}
}
