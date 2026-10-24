using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [Header("Target & Follow")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 8.0f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Mobile Portrait Framing")]
    [Tooltip("Orthographic size 7 to 8 gives roughly 7-10% character height ratio on 1080x1920 portrait screen.")]
    [SerializeField] private float orthographicSize = 7.5f;

    private Camera cam;
    private Vector3 shakeOffset;
    private float shakeTimer;
    private float shakeIntensity;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        cam = GetComponent<Camera>();
        if (cam != null && cam.orthographic)
        {
            cam.orthographicSize = orthographicSize;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Smooth follow
        Vector3 targetPos = target.position + offset;
        Vector3 desiredPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        // Shake offset
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            shakeOffset = Random.insideUnitSphere * shakeIntensity;
            shakeOffset.z = 0;
        }
        else
        {
            shakeOffset = Vector3.zero;
        }

        transform.position = desiredPos + shakeOffset;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void Shake(float intensity, float duration)
    {
        shakeIntensity = intensity;
        shakeTimer = duration;
    }
}
