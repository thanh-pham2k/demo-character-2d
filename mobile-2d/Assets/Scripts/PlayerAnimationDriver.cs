using System.Collections;
using UnityEngine;

public class PlayerAnimationDriver : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] walkSprites;
    [SerializeField] private Sprite attackWindupSprite;
    [SerializeField] private Sprite attackImpactSprite;
    [SerializeField] private Sprite dodgeSprite;
    [SerializeField] private Sprite hitSprite;

    private Rigidbody2D rb;
    private PlayerMovement movement;
    private PlayerCombat combat;

    private float animTimer;
    private int animIndex;

    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
    }

    public void SetSprites(Sprite[] idle, Sprite[] walk, Sprite windup, Sprite impact, Sprite dodge, Sprite hit)
    {
        idleSprites = idle;
        walkSprites = walk;
        attackWindupSprite = windup;
        attackImpactSprite = impact;
        dodgeSprite = dodge;
        hitSprite = hit;
    }

    private void Update()
    {
        if (spriteRenderer == null) return;

        // 1. Trạng thái Dodge: Đổi ảnh dáng cúi thấp lướt nhanh
        if (movement != null && movement.IsDodging)
        {
            if (dodgeSprite != null) spriteRenderer.sprite = dodgeSprite;
            return;
        }

        // 2. Trạng thái Attack: Đổi ảnh vung kiếm chém hết biên độ
        if (combat != null && combat.IsAttacking)
        {
            if (attackImpactSprite != null) spriteRenderer.sprite = attackImpactSprite;
            return;
        }

        // 3. Trạng thái Walk: Khi có vận tốc di chuyển thực tế (đổi frame bước chân)
        bool isMoving = rb != null && rb.linearVelocity.sqrMagnitude > 0.05f;
        if (isMoving && walkSprites != null && walkSprites.Length > 0)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= 0.16f) // Tốc độ bước chân tự nhiên (~6-7 fps)
            {
                animTimer = 0f;
                animIndex = (animIndex + 1) % walkSprites.Length;
            }
            spriteRenderer.sprite = walkSprites[animIndex];
            return;
        }

        // 4. Trạng thái Idle: Nhấp nhô thở nhẹ
        if (idleSprites != null && idleSprites.Length > 0)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= 0.35f) // Thở êm ái
            {
                animTimer = 0f;
                animIndex = (animIndex + 1) % idleSprites.Length;
            }
            spriteRenderer.sprite = idleSprites[animIndex];
        }
    }
}
