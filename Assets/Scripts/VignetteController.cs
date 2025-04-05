using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteController : MonoBehaviour
{
    public Volume volume;
    public float targetIntensity = 0.5f;
    public float transitionDuration = 1f;
    public bool changeColor = false;
    public Color targetColor = Color.black;

    private Vignette vignette;
    private Coroutine vignetteCoroutine;
    private float initialIntensity;
    private Color initialColor;

    private int playerCollidersInside = 0; // ���������� ����������� ������ ������
    private static VignetteController activeZone; // ������� �������� ���� ��������

    private void Start()
    {
        if (volume.profile.TryGet(out vignette))
        {
            initialIntensity = vignette.intensity.value;
            initialColor = vignette.color.value;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCollidersInside++; // ����������� ������� ������

            if (activeZone != this)
            {
                if (activeZone != null)
                {
                    activeZone.ResetVignette(); // ��������� ������ ��������
                }

                activeZone = this; // ��������� ������� ���� ��������
                StartVignetteEffect(targetIntensity, changeColor ? targetColor : initialColor);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerCollidersInside--; // ��������� ������� �������

            if (playerCollidersInside <= 0) // ������ ���� ��� ���������� �����
            {
                if (activeZone == this)
                {
                    activeZone = null; // ������� �������� ����
                    StartVignetteEffect(initialIntensity, initialColor);
                }
            }
        }
    }

    private void StartVignetteEffect(float targetValue, Color targetVignetteColor)
    {
        if (vignetteCoroutine != null)
        {
            StopCoroutine(vignetteCoroutine);
        }

        vignetteCoroutine = StartCoroutine(ChangeVignetteEffect(targetValue, targetVignetteColor));
    }

    private IEnumerator ChangeVignetteEffect(float targetValue, Color targetVignetteColor)
    {
        float startIntensity = vignette.intensity.value;
        Color startColor = vignette.color.value;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(startIntensity, targetValue, elapsedTime / transitionDuration);
            vignette.color.value = Color.Lerp(startColor, targetVignetteColor, elapsedTime / transitionDuration);
            yield return null;
        }

        vignette.intensity.value = targetValue;
        vignette.color.value = targetVignetteColor;
    }

    private void ResetVignette()
    {
        StartVignetteEffect(initialIntensity, initialColor);
    }
}
