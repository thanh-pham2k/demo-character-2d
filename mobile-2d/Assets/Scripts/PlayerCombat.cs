using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDuration = 0.35f;
    [SerializeField] private float attackCooldown = 0.45f;
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private int attackDamage = 25;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Slash Effect")]
    [SerializeField] private SlashVFX slashVFXPrefab;
    [SerializeField] private Transform slashSpawnPoint;

    [Header("Combat Feel / Feedback")]
    [SerializeField] private float hitStopDuration = 0.05f;
    [SerializeField] private float cameraShakeIntensity = 0.15f;
    [SerializeField] private float cameraShakeDuration = 0.12f;

    [Header("Components")]
    [SerializeField] private PlayerMovement movement;

    private bool isAttacking;
    private float lastAttackTime;

    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        if (movement == null) movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        bool attackPressed = false;

        var mouse = Mouse.current;
        var keyboard = Keyboard.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame) attackPressed = true;
        if (keyboard != null && keyboard.jKey.wasPressedThisFrame) attackPressed = true;

        if (attackPressed)
        {
            TryAttack();
        }
    }

    public void TryAttack()
    {
        if (isAttacking || Time.time - lastAttackTime < attackCooldown) return;
        if (movement != null && movement.IsDodging) return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        Vector2 attackDir = movement != null ? movement.LastMoveDirection : Vector2.right;

        // Spawn Large Slash VFX
        if (slashVFXPrefab != null)
        {
            Vector3 spawnPos = slashSpawnPoint != null ? slashSpawnPoint.position : transform.position + (Vector3)(attackDir * 0.7f);
            float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;
            SlashVFX vfx = Instantiate(slashVFXPrefab, spawnPos, Quaternion.Euler(0, 0, angle));
            vfx.Play();
        }

        yield return new WaitForSeconds(0.1f);

        // Hit Detection
        Vector2 hitCenter = (Vector2)transform.position + attackDir * (attackRange * 0.5f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitCenter, attackRange * 0.7f, enemyLayer);

        bool connected = false;
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;

            DummyEnemy enemy = hit.GetComponent<DummyEnemy>();
            if (enemy != null)
            {
                enemy.TakeHit(attackDamage, attackDir);
                connected = true;
            }
        }

        if (connected)
        {
            if (CameraController.Instance != null)
            {
                CameraController.Instance.Shake(cameraShakeIntensity, cameraShakeDuration);
            }
            if (hitStopDuration > 0)
            {
                StartCoroutine(HitStopRoutine(hitStopDuration));
            }
        }

        yield return new WaitForSeconds(Mathf.Max(0, attackDuration - 0.1f));
        isAttacking = false;
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1.0f;
    }
}
