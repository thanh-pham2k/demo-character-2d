using UnityEngine;

public class RotatingFan : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360f; // degrees per second
    public bool isSpinning = true;

    private void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }
}
