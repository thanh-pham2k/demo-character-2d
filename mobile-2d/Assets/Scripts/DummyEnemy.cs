using System.Collections;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Color hitColor = Color.white;
    [SerializeField] private float knockbackForce = 6.0f;

    private int currentHealth;
    private Color originalColor;
    private Material originalMaterial;

    private void Awake()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        if (spriteRenderer != null) originalColor = spriteRenderer.color;
    }

    public void TakeHit(int damage, Vector2 hitDirection)
    {
        currentHealth -= damage;
        StartCoroutine(HitFlashRoutine());

        if (rb != null)
        {
            rb.linearVelocity = hitDirection.normalized * knockbackForce;
        }

        if (currentHealth <= 0)
        {
            currentHealth = maxHealth; // Reset dummy health
        }
    }

    private IEnumerator HitFlashRoutine()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.08f);
            spriteRenderer.color = originalColor;
        }
    }
}
