using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPTtest : MonoBehaviour
{
    [Range(0f, 10f)]
    public float radius = 0.5f; // Deformation radius
    [Range(0f, 10f)]
    public float deformationStrength = 0.5f; // Deformation strength
    [Range(0f, 20f)]
    public float noDeformationRadius = 10f; // Radius around the car where deformation is not allowed

    private Transform carTransform; // Reference to the car's transform
    private Mesh mesh;
    private Vector3[] vertices, modifiedVerts;

    public EndGame endScript;

    void Start()
    {
        GameObject car = GameObject.FindGameObjectWithTag("Player");
        if (car != null)
        {
            carTransform = car.transform;
        }
        else
        {
            Debug.LogError("Car with 'Player' tag not found.");
            return;
        }

        MeshFilter childMeshFilter = GetComponentInChildren<MeshFilter>();
        if (childMeshFilter != null)
        {
            mesh = childMeshFilter.mesh;
            vertices = mesh.vertices;
            modifiedVerts = (Vector3[])vertices.Clone();
        }
        else
        {
            Debug.LogError("No MeshFilter found on child object.");
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

        foreach (var hit in hits)
        {
            // Debug: Log all hits
          //  Debug.Log($"Hit: {hit.collider.name}, Tag: {hit.collider.tag}");

            // If the hit object is the player, exit the method
            if (hit.collider.CompareTag("Player"))
            {
                //Debug.Log("Hit Player - Skipping Deformation");
                return; // Exit the function early
            }
        }

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Plane"))
            {
                Vector3 hitPoint = hit.point;
                if (carTransform != null)
                {
                    Vector3 anchorPoint = carTransform.position; // Adjust this if you have an offset
                    float distanceFromAnchor = Vector3.Distance(hitPoint, anchorPoint);

                    if (distanceFromAnchor > noDeformationRadius)
                    {
                        //Debug.Log("Out");
                        ApplyDeformation(hit, hitPoint);
                        break;
                    }
                    else
                    {
                        //Debug.Log("In");
                        break;
                    }
                }

            }
        }
    }

    void ApplyDeformation(RaycastHit hit, Vector3 hitPoint)
    {
        MeshFilter childMeshFilter = GetComponentInChildren<MeshFilter>();
        Vector3 localHitPoint = childMeshFilter.transform.InverseTransformPoint(hit.point);

        float scaleFactor = childMeshFilter.transform.lossyScale.x;
        float adjustedRadius = radius * scaleFactor;

        // Determine the mesh bounds
        Bounds meshBounds = mesh.bounds;
        float edgeThreshold = 0.1f; // Distance from edge to restrict deformation
        if (endScript.ended == false)
        {


            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.touchCount == 2)
            {
                bool meshModified = false;

                for (int v = 0; v < modifiedVerts.Length; v++)
                {
                    Vector3 localVertexPosition = childMeshFilter.transform.TransformPoint(modifiedVerts[v]);
                    float distance = Vector3.Distance(localVertexPosition, hitPoint);

                    // Check if the vertex is within the deformation radius and not too close to the edge
                    if (distance < adjustedRadius && IsVertexFarFromEdge(meshBounds, modifiedVerts[v], edgeThreshold))
                    {
                        float deformationFactor = (1 - (distance / adjustedRadius)) * deformationStrength;
                        Vector3 deformationDirection = Vector3.up;
                        if (Input.GetMouseButton(0))
                        {
                            deformationDirection = Vector3.up;
                        }
                        else if (Input.GetMouseButton(1) || Input.touchCount == 2)
                        {
                            deformationDirection = Vector3.down;
                        }


                        modifiedVerts[v] += deformationDirection * deformationFactor;
                        meshModified = true;
                    }
                }

                if (meshModified)
                {
                    RecalculateMesh();
                }
            }
        }
    }

    bool IsVertexFarFromEdge(Bounds bounds, Vector3 vertex, float threshold)
    {
        // Check each axis to see if the vertex is within the threshold of the mesh bounds
        return Mathf.Abs(vertex.x - bounds.min.x) > threshold &&
               Mathf.Abs(vertex.x - bounds.max.x) > threshold &&
               Mathf.Abs(vertex.z - bounds.min.z) > threshold &&
               Mathf.Abs(vertex.z - bounds.max.z) > threshold;
    }


    void RecalculateMesh()
    {
        mesh.vertices = modifiedVerts;
        mesh.RecalculateNormals();
        MeshCollider meshCollider = GetComponentInChildren<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = mesh;
        }
    }


    public void ResetMesh()
    {
        if (vertices == null || mesh == null)
        {
            Debug.Log("Mesh or vertices are not initialized.");
            return;
        }

        modifiedVerts = (Vector3[])vertices.Clone(); // Reset modifiedVerts to the original vertices
        RecalculateMesh(); // Apply the reset to the mesh and recalculate normals
    }
}
