using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
     public AudioSource SFXSource;
     public AudioSource MusicSource;
     public AudioSource AmbientSource;

    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Clips")]
    public AudioClip softLand;
    public AudioClip hardLand;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip doorClosed;
    public AudioClip falling;
    public AudioClip noteOpen;
    public AudioClip noteClose;
    public AudioClip jumpingOff;
    public AudioClip carSuspension;
    public AudioClip carLightSwitch;
    public AudioClip platformBreak;
    public AudioClip doorOpen;
    public AudioClip ladderClimbing;
    public AudioClip dashRecovery;
    public AudioClip typeWriter;
    public AudioClip start;
    public AudioClip lampFade;
    public AudioClip hum;

    public AudioClip mainMenuMusic;
    public AudioClip firstSceneMusic;

    [SerializeField] private UnityEngine.UI.Slider masterSlider;
    [SerializeField] private UnityEngine.UI.Slider musicSlider;
    [SerializeField] private UnityEngine.UI.Slider sfxSlider;
    [SerializeField] private UnityEngine.UI.Slider ambientSlider;

    private void Start()
    {
        LoadSavedVolumes();
        MusicSource.Play();

    }
    public void PlayMusicStatic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.loop = true;
        MusicSource.Play();
    }
    public void PlayMusic(AudioClip clip, float volume)
    {
        MusicSource.clip = clip;
        MusicSource.loop = true;
        MusicSource.volume = volume;
        MusicSource.Play();
    }
    public void PlaySFXStatic(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
   

    public void PlaySFX(AudioClip clip, float volume)
    {
        SFXSource.volume = volume;
        SFXSource.PlayOneShot(clip);
    }
    public void PlayAmbientStatic(AudioClip clip, float volume)
    {
        AmbientSource.volume = volume;
        AmbientSource.PlayOneShot(clip);
    }
    public void PlayAmbient(AudioClip clip, float volume)
    {
        AmbientSource.clip = clip;
        AmbientSource.volume = volume;
        AmbientSource.Play();
    }

    public void HandleFadeVolume()
    {
        StartCoroutine(FadeVolumeCoroutine("MusicVolume", 0f, 1.7f));
    }
    public void HandleFadeWind()
    {
        StartCoroutine(FadeVolumeCoroutine("AmbientVolume", 0f, 1.7f));
    }

    public void FadeVolume(string parameter, float targetVolume, float duration)
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

        PlayerPrefs.SetFloat(parameter, sliderValue); 
    }
    public void LoadSavedVolumes()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float ambient = PlayerPrefs.GetFloat("AmbientVolume", 1f);

        SetVolumeFromSlider("MasterVolume", master);
        SetVolumeFromSlider("MusicVolume", music);
        SetVolumeFromSlider("SFXVolume", sfx);
        SetVolumeFromSlider("AmbientVolume", ambient);

        if (masterSlider != null) masterSlider.value = master;
        if (musicSlider != null) musicSlider.value = music;
        if (sfxSlider != null) sfxSlider.value = sfx;
        if (ambientSlider != null) ambientSlider.value = ambient;
    }
 
}
