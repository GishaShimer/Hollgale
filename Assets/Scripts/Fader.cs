using System.Collections;
using UnityEngine;

public class FadeOnTrigger : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    private int playerCount = 0;

    private SpriteRenderer spriteRenderer;
    private Color newColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        newColor = spriteRenderer.color;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerCount++;
            StartCoroutine(FadeToAlpha(0.2f));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerCount--;

            if (playerCount <= 0)
            {
                StartCoroutine(FadeToAlpha(1f));
            }
        }
    }
    public IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = newColor.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            newColor.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            spriteRenderer.color = newColor;
            yield return null;
        }

        newColor.a = targetAlpha;
        spriteRenderer.color = newColor;
    }
}