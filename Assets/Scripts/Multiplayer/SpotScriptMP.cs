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

public class SpotScriptMP : NetworkBehaviour
{
    public int line;
    public int column;

    public Image img;
    public Sprite spriteX;
    public Sprite spriteO;
    public Sprite spriteDead;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void MakePlay()
    {
        if(BoardControllerMP.instance.CanPlay())
        {
            BoardControllerMP.instance.lastLineClicked = line;
            BoardControllerMP.instance.lastColClicked = column;
            BoardControllerMP.instance.playHappening = true;
        }
    }

    public void UpdateGame()
    {
        if (BoardControllerMP.instance.isCircle) img.sprite = spriteO;
        else img.sprite = spriteX;

        img.color = new Color(1, 1, 1, 1);

        BoardControllerMP.instance.isCircle = !BoardControllerMP.instance.isCircle;

        BoardControllerMP.instance.isHostTurn = !BoardControllerMP.instance.isHostTurn;
    }
}
