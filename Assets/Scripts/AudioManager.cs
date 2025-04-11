using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{

    [Header("Audio Source")]
    public AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource ambientSource;
    [SerializeField] AudioSource FallSource; 

    [Header("Audio Clip")]
    public AudioClip softLand;
    public AudioClip hardLand;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip background;
    public AudioClip wind;
    public AudioClip doorClosed;
    public AudioClip falling;
    public AudioClip noteOpen;
    public AudioClip noteClose;
    public AudioClip jumpingOff;
    public AudioClip carSuspension;
    public AudioClip carLightSwitch;
    public AudioMixer audioMixer;
    public AudioClip platformBreak;
    public AudioClip doorOpen;
    public AudioClip ladderClimbing;
    public AudioClip dashRecovery;
    public AudioClip typeWriter;
    


    public List<AudioClip> ambientClips; // Список доступных клипов

    private Dictionary<string, AudioClip> clipDictionary; // Словарь для быстрого доступа к клипам

    private void Awake()
    {
        if (background != null)
            background.LoadAudioData();
        clipDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in ambientClips)
        {
            if (clip != null)
            {
                clipDictionary[clip.name] = clip;
            }
        }
    }

    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();


        ambientSource.clip = wind;
        ambientSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void PlayAmbient(string clipName)
    {
        if (clipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            ambientSource.PlayOneShot(clip); // Воспроизводим клип
        }
     
    }


    public void FadeVolume(string parameter,  float targetVolume, float duration)
    {
        StartCoroutine(FadeVolumeCoroutine(parameter, targetVolume, duration));
    }

    private IEnumerator FadeVolumeCoroutine(string parameter, float targetVolume, float duration)
    {
        audioMixer.GetFloat(parameter, out float currentVolume);
        float startVolume = currentVolume;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float newVolume = Mathf.Lerp(startVolume, Mathf.Lerp(-80, 0, targetVolume), elapsedTime / duration);
            audioMixer.SetFloat(parameter, newVolume);
            yield return null;
        }

        audioMixer.SetFloat(parameter, Mathf.Lerp(-80, 0, targetVolume));
    }


    public void PlayFall(AudioClip clip)
    {
        FallSource.clip = clip;
        FallSource.Play();
    }
    public void StopFallSound()
    {
        FallSource.Stop();
    }
    public void SetVolume(string parameter, float targetVolume)
    {
        float dB = Mathf.Clamp(Mathf.Log10(Mathf.Max(targetVolume, 0.0001f)) * 20, -80, 0);
        audioMixer.SetFloat(parameter, dB);
    }

}
