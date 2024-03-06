using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarFollower : MonoBehaviour
{
    public GameObject car; // Reference to the car GameObject
    public float orbitSpeed = 20f; // Speed of orbiting
    public float hoverHeight = 5f; // Height at which UFO hovers above the car
    public float maxRotationOffset = 30f; // Maximum rotation offset from the car
    public float noiseMagnitude = 1f; // Magnitude of noise for drifting
    public Vector3 positionOffset; // Random offset of initial position
    public float distanceFromCar = 10f; // Distance that the UFO is from the car

    private Vector3 initialPosition; // Initial position of the UFO relative to the car
    private Vector3 noiseOffset; // Noise offset for drifting behavior

    void Start()
    {
        // Store the initial position relative to the car with random offset
        initialPosition = transform.position - car.transform.position + positionOffset;

        // Initialize noise offset with random seed
        noiseOffset = new Vector3(
            Random.Range(-1000f, 1000f),
            Random.Range(-1000f, 1000f),
            Random.Range(-1000f, 1000f)
        );
    }

    void Update()
    {
        // Calculate the target position for the UFO to orbit around
        Vector3 targetPosition = car.transform.position + initialPosition;

        // Calculate rotation towards the car
        Quaternion targetRotation = Quaternion.LookRotation(car.transform.position - transform.position, Vector3.up);

        // Apply rotation and orbit
        transform.RotateAround(targetPosition, Vector3.up, orbitSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * orbitSpeed); // Always point Z axis towards car

        // Add drifting behavior
        Vector3 driftOffset = new Vector3(
            Mathf.PerlinNoise(Time.time * noiseMagnitude + noiseOffset.x, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * noiseMagnitude + noiseOffset.y) - 0.5f,
            Mathf.PerlinNoise(Time.time * noiseMagnitude + noiseOffset.z, Time.time * noiseMagnitude + noiseOffset.z) - 0.5f
        ) * noiseMagnitude;

        // Set position behind the car with drift and distance control
        transform.position = targetPosition + transform.forward * (-distanceFromCar + driftOffset.magnitude);
        transform.position += Vector3.up * Mathf.Sin(Time.time) * hoverHeight; // Optional hover effect
    }
}