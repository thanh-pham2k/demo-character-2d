using UnityEngine;

[DisallowMultipleComponent]
public sealed class PropReservation : MonoBehaviour
{
    private Object owner;
    public Object Owner=>owner!=null?owner:null;
    public bool IsAvailableFor(Object actor)=>actor!=null && (Owner==null || Owner==actor);
    public bool TryReserve(Object actor){if(!IsAvailableFor(actor))return false;owner=actor;return true;}
    public void Release(Object actor){if(owner==actor)owner=null;}
    public static PropReservation For(GameObject prop)
    {var reservation=prop.GetComponent<PropReservation>();return reservation!=null?reservation:prop.AddComponent<PropReservation>();}
    private void OnDisable(){owner=null;}
}
