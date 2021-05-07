using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MPMenu : MonoBehaviour
{
    public GameObject lanPanel; 
    public GameObject onlinePanel;
    public GameObject levelSelectionPanel;
    public GameObject lanNetworkManager;
    public GameObject onlineNetworkManager;

    public void LanPlay()
    {
        levelSelectionPanel.SetActive(false);
        lanPanel.SetActive(true);
        lanNetworkManager.SetActive(true);
        Destroy(onlineNetworkManager);
    }

    public void OnlinePlay()
    {
        levelSelectionPanel.SetActive(false);
        onlinePanel.SetActive(true);
        onlineNetworkManager.SetActive(true);
        Destroy(lanNetworkManager);
    }

    public void BackToSelection()
    {
        if (NetworkManager.Singleton) Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene("MultiplayerLevel", LoadSceneMode.Single);
    }
}
