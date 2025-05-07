using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TypeWriterEffect : MonoBehaviour
{
    public float delay = 0.1f;
    private TMP_Text textComponent;
    private string fullText;
    private Coroutine typingCoroutine;
    public bool isSound = false;
    SoundManager audioManager;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        fullText = textComponent.text;
        textComponent.text = ""; // Очищаем текст, но НЕ запускаем эффект сразу
    }

    public void StartTypeWriter()
    {
        textComponent.text = fullText;
     

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            if (fullText[i] != ' ')
            {
                if (isSound)
                    audioManager.PlaySFX(audioManager.typeWriter, 1f);
            }

            textComponent.text = fullText.Substring(0, i + 1);
            yield return new WaitForSeconds(delay);
        }
    }

}
