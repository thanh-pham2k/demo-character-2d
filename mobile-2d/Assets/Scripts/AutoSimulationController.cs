using System.Collections;
using UnityEngine;

public class AutoSimulationController : MonoBehaviour
{
    [Header("Simulation Target")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private Transform dummyTransform;

    [Header("Status")]
    [SerializeField] private bool autoSimulate = true;

    private Vector3 initialPlayerPos;
    private Vector3 initialDummyPos;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSr;
    private DummyEnemy dummyEnemy;

    private void Start()
    {
        if (playerMovement == null) playerMovement = FindFirstObjectByType<PlayerMovement>();
        if (playerCombat == null) playerCombat = FindFirstObjectByType<PlayerCombat>();
        if (dummyTransform == null)
        {
            dummyEnemy = FindFirstObjectByType<DummyEnemy>();
            if (dummyEnemy != null) dummyTransform = dummyEnemy.transform;
        }

        if (playerMovement != null)
        {
            initialPlayerPos = new Vector3(-3.5f, 0, 0);
            playerMovement.transform.position = initialPlayerPos;
            playerRb = playerMovement.GetComponent<Rigidbody2D>();
            playerSr = playerMovement.GetComponentInChildren<SpriteRenderer>();
        }

        if (dummyTransform != null)
        {
            initialDummyPos = new Vector3(2.5f, 0, 0);
            dummyTransform.position = initialDummyPos;
            if (dummyEnemy == null) dummyEnemy = dummyTransform.GetComponent<DummyEnemy>();
        }

        if (autoSimulate && playerMovement != null)
        {
            StartCoroutine(EpicSimulationLoop());
        }
    }

    private IEnumerator EpicSimulationLoop()
    {
        while (true)
        {
            // === 1. RESET BAN ĐẦU ===
            playerMovement.transform.position = initialPlayerPos;
            if (dummyTransform != null) dummyTransform.position = initialDummyPos;
            if (playerRb != null) playerRb.linearVelocity = Vector2.zero;
            if (playerSr != null) playerSr.flipX = false;
            playerMovement.SetSimulatedInput(Vector2.zero);

            yield return new WaitForSeconds(1.0f);

            // === 2. CHẠY MỘT ĐOẠN DÀI TỪ TRÁI SANG PHẢI (ÁP SÁT DUMMY) ===
            float moveTime = 1.8f;
            float elapsed = 0f;
            while (elapsed < moveTime)
            {
                // Bơm trực tiếp vector di chuyển vào PlayerMovement
                playerMovement.SetSimulatedInput(new Vector2(1.0f, 0f));
                elapsed += Time.deltaTime;
                yield return null;
            }
            playerMovement.SetSimulatedInput(Vector2.zero);

            yield return new WaitForSeconds(0.2f);

            // === 3. TUNG CHIÊU TẤN CÔNG (SLASH VFX KHỔNG LỒ) ===
            if (playerCombat != null)
            {
                playerCombat.TryAttack();
            }
            yield return new WaitForSeconds(0.4f);

            // === 4. CHÉM LIÊN HOÀN PHÁT THỨ 2 ===
            if (playerCombat != null)
            {
                playerCombat.TryAttack();
            }
            yield return new WaitForSeconds(0.5f);

            // === 5. LƯỚT NÉ (DODGE) VƯỢT QUA PHÍA SAU LƯNG DUMMY ===
            playerMovement.SetSimulatedInput(new Vector2(1.0f, 0f));
            playerMovement.TryDodge();
            yield return new WaitForSeconds(0.35f);
            playerMovement.SetSimulatedInput(Vector2.zero);

            yield return new WaitForSeconds(0.4f);

            // === 6. QUAY MẶT LẠI VÀ LƯỚT NÉ LÙI RA XA ===
            playerMovement.SetSimulatedInput(new Vector2(-1.0f, 0f));
            playerMovement.TryDodge();
            yield return new WaitForSeconds(0.35f);
            playerMovement.SetSimulatedInput(Vector2.zero);

            yield return new WaitForSeconds(0.3f);

            // === 7. CHẠY LÙI TỪ TỪ VỀ LẠI VỊ TRÍ XUẤT PHÁT ===
            elapsed = 0f;
            while (elapsed < 1.4f)
            {
                playerMovement.SetSimulatedInput(new Vector2(-1.0f, 0f));
                elapsed += Time.deltaTime;
                yield return null;
            }
            playerMovement.SetSimulatedInput(Vector2.zero);

            yield return new WaitForSeconds(1.8f);
        }
    }
}
