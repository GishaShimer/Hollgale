using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

public class Note : MonoBehaviour
{
    public Image highlightImage; // Подсветка при приближении
    public Canvas canvas;
    public GameObject notePrefabRoot;
    public Image img;            // Фон записки (сначала чистый)
    public TMP_Text txt;         // Текст записки (появляется на 2 клике)

    public Player player;
    public Animator anim; // Аниматор на дочернем объекте подсветки

    AudioManager audioManager;

    private bool playerInTrigger = false; // Игрок в триггере
    private int noteState = 0;
    private bool isClosing = false; // Флаг, что начинается процесс закрытия

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        highlightImage.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
        txt.gameObject.SetActive(false);

        // Если аниматор не установлен вручную, получаем его из дочерних объектов подсветки
        if (anim == null)
        {
            anim = highlightImage.GetComponentInChildren<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            highlightImage.gameObject.SetActive(true);
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Если игрок покидает триггер и процесс закрытия ещё не запущен
        if (other.CompareTag("Player"))
        {
                anim.SetTrigger("Hide");
     
            StartCoroutine(WaitForEndAndDisable());
            

        }
    }

    private IEnumerator WaitForEndAndDisable()
    {
        // Ждем, пока анимация "Hide" не завершится.
        // normalizedTime отсчитывается от 0 до 1, где 1 – окончание состояния.
        yield return new  WaitForSeconds(0.1f);

        highlightImage.gameObject.SetActive(false);
        playerInTrigger = false;

    }

    private void Update()
    {
        if (playerInTrigger && Input.GetMouseButtonDown(0))
        {
            if (noteState == 0)
            {
                OpenNote();
            }
            else if (noteState == 1)
            {
                ShowText();
            }
            else if (noteState == 2)
            {
                CloseNote();
            }
        }
    }

    private void OpenNote()
    {
        player.DisableControls();
      

        audioManager.FadeVolume("MusicVolume", 0.4f, 1);
        audioManager.PlaySFX(audioManager.noteOpen);
        canvas.gameObject.SetActive(true);
        noteState = 1;
    }

    private void ShowText()
    {
        audioManager.PlaySFX(audioManager.noteOpen);
        img.color = new Color32(155, 155, 155, 100);
        txt.gameObject.SetActive(true); // Показать текст записки
        noteState = 2;
    }

    private void CloseNote()
    {
        player.isEnabled = true;
        audioManager.FadeVolume("MusicVolume", 1f, 1);
        audioManager.PlaySFX(audioManager.noteClose);
        canvas.gameObject.SetActive(false);

      
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        Destroy(notePrefabRoot);
    }

}
