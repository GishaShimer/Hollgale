
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

    public void LoadScene(string SceneName)
    {
        StartCoroutine(LoadSceneWithDelay(SceneName));
    }
    private IEnumerator LoadSceneWithDelay(string SceneName)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneName);
    }
    public void QuitScene()
    {
        StartCoroutine(QuitSceneWithDelay());
        
    }
    private IEnumerator QuitSceneWithDelay()
    {
        yield return new WaitForSeconds(2f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
    public void RestartScene()
    {
        StartCoroutine(RestartSceneWithDelay());

    }
    private IEnumerator RestartSceneWithDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SelectionOn(Image select)
    {
        select.gameObject.SetActive(true);
    }
    public void SelectionOf(Image select)
    {
        select.gameObject.SetActive(false);
    }
}
