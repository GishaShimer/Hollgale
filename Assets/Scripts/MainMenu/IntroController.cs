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
        // ��� 1: ���������� ������
        Fader.ChangeAlpha(1f);

        // ��� 2: ��� 2 �������
        yield return new WaitForSeconds(2f);

        // ��� 3: ����������� ������ (������ ������������������!)
        cam.Priority = 11;

        // ��� 4: ��� ��� 2.5 �������
        yield return new WaitForSeconds(2.5f);

        // ��� 5: ��������� �����
        Fader.ChangeAlpha(0f);

        // ��� 6: ��������� �������� ����� �������
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
