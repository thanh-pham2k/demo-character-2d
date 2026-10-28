using UnityEngine;

namespace DormRoom.Prototype
{
    // Preserve composition at 3:2 while leaving unused screen area outside the camera viewport.
    [RequireComponent(typeof(Camera))]
    public sealed class Room404CameraViewport : MonoBehaviour
    {
        const float TargetAspect = 3f / 2f;
        Camera roomCamera;
        Camera clearCamera;
        int lastWidth, lastHeight;

        void Awake()
        {
            roomCamera = GetComponent<Camera>();
            var host = new GameObject("Room404ViewportClear") { hideFlags = HideFlags.DontSave };
            host.transform.SetParent(transform, false);
            clearCamera = host.AddComponent<Camera>();
            clearCamera.clearFlags = CameraClearFlags.SolidColor;
            clearCamera.backgroundColor = Color.black;
            clearCamera.cullingMask = 0;
            clearCamera.depth = roomCamera.depth - 1f;
            clearCamera.allowHDR = false;
            clearCamera.allowMSAA = false;
            Apply();
        }
        void OnDestroy()
        {
            if (clearCamera != null) Destroy(clearCamera.gameObject);
        }
        void LateUpdate()
        {
            if (Screen.width != lastWidth || Screen.height != lastHeight) Apply();
        }
        void Apply()
        {
            lastWidth = Screen.width; lastHeight = Screen.height;
            if (lastWidth <= 0 || lastHeight <= 0) return;
            float aspect = (float)lastWidth / lastHeight;
            if (aspect > TargetAspect)
            {
                float width = TargetAspect / aspect;
                roomCamera.rect = new Rect((1f - width) * .5f, 0f, width, 1f);
            }
            else
            {
                float height = aspect / TargetAspect;
                roomCamera.rect = new Rect(0f, (1f - height) * .5f, 1f, height);
            }
            roomCamera.aspect = TargetAspect;
        }
    }
}
