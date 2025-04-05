using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class universalAmbientTrigger : MonoBehaviour
{
    public string ambientName;
    private AudioManager audioManager;

    private bool hasTriggered = false;
    private void Start()
    {
       
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
         
                audioManager.PlayAmbient(ambientName);
                hasTriggered = true;
            
        }
    }
}
