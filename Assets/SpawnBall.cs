
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


[System.Serializable] // Make it visible in the inspector
public struct Spawnable
{
    public GameObject prefab; // The prefab to spawn
    public int samplePoint; // The sample point to spawn at
}

public class SpawnBall : MonoBehaviour
{

    public float generationRadius = 5f;
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


        if (currentSample < 100000) // Replace `certainNumber` with your specific audio sample threshold
        {
            //Debug.Log("Moving objects underground");
            ResetForReplay();
        }
    }

    void ResetForReplay()
    {
       // Debug.Log("Reset");
        instantiatedPoints.Clear(); // Clears the record of instantiated sample points
        foreach (GameObject obj in cubePool)
        {
            obj.SetActive(false); // Ensures all pooled objects are inactive and ready for reuse
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
            cubePool.Enqueue(instance); // Add the object to the pool when first instantiated
        }

        // Start the object below ground
        Vector3 spawnPosition = CalculateSpawnPosition();
        spawnPosition.y = -5f; // Adjust this value based on how far below ground you want objects to start
        instance.transform.position = spawnPosition;
        instance.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        instance.SetActive(true);

        // Start coroutine to move the object upwards
        StartCoroutine(MoveObject(instance.transform, spawnPosition + Vector3.up * 5f, 1f, -15f, 1.5f)); // Adjust the duration to control the speed of rising
    }

    IEnumerator MoveObject(Transform objTransform, Vector3 finalUpwardPosition, float upwardDuration, float undergroundTargetY, float undergroundDuration)
    {
        float elapsedTime = 0;
        Vector3 startingPosition = objTransform.position;
        Vector3 undergroundEndPosition = new Vector3(startingPosition.x, undergroundTargetY, startingPosition.z);

        // Move the object upwards
        while (elapsedTime < upwardDuration)
        {
            objTransform.position = Vector3.Lerp(startingPosition, finalUpwardPosition, (elapsedTime / upwardDuration));
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        objTransform.position = finalUpwardPosition; // Ensure the object is exactly at the final upward position

        // Wait for 2 seconds
        yield return new WaitForSeconds(2);

        // Move the object underground
        elapsedTime = 0; // Reset elapsed time for the underground movement
        startingPosition = objTransform.position; // Update starting position for the underground movement

        while (elapsedTime < undergroundDuration)
        {
            objTransform.position = Vector3.Lerp(startingPosition, undergroundEndPosition, (elapsedTime / undergroundDuration));
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        objTransform.position = undergroundEndPosition; // Ensure the object is exactly at the final underground position

        // Deactivate the object and enqueue it for reuse
        objTransform.gameObject.SetActive(false);
        cubePool.Enqueue(objTransform.gameObject);
    }


    Vector3 CalculateSpawnPosition()
    {
        // Increase the forward offset distance to place objects further in front of the car
        float forwardOffsetDistance = 40f; // Adjust this value as needed to position objects further ahead

        // Generate a random position within a circle on the XZ plane
        Vector2 randomCircle = Random.insideUnitCircle * generationRadius;

        // Calculate the forward offset position based on the car's current position and orientation
        Vector3 forwardOffsetPosition = transform.position + transform.forward * forwardOffsetDistance;

        // Set the spawn position with y explicitly set to 0 to ensure objects are at ground level
        return new Vector3(forwardOffsetPosition.x + randomCircle.x, 0f, forwardOffsetPosition.z + randomCircle.y);
    }
}