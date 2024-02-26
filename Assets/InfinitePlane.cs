using UnityEngine;
using System.Collections.Generic;

public class InfiniteCarMover : MonoBehaviour
{
    public GameObject car; // Assign your car GameObject in the Inspector
    public List<GameObject> planes; // Assign all plane GameObjects in the scene
    public float resetThreshold = 1000f; // Distance from origin at which to reset the car position
    public float planeSize = 1000f; // Size of your planes, adjust according to your actual plane size

    private Vector3 startPosition;
    private Queue<GameObject> planesToReset = new Queue<GameObject>();

    void Start()
    {
        startPosition = car.transform.position;
    }

    void Update()
    {
        // Check the distance from the start position
        if (Vector3.Distance(car.transform.position, startPosition) > resetThreshold)
        {
            ResetCarPosition();
            ResetPassedPlanes();
        }

        CheckPassedPlanes();
    }

    void ResetCarPosition()
    {
        // Reset the car to the start position
        car.transform.position = startPosition;

        // If the car has a Rigidbody and you wish to maintain its momentum, adjust accordingly
        Rigidbody carRigidbody = car.GetComponent<Rigidbody>();
        if (carRigidbody != null)
        {
            Vector3 currentVelocity = carRigidbody.velocity;
            carRigidbody.position = startPosition; // Use Rigidbody.position for physics consistency
            carRigidbody.velocity = currentVelocity;
        }
    }

    void CheckPassedPlanes()
    {
        foreach (var plane in planes)
        {
            float distanceToPlane = Vector3.Distance(car.transform.position, plane.transform.position);
            // If the car has moved beyond the plane (considering planeSize/2 as the boundary),
            // and the plane is not already in the queue to be reset
            if (distanceToPlane > planeSize / 2 && !planesToReset.Contains(plane))
            {
                planesToReset.Enqueue(plane);
            }
        }
    }

    void ResetPassedPlanes()
    {
        while (planesToReset.Count > 0)
        {
            GameObject plane = planesToReset.Dequeue();
            // Assuming your planes have a script named GPTTest for resetting the mesh
            GPTtest planeScript = plane.GetComponent<GPTtest>();
            if (planeScript != null)
            {
                planeScript.ResetMesh();
            }
        }
    }
}
