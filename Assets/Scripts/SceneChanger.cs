using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    string nextScene;

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    [YarnCommand("EndScene")]
    public void DelayedLoadScene(float delay)
    {
        StartCoroutine(WaitThenLoadNextScene(delay));
    }

    IEnumerator WaitThenLoadNextScene(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadScene(nextScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
