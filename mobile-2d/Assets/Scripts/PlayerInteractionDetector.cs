using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteractionDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 2.0f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject promptCanvas;
    [SerializeField] private TextMeshProUGUI promptText;

    private IInteractable currentInteractable;

    private void Update()
    {
        FindClosestInteractable();

        if (currentInteractable != null)
        {
            if (promptCanvas != null && !promptCanvas.activeSelf)
                promptCanvas.SetActive(true);

            if (promptText != null)
                promptText.text = currentInteractable.InteractionPrompt;

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                currentInteractable.Interact();
            }
        }
        else
        {
            if (promptCanvas != null && promptCanvas.activeSelf)
                promptCanvas.SetActive(false);
        }
    }

    private void FindClosestInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        float closestDist = float.MaxValue;
        IInteractable closest = null;

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();
            if (interactable == null)
                interactable = hit.GetComponentInParent<IInteractable>();

            if (interactable != null)
            {
                var prop = interactable as InteractableProp;
                if (prop != null && !prop.IsActive) continue;

                float dist = Vector2.Distance(transform.position, interactable.GetTransform().position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = interactable;
                }
            }
        }

        currentInteractable = closest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
