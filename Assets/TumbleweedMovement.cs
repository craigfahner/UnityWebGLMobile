using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleweedMovement : MonoBehaviour
{
    public float speed = 5f; // Public variable for speed, you can adjust this in the Unity Inspector
    private bool started = false; // Boolean variable to track whether movement has started

    void Start()
    {
        // Randomize the scale of the first child object proportionately
        if (transform.childCount > 0)
        {
            Transform firstChild = transform.GetChild(0);
            float randomScale = Random.Range(5f, 15f);
            firstChild.localScale = new Vector3(randomScale, randomScale, randomScale);
            speed = Random.Range(5f, 10f);
        }
    }

    void Update()
    {
        // Check if movement has started
     //   if (started)
     //   {
            // Move the object along the Z-axis (left to right)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
      //  }
    }

    // Function to start movement
    public void StartMovement()
    {
        started = true;
    }
}