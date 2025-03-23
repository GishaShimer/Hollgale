using System.Collections;
using UnityEngine;

public class MusicFadeZone : MonoBehaviour
{
    public AudioManager audioManager;
    public Transform player; // ��������� �� ������
    public float fadeDuration = 0.5f; // ��� �������� �����

    private float topY;  // ������ ����
    private float bottomY; // ����� ����
    private bool isPlayerInside = false;

    private void Start()
    {
        if (audioManager == null)
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }

        // �������� ��� BoxCollider2D
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        topY = col.bounds.max.y;
        bottomY = col.bounds.min.y;
    }

    private void Update()
    {
        if (isPlayerInside)
        {
            float playerY = player.position.y;

            // ���������� �������� �� 0 (���) �� 1 (����)
            float t = Mathf.InverseLerp(bottomY, topY, playerY);

            // ������� ������� ������
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
            audioManager.FadeVolume("MusicVolume", 1f, fadeDuration); // ³��������� ���� ��� �����
        
    }
}
