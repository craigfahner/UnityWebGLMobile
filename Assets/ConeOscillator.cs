using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeOscillator : MonoBehaviour
{
    public float minScale = 0.5f; // Minimum scale value
    public float maxScale = 1.5f; // Maximum scale value
    public float oscillationSpeed = 1.0f; // Oscillation speed

    private float scaleMultiplier = 1.0f;

    void Start()
    {
        // Ensure the scale is set to the default size at the start
        transform.localScale = new Vector3(maxScale, maxScale, transform.localScale.z);
    }

    void Update()
    {
        // Calculate the scale multiplier using sinusoidal motion
        scaleMultiplier = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * oscillationSpeed) + 1.0f) / 2.0f);

        // Apply the scale, only changing X and Y axes
        transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, transform.localScale.z);
    }
}
