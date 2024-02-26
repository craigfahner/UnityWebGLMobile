using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [SerializeField, Range(0, 24)] private float TimeOfDay;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioTimeMapping> audioTimeMappings;
    [SerializeField] private float transitionSpeed = 0.1f; // Adjust for smoother transition

    private int lastSampleIndex = -1;

    [System.Serializable]
    public class AudioTimeMapping
    {
        public int samplePoint;
        public float targetTimeOfDay;
    }

    private void Update()
    {
        if (Preset == null || audioSource == null)
            return;

        if (Application.isPlaying)
        {
            UpdateTimeOfDayBasedOnAudioSample();
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    void UpdateTimeOfDayBasedOnAudioSample()
    {
        int currentSample = audioSource.timeSamples;
        for (int i = 0; i < audioTimeMappings.Count; i++)
        {
            AudioTimeMapping mapping = audioTimeMappings[i];
            AudioTimeMapping nextMapping = (i + 1 < audioTimeMappings.Count) ? audioTimeMappings[i + 1] : null;

            if (currentSample >= mapping.samplePoint && (nextMapping == null || currentSample < nextMapping.samplePoint))
            {
                if (lastSampleIndex != i)
                {
                    lastSampleIndex = i;
                    // Reset TimeOfDay to start of current mapping's target time if it's the first time reaching this sample
                    TimeOfDay = mapping.targetTimeOfDay;
                }

                if (nextMapping != null)
                {
                    // Calculate progress between current and next mapping sample points
                    float progress = (float)(currentSample - mapping.samplePoint) / (nextMapping.samplePoint - mapping.samplePoint);
                    // Interpolate TimeOfDay based on progress to next mapping's target time
                    TimeOfDay = Mathf.Lerp(mapping.targetTimeOfDay, nextMapping.targetTimeOfDay, progress);
                }

                // Ensure TimeOfDay loops within 24 hours
                TimeOfDay %= 24;
                break;
            }
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }
}
