using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weeds : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject weeds;
    public Transform carTransform; // Reference to the car's transform.
    public Vector3 offsetFromCar; // Offset from the car's position.

    public int startSampleRate = 3236351;
    public int endSampleRate = 3938188;

    // No need for an isActive flag since GameObject's active state is used directly.
    void Start()
    {
        weeds.SetActive(false);
    }
    void Update()
    {
       

        if (audioSource.timeSamples > startSampleRate && audioSource.timeSamples < endSampleRate)
        {
            //weeds.transform.position = carTransform.position + offsetFromCar;
            weeds.SetActive(true);

        }
        else if (audioSource.timeSamples < 10000 || audioSource.timeSamples >= endSampleRate)
        {
            weeds.SetActive(false);
        }
    }
}