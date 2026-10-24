using UnityEngine;
using UnityEngine.Events;

public class InteractableProp : MonoBehaviour, IInteractable
{
    [SerializeField] private string interactionPrompt = "Tương tác [E]";
    [SerializeField] private bool isInteractable = true;
    public UnityEvent onInteract;

    public string InteractionPrompt => interactionPrompt;

    public void SetPrompt(string newPrompt)
    {
        interactionPrompt = newPrompt;
    }

    public void SetInteractable(bool active)
    {
        isInteractable = active;
    }

    public bool IsActive => isInteractable;

    public void Interact()
    {
        if (!isInteractable) return;
        onInteract?.Invoke();
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
