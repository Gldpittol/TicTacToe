using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;
using MLAPI.Transports.PhotonRealtime;
using MLAPI.Messaging;
using UnityEngine.SceneManagement;
using WebSocketSharp;
public class UnetController : MonoBehaviour
{
    public InputField IPField;

    private void Start()
    {
        if (PlayerPrefs.HasKey("LastUsedKeyUnet"))
        {
            IPField.text = PlayerPrefs.GetString("LastUsedKeyUnet");
        }
    }

    public void UpdateLastUsedRoom()
    {
        PlayerPrefs.SetString("LastUsedKeyUnet", IPField.text);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        BoardControllerMP.instance.photonWaitingPlayers.SetActive(true);
        BoardControllerMP.instance.photonButtons.SetActive(false);
        BoardControllerMP.instance.photonButtons.transform.parent.gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
    }

    public void StartClient()
    {
        var networkManager = NetworkManager.Singleton;
        networkManager.GetComponent<UNetTransport>().ConnectAddress = IPField.text;
        networkManager.StartClient();
    }


}
