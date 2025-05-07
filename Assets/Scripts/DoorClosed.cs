using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClosed : MonoBehaviour
{
    public GameObject PopupTextPrefab; // Префаб, а не объект из сцены
    private GameObject popupInstance;

    public Sprite newSprite;
    public Sprite oldSprite;
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer targetRenderer;

    public bool canOpen;
    public Sprite doorOpen;
    public Collider2D notTriggerCollider2D;
    private bool isOpen;

    private Color newColor;


    SoundManager audioManager;

    private bool playerInTrigger = false; // Флаг, находится ли игрок в триггере



    private void Awake()
    {
        popupInstance = Instantiate(PopupTextPrefab, transform.position, Quaternion.identity, transform);
        popupInstance.SetActive(false);
    }
    private void Start()
    {
        newColor = targetRenderer.color;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpen)
        {
            spriteRenderer.sprite = newSprite;
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        playerInTrigger = false;

        if (!isOpen)
        {
            spriteRenderer.sprite = oldSprite; 
        }
    }

    private void Update()
    {
        bool oneTime = false;
            if (playerInTrigger && Input.GetMouseButtonDown(0) && !delay)
            {
                if (!canOpen)
                {
                    audioManager.PlaySFX(audioManager.doorClosed, 1f);
                    popupInstance.transform.position = transform.position;
                StartCoroutine(Delay());
                popupInstance.SetActive(true);
                    StartCoroutine(HideText());
                }
                else
                {
                if(!oneTime && targetRenderer != null)
                    audioManager.PlaySFX(audioManager.doorOpen, 1f);
                oneTime = true;
                    spriteRenderer.sprite = doorOpen;
                    isOpen = true;
                    notTriggerCollider2D.enabled = false;

                    StartCoroutine(FadeToAlpha(targetRenderer, 0f));
                }
            
        }
    }

    bool delay;
    private IEnumerator Delay()
    {
        delay = true;
        yield return new WaitForSeconds(1f);
        delay = false;
    }

        public IEnumerator FadeToAlpha(SpriteRenderer targetRenderer, float targetAlpha)
    {
        if (targetRenderer == null) yield break; // Проверка на null

        float fadeDuration = 0.5f;
        float startAlpha = targetRenderer.color.a; // Берём альфа-канал из целевого объекта
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            newColor = targetRenderer.color; // Получаем текущий цвет объекта
            newColor.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            targetRenderer.color = newColor;
            yield return null;
        }

        newColor.a = targetAlpha;
        targetRenderer.color = newColor;
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(1f);
        popupInstance.SetActive(false);

    }
   
}