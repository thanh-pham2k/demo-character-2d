using UnityEngine;

public class DormDoorwayTrigger : MonoBehaviour
{
    [SerializeField] private DormJourneyController journey;
    [SerializeField] private HingedRoomDoor door;
    [SerializeField] private bool toHallway;
    private bool wasPastThreshold;
    public void Configure(DormJourneyController controller, HingedRoomDoor roomDoor, bool outward)
    { journey = controller; door = roomDoor; toHallway = outward; }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (journey == null || other.attachedRigidbody == null) return;
        var player = other.attachedRigidbody.GetComponent<PlayerInteractionStateController>();
        if (player != journey.Player) return;
        bool pastThreshold = player.transform.position.y > transform.position.y;
        // Require a fresh crossing to return: holding Up while exiting must not bounce back after cooldown.
        if (player.CanInteract && pastThreshold && (toHallway || !wasPastThreshold)) journey.TryPortal(toHallway, door);
        wasPastThreshold = pastThreshold;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (journey != null && other.attachedRigidbody != null &&
            other.attachedRigidbody.GetComponent<PlayerInteractionStateController>() == journey.Player)
            wasPastThreshold = false;
    }
}
