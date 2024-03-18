using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDistance : MonoBehaviour
{
    public Transform carTransform;
    public AudioSource audioSource;
    public float distanceAhead = 80f;
    public float directionSmoothTime = 0.5f;
    public int sampleRateThreshold = 1000000;
    public float descentDepth = -5f;
    public float descentSpeed = 8f;
    public GameObject phoneBooth;
    public GameObject cones;

    private Vector3 previousCarPosition;
    private Vector3 velocity = Vector3.zero;
    private bool isDescending = false;
    private bool descentCompleted = false;

    public GameObject ground; // Assign the ground GameObject here.
    public Material blackMaterial; // Assign the black material here.
    public Material originalMaterial;

    private int startSampleRate = 3236351;
    private int endSampleRate = 3938188;

    private float currentDistance;
    public float startDistance = 100f; // Starting distance ahead of the car for the phone booth.
    public float endDistance = 80f;

    void Start()
    {
        previousCarPosition = carTransform.position;
        phoneBooth.SetActive(false);
        cones.SetActive(false);
    }

    void Update()
    {
        if (audioSource.timeSamples < startSampleRate) // This checks if the audio has looped back or restarted.
        {
            // Reset flags and phone booth position for a new cycle.
            descentCompleted = false;
            isDescending = false; // Ready to start descending again in a new audio playthrough.
                                  // Optionally reset the phone booth's position to above ground, ready for descent.
        }
        // Activation logic based on sample rate, ensuring phone booth is correctly positioned before being shown.
        if (audioSource.timeSamples > 3236351 && audioSource.timeSamples < 3938188 && !descentCompleted)
        {
            cones.SetActive(true);
            ground.GetComponent<Renderer>().material = blackMaterial;
            // Ensure the phone booth is at the correct y position before activating
            if (!phoneBooth.activeSelf)
            {
                phoneBooth.transform.position = new Vector3(phoneBooth.transform.position.x, 9, phoneBooth.transform.position.z);
            }
            phoneBooth.SetActive(true);
            float progress = (audioSource.timeSamples - startSampleRate) / (float)(endSampleRate - startSampleRate);
            currentDistance = Mathf.Lerp(startDistance, endDistance, progress);
        }
        else if (audioSource.timeSamples < 10000 || audioSource.timeSamples >= 3938188)
        {
            phoneBooth.SetActive(false);
            cones.SetActive(false);
            ground.GetComponent<Renderer>().material = originalMaterial;
        }

        // Only proceed with the following logic if the phone booth is active
        if (phoneBooth.activeSelf)
        {
            float distanceMoved = Vector3.Distance(carTransform.position, previousCarPosition);
            //Vector3 targetDirectPosition = carTransform.position + carTransform.forward * distanceAhead;
            Vector3 targetDirectPosition = carTransform.position + carTransform.forward * currentDistance;
            targetDirectPosition.y = phoneBooth.transform.position.y; // Maintain current y position unless descending

            if (audioSource.timeSamples > sampleRateThreshold)
            {
                isDescending = true;
            }

            if (isDescending)
            {
                float newYPosition = Mathf.MoveTowards(phoneBooth.transform.position.y, descentDepth, descentSpeed * Time.deltaTime);
                phoneBooth.transform.position = new Vector3(phoneBooth.transform.position.x, newYPosition, phoneBooth.transform.position.z);

                // Check if the descent has reached the target depth
                if (phoneBooth.transform.position.y <= descentDepth)
                {
                    isDescending = false;
                    descentCompleted = true; // Mark the descent as completed to prevent re-descent
                                             // Reset y position for next activation, but keep the phone booth deactivated
                    phoneBooth.SetActive(false);
                   // phoneBooth.transform.position = new Vector3(phoneBooth.transform.position.x, 9, phoneBooth.transform.position.z);
                    
                }
            }
            else if (!isDescending)
            {
                // Optional: Condition to reset descent logic if needed, e.g., on a specific event or sample rate condition
                // This is where you might listen for a game event or check if the audio sample is below a threshold to reset.
                if (audioSource.timeSamples < 10000) // Example condition to reset the descent logic
                {
                    //descentCompleted = false;
                    phoneBooth.transform.position = new Vector3(phoneBooth.transform.position.x, 9, phoneBooth.transform.position.z);
                }
            }

            if (distanceMoved < 10f)
            {
                targetDirectPosition = Vector3.SmoothDamp(phoneBooth.transform.position, targetDirectPosition, ref velocity, directionSmoothTime);
            }

            // Update the position based on smooth damp or directly, but keep y at 9 unless descending
            phoneBooth.transform.position = new Vector3(targetDirectPosition.x, phoneBooth.transform.position.y, targetDirectPosition.z);
            Vector3 direction = carTransform.position - transform.position;
            direction.y = 0; // Restrict rotation to only the Y-axis

            // Rotate towards the target
            if (direction != Vector3.zero)
            {
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.9f);
            }

            previousCarPosition = carTransform.position; // Update for the next frame
        }
    }

}

