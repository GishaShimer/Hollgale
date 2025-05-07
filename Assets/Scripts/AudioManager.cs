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
    public AudioSource ambientSource;


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
    public AudioClip mainMenuMusic;
    public AudioClip buttonClick;
    public AudioClip mouseButtonEnter;
    public AudioClip start;

    public List<AudioClip> ambientClips;
    private Dictionary<string, AudioClip> clipDictionary; 

    private void Awake()
    {
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else if (instance != this)
        //{
        //    Destroy(gameObject); // Удаляем дубликат, если он появился при запуске другой сцены
        //}
        //if (background != null)
        //    background.LoadAudioData();
        clipDictionary = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in ambientClips)
        {
            if (clip != null)
            {
                clipDictionary[clip.name] = clip;
            }
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayAmbientSound(AudioClip clip)
    {
        ambientSource.clip = clip;
        ambientSource.loop = true;
        ambientSource.Play();
    }

    public void PlayTriggerZones(string clipName)
    {
        if (clipDictionary.TryGetValue(clipName, out AudioClip clip))
        {
            ambientSource.PlayOneShot(clip); 
        }
    }
    public void HandleFadeVolume()
    {
        StartCoroutine(FadeVolumeCoroutine("MusicVolume", 0f, 1.7f));
    }
    public void HandleFadeWind()
    {
        StartCoroutine(FadeVolumeCoroutine("AmbientVolume", 0f, 1.7f));
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
    public void SetMasterVolume(float value)
    {
        SetVolumeFromSlider("MasterVolume", value);
    }
    public void SetMusicVolume(float value)
    {
        SetVolumeFromSlider("MusicVolume", value);
    }
    public void SetSFXVolume(float value)
    {
        SetVolumeFromSlider("SFXVolume", value);
    }
    public void SetAmbientVolume(float value)
    {
        SetVolumeFromSlider("AmbientVolume", value);
    }
   

    public void SetVolumeFromSlider(string parameter, float sliderValue)
    {
        sliderValue = Mathf.Clamp(sliderValue, 0.001f, 1f);

        float dB = Mathf.Log10(sliderValue) * 20f;
        audioMixer.SetFloat(parameter, dB);

        PlayerPrefs.SetFloat(parameter, sliderValue); // сохранить
    }
}
