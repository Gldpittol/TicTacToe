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

public class BoardControllerMP : NetworkBehaviour
{
    public static BoardControllerMP instance;

    public bool isCircle = false;

    public bool isHostTurn;

    public Text debugText;

    public Text debugText2;

    public int playLine = -1, playCol = -1;

    public int lastLineClicked = -1;

    public int lastColClicked = -1;

    public SpotScriptMP[] spots;

    public bool playHappening = false;
    private void Awake()
    {
        instance = this;
    }

    public bool CanPlay()
    {
        return  ((IsHost && isHostTurn) || (!IsHost && !isHostTurn));
    }

    public void UpdateGame(int index)
    {
        spots[index].UpdateGame();
    }
}
