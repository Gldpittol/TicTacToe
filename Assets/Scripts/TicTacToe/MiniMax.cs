using JetBrains.Annotations;
using System;
using UnityEngine;

public class MiniMax :MonoBehaviour
{
    public struct Play
    {
        public int Line, Column;
        public int Score;
    }

    public BoardController _boardController;

    public bool DoMiniMax(bool isPlayer, out Play bestPlay)
    {
        //Debug.LogError("Começou MiniMax");

        bool isMin = isPlayer;
        bestPlay.Line = bestPlay.Column = -1;
        bestPlay.Score = (isMin ? 999 : -999);

        //board.PrintBoard();


        int winner = _boardController.CheckWin();
        if (winner != 0)
        {
          //  print("win");
           // board.PrintBoard();
            switch (winner)
            {
                case 1:
                    bestPlay.Score = 100;
                    break;
                case -1:
                    bestPlay.Score = -100;
                    break;
            }

            return false;
        }

        bool foundPlay = false;
        //Debug.LogError("Não Achou Winner");

       // print("chegou aqui");

        for (int l = 0; l < _boardController.numberOfLines; l++)
        {
            for (int c = 0; c < _boardController.numberOfColumns; c++)
            {
                if (_boardController.board[l, c] == 0)
                {
                   // board.PrintBoard();


                    foundPlay = true;


                    if (isPlayer)
                        _boardController.board[l, c] = -1;
                    else
                        _boardController.board[l, c] = 1;


                    

                    DoMiniMax(!isPlayer, out var nextPlay);

                    if ((isMin && nextPlay.Score <= bestPlay.Score) || (!isMin && nextPlay.Score >= bestPlay.Score))
                    {
                      //  print("ATUALIZOU");
                        if (bestPlay.Score != nextPlay.Score || UnityEngine.Random.value < 0.11f)
                        {
                            bestPlay.Score = nextPlay.Score;
                            bestPlay.Line = l;
                            bestPlay.Column = c;
                        }
                    }

                    //Debug.LogError("Acabou um for");

                    _boardController.board[l, c] = 0;
                }                
            }
        }
        //Debug.LogError("Acabou segundo for");


        if (!foundPlay)
        {
            bestPlay.Score = 0;
        }


        return foundPlay;
        
    }
}