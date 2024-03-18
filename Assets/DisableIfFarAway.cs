using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfFarAway : MonoBehaviour
{
    public Transform car; // Reference to the target GameObject
    public float threshold = 300f; // Distance threshold

    private MeshRenderer[] childRenderers;
    private bool[] originalVisibility; // Stores the original visibility state of child renderers

    void Start()
    {
        // Get all child MeshRenderers and store their initial visibility
        childRenderers = GetComponentsInChildren<MeshRenderer>();
        originalVisibility = new bool[childRenderers.Length];
        for (int i = 0; i < childRenderers.Length; i++)
        {
            originalVisibility[i] = childRenderers[i].enabled;
        }
    }

    void Update()
    {
        // Ensure target is assigned
        if (car != null)
        {
            // Calculate distance between local and target GameObjects
            float distance = Vector3.Distance(transform.position, car.position);

            // Check if distance exceeds threshold
            if (distance > threshold)
            {
                // Disable all MeshRenderer components in children
                foreach (var renderer in childRenderers)
                {
                    renderer.enabled = false;
                }
            }
            else
            {
                // Activate all previously active MeshRenderer components
                for (int i = 0; i < childRenderers.Length; i++)
                {
                    childRenderers[i].enabled = originalVisibility[i];
                }
            }
        }
        else
        {
            Debug.LogError("Target GameObject is not assigned!");
        }
    }
}