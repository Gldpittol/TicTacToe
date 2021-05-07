using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSelect : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

    public void GoToTitle(string sceneName)
    {
        if(NetworkManager.Singleton.gameObject) Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
