using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinitePlaneGenerator : MonoBehaviour
{
    public GameObject planePrefab;
    public List<GameObject> prefabTypes; // List of prefabs to spawn
    public int renderDistance = 5;
    public float planeSize = 10f;
    public int maxPrefabsPerPlane = 50; // Maximum number of prefabs per plane

    private Vector3 lastPlayerPosition;
    private Dictionary<Vector3, GameObject> activePlanes = new Dictionary<Vector3, GameObject>();
    private Dictionary<GameObject, List<GameObject>> planeToPrefabsMap = new Dictionary<GameObject, List<GameObject>>();

    void Start()
    {
        lastPlayerPosition = transform.position;
        UpdatePlanes();
    }

    void Update()
    {
        if (Vector3.Distance(lastPlayerPosition, transform.position) > planeSize / 2)
        {
            lastPlayerPosition = transform.position;
            UpdatePlanes();
        }
    }

    void UpdatePlanes()
    {
        StartCoroutine(GeneratePlanes());
    }

    IEnumerator GeneratePlanes()
    {
        HashSet<Vector3> newPlanePositions = new HashSet<Vector3>();
        int playerX = Mathf.FloorToInt(transform.position.x / planeSize);
        int playerZ = Mathf.FloorToInt(transform.position.z / planeSize);

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector3 planePosition = new Vector3((playerX + x) * planeSize, 0, (playerZ + z) * planeSize);
                newPlanePositions.Add(planePosition);
                if (!activePlanes.ContainsKey(planePosition))
                {
                    GameObject plane = Instantiate(planePrefab, planePosition, Quaternion.identity);
                    activePlanes[planePosition] = plane;
                    yield return StartCoroutine(SpawnPrefabsOnPlane(plane));
                }
            }
        }

        // Remove old planes and their associated prefabs
        List<Vector3> planesToRemove = new List<Vector3>();
        foreach (var planeEntry in activePlanes)
        {
            if (!newPlanePositions.Contains(planeEntry.Key))
            {
                if (planeToPrefabsMap.TryGetValue(planeEntry.Value, out List<GameObject> prefabs))
                {
                    foreach (GameObject prefab in prefabs)
                    {
                        Destroy(prefab);
                    }
                    planeToPrefabsMap.Remove(planeEntry.Value);
                }

                Destroy(planeEntry.Value);
                planesToRemove.Add(planeEntry.Key);
            }
        }

        foreach (var position in planesToRemove)
        {
            activePlanes.Remove(position);
        }
    }

    IEnumerator SpawnPrefabsOnPlane(GameObject plane)
    {
        GameObject shrubPrefab = prefabTypes[0]; // Assuming the shrub is the first prefab in the list
        int shrubCount = 20; // The number of shrubs to spawn on each plane

        // Spawn 20 shrubs on the plane
        for (int i = 0; i < shrubCount; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(plane.transform.position.x - planeSize / 2, plane.transform.position.x + planeSize / 2),
                2f, // Adjust this height as needed
                Random.Range(plane.transform.position.z - planeSize / 2, plane.transform.position.z + planeSize / 2)
            );

            Instantiate(shrubPrefab, spawnPosition, Quaternion.identity);
        }
        if (!planeToPrefabsMap.ContainsKey(plane))
        {
            planeToPrefabsMap[plane] = new List<GameObject>();
        }

        int prefabsToSpawn = Random.Range(1, maxPrefabsPerPlane + 1);
        for (int i = 0; i < prefabsToSpawn; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(plane.transform.position.x - planeSize / 2, plane.transform.position.x + planeSize / 2),
                1.7f, // Adjust this height as needed
                Random.Range(plane.transform.position.z - planeSize / 2, plane.transform.position.z + planeSize / 2)
            );

            GameObject prefabToSpawn = prefabTypes[Random.Range(0, prefabTypes.Count)];
            GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            planeToPrefabsMap[plane].Add(spawnedPrefab);

            Debug.Log($"Spawned prefab {spawnedPrefab.name} at {spawnPosition}");
            yield return null;
        }
    }
}
