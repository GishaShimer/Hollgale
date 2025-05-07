using UnityEngine;

[RequireComponent(typeof(AudioLowPassFilter))]
public class StrikeController : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 20f;
    public float minCutoff = 500f;  // глушение вдали
    public float maxCutoff = 22000f; // чистый звук рядом

    private AudioLowPassFilter lowPass;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        lowPass = GetComponent<AudioLowPassFilter>();
        audioSource.Play();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float t = Mathf.Clamp01(distance / maxDistance);

        float cutoff = Mathf.Lerp(maxCutoff, minCutoff, t);
        lowPass.cutoffFrequency = cutoff;

        audioSource.volume = Mathf.Lerp(1f, 0.2f, t);
    }
}
