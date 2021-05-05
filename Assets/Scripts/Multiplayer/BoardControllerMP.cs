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

    public int[,] board;

    public int numberOfLines = 3;
    public int numberOfColumns = 3;

    public int winner = 0;

    public bool gameEnded;

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
                UpdateWinnerServerRPC(winner);
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
                        //    PrintWinLine(i, -1, 90, toCompare);

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
                        //    PrintWinLine(-1, i, 0, toCompare);

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
                    //    PrintWinLine(-1, -1, 45, toCompare);

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
                    //    PrintWinLine(-1, -1, 135, toCompare);

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

        return winner;
    }

    [ServerRpc]
    public void UpdateWinnerServerRPC(int winner)
    {
        UpdateWinnerClientRPC(winner);
    }
    [ClientRpc]
    public void UpdateWinnerClientRPC(int _winner)
    {
        winner = _winner;
    }
}
