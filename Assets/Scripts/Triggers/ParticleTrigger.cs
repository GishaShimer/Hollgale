using UnityEngine;

public class ParticleGravityTrigger : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    private float[] initialGravities;

    private void Start()
    {
        if (particleSystems == null || particleSystems.Length == 0)
            particleSystems = GetComponentsInChildren<ParticleSystem>();

        initialGravities = new float[particleSystems.Length];
        for (int i = 0; i < particleSystems.Length; i++)
        {
            initialGravities[i] = particleSystems[i].main.gravityModifier.constant;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (var ps in particleSystems)
        {
            var main = ps.main;
            main.gravityModifier = new ParticleSystem.MinMaxCurve(-0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            var main = particleSystems[i].main;
            main.gravityModifier = new ParticleSystem.MinMaxCurve(initialGravities[i]);
        }
    }
}