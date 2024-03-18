using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


public class EndGame : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GameOver();
    public AudioSource audioSource; // Assign your AudioSource in the inspector
    public GameObject tapToReplayUI; // Assign your Tap to Replay UI GameObject in the inspector
    public GameObject panel;
    public GameObject car;
    public GameObject weeds;
    public bool sentMessage = false;
    public bool ended = false;


    private void Start()
    {
        // Ensure the UI is not visible at the start
        tapToReplayUI.SetActive(false);
    }

    private void Update()
    {
        // Check if the audio sample has reached 7500000
        if (audioSource.timeSamples >= 7550000)
        {
            // Pause the audio and show the UI
            audioSource.Pause();
            panel.SetActive(true);
            if (sentMessage == false)
            {
                ended = true;
                GameOver();
                sentMessage = true;
                Invoke("QuitApplication", 2f);
            }

            // this is where the CSS popup thing would happen
        }

        // Check for user input if the UI is active
        //if (tapToReplayUI.activeSelf && Input.GetMouseButtonDown(0))
        //{
        //    // Hide the UI
        //    tapToReplayUI.SetActive(false);

        //    // Reset the audio sample to 0 and play the audio again
        //    audioSource.timeSamples = 0;
        //    audioSource.Play();

        //    car.transform.position = new Vector3 (-282f, 1.5f, -587f);
        //    car.transform.rotation = Quaternion.identity;

        //    Rigidbody carRigidbody = car.GetComponent<Rigidbody>();
        //    if (carRigidbody != null)
        //    {
        //        carRigidbody.velocity = Vector3.zero; // Reset linear velocity
        //        carRigidbody.angularVelocity = Vector3.zero; // Reset angular velocity
        //    }

        //    weeds.transform.position = new Vector3(-304f, 1.5f, -399f);
        //    weeds.transform.rotation = Quaternion.identity;
        //}
    }

    void QuitApplication()
    {
        Application.Quit();
    }
}
