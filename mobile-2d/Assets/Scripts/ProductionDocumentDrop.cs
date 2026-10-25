using UnityEngine;
using UnityEngine.EventSystems;
public sealed class ProductionDocumentDrop : MonoBehaviour,IDropHandler
{
    public ProductionOpeningController controller;
    public void OnDrop(PointerEventData data)
    {
        var card=data.pointerDrag!=null?data.pointerDrag.GetComponent<ProductionDocumentCard>():null;
        if(card!=null)controller.PresentDocument(card.item);
    }
}
