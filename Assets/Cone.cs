using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{

    public Transform carTransform; // Assign the car's transform in the inspector.
    public Vector3 offsetFromHeadlight; // Use this to adjust the cone's position relative to the car's headlight.
    public Vector3 rotationOffset; // Rotation offset for the headlight cone.

    void Update()
    {
        // Position the cone at the headlight's position, plus some offset if necessary.
        transform.position = carTransform.position + carTransform.TransformDirection(offsetFromHeadlight);

        // Make the cone face the same direction as the car, with an added rotation offset.
        transform.rotation = carTransform.rotation * Quaternion.Euler(rotationOffset);
    }
}
