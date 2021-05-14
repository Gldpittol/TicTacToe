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

    public int playLine = -1, playCol = -1;
    public int lastLineClicked = -1;
    public int lastColClicked = -1;
    public int numberOfLines = 3;
    public int numberOfColumns = 3;
    public int winner = 0;
    public int lineToPrintLine;
    public int colToPrintLine;
    public int[,] board;
    
    public float _angle;
  
    public bool isCircle = false;
    public bool playHappening = false;
    public bool didPlayerStart = true;
    public bool isRestarting = false;
    public bool isHostTurn;
    public bool gameEnded;

    public GameObject panelPhoton;
    public GameObject panelUnet;
    public GameObject photonWaitingPlayers;
    public GameObject photonButtons;
    public GameObject unetWaitingPlayers;
    public GameObject unetButtons;
    public GameObject playerHolder;
    public GameObject winLine;
    public GameObject canv;

    public Text debugText;
    public Text debugText2;

    public SpotScriptMP[] spots;

    private void Awake()
    {
        board = new int[numberOfLines, numberOfColumns];

        instance = this;
    }

    private void Update()
    {
        if (!IsOwner) return;
        else
        {
            int winner = CheckWin();

            if (winner != 0 && !gameEnded)
            {
                UpdateWinnerServerRPC(winner, lineToPrintLine, colToPrintLine, _angle);
                gameEnded = true;
            }
        }
    }

    public bool CanPlay()
    {
        return  ((IsHost && isHostTurn) || (!IsHost && !isHostTurn));
    }

    public void UpdateGame(int line, int col)
    {
        if (isHostTurn) board[line, col] = 1;
        else board[line, col] = -1;

        spots[line * 3 + col].UpdateGame();
    }

    public int CheckWin()
    {
        int i = 0;
        int j = 0;
        int tempCounter = 0;
        int toCompare = 0;
        int winner = 0;

        //Linhas
        for (i = 0; i < numberOfLines; i++)
        {
            toCompare = board[i, j];
            tempCounter = 0;

            for (j = 0; j < numberOfColumns; j++)
            {
                if (toCompare == board[i, j])
                {
                    tempCounter++;
                    if (tempCounter == numberOfColumns && toCompare != 0)
                    {
                        winner = toCompare;

                        //winner = AnnounceWinner(toCompare);
                        //print("winner " + winner);

                        //if (!IsPCTurn)
                            //PrintWinLine(i, -1, 90, toCompare);
                        lineToPrintLine = i;
                        colToPrintLine = -1;
                        _angle = 90;


                        return winner;
                    }
                }
            }

            j = 0;
            tempCounter = 0;
        }

        //Colunas
        for (i = 0; i < numberOfLines; i++)
        {
            toCompare = board[j, i];
            tempCounter = 0;

            for (j = 0; j < numberOfColumns; j++)
            {
                if (toCompare == board[j, i])
                {
                    tempCounter++;
                    if (tempCounter == numberOfColumns && toCompare != 0)
                    {
                        winner = toCompare;

                        //winner = AnnounceWinner(toCompare);
                        //print("winner " + winner);

                        //if (!IsPCTurn)
                            //PrintWinLine(-1, i, 0, toCompare);
                        lineToPrintLine = -1;
                        colToPrintLine = i;
                        _angle = 0;

                        return winner;
                    }
                }
            }

            j = 0;
            tempCounter = 0;
        }

        //Diagonal Principal
        toCompare = board[0, 0];
        tempCounter = 0;
        for (i = 0; i < numberOfLines; i++)
        {
            j = i;

            if (toCompare == board[i, j])
            {
                tempCounter++;
                if (tempCounter == numberOfColumns && toCompare != 0)
                {
                    winner = toCompare;

                    //winner = AnnounceWinner(toCompare);
                    // print("winner " + winner);

                    //if (!IsPCTurn)
                        //PrintWinLine(-1, -1, 45, toCompare);
                    lineToPrintLine = -1;
                    colToPrintLine = -1;
                    _angle = 45;

                    return winner;
                }
            }
        }

        //diagonal secundária
        toCompare = board[0, numberOfColumns - 1];
        tempCounter = 0;
        for (i = 0; i < numberOfLines; i++)
        {
            j = numberOfColumns - i - 1;

            if (toCompare == board[i, j])
            {
                tempCounter++;
                if (tempCounter == numberOfColumns && toCompare != 0)
                {
                    winner = toCompare;

                    //winner = AnnounceWinner(toCompare);
                    //print("winner " + winner);

                    //if (!IsPCTurn)
                        //PrintWinLine(-1, -1, 135, toCompare);
                    lineToPrintLine = 1;
                    colToPrintLine = -1;
                    _angle = 135;

                    return winner;
                }
            }
        }

        //print("No winners yet");
        //print("winner " + winner);

        //for (int a = 0; a < numberOfLines; a++)
        //{
        //    for (int b = 0; b < numberOfColumns; b++)
        //    {
        //        print(board[a, b] + " ");
        //    }
        //    print(" \n ");
        //}

        for (i = 0; i < numberOfLines; i++)
        {
            for (j = 0; j < numberOfColumns; j++)
            {
                if (board[i, j] == 0) return winner;
            }
        }

        return 9;
    }


    public void PrintWinLine(int line, int column, float rotation, int result)
    {
        GameObject temp;
        if (winLine)
        {
            if (line == -1 && column == -1)
            {
                temp = Instantiate(winLine, transform.position, Quaternion.identity, canv.transform);
                temp.transform.localScale = new Vector3(1f, 1f, 1f);
                temp.transform.localPosition = new Vector3(0f, 0f, 0f);
                temp.transform.Rotate(0f, 0f, rotation);
                ChangeLineColor(result, temp);
            }
            else if (line != -1 && column == -1)
            {
                temp = Instantiate(winLine, transform.position, Quaternion.identity, canv.transform);
                temp.transform.localScale = new Vector3(1f, 1f, 1f);
                temp.transform.localPosition = new Vector3(0f, 200f - (line * 200), 0f);
                temp.transform.Rotate(0f, 0f, rotation);
                ChangeLineColor(result, temp);
            }
            else if (line == -1 && column != -1)
            {
                temp = Instantiate(winLine, transform.position, Quaternion.identity, canv.transform);
                temp.transform.localScale = new Vector3(1f, 1f, 1f);
                temp.transform.localPosition = new Vector3(-200f + (column * 200), 0f, 0f);
                temp.transform.Rotate(0f, 0f, rotation);
                ChangeLineColor(result, temp);
            }
        }

        for (int i = 0; i < numberOfLines; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
            {
                if (board[i, j] != result && board[i, j] != 0) spots[i * 3 + j].img.sprite = spots[i * 3 + j].spriteDead;
            }
        }

        temp = null;
    }

    public void ChangeLineColor(int result, GameObject line)
    {
        if (result == -1)
        {
            line.GetComponent<Image>().color = new Color(255, 0, 0, 0);
            if(IsHost) SoundScript.instance.PlayAudio("Lose");
            else SoundScript.instance.PlayAudio("Win");

        }
        else
        {
            line.GetComponent<Image>().color = new Color(0, 0, 255, 0);
            if (IsHost) SoundScript.instance.PlayAudio("Win");
            else SoundScript.instance.PlayAudio("Lose");
        }
    }

    [ServerRpc]
    public void UpdateWinnerServerRPC(int winner, int lineToPrint, int colToPrint, float angle)
    {
        UpdateWinnerClientRPC(winner, lineToPrint, colToPrint, angle);
    }
    [ClientRpc]
    public void UpdateWinnerClientRPC(int _winner, int lineToPrint, int colToPrint, float newAngle)
    {
        gameEnded = true;
        winner = _winner;
        if(winner != 9) PrintWinLine(lineToPrint, colToPrint, newAngle, _winner);
    }

    public void RestartGame()
    {
        isRestarting = true;
    }
}
