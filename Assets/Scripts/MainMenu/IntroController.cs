using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    public TimelineFader Fader;
    public SceneController sceneController;
    SoundManager audioManager;
    public Cinemachine.CinemachineVirtualCamera cam;
    private bool flag;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }
    public void Intro()
    {
        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
  //      brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0);
        // Шаг 1: Затемнение экрана
        Fader.ChangeAlpha(1f);

        // Шаг 2: Ждём 2 секунды
        yield return new WaitForSeconds(2f);

        // Шаг 3: Переключаем камеру (внутри последовательности!)
        cam.Priority = 11;

        // Шаг 4: Ждём ещё 2.5 секунды
        yield return new WaitForSeconds(2.5f);

        // Шаг 5: Осветляем экран
        Fader.ChangeAlpha(0f);

        // Шаг 6: Разрешаем нажимать любую клавишу
        flag = true;
    }

    void Update()
    {
        if (Input.anyKeyDown && flag)
        {
            audioManager.HandleFadeWind();
            audioManager.PlaySFX(audioManager.start, 1f);
            Fader.ChangeAlpha(1f);
            sceneController.LoadScene("FirstScene");
        }
    }
}
