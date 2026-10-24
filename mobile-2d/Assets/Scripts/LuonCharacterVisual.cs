using UnityEngine;

public class LuonCharacterVisual : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite[] walkSprites;
    public Sprite sitLaptopSprite;

    [Header("Animation Settings")]
    public float walkFrameRate = 8f; // frames per second

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private float animTimer;
    private int currentWalkFrame;
    private bool isSitting;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    public void SetSitting(bool sit)
    {
        isSitting = sit;
        if (isSitting && sitLaptopSprite != null)
        {
            sr.sprite = sitLaptopSprite;
        }
    }

    private void Update()
    {
        if (isSitting) return;

        Vector2 vel = rb != null ? rb.linearVelocity : Vector2.zero;
        bool isMoving = vel.magnitude > 0.1f;

        if (isMoving)
        {
            // flip sprite based on movement
            if (vel.x > 0.05f) sr.flipX = false;
            else if (vel.x < -0.05f) sr.flipX = true;

            if (walkSprites != null && walkSprites.Length > 0)
            {
                animTimer += Time.deltaTime;
                if (animTimer >= 1f / walkFrameRate)
                {
                    animTimer = 0f;
                    currentWalkFrame = (currentWalkFrame + 1) % walkSprites.Length;
                    sr.sprite = walkSprites[currentWalkFrame];
                }
            }
        }
        else
        {
            currentWalkFrame = 0;
            animTimer = 0f;
            if (idleSprite != null)
            {
                sr.sprite = idleSprite;
            }
        }
    }
}
