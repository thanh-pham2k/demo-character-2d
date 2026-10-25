using UnityEngine;
using System.Collections;

public class RestroomInteractable : MonoBehaviour, IPlayerInteractable
{
    public enum Gender { Male, Female }
    [SerializeField] private Gender gender;
    [SerializeField] private DormJourneyController journey;
    [SerializeField] private Transform exitPivot;
    [SerializeField] private Transform doorLeaf;
    [SerializeField] private SpriteRenderer opening;
    public bool DoorIsOpen { get; private set; }
    public void ResetDoor(){DoorIsOpen=false;if(doorLeaf!=null)doorLeaf.localScale=Vector3.one;if(opening!=null)opening.enabled=false;}
    public void ConfigureDoor(Transform leaf,SpriteRenderer interior){doorLeaf=leaf;opening=interior;}
    public IEnumerator AnimateDoor(bool open)
    {
        DoorIsOpen=open;
        if(opening!=null)opening.enabled=true;
        if(doorLeaf!=null)
        {
            float start=doorLeaf.localScale.x,target=open?.12f:1f;
            for(float t=0;t<.2f;t+=Time.deltaTime)
            {doorLeaf.localScale=new Vector3(Mathf.Lerp(start,target,t/.2f),1,1);yield return null;}
            doorLeaf.localScale=new Vector3(target,1,1);
        }
        if(opening!=null)opening.enabled=open;
    }
    public Gender RoomGender => gender;
    public Transform ExitPivot => exitPivot;
    public bool CanInteract => isActiveAndEnabled && journey != null && journey.IsInHallway && !journey.IsBusy;
    public string InteractionPrompt => gender == Gender.Male ? "E - WC Nam • Rửa mặt" : "E - WC Nữ • Không được vào";
    public Transform GetTransform() => transform;
    public void Configure(Gender type, DormJourneyController controller, Transform exit) { gender = type; journey = controller; exitPivot = exit; }
    public bool CanInteractWith(PlayerInteractionStateController actor) => CanInteract && actor == journey.Player && actor.CanInteract;
    public void Interact() => Interact(journey != null ? journey.Player : null);
    public void Interact(PlayerInteractionStateController actor) { if (CanInteractWith(actor)) journey.UseRestroom(actor, gender, exitPivot,this); }
}
