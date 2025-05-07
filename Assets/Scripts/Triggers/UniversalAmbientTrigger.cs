using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class universalAmbientTrigger : MonoBehaviour
{

    private SoundManager audioManager;
    public AudioClip[] audioClips;
    public int choice;
    private bool hasTriggered = false;
    private void Start()
    {
       
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {

            audioManager.PlayAmbientStatic(audioClips[choice],1f);
                hasTriggered = true;
            
        }
    }
}
