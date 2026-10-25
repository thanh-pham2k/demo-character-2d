using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteractionStateController : MonoBehaviour
{
    private PlayerMovement movement;
    private Rigidbody2D body;
    private LuonCharacterVisual characterVisual;
    private IInteractable interactionOwner;
    private bool hasInteractionOwner;
    private bool hasPhysicsOverride;
    private bool previousBodySimulated;
    private Vector3 entryPosition;
    private bool hasEntryPosition;

    public PlayerInteractionState CurrentState { get; private set; } = PlayerInteractionState.Standing;
    public bool CanInteract
    {
        get
        {
            ResolveComponents();
            return isActiveAndEnabled && CurrentState == PlayerInteractionState.Standing &&
                movement != null && movement.isActiveAndEnabled && !movement.IsDodging;
        }
    }
    public IInteractable ActiveInteractable => InteractionTargetUtility.IsAlive(interactionOwner) ? interactionOwner : null;
    public LuonCharacterVisual CharacterVisual { get { ResolveComponents(); return characterVisual; } }
    public bool IsInteractingWith(IInteractable target) => CurrentState != PlayerInteractionState.Standing &&
        object.ReferenceEquals(interactionOwner, target);

    public static PlayerInteractionStateController GetOrCreate(GameObject player)
    {
        if (player == null || player.GetComponent<PlayerMovement>() == null) return null;
        var controller = player.GetComponent<PlayerInteractionStateController>();
        return controller != null ? controller : player.AddComponent<PlayerInteractionStateController>();
    }

    private void Awake()
    {
        ResolveComponents();
        ExitInteraction();
    }

    private void ResolveComponents()
    {
        if (movement == null) movement = GetComponent<PlayerMovement>();
        if (body == null) body = GetComponent<Rigidbody2D>();
        if (characterVisual == null) characterVisual = GetComponentInChildren<LuonCharacterVisual>(true);
    }

    public bool CanInteractWith(IInteractable target)
    {
        if (!isActiveAndEnabled || !InteractionTargetUtility.IsAvailable(target)) return false;
        return CanInteract || (CurrentState != PlayerInteractionState.Standing &&
            object.ReferenceEquals(interactionOwner, target));
    }

    public bool TryEnterState(PlayerInteractionState state, Transform poseMarker = null, IInteractable owner = null, bool snapToMarker = true)
    {
        ResolveComponents();
        if (!System.Enum.IsDefined(typeof(PlayerInteractionState), state)) return false;
        if (state == PlayerInteractionState.Standing)
        {
            ExitInteraction();
            return true;
        }

        // Allow entering Transitioning, Sitting, Lying, Sleeping from valid states or during an active session with same owner
        bool isNewSession = CurrentState == PlayerInteractionState.Standing;
        bool isContinuingSession = !isNewSession && hasInteractionOwner && object.ReferenceEquals(interactionOwner, owner);

        if (!isNewSession && !isContinuingSession) return false;
        if (isNewSession && !CanInteract) return false;
        if (!object.ReferenceEquals(owner, null) &&
            !(isContinuingSession ? InteractionTargetUtility.IsAlive(owner) : InteractionTargetUtility.IsAvailable(owner))) return false;

        CurrentState = state;
        if (isNewSession)
        {
            hasEntryPosition = poseMarker != null;
            if (hasEntryPosition) entryPosition = transform.position;
            interactionOwner = owner;
            hasInteractionOwner = !object.ReferenceEquals(owner, null);
            movement.SetInteractionMovementLocked(true);
            if (body != null)
            {
                previousBodySimulated = body.simulated;
                hasPhysicsOverride = true;
                body.simulated = false;
            }
        }
        if (poseMarker != null && snapToMarker) MoveToMarker(poseMarker);
        if (characterVisual != null) characterVisual.ApplyInteractionState(state);
        return true;
    }

    public void ExitInteraction(Transform exitMarker = null)
    {
        ResolveComponents();
        CurrentState = PlayerInteractionState.Standing;
        interactionOwner = null;
        hasInteractionOwner = false;
        if (exitMarker != null) MoveToMarker(exitMarker);
        else if (hasEntryPosition) MoveToPosition(entryPosition);
        hasEntryPosition = false;
        if (hasPhysicsOverride && body != null) body.simulated = previousBodySimulated;
        hasPhysicsOverride = false;
        if (movement != null) movement.SetInteractionMovementLocked(false);
        if (characterVisual != null) characterVisual.ApplyInteractionState(CurrentState);
    }

    private void MoveToMarker(Transform marker)
    {
        MoveToPosition(marker.position);
    }

    private void MoveToPosition(Vector3 position)
    {
        if (body != null)
        {
            body.linearVelocity = Vector2.zero;
            body.position = position;
        }
        transform.position = position;
    }

    public void UpdatePosePosition(Vector3 position, IInteractable owner)
    {
        if (IsInteractingWith(owner)) MoveToPosition(position);
    }

    private void Update()
    {
        if (hasInteractionOwner && !InteractionTargetUtility.IsAlive(interactionOwner)) ExitInteraction();
    }

    private void OnDisable()
    {
        ExitInteraction();
    }
}
