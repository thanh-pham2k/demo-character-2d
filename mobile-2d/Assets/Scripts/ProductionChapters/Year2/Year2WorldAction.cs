using UnityEngine;
namespace ProductionChapters.Year2
{
    public sealed class Year2WorldAction : MonoBehaviour,IPlayerInteractable
    {
        public Year2Module module;
        public string station;
        public bool CanInteract => isActiveAndEnabled && module!=null && module.CanOpen(station);
        public string InteractionPrompt => "E — "+station;
        public Transform GetTransform()=>transform;
        public bool CanInteractWith(PlayerInteractionStateController actor)=>CanInteract && actor==module.Player && actor.CanInteract && Vector2.Distance(actor.transform.position,transform.position)<=1.8f;
        public void Interact(){if(module!=null)Interact(module.Player);}
        public void Interact(PlayerInteractionStateController actor){if(CanInteractWith(actor))module.Open(this);}
    }
}
