using UnityEngine;
using System.Collections.Generic;

public class Cube : MonoBehaviour
{
    public GameObject cubePrefab;
    public float generationRadius = 5f;
    public int maxCubes = 20; // Maximum number of cubes allowed
    public int sampleOffset = 3500;

    private AudioSource audioSource;
    public List<int> samplePoints; // List of sample points for cube generation
    private HashSet<int> instantiatedPoints; // To keep track of instantiated points
    private int currentIndex = 0;

    private Queue<GameObject> cubeQueue = new Queue<GameObject>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        instantiatedPoints = new HashSet<int>();

        Debug.Log("Sample points: " + string.Join(", ", samplePoints));
    }


    void Update()
    {
        if (audioSource.isPlaying)
            Debug.Log("Audio is playing. Current sample: " + audioSource.timeSamples);
        else
            Debug.Log("Audio is not playing.");

        int currentSample = audioSource.timeSamples;
        int tolerance = 600; // Example tolerance value, adjust as needed

        //foreach (int samplePoint in samplePoints)
        //{
        //    if (!instantiatedPoints.Contains(samplePoint) &&
        //        currentSample >= samplePoint)
        //    {
        //        GenerateCubesAroundCar();
        //        instantiatedPoints.Add(samplePoint);
        //        break; // Break after instantiating to avoid multiple instantiations per frame
        //    }
        //}


        for (int i = 0; i<samplePoints.Count; i++)
        {
            if (!instantiatedPoints.Contains(samplePoints[i]) &&
                 currentSample >= samplePoints[i])
            {
                GenerateCubesAroundCar();
                instantiatedPoints.Add(samplePoints[i]);
                currentIndex = i+1;
                break; // Break after instantiating to avoid multiple instantiations per frame
            }
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    samplePoints.Add(currentSample);
        //}

    }

    void GenerateCubesAroundCar()
    {
        Debug.Log("Generating cube around car. Sample is " + audioSource.timeSamples + " at index " + currentIndex);
        // Check if the maximum number of cubes is reached
        if (cubeQueue.Count >= maxCubes)
        {
            GameObject oldCube = cubeQueue.Dequeue();
            Destroy(oldCube);
        }

        // Generate a random offset in a circle perpendicular to the car's forward direction
        Vector2 randomCircle = Random.insideUnitCircle * generationRadius;
        float fixedHeight = 1f; // Fixed height for the cubes
        Vector3 randomOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);

        // Position in front of the car with an offset
        Vector3 forwardOffsetPosition = transform.position + transform.forward * 20f; // 10 units in front of the car

        // Apply the random offset relative to the car's orientation
        Vector3 spawnPosition = forwardOffsetPosition + transform.TransformDirection(randomOffset);

        // Generate a random rotation around the y-axis
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        // Instantiate the cube with random rotation on the y-axis
        GameObject newCube = Instantiate(cubePrefab, spawnPosition, randomRotation);

        // Add the new cube to the queue
        cubeQueue.Enqueue(newCube);
    }
}
