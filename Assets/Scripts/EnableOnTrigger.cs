using System.Collections;
using TMPro;
using UnityEngine;

public class EnableOnTrigger : MonoBehaviour
{
    public bool once;
    private TMP_Text textComponent;
    private Color newColor;
    private Coroutine fadeCoroutine;
    private TypeWriterEffect typeEffect;
    private bool isFading;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        newColor = textComponent.color;
        typeEffect = GetComponent<TypeWriterEffect>();
  
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !isFading)
        {
            typeEffect.StartTypeWriter();
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            textComponent.enabled = true;
            fadeCoroutine = StartCoroutine(FadeToAlpha(1f));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
         
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);


            isFading = true;
            fadeCoroutine = StartCoroutine(FadeOutAndDisable());
        }
    }

    private IEnumerator FadeOutAndDisable()
    {
        yield return StartCoroutine(FadeToAlpha(0f)); // Ждём завершения анимации

          isFading = false;

        if (once)
        {
          gameObject.SetActive(false);
        }
        else
        {
            textComponent.enabled = false;
        }
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float fadeDuration = 1f;
        float startAlpha = textComponent.color.a;
        float elapsed = 0f;

        if (Mathf.Approximately(startAlpha, targetAlpha))
            yield break; // Если значение уже установлено, ничего не делаем

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            newColor.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            textComponent.color = newColor;
            yield return null;
        }

        newColor.a = targetAlpha;
        textComponent.color = newColor;
    }
}
