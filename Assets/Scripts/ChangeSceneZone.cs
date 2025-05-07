using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChangeSceneZone : MonoBehaviour
{
    public  SceneController sceneController;
    public TimelineFader Fader;
    SoundManager audioManager;
    [SerializeField] private string SceneName;
    public PlayableDirector playableDirector;


    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            {
            playableDirector.Play();
            audioManager.HandleFadeVolume();
            audioManager.HandleFadeWind();
            Fader.ChangeAlpha(1f);
            sceneController.LoadScene(SceneName);
           
        }
    }
}
