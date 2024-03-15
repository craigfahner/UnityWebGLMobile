using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed

    void Update()
    {
        // Move the dice from left to right by modifying its X position
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
