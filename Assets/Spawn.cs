using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[System.Serializable] // Make it visible in the inspector
public struct SpawnableItem
{
    public GameObject prefab; // The prefab to spawn
    public int samplePoint; // The sample point to spawn at
}

public class Spawn: MonoBehaviour
{
    public float generationRadius = 5f;
    //public int maxCubes = 20; // Maximum number of cubes allowed
    public int sampleOffset = 3500;

    private AudioSource audioSource;
    public List<SpawnableItem> spawnableItems;
    private HashSet<int> instantiatedPoints = new HashSet<int>(); // To keep track of instantiated points

    private Queue<GameObject> cubePool = new Queue<GameObject>(); // Pool of cubes

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Sample points: " + string.Join(", ", spawnableItems.Select(item => item.samplePoint.ToString()).ToArray()));

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !audioSource.isPlaying)
        {
            audioSource.Play(); // Start playing the audio
        }

        int currentSample = audioSource.timeSamples;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Sample point: " + currentSample);
        }


        foreach (var item in spawnableItems)
        {
            if (!instantiatedPoints.Contains(item.samplePoint) && currentSample >= item.samplePoint)
            {
                GenerateObject(item.prefab);
                instantiatedPoints.Add(item.samplePoint);
                break; // Break after instantiating to avoid multiple instantiations per frame
            }
        }
    }

    void GenerateObject(GameObject prefab)
    {
        Debug.Log("Generating object. Sample is " + audioSource.timeSamples);

        GameObject instance;
        if (cubePool.Count > 0 && !cubePool.Peek().activeInHierarchy)
        {
            instance = cubePool.Dequeue();
        }
        else
        {
            instance = Instantiate(prefab);
        }

        instance.transform.position = CalculateSpawnPosition();
        instance.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        instance.SetActive(true);

        StartCoroutine(SetInactiveAfterDelay(instance, 6f)); // Set object to inactive after 6 seconds
        cubePool.Enqueue(instance); // Add the object back to the pool
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
