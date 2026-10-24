using UnityEngine;

[ExecuteAlways]
public class HingedRoomDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer doorLeaf;
    [SerializeField] private BoxCollider2D blockingCollider;
    [SerializeField] private float leafWidth = 1.28f;
    [SerializeField] private float openAngle = 70f;
    [SerializeField] private float swingSpeed = 100f;
    [SerializeField] private float currentAngle = 25f;
    [SerializeField] private float targetAngle = 25f;
    private MaterialPropertyBlock properties;

    public float CurrentAngle => currentAngle;
    public bool CanInteract => isActiveAndEnabled && doorLeaf != null;
    public string InteractionPrompt => targetAngle > 5f ? "E - Đóng cửa" : "E - Mở cửa";
    public Transform GetTransform() => transform;

    public void Configure(SpriteRenderer leaf, BoxCollider2D collider, float width)
    {
        doorLeaf = leaf;
        blockingCollider = collider;
        leafWidth = width;
        SetPreviewAngle(25f);
    }

    public void Interact()
    {
        if (!CanInteract) return;
        if (targetAngle > 5f)
        {
            if (!IsClosingPathClear(currentAngle, 0f)) return;
            targetAngle = 0f;
        }
        else targetAngle = openAngle;
        ApplyAngle();
    }

    public void SetPreviewAngle(float angle)
    {
        currentAngle = Mathf.Clamp(angle, 0f, openAngle);
        targetAngle = currentAngle;
        ApplyAngle();
    }

    private void OnEnable() => ApplyAngle();
    private void OnValidate() => ApplyAngle();

    private void Update()
    {
        if (Application.isPlaying) Advance(Time.deltaTime);
        else ApplyAngle();
    }

    public void Advance(float deltaTime)
    {
        float next = Mathf.MoveTowards(currentAngle, targetAngle, swingSpeed * Mathf.Max(0f, deltaTime));
        if (next < currentAngle && !IsClosingPathClear(currentAngle, 0f))
            targetAngle = openAngle; // Reopen if an actor steps into the remaining closing sweep.
        else currentAngle = next;
        ApplyAngle();
    }

    private bool IsClosingPathClear(float from, float to)
    {
        int samples = Mathf.Max(1, Mathf.CeilToInt(Mathf.Abs(from - to) / 5f));
        for (int sample = 0; sample <= samples; sample++)
        {
            float radians = Mathf.Lerp(from, to, (float)sample / samples) * Mathf.Deg2Rad;
            float width = leafWidth * Mathf.Cos(radians);
            float depth = leafWidth * Mathf.Sin(radians) * 0.45f;
            Vector3 center = transform.TransformPoint(new Vector3(-width * 0.5f, -depth * 0.5f, 0f));
            Vector2 size = new Vector2((width + 0.1f) * Mathf.Abs(transform.lossyScale.x),
                (depth + 0.18f) * Mathf.Abs(transform.lossyScale.y));
            foreach (Collider2D hit in Physics2D.OverlapBoxAll(center, size, transform.eulerAngles.z,
                Physics2D.AllLayers, float.NegativeInfinity, float.PositiveInfinity))
            {
                if (hit == blockingCollider || hit.isTrigger || hit.attachedRigidbody == null) continue;
                if (blockingCollider != null && hit.attachedRigidbody == blockingCollider.attachedRigidbody) continue;
                return false;
            }
        }
        return true;
    }

    private void ApplyAngle()
    {
        if (doorLeaf == null) return;
        float radians = currentAngle * Mathf.Deg2Rad;
        float width = leafWidth * Mathf.Cos(radians);
        float depth = leafWidth * Mathf.Sin(radians) * 0.45f;
        if (properties == null) properties = new MaterialPropertyBlock();
        doorLeaf.GetPropertyBlock(properties);
        // The source sprite's bottom-right pivot is the existing right-hand hinge.
        // Project the inward swing onto the room floor while keeping vertical edges upright.
        properties.SetFloat("_CompressX", Mathf.Cos(radians));
        properties.SetFloat("_ShearY", Mathf.Sin(radians) * 0.45f);
        doorLeaf.SetPropertyBlock(properties);

        if (blockingCollider != null)
        {
            blockingCollider.enabled = targetAngle <= currentAngle && currentAngle < openAngle - 0.5f;
            blockingCollider.size = new Vector2(width, depth + 0.08f);
            blockingCollider.offset = new Vector2(-width * 0.5f, -depth * 0.5f);
        }
    }
}
