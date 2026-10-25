using UnityEngine;
using UnityEngine.EventSystems;
public sealed class ProductionHoldButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler
{
    public bool Held {get;set;}
    public void OnPointerDown(PointerEventData data){Held=true;}
    public void OnPointerUp(PointerEventData data){Held=false;}
    public void OnPointerExit(PointerEventData data){Held=false;}
    private void OnDisable(){Held=false;}
}
