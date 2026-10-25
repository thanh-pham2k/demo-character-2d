using UnityEngine;

public class BusStopInteractable : MonoBehaviour, IPlayerInteractable
{
    [SerializeField] private CampusCommuteController commute;
    public bool CanInteract=>isActiveAndEnabled && commute!=null && commute.IsOutdoors && !commute.Journey.IsBusy && !commute.HasCompletedTrip;
    public string InteractionPrompt
    {
        get
        {
            if(commute==null)return "";
            if(!commute.BusAtStop)return commute.MissedBus?"Lỡ chuyến trước • Chờ xe 33 tiếp theo...":"Xe buýt 33 đang ghé trạm...";
            if(commute.MissedBus)return "E - Lên chuyến sau (Vé 7.000đ)";
            return commute.IsReturning?"E - Xe về KTX (7.000đ)":"E - Lên xe buýt 33 (Vé 7.000đ)";
        }
    }
    public Transform GetTransform()=>transform;
    public void Configure(CampusCommuteController controller)=>commute=controller;
    public bool CanInteractWith(PlayerInteractionStateController actor)=>CanInteract && actor==commute.Journey.Player && actor.CanInteract;
    public void Interact()=>Interact(commute!=null?commute.Journey.Player:null);
    public void Interact(PlayerInteractionStateController actor){if(CanInteractWith(actor))commute.TryBoard(actor);}
}
