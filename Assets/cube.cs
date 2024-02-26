using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube : MonoBehaviour
{
    public GameObject cubePrefab;
    public float generationRadius = 5f;
    public int maxCubes = 20; // Maximum number of cubes allowed
    public int sampleOffset = 3500;

    private AudioSource audioSource;
    public List<int> samplePoints; // List of sample points for cube generation
    private HashSet<int> instantiatedPoints = new HashSet<int>(); // To keep track of instantiated points

    private Queue<GameObject> cubePool = new Queue<GameObject>(); // Pool of cubes

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Sample points: " + string.Join(", ", samplePoints));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !audioSource.isPlaying)
        {
            audioSource.Play(); // Start playing the audio
        }

        int currentSample = audioSource.timeSamples;

        foreach (int samplePoint in samplePoints)
        {
            if (!instantiatedPoints.Contains(samplePoint) && currentSample >= samplePoint)
            {
                GenerateCubesAroundCar();
                instantiatedPoints.Add(samplePoint);
                break; // Break after instantiating to avoid multiple instantiations per frame
            }
        }
    }

    void GenerateCubesAroundCar()
    {
        Debug.Log("Generating cube. Sample is " + audioSource.timeSamples);

        GameObject cube;
        if (cubePool.Count > 0 && !cubePool.Peek().activeInHierarchy)
        {
            cube = cubePool.Dequeue();
            cube.transform.position = CalculateSpawnPosition();
            cube.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
            cube.SetActive(true);
        }
        else
        {
            cube = Instantiate(cubePrefab, CalculateSpawnPosition(), Quaternion.Euler(0, Random.Range(0f, 360f), 0));
        }

        StartCoroutine(SetInactiveAfterDelay(cube, 6f)); // Set cube to inactive after 6 seconds
        cubePool.Enqueue(cube); // Add the cube back to the pool
    }

    Vector3 CalculateSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * generationRadius;
        Vector3 forwardOffsetPosition = transform.position + transform.forward * 20f;
        return forwardOffsetPosition + transform.TransformDirection(new Vector3(randomCircle.x, 0f, randomCircle.y));
    }

    IEnumerator SetInactiveAfterDelay(GameObject cube, float delay)
    {
        yield return new WaitForSeconds(delay);
        cube.SetActive(false);
    }
}
