using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

public class CarTrigger : MonoBehaviour
{
    public Animator anim;
    public Light2D[] lights;
    public Sprite newSprite;
    public SpriteRenderer SpriteRenderer;
    private AudioManager audioManager;

    private bool lightsOn = false; // Флаг, чтобы звук фар проигрывался один раз

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        foreach(var light in lights)
        {
            light.enabled = false;
        }
      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        audioManager.PlaySFX(audioManager.carSuspension); // Звук подвески
        SpriteRenderer.sprite = newSprite;
        anim.SetBool("CarBounce", true);

        if (!lightsOn)
        {
            audioManager.PlaySFX(audioManager.carLightSwitch); // Звук включения фар
            lightsOn = true;
        }
        foreach (var light in lights)
        {
            light.enabled = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        anim.SetBool("CarBounce", false);
    }
}
