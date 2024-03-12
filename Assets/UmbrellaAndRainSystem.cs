using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Make it visible in the inspector
public struct UmbrellaEvent
{
    public string trigger; // The trigger to run
    public int samplePoint; // The sample point to spawn at
}

public class UmbrellaAndRainSystem : MonoBehaviour
{
    private AudioSource audioSource;
    public int sampleOffset = 3500;
    public GameObject umbrella;
    public List<UmbrellaEvent> umbrellaEvents;
    private HashSet<int> instantiatedPoints = new HashSet<int>(); // To keep track of instantiated points
    public Animator umbrellaAnimator;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        umbrella.SetActive(false);
        umbrellaAnimator = umbrella.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        int currentSample = audioSource.timeSamples;
        foreach (var item in umbrellaEvents)
        {
            if (!instantiatedPoints.Contains(item.samplePoint) && currentSample >= item.samplePoint)
            {
                umbrella.SetActive(true);
                umbrellaAnimator.SetTrigger(item.trigger);
                instantiatedPoints.Add(item.samplePoint);
                break; // Break after instantiating to avoid multiple instantiations per frame
            }
        }

        if (currentSample > 2870000) // Replace `certainNumber` with your specific audio sample threshold
        {
            //Debug.Log("Moving objects underground");

            umbrella.SetActive(false);
        }
    }
}
