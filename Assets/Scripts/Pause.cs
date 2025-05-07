using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public SceneController sceneController;
    [SerializeField] Canvas canvas;
    private bool pause = false;
    public CursorController cursorController;
    public GameObject[] arrows;

    private bool flag = false;
    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            canvas.gameObject.SetActive(pause);
            
        }
        if (pause)
        {
            Method();
            cursorController.SetCursorTrue();
            Time.timeScale = 0f;
            audioMixer.SetFloat("MusicLPF", 500f);
            audioMixer.SetFloat("SFXVolume", -80f);
        }
        else
        {
            flag =false;
            cursorController.SetCursorFalse();
            audioMixer.SetFloat("MusicLPF", 22000f);
            audioMixer.SetFloat("SFXVolume", 0f);
            Time.timeScale = 1f;
        }
    }

  
    private void Method()
    {
        if(!flag && pause)
        {
            foreach (var arrow in arrows)
            {
                arrow.SetActive(false);
            }
            flag = true;
        }
    }
    public void Return()
    {
      canvas.gameObject.SetActive(false);
        pause = false;

    }
    public void QuitToMenu()
    {
        sceneController.LoadScene("MainMenu");
    }
}
