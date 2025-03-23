using UnityEngine;
using System.Collections;

public class RainIntensityController : MonoBehaviour
{
    // Reference to the rain particle system
    public ParticleSystem rainParticleSystem;

    // Initial emission rate (light rain)
    public float initialEmissionRate = 2f;

    // Maximum emission rate (heavy downpour)
    public float maxEmissionRate = 100f;

    // Duration over which the emission rate will increase
    public float increaseDuration = 5f;

    // Emission module of the particle system (used to modify emission rate)
    private ParticleSystem.EmissionModule emissionModule;

    void Start()
    {
        // Ensure the rain particle system is properly assigned
        if (rainParticleSystem == null)
        {
            Debug.LogError("Rain Particle System is not assigned in the inspector.");
            return;
        }

        // Get the emission module and set the initial emission rate
        emissionModule = rainParticleSystem.emission;
        emissionModule.rateOverTime = initialEmissionRate;
    }

    // Detect when an object enters the trigger
    void OnTriggerEnter(Collider other)
    {
        // Ensure only the intended object (e.g., "Player") triggers the intensity change
        if (other.CompareTag("Player"))
        {
            // Start gradually increasing the rain intensity
            StartCoroutine(IncreaseRainIntensity());
        }
    }

    // Coroutine to gradually increase the emission rate over the specified duration
    private IEnumerator IncreaseRainIntensity()
    {
        float elapsedTime = 0f; // Tracks how much time has passed

        // Gradually increase the emission rate over time
        while (elapsedTime < increaseDuration)
        {
            // Linearly interpolate the emission rate from initial to max based on elapsed time
            float newEmissionRate = Mathf.Lerp(initialEmissionRate, maxEmissionRate, elapsedTime / increaseDuration);
            emissionModule.rateOverTime = newEmissionRate;

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Ensure the final emission rate is set to the max value
        emissionModule.rateOverTime = maxEmissionRate;
    }
}
