using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineFader : MonoBehaviour
{
    private float targetAlpha;
    private float alphaChangeSpeed;
    private bool isFading = true;
    private Image image;
    private void Start()
    {
        image = GetComponent<Image>();

        // Установить полностью видимый цвет
        Color color = image.color;
        color.a = 1f;
        image.color = color;

        // И сразу начать исчезновение
        ChangeAlpha(0f);
    }

    private void Update()
    {
        if (isFading)
        {
            Color color = image.color;
            color.a = Mathf.MoveTowards(color.a, targetAlpha, alphaChangeSpeed * Time.deltaTime);
            image.color = color;

            if (Mathf.Approximately(color.a, targetAlpha)) // Когда альфа достигла цели
            {
                isFading = false;
            }
        }
    } 

    public void ChangeAlpha(float newAlpha)
    {
        float time = 1.5f;
        targetAlpha = Mathf.Clamp01(newAlpha); // Ограничиваем значение от 0 до 1
        alphaChangeSpeed = Mathf.Abs(targetAlpha - image.color.a) / time;
        isFading = true;
    }

}
