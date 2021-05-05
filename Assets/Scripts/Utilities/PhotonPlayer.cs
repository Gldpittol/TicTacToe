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

public class PhotonPlayer : NetworkBehaviour
{
    public bool gameStarted = false;
    public bool hasPlayFinished = true;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if(IsClient && !IsHost) GoToGameSceneServerRPC();
    }

    private void Update()
    {
        if (!IsOwner) return;

        BoardControllerMP.instance.debugText.text = "Is Host: " + IsHost + "\nIs Host Turn: " + BoardControllerMP.instance.isHostTurn + "\nGame Started: " + gameStarted;
        BoardControllerMP.instance.debugText2.text = "Linha: " + BoardControllerMP.instance.lastLineClicked + "\nColuna: " + BoardControllerMP.instance.lastColClicked;

        if (BoardControllerMP.instance.playHappening && BoardControllerMP.instance.CanPlay())
        {
            MakePlayServerRPC(BoardControllerMP.instance.lastLineClicked, BoardControllerMP.instance.lastColClicked);
            BoardControllerMP.instance.playHappening = false;
        }
    }

    [ServerRpc]
    public void GoToGameSceneServerRPC()
    {
        GoToGameSceneClientRPC();
    }

    [ClientRpc]
    public void GoToGameSceneClientRPC()
    {
        BoardControllerMP.instance.isHostTurn = true;
        gameStarted = true;
    }

    [ServerRpc]
    public void MakePlayServerRPC(int line, int col)
    {
        MakePlayClientRPC(line, col);
    }

    [ClientRpc]
    public void MakePlayClientRPC(int line, int col)
    {
        BoardControllerMP.instance.lastLineClicked = line;
        BoardControllerMP.instance.lastColClicked = col;
        BoardControllerMP.instance.UpdateGame(line * 3 + col);

        //print(hasPlayFinished);

        //if(BoardControllerMP.instance.lastSpotClicked)
        //{
        //    if (BoardControllerMP.instance.isCircle) BoardControllerMP.instance.lastSpotClicked.img.sprite = BoardControllerMP.instance.lastSpotClicked.spriteO;
        //    else BoardControllerMP.instance.lastSpotClicked.img.sprite = BoardControllerMP.instance.lastSpotClicked.spriteX;

        //    BoardControllerMP.instance.lastSpotClicked.img.color = new Color(1, 1, 1, 1);

        //    BoardControllerMP.instance.isCircle = !BoardControllerMP.instance.isCircle;

        //    BoardControllerMP.instance.isHostTurn = !BoardControllerMP.instance.isHostTurn;

        //    BoardControllerMP.instance.lastSpotClicked = null;

        //    hasPlayFinished = true;
        //}
    }


}
