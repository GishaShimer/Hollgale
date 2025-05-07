using UnityEngine;

public class Music : MonoBehaviour
{
    SoundManager audioManager;

    [Header("1 = Main Menu, 2 = Background")]
    public int music;


    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();


        switch (music)
        {
            case 1:
                audioManager.PlayMusic(audioManager.mainMenuMusic, 1f);
                break;
            case 2:
                audioManager.PlayMusic(audioManager.firstSceneMusic, 1f);
                break;
        }
    
    }
}
