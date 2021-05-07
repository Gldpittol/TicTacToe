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
        //DontDestroyOnLoad(this.gameObject);
        if(IsClient && !IsHost) GoToGameSceneServerRPC();
    }

    private void Update()
    {
        if (!IsOwner) return;

        if ((!BoardControllerMP.instance.panelPhoton.activeInHierarchy && !BoardControllerMP.instance.panelUnet.activeInHierarchy) && BoardControllerMP.instance.playerHolder.transform.childCount != 2)
        {
            if (NetworkManager.Singleton) Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("LevelSelection", LoadSceneMode.Single);
        }

        if (BoardControllerMP.instance.isRestarting)
        {
            RestartGameServerRPC();
        }

        if ((BoardControllerMP.instance.isHostTurn && IsHost) || (!BoardControllerMP.instance.isHostTurn && !IsHost)) BoardControllerMP.instance.debugText.text = "<color=green>Your Turn</color>";
        else BoardControllerMP.instance.debugText.text = "<color=red>Opponent's Turn</color>";

        if(BoardControllerMP.instance.gameEnded)
        {
            BoardControllerMP.instance.debugText.gameObject.SetActive(false);
            BoardControllerMP.instance.debugText2.gameObject.SetActive(true);
            if ((IsHost && BoardControllerMP.instance.winner == 1) || (!IsHost && BoardControllerMP.instance.winner == -1)) BoardControllerMP.instance.debugText2.text = "<color=green>Victory!</color>";
            else BoardControllerMP.instance.debugText2.text = "<color=red>Defeat!</color>";

            if(BoardControllerMP.instance.winner == 9) BoardControllerMP.instance.debugText2.text = "<color=yellow>It's a tie!</color>";
        }
        else
        {
            BoardControllerMP.instance.debugText.gameObject.SetActive(true);
            BoardControllerMP.instance.debugText2.gameObject.SetActive(false);
        }

        //BoardControllerMP.instance.debugText.text = "Is Host: " + IsHost + "\nIs Host Turn: " + BoardControllerMP.instance.isHostTurn + "\nGame Started: " + gameStarted;
        //BoardControllerMP.instance.debugText2.text = "Linha: " + BoardControllerMP.instance.lastLineClicked + "\nColuna: " + BoardControllerMP.instance.lastColClicked + "\nWinner: " + BoardControllerMP.instance.winner;

        if (BoardControllerMP.instance.playHappening && BoardControllerMP.instance.CanPlay() && !BoardControllerMP.instance.gameEnded)
        {
            MakePlayServerRPC(BoardControllerMP.instance.lastLineClicked, BoardControllerMP.instance.lastColClicked);
            BoardControllerMP.instance.playHappening = false;
        }
    }

    [ServerRpc]
    public void GoToGameSceneServerRPC()
    {
        bool hostStart = Random.value < 0.5f;
        GoToGameSceneClientRPC(hostStart);
    }

    [ClientRpc]
    public void GoToGameSceneClientRPC(bool hostStart)
    {
        BoardControllerMP.instance.isHostTurn = hostStart;
        BoardControllerMP.instance.isCircle = !hostStart;
        gameStarted = true;

        GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject g in tempObjects) g.transform.parent = BoardControllerMP.instance.playerHolder.transform;

        BoardControllerMP.instance.panelPhoton.SetActive(false);
        BoardControllerMP.instance.panelUnet.SetActive(false);

    }

    [ServerRpc]
    public void MakePlayServerRPC(int line, int col)
    {
        if(BoardControllerMP.instance.board[line,col] == 0)
        MakePlayClientRPC(line, col);
    }

    [ClientRpc]
    public void MakePlayClientRPC(int line, int col)
    {
        BoardControllerMP.instance.lastLineClicked = line;
        BoardControllerMP.instance.lastColClicked = col;
        BoardControllerMP.instance.UpdateGame(line, col);
        SoundScript.instance.PlayAudio("Play");


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

    [ServerRpc]
    public void RestartGameServerRPC()
    {
        bool boolRandom = Random.value < 0.5f;
        RestartGameClientRPC(boolRandom);
    }

    [ClientRpc]
    public void RestartGameClientRPC(bool boolRandom)
    {
        for (int i = 0; i < BoardControllerMP.instance.numberOfLines; i++)
        {
            for (int j = 0; j < BoardControllerMP.instance.numberOfColumns; j++)
            {
                BoardControllerMP.instance.board[i, j] = 0;

                foreach(Transform t in BoardControllerMP.instance.canv.GetComponentInChildren<Transform>())
                {
                    if (t.gameObject.CompareTag("WinLine")) Destroy(t.gameObject);
                    else if (t.gameObject.CompareTag("Spot"))
                    {
                        t.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    }
                }
            }
        }

        BoardControllerMP.instance.winner = 0;
        BoardControllerMP.instance.playHappening = false;
        BoardControllerMP.instance.isHostTurn = boolRandom;
        BoardControllerMP.instance.isCircle = !boolRandom;
        BoardControllerMP.instance.isRestarting = false;
        BoardControllerMP.instance.gameEnded = false;
    }
}
