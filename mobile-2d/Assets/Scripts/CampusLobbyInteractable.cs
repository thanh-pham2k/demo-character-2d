using UnityEngine;

public class CampusLobbyInteractable : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private CampusCommuteController commute;
    public void Configure(CampusCommuteController controller)=>commute=controller;
    public bool CanInteract=>isActiveAndEnabled && commute!=null && commute.IsOutdoors && !commute.Journey.IsBusy;
    public string InteractionPrompt=>"E - Trở lên tầng 4";
    public Transform GetTransform()=>transform;
    public bool CanInteractWith(PlayerInteractionStateController actor)=>CanInteract && actor==commute.Journey.Player && actor.CanInteract;
    public void Interact()=>Interact(commute!=null?commute.Journey.Player:null);
    public void Interact(PlayerInteractionStateController actor){if(CanInteractWith(actor))commute.TryReturnUpstairs(actor);}
}
