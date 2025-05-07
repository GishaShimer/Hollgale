using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MusicFadeZone : MonoBehaviour
{
    public SoundManager audioManager;


    public SpriteRenderer spriteRenderer1;
    public SpriteRenderer spriteRenderer2;

    public Sprite sprite1;
    public Sprite sprite2;

    public GameObject targetNote;
    public Light2D[] lightObjects;

    private bool oneTime;

    private void Start()
    {
        if (audioManager == null)
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        }


    }

    private void Update()
    {
        if (targetNote == null || !targetNote.activeInHierarchy)
        {
            audioManager.MusicSource.Stop();
          
            spriteRenderer1.sprite = sprite1;
            spriteRenderer2.sprite = sprite2;
            foreach (Light2D light in lightObjects)
            {
                if (light != null)
                {
                    light.enabled = false;
                }
            }
        }

    }


}
