using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public GameObject tapToStartOverlay; // Assign this in the inspector
    private bool overlayActive = true; // Track if the overlay is currently active

    // This method is called to hide the overlay
    public void HideOverlay()
    {
        tapToStartOverlay.SetActive(false);
        overlayActive = false; // Update the flag when the overlay is hidden
        // Add any additional actions to start the game here
    }

    void Update()
    {
        // Check if the overlay is active and the user has clicked
        if (overlayActive && Input.GetMouseButtonDown(0))
        {
            HideOverlay();
        }
    }
}
