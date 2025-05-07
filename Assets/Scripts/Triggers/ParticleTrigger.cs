using UnityEngine;

public class ParticleGravityTrigger : MonoBehaviour
{
    public ParticleSystem[] particleSystems;
    private float[] initialGravities;
    private int playerCount = 0;

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
        if (!IsPlayer(other) || particleSystems == null) return;

        playerCount++;

        for (int i = 0; i < particleSystems.Length; i++)
        {
            if (particleSystems[i] == null) continue;

            var main = particleSystems[i].main;
            main.gravityModifier = new ParticleSystem.MinMaxCurve(-0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsPlayer(other) || particleSystems == null || initialGravities == null) return;

        playerCount--;

        if (playerCount <= 0)
        {
            for (int i = 0; i < particleSystems.Length; i++)
            {
                if (particleSystems[i] == null || i >= initialGravities.Length) continue;

                var main = particleSystems[i].main;
                main.gravityModifier = new ParticleSystem.MinMaxCurve(initialGravities[i]);
            }
        }
    }

    private bool IsPlayer(Collider2D other)
    {
        return other.gameObject.CompareTag("Player");
    }
}