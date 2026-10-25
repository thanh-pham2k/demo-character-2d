using UnityEngine;

public interface IInteractable
{
    bool CanInteract { get; }
    string InteractionPrompt { get; }
    void Interact();
    Transform GetTransform();
}

public interface IPlayerInteractable : IInteractable
{
    bool CanInteractWith(PlayerInteractionStateController player);
    void Interact(PlayerInteractionStateController player);
}

public static class InteractionTargetUtility
{
    public static bool IsAlive(IInteractable target)
    {
        if (object.ReferenceEquals(target, null)) return false;
        // Interfaces do not use Unity's destroyed-object null comparison.
        if (target is Object unityObject && unityObject == null) return false;
        if (target is Behaviour behaviour && !behaviour.isActiveAndEnabled) return false;
        Transform anchor = target.GetTransform();
        return anchor != null && anchor.gameObject.activeInHierarchy;
    }

    public static bool IsAvailable(IInteractable target) => IsAlive(target) && target.CanInteract;
}
