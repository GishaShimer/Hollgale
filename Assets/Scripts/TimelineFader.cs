using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineFader : MonoBehaviour
{
    private float targetAlpha;
    private float alphaChangeSpeed;
    private bool isFading;
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
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
