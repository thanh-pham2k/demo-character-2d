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
    public string InteractionPrompt => currentAngle > 5f ? "Bấm [E] đóng cửa" : "Bấm [E] mở cửa";
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
        targetAngle = targetAngle > 5f ? 0f : openAngle;
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
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, swingSpeed * Mathf.Max(0f, deltaTime));
        ApplyAngle();
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
            blockingCollider.size = new Vector2(width, depth + 0.08f);
            blockingCollider.offset = new Vector2(-width * 0.5f, -depth * 0.5f);
        }
    }
}
