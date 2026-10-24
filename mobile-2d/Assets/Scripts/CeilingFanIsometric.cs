using UnityEngine;

public class CeilingFanIsometric : MonoBehaviour
{
    [SerializeField] private Transform bladesTransform;
    [SerializeField] private float spinSpeed = 720f; // degrees/sec

    public void SetBlades(Transform blades)
    {
        bladesTransform = blades;
    }

    private void Update()
    {
        if (bladesTransform != null)
        {
            // Spin around its tilted local axis to preserve isometric oval perspective
            bladesTransform.Rotate(0, 0, -spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}
