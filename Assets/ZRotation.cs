using UnityEngine;

public class ZRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // Speed of rotation around the Z axis

    void Update()
    {
        // Rotate the GameObject around the Z axis
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}