using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public AudioSource audioSource; // Assign your AudioSource in the inspector
    public GameObject tapToReplayUI; // Assign your Tap to Replay UI GameObject in the inspector

    private void Start()
    {
        // Ensure the UI is not visible at the start
        tapToReplayUI.SetActive(false);
    }

    private void Update()
    {
        // Check if the audio sample has reached 7500000
        if (audioSource.timeSamples >= 7500000)
        {
            // Pause the audio and show the UI
            audioSource.Pause();
            tapToReplayUI.SetActive(true);
        }

        // Check for user input if the UI is active
        if (tapToReplayUI.activeSelf && Input.GetMouseButtonDown(0))
        {
            // Hide the UI
            tapToReplayUI.SetActive(false);

            // Reset the audio sample to 0 and play the audio again
            audioSource.timeSamples = 0;
            audioSource.Play();
        }
    }
}
