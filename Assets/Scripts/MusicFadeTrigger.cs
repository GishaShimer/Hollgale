using System.Collections;
using UnityEngine;

public class MusicFadeZone : MonoBehaviour
{
    public AudioManager audioManager;
    public Transform player; // Посилання на гравця
    public float fadeDuration = 0.5f; // Час згасання звуку

    private float topY;  // Верхня межа
    private float bottomY; // Нижня межа
    private bool isPlayerInside = false;

    private void Start()
    {
        if (audioManager == null)
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }

        // Отримуємо межі BoxCollider2D
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        topY = col.bounds.max.y;
        bottomY = col.bounds.min.y;
    }

    private void Update()
    {
        if (isPlayerInside)
        {
            float playerY = player.position.y;

            // Нормалізуємо значення від 0 (низ) до 1 (верх)
            float t = Mathf.InverseLerp(bottomY, topY, playerY);

            // Змінюємо гучність музики
            audioManager.FadeVolume("MusicVolume", t, fadeDuration);
     
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
     
            isPlayerInside = true;
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
      
            isPlayerInside = false;
            audioManager.FadeVolume("MusicVolume", 1f, fadeDuration); // Відновлюємо звук при виході
        
    }
}
