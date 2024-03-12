using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTumbleweeds : MonoBehaviour
{

    // Function to activate all children of the GameObject
    public void StartTumbleweeds()
    {
        // Activate all children of the game object
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            float xOffset = -5f * transform.GetSiblingIndex(); // Shift x position by -5 for each subsequent child
            Vector3 newPosition = child.position + new Vector3(xOffset, 0f, 0f);
            child.position = newPosition;
        }
    }
}