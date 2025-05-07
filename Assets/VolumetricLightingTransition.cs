using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class VolumetricLightingTransition : MonoBehaviour
{
    Light2D light2d;
    SoundManager audioManager;

    public float fadeDuration = 0.5f;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        light2d = GetComponent<Light2D>();
    }

    public IEnumerator FadeToLight()
    {
        float elapsed = 0f;

        audioManager.PlaySFX(audioManager.lampFade, 1f);
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            light2d.intensity = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
    }

    bool flag = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !flag)
        {
            StartCoroutine(FadeToLight());
            flag = true;
        }
    }

}
