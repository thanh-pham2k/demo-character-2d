using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float dodgeSpeed = 12.0f;
    [SerializeField] private float dodgeDuration = 0.25f;
    [SerializeField] private float dodgeCooldown = 0.5f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 inputVector;
    private Vector2 simulatedInputVector;
    private Vector2 lastMoveDirection = Vector2.right;
    private bool isDodging;
    private float dodgeTimer;
    private float dodgeCooldownTimer;
    private bool interactionMovementLocked;

    public bool IsDodging => isDodging;
    public Vector2 LastMoveDirection => lastMoveDirection;
    public bool IsInteractionMovementLocked => interactionMovementLocked;

    private void Awake()
    {
        ResolveComponents();
    }

    private void ResolveComponents()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
        {
            var visual = GetComponentInChildren<LuonCharacterVisual>();
            spriteRenderer = visual != null ? visual.GetComponent<SpriteRenderer>() : GetComponentInChildren<SpriteRenderer>();
        }
    }

    public void SetInteractionMovementLocked(bool locked)
    {
        ResolveComponents();
        if (!locked && !interactionMovementLocked) return;
        interactionMovementLocked = locked;
        inputVector = Vector2.zero;
        simulatedInputVector = Vector2.zero;
        isDodging = false;
        dodgeTimer = 0f;
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    public void SetSimulatedInput(Vector2 simInput)
    {
        simulatedInputVector = simInput;
    }

    private void Update()
    {
        if (dodgeCooldownTimer > 0)
            dodgeCooldownTimer -= Time.deltaTime;

        if (interactionMovementLocked)
        {
            inputVector = Vector2.zero;
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }
        if (isDodging)
        {
            dodgeTimer -= Time.deltaTime;
            if (dodgeTimer <= 0)
            {
                isDodging = false;
            }
            return;
        }

        // Lấy Input từ người chơi (Keyboard & Joystick)
        float h = 0f;
        float v = 0f;

        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) h -= 1f;
            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) h += 1f;
            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) v -= 1f;
            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) v += 1f;
        }

        if (VirtualJoystick.Instance != null && VirtualJoystick.Instance.InputVector != Vector2.zero)
        {
            h = VirtualJoystick.Instance.InputVector.x;
            v = VirtualJoystick.Instance.InputVector.y;
        }

        // Nếu người chơi không bấm phím thì nhận Simulated Input từ kịch bản Demo
        if (h == 0 && v == 0 && simulatedInputVector != Vector2.zero)
        {
            h = simulatedInputVector.x;
            v = simulatedInputVector.y;
        }

        inputVector = new Vector2(h, v);
        if (inputVector.sqrMagnitude > 1f)
        {
            inputVector.Normalize();
        }

        if (inputVector != Vector2.zero)
        {
            lastMoveDirection = inputVector;
            if (spriteRenderer != null)
            {
                if (inputVector.x > 0.05f)
                    spriteRenderer.flipX = false;
                else if (inputVector.x < -0.05f)
                    spriteRenderer.flipX = true;
            }
        }

        if (keyboard != null && (keyboard.spaceKey.wasPressedThisFrame || keyboard.leftShiftKey.wasPressedThisFrame))
        {
            TryDodge();
        }
    }

    private void FixedUpdate()
    {
        if (rb == null) return;
        if (interactionMovementLocked)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        if (isDodging)
        {
            rb.linearVelocity = lastMoveDirection * dodgeSpeed;
            return;
        }

        var combat = GetComponent<PlayerCombat>();
        if (combat != null && combat.IsAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = inputVector * moveSpeed;
    }

    public void TryDodge()
    {
        if (!isActiveAndEnabled || interactionMovementLocked || isDodging || dodgeCooldownTimer > 0) return;

        isDodging = true;
        dodgeTimer = dodgeDuration;
        dodgeCooldownTimer = dodgeCooldown;
    }
}
