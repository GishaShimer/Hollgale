using System.Collections;
using TMPro;
using UnityEngine;

public class EnableOnTrigger : MonoBehaviour
{
    private TMP_Text textComponent;
    private Color newColor;
    private Coroutine fadeCoroutine;
    private TypeWriterEffect typeEffect;
    private bool flag = false;
    Player player;
    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        typeEffect = GetComponent<TypeWriterEffect>();

        newColor = textComponent.color;
        textComponent.enabled = false;
     
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsPlayer(other) || flag) return;

            textComponent.enabled = true;
                typeEffect.StartTypeWriter();
            fadeCoroutine = StartCoroutine(FadeToAlpha(1f));
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsPlayer(other) || flag) return;
        if (!gameObject.activeInHierarchy || this == null) return;

        flag = true;
            fadeCoroutine = StartCoroutine(FadeOutAndDisable());
    }

    private IEnumerator FadeOutAndDisable()
    {
        yield return StartCoroutine(FadeToAlpha(0f));

        
        gameObject.SetActive(false);
       
           
        
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
        return (other.gameObject.CompareTag("Player") && !player._isDashing);
    }
}
