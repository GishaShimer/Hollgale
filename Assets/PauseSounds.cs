using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSounds : MonoBehaviour
{
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(AudioClip clip)
    {
        audioSource.ignoreListenerPause = true;
        audioSource.clip = clip;
 
        audioSource.Play();
    }
}
