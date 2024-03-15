using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UmbrellaCarFollower : MonoBehaviour
{
    public GameObject car; // Reference to the car GameObject
    public Animator umbAnim;
    public bool openSesame = false;

    void Update()
    {
            // Get the position of the car
            Vector3 carPosition = car.transform.position;

            // Set the GameObject's transform position to match the car's position
            transform.position = carPosition;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Set a trigger parameter named "NextState" to transition to the next animation state
            umbAnim.SetTrigger("NextState");
            umbAnim.speed = 0f;
        }
    }
}