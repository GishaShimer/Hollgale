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
    private int playerCollidersInside = 0; // ���������� �������� �����������

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        typeEffect = GetComponent<TypeWriterEffect>();

        newColor = textComponent.color;
        textComponent.enabled = false; // ���������� �������� �����
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;

        playerCollidersInside++; // ����������� ������� ������

        if (playerCollidersInside == 1) // ������ ���� ��� ������ ����
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            isFading = false;
            textComponent.enabled = true;

            if (typeEffect != null)
            {
                typeEffect.StartTypeWriter();
            }

            fadeCoroutine = StartCoroutine(FadeToAlpha(1f));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsPlayer(other)) return;

        playerCollidersInside--; // ��������� ������� �������

        if (playerCollidersInside <= 0) // ������ ���� ��� ���������� �����
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            isFading = true;
            fadeCoroutine = StartCoroutine(FadeOutAndDisable());
        }
    }

    private IEnumerator FadeOutAndDisable()
    {
        yield return StartCoroutine(FadeToAlpha(0f));

        if (!isFading) yield break;

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
        if (textComponent == null) yield break;

        float fadeDuration = 1f;
        float startAlpha = textComponent.color.a;
        float elapsed = 0f;

        if (Mathf.Approximately(startAlpha, targetAlpha)) yield break;

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

    private bool IsPlayer(Collider2D other)
    {
        return other.gameObject.CompareTag("Player");
    }
}
