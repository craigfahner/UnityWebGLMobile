using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public Transform targetObject; // Reference to the other game object
    public float distance = 10f;

    void Update()
    {
        // Move the dice from left to right by modifying its X position
        //transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (targetObject != null)
        {
            Vector3 forwardDirection = targetObject.forward * distance;

            // Calculate the left direction of the target object
            Vector3 leftDirection = -targetObject.right * distance;

            // Calculate the target position
            Vector3 targetPosition = targetObject.position + forwardDirection + leftDirection;

            // Calculate the direction vector from the Dice object's position to the target position
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

}
