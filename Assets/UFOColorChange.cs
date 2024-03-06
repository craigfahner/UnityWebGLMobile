using UnityEngine;

public class UFOColorChange : MonoBehaviour
{
    public Color[] lightColors; // Array of colors for the lights
    public Material[] coneMaterials; // Array of materials for the cone
    public float changeInterval = 3f; // Time interval for color and material changes
    public bool oscillationEnabled = false; // Boolean to control oscillation
    public int initialColorPhase = 0; // Initial color phase
    public int initialMaterialPhase = 0; // Initial material phase

    private Light[] lights; // Array to store child lights
    private Renderer coneRenderer; // Renderer for the cone
    private int colorIndex = 0; // Index for current light color
    private int materialIndex = 0; // Index for current cone material
    private bool isOscillating = false; // Flag to track oscillation status

    void Start()
    {
        // Find child lights and cone renderer
        lights = GetComponentsInChildren<Light>();
        coneRenderer = transform.Find("cone").GetComponent<Renderer>();

        // Set initial color and material phases
        colorIndex = initialColorPhase;
        materialIndex = initialMaterialPhase;

        // Start oscillation if enabled
        if (oscillationEnabled)
            StartOscillation();
    }

    void Update()
    {
        // Check if oscillation should start or stop
        if (oscillationEnabled && !isOscillating)
            StartOscillation();
        else if (!oscillationEnabled && isOscillating)
            StopOscillation();
    }

    void StartOscillation()
    {
        // Set the flag to indicate oscillation is active
        isOscillating = true;

        // Start invoking the color and material change method
        InvokeRepeating("ChangeColorAndMaterial", 0f, changeInterval);
    }

    void StopOscillation()
    {
        // Clear the flag to indicate oscillation is inactive
        isOscillating = false;

        // Stop invoking the color and material change method
        CancelInvoke("ChangeColorAndMaterial");
    }

    void ChangeColorAndMaterial()
    {
        // Change light color to next color in array
        foreach (Light light in lights)
        {
            light.color = lightColors[colorIndex];
        }

        // Change cone material to next material in array
        coneRenderer.material = coneMaterials[materialIndex];

        // Increment color and material indices or loop back to start
        colorIndex = (colorIndex + 1) % lightColors.Length;
        materialIndex = (materialIndex + 1) % coneMaterials.Length;
    }
}