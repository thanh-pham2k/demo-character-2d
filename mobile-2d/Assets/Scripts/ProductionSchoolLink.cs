using UnityEngine;

// Physical navigation only; a doorway never completes a mission event by itself.
public sealed class ProductionSchoolLink : MonoBehaviour,IPlayerInteractable
{
    public enum Route { Hallway, Lobby, ReturnStop }
    public ProductionOpeningController controller;
    public Route route;
    public TMPro.TMP_Text marker;
    private void Update(){if(marker!=null)marker.gameObject.SetActive(CanInteract);}
    public string InteractionPrompt=>route==Route.Hallway?"E — Ra hành lang, tới WC Nam":route==Route.Lobby?"E — Xuống sảnh, đi tới trạm 33":"E — Ra cổng H6, đón xe về KTX";
    public bool CanInteract=>isActiveAndEnabled && controller!=null && controller.CanUseSchoolLink(route);
    public Transform GetTransform()=>transform;
    public bool CanInteractWith(PlayerInteractionStateController actor)=>CanInteract && actor==controller.player && actor.CanInteract && Vector2.Distance(actor.transform.position,transform.position)<=1.8f;
    public void Interact()=>Interact(controller!=null?controller.player:null);
    public void Interact(PlayerInteractionStateController actor){if(CanInteractWith(actor))controller.UseSchoolLink(route);}
}
