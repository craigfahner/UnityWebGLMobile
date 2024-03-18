using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardRotate : MonoBehaviour
{
    public Transform car; // Reference to the target GameObject
    public float rotationSpeed = 0.1f;

    void Update()
    {
        if (car != null)
        {
            // Get the direction from the current position to the target position
            Vector3 direction = car.position - transform.position;
            direction.y = 0; // Restrict rotation to only the Y-axis

            // Rotate towards the target
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
