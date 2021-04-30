using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardController : MonoBehaviour
{

    [Header("Board Settings")]
    public int numberOfLines;
    public int numberOfColumns;
    public int[,] board;

    [Header("Game Settings")]
    public int currentTurn;

    public bool isPlayerTurn;
    public bool isCircleNext;
    public bool hasGameStarted;
    public bool didPlayerStart;
    public bool isGameOver;


    [Header("AI Settings")]
    public int difficulty;

    public float IADelay;

    public bool isPVP;
    public bool IsPCTurn;
    public bool isRandomPlay;


    [Header("Texts")]
    public Text playerTurnText;
    public Text PCTurnText;
    public Text playerWinText;
    public Text PCWinText;

    [Header("Script References")]
    public MiniMax miniMaxScript;

    [Header("Prefabs")]
    public GameObject winLine;

    [Header("Canvas")]
    public Canvas canv;
    public Canvas menuCanvas;


    private void Awake()
    {
        board = new int[numberOfLines, numberOfColumns];
        currentTurn = 1;
        isRandomPlay = true;

        if(UnityEngine.Random.value < 0.5)
        {
            isPlayerTurn = true;
            didPlayerStart = true;
        }
        else
        {
            isPlayerTurn = false;
            didPlayerStart = false;
        }

        //PrintBoard();
    }

    void Update()
    {
        if(hasGameStarted)
        {
            if (!didPlayerStart)
            {
                if(!isPVP) StartCoroutine(CallAI());
                PCTurnText.gameObject.SetActive(true);
            }
            else
            {
                playerTurnText.gameObject.SetActive(true);
            }
            hasGameStarted = false;
        }

        if(currentTurn == 10 && !isGameOver)
        {
            playerWinText.text = "It's a Tie!";
            playerWinText.gameObject.SetActive(true);
            playerTurnText.gameObject.SetActive(false);
            PCTurnText.gameObject.SetActive(false);
            isGameOver = true;
        }
    }

    public IEnumerator CallAI()
    {
        if(currentTurn <10 && !isGameOver)
        {
            isRandomPlay = DecideIfRandom();

            IsPCTurn = true;
            isPlayerTurn = false;
            yield return new WaitForSeconds(IADelay);

            if (!isRandomPlay)
            {
                miniMaxScript.DoMiniMax(false, out var bestPlay);
                board[bestPlay.Line, bestPlay.Column] = -1;
            }
            else
                PlayOnRandomSpot();

            //print(bestPlay.Line + " " + bestPlay.Column + "Score = " + bestPlay.Score);
            currentTurn++;

            yield return new WaitForSeconds(IADelay);
            isPlayerTurn = true;
            IsPCTurn = false;
            CheckWin();
            //print("Minimax finished, printing board");
            //PrintBoard();
        }
    }

    public bool DecideIfRandom()
    {
        if (difficulty == 0)
            return true;

        else if (difficulty == 2)
            return false;

        else
        {
            isRandomPlay = !isRandomPlay;
            return isRandomPlay;
        }
    }

    public void PlayOnRandomSpot()
    {
        int line;
        int column;

        do
        {
            line = UnityEngine.Random.Range(0,3);
            column = UnityEngine.Random.Range(0, 3);
        } 
        while (board[line,column] != 0);

        board[line, column] = 1;

    }
    public void PrintBoard()
    {
        print("Printing board");

        for (int i = 0; i < numberOfLines; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
                print("line " + i + "column " + j + "value " + board[i,j] + " ");
        }
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
                if(toCompare == board[i,j])
                {
                    tempCounter++;
                    if(tempCounter == numberOfColumns && toCompare != 0)
                    {
                         winner = AnnounceWinner(toCompare);
                        //print("winner " + winner);

                        if (!IsPCTurn)
                            PrintWinLine(i, -1, 90, toCompare);

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
                         winner = AnnounceWinner(toCompare);
                        //print("winner " + winner);

                        if (!IsPCTurn)
                            PrintWinLine(-1, i, 0, toCompare);

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
                     winner = AnnounceWinner(toCompare);
                    // print("winner " + winner);

                    if (!IsPCTurn)
                        PrintWinLine(-1, -1, 45, toCompare);

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
                     winner = AnnounceWinner(toCompare);
                    //print("winner " + winner);

                    if (!IsPCTurn)
                        PrintWinLine(-1, -1, 135, toCompare);

                    return winner;
                }
            }
        }

        //print("No winners yet");
        //print("winner " + winner);
        return winner;
       }

    public void PrintWinLine(int line, int column, float rotation, int result)
    {
        GameObject temp;
        if(winLine)
        {
            if (line == -1 && column == -1)
            {
                temp = Instantiate(winLine, transform.position, Quaternion.identity);
                temp.transform.SetParent(canv.transform);
                temp.transform.localScale = new Vector3(1f, 1f, 1f);
                temp.transform.localPosition = new Vector3(0f, 0f, 0f);
                temp.transform.Rotate(0f, 0f, rotation);
                ChangeLineColor(result, temp);
            }
            else if (line != -1 && column == -1)
            {
                temp = Instantiate(winLine, transform.position, Quaternion.identity);
                temp.transform.SetParent(canv.transform);
                temp.transform.localScale = new Vector3(1f, 1f, 1f);
                temp.transform.localPosition = new Vector3(0f, 200f - (line * 200), 0f);
                temp.transform.Rotate(0f, 0f, rotation);
                ChangeLineColor(result, temp);
            }
            else if (line == -1 && column != -1)
            {
                temp = Instantiate(winLine, transform.position, Quaternion.identity);
                temp.transform.SetParent(canv.transform);
                temp.transform.localScale = new Vector3(1f, 1f, 1f);
                temp.transform.localPosition = new Vector3(-200f + (column * 200), 0f, 0f);
                temp.transform.Rotate(0f, 0f, rotation);
                ChangeLineColor(result, temp);
            }
        }

        winLine = null;     
    }
    
    public void ChangeLineColor(int result, GameObject line)
    {
        if(didPlayerStart && result == -1)
            line.GetComponent<Image>().color = new Color(255, 0, 0, 0);
        else if(!didPlayerStart && result == 1)
            line.GetComponent<Image>().color = new Color(255, 0, 0, 0);
        else
            line.GetComponent<Image>().color = new Color(0, 0, 255, 0);
    }

    public int AnnounceWinner(int result)
    {
        if (result == 1)
        {
            if(!isGameOver)
            if (!IsPCTurn)
            {
                PCWinText.gameObject.SetActive(true);
                PCTurnText.gameObject.SetActive(false);
                playerTurnText.gameObject.SetActive(false);

                if(!isPVP) GameObject.FindGameObjectWithTag("AudioManager").GetComponent<SoundScript>().PlayAudio("Lose");
                else GameObject.FindGameObjectWithTag("AudioManager").GetComponent<SoundScript>().PlayAudio("Win");
                if (isPVP) GameObject.FindGameObjectWithTag("AudioManager").GetComponent<SoundScript>().PlayAudio("Play");


                    isGameOver = true;
                }
            return 1;
        }
        else
        {
            if (!isGameOver)
            if (!IsPCTurn)
            {
                playerWinText.gameObject.SetActive(true);
                PCTurnText.gameObject.SetActive(false);
                playerTurnText.gameObject.SetActive(false);

                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<SoundScript>().PlayAudio("Win");
                if(isPVP) GameObject.FindGameObjectWithTag("AudioManager").GetComponent<SoundScript>().PlayAudio("Play");

                isGameOver = true;
                }
            return -1;
        }

        
    }

}
