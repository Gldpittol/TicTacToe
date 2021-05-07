using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSelect : MonoBehaviour
{
    public GameObject gameControllerNotMP;
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
        if(NetworkManager.Singleton) Destroy(NetworkManager.Singleton.gameObject);
        if (gameControllerNotMP) Destroy(gameControllerNotMP);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
