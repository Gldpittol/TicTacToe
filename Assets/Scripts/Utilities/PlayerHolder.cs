using MLAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHolder : MonoBehaviour
{
    public bool canReset;
    void Update()
    {
        if (BoardControllerMP.instance.playerHolder.transform.childCount == 2 && !canReset) canReset = true;

        if ((!BoardControllerMP.instance.panelPhoton.activeInHierarchy && !BoardControllerMP.instance.panelUnet.activeInHierarchy) && BoardControllerMP.instance.playerHolder.transform.childCount != 2 && canReset)
        {
            if (NetworkManager.Singleton) Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("LevelSelection", LoadSceneMode.Single);
        }
    }
}
