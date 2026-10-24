using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerInteractionStateController))]
public class PlayerInteractionDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 2.0f;
    [Tooltip("Zero preserves legacy scenes: use the default Physics2D query layers.")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private GameObject promptCanvas;
    [SerializeField] private TextMeshProUGUI promptText;

    private IInteractable currentInteractable;
    private PlayerInteractionStateController interactionState;

    public IInteractable CurrentInteractable => InteractionTargetUtility.IsAvailable(currentInteractable) ? currentInteractable : null;

    private void Awake()
    {
        // Existing scenes acquire the foundation at runtime without changing their serialized assets.
        interactionState = PlayerInteractionStateController.GetOrCreate(gameObject);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) TryInteract();
        currentInteractable = FindClosestInteractable();
        UpdatePrompt();
    }

    public IInteractable FindClosestInteractable()
    {
        if (!isActiveAndEnabled || interactionState == null) return null;
        if (!interactionState.CanInteract)
        {
            IInteractable owner = interactionState.ActiveInteractable;
            return CanSelect(owner) ? owner : null;
        }
        int mask = interactableLayer.value == 0 ? Physics2D.DefaultRaycastLayers : interactableLayer.value;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, Mathf.Max(0f, detectionRadius),
            mask, float.NegativeInfinity, float.PositiveInfinity);
        float closestDistance = float.MaxValue;
        IInteractable closest = null;

        foreach (Collider2D hit in hits)
        {
            if (hit == null || !hit.enabled || (mask & (1 << hit.gameObject.layer)) == 0) continue;
            IInteractable target = hit.GetComponent<IInteractable>();
            if (target == null) target = hit.GetComponentInParent<IInteractable>();
            if (!CanSelect(target)) continue;

            float distance = ((Vector2)(transform.position - target.GetTransform().position)).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }
        return closest;
    }

    public bool TryInteract()
    {
        currentInteractable = FindClosestInteractable();
        if (!InteractionTargetUtility.IsAvailable(currentInteractable) ||
            !interactionState.CanInteractWith(currentInteractable)) return false;
        if (currentInteractable is IPlayerInteractable contextual) contextual.Interact(interactionState);
        else currentInteractable.Interact();
        return true;
    }

    private bool CanSelect(IInteractable target)
    {
        return interactionState.CanInteractWith(target) &&
            (!(target is IPlayerInteractable contextual) || contextual.CanInteractWith(interactionState));
    }

    public void ConfigurePrompt(GameObject canvas, TextMeshProUGUI text)
    {
        promptCanvas = canvas;
        promptText = text;
    }

    private void UpdatePrompt()
    {
        bool hasTarget = InteractionTargetUtility.IsAvailable(currentInteractable);
        if (promptCanvas != null && promptCanvas.activeSelf != hasTarget) promptCanvas.SetActive(hasTarget);
        if (promptText != null) promptText.text = hasTarget ? currentInteractable.InteractionPrompt : string.Empty;
    }

    private void OnDisable()
    {
        currentInteractable = null;
        if (promptCanvas != null) promptCanvas.SetActive(false);
        if (promptText != null) promptText.text = string.Empty;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Mathf.Max(0f, detectionRadius));
    }
}
