using UnityEngine;
using UnityEngine.EventSystems;
public sealed class ProductionDocumentCard : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public string item;
    private Vector3 origin;
    private CanvasGroup group;
    public void OnBeginDrag(PointerEventData data){origin=transform.position;group=GetComponent<CanvasGroup>();group.blocksRaycasts=false;}
    public void OnDrag(PointerEventData data){transform.position=data.position;}
    public void OnEndDrag(PointerEventData data){transform.position=origin;group.blocksRaycasts=true;}
}
