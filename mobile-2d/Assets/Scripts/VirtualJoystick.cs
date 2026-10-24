using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static VirtualJoystick Instance { get; private set; }

    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float handleRange = 60f;

    public Vector2 InputVector { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (background == null) background = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / background.sizeDelta.x);
            pos.y = (pos.y / background.sizeDelta.y);

            Vector2 rawInput = new Vector2(pos.x * 2, pos.y * 2);
            InputVector = (rawInput.magnitude > 1.0f) ? rawInput.normalized : rawInput;

            if (handle != null)
            {
                handle.anchoredPosition = new Vector2(InputVector.x * (background.sizeDelta.x / 2), InputVector.y * (background.sizeDelta.y / 2));
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputVector = Vector2.zero;
        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }
    }
}
