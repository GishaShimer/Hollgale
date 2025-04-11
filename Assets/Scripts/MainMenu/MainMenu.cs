using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginButton : MonoBehaviour
{

    public void LoadScene()
    {
        StartCoroutine(LoadSceneWithDelay());
 
        
    }
    private IEnumerator LoadSceneWithDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitScene()
    {
      Application.Quit();
    }
}
