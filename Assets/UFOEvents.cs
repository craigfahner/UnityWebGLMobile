using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UFOEvents : MonoBehaviour
{
    public GameObject UFO1; // Reference to the first UFO GameObject
    public GameObject UFO2; // Reference to the second UFO GameObject

    [System.Serializable]
    public class UFOSettings // Settings for each UFO
    {
        public float startDistanceFromCar = 10f; // Initial distance from car for the UFO
        public float finalDistanceFromCar = 1.5f; // Final distance from car for the UFO
        public float duration = 5f; // Duration of interpolation in seconds
        public float flyAwayDuration = 5f;
        public int appearPoint = 10000;
        public int blinkPoint = 10000;
        public int disappearPoint = 10000;
        public GameObject child;
    }

    public UFOSettings UFO1Settings; // Settings for UFO1
    public UFOSettings UFO2Settings; // Settings for UFO2

    private CarFollower UFO1Follower; // Reference to the CarFollower script on the first UFO
    private CarFollower UFO2Follower; // Reference to the CarFollower script on the second UFO
    private UFOColorChange UFO1Color;
    private UFOColorChange UFO2Color;

    private AudioSource audioSource;


    void Start()
    {
        // Get the CarFollower script component from each UFO GameObject
        UFO1Follower = UFO1.GetComponent<CarFollower>();
        UFO2Follower = UFO2.GetComponent<CarFollower>();
        UFO1Color = UFO1Settings.child.GetComponent<UFOColorChange>();
        UFO2Color = UFO2Settings.child.GetComponent<UFOColorChange>();
        audioSource = GetComponent<AudioSource>();

        // Start interpolating the distanceFromCar variable for each UFO
        //StartCoroutine(InterpolateDistance(UFO1Follower, UFO1Settings));
        //StartCoroutine(InterpolateDistance(UFO2Follower, UFO2Settings));
    }

    void Update()
    {
        int currentSample = audioSource.timeSamples;

        if(currentSample >= UFO1Settings.appearPoint && UFO1Settings.child.active == false && currentSample < UFO1Settings.disappearPoint)
        {
            UFO1Settings.child.SetActive(true);
            StartCoroutine(InterpolateDistance(UFO1Follower, UFO1Settings));
        }

        if (currentSample >= UFO2Settings.appearPoint && UFO2Settings.child.active == false && currentSample < UFO2Settings.disappearPoint)
        {
            UFO2Settings.child.SetActive(true);
            StartCoroutine(InterpolateDistance(UFO2Follower, UFO2Settings));
        }

        if (currentSample >= UFO1Settings.blinkPoint && UFO1Color.oscillationEnabled == false)
        {
            UFO1Color.oscillationEnabled = true;
        }
        if (currentSample >= UFO2Settings.blinkPoint && UFO2Color.oscillationEnabled == false)
        {
            UFO2Color.oscillationEnabled = true;
        }

        if(currentSample >= UFO1Settings.disappearPoint && UFO1Follower.distanceFromCar < 5f)
        {
            StartCoroutine(FlyAway(UFO1Follower, UFO1Settings));
            StartCoroutine(FlyAway(UFO2Follower, UFO2Settings));
        }

    }

    IEnumerator InterpolateDistance(CarFollower follower, UFOSettings settings)
    {
        float elapsedTime = 0f;
        float startDistanceFromCar = settings.startDistanceFromCar;

        // Interpolate the distanceFromCar variable over the specified duration
        while (elapsedTime < settings.duration)
        {
            float t = elapsedTime / settings.duration;
            follower.distanceFromCar = Mathf.Lerp(startDistanceFromCar, settings.finalDistanceFromCar, t);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // Ensure the final distance is set
        follower.distanceFromCar = settings.finalDistanceFromCar;
    }

    IEnumerator FlyAway(CarFollower follower, UFOSettings settings)
    {
        float elapsedTime = 0f;
        float startDistanceFromCar = settings.startDistanceFromCar;

        // Interpolate the distanceFromCar variable over the specified duration
        while (elapsedTime < settings.flyAwayDuration)
        {
            float t = elapsedTime / settings.flyAwayDuration;
            follower.distanceFromCar = Mathf.Lerp(settings.finalDistanceFromCar, startDistanceFromCar, t);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // Ensure the final distance is set
        follower.distanceFromCar = settings.startDistanceFromCar;
        settings.child.SetActive(false);
    }
}