using UnityEngine;
using System.Collections.Generic;

[System.Serializable] // Make it visible in the inspector
public struct SpawnableItemm
{
    public GameObject prefab; // The prefab to spawn
    public int samplePoint; // The sample point to spawn at
}

public class DiceSpawner : MonoBehaviour
{
    public Transform carTransform;
    public float generationRadius = 5f; // Not used if spawning at a fixed point
    public int sampleOffset = 3500;

    private AudioSource audioSource;
    public List<SpawnableItem> spawnableItems;
    private HashSet<int> instantiatedPoints = new HashSet<int>(); // To keep track of instantiated points

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        int currentSample = audioSource.timeSamples;

        foreach (var item in spawnableItems)
        {
            if (!instantiatedPoints.Contains(item.samplePoint) && currentSample >= item.samplePoint)
            {
                GenerateObject(item.prefab);
                instantiatedPoints.Add(item.samplePoint);
                // Assuming one spawn per sample point, remove break if multiple spawns per frame are okay
                break;
            }
        }

        if (currentSample < 100000) // Example condition for resetting, adjust as needed
        {
            ResetForReplay();
        }
    }

    void GenerateObject(GameObject prefab)
    {
        Vector3 spawnPosition = CalculateSpawnPosition();
        // Instantiate the prefab at the calculated spawn position with no rotation
        GameObject spawnedObject = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Destroy the instantiated object after 2 seconds
        Destroy(spawnedObject, 3f);
    }

    void ResetForReplay()
    {
        Debug.Log("Reset");
        instantiatedPoints.Clear(); // Clears the record of instantiated sample points
    }

    Vector3 CalculateSpawnPosition()
    {
        Vector3 forwardOffset = carTransform.forward * 100; // Moves 100 units forward from the car's current position

        // Calculate left offset based on the car's current orientation
        // Note: Multiplying by -1 to move to the left as Transform.right points to the right
        Vector3 leftOffset = carTransform.right * 40; // Moves 40 units to the left of the car's current position

        // Combine the offsets with the car's current position to get the spawn position
        Vector3 spawnPosition = carTransform.position + forwardOffset + leftOffset;

        // Set the spawn position's y component to 6.4
        spawnPosition.y = 6.4f; // Adjusts the height at which the dice spawns

        return spawnPosition;
    }
}
