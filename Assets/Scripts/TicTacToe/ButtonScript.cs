using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ButtonScript : MonoBehaviour
{
   
    public void SetDifficulty(int difficulty)
    {
        BoardController temp = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoardController>();
        temp.difficulty = difficulty;
        temp.menuCanvas.gameObject.SetActive(false);
        temp.hasGameStarted = true;
    }


    public void Restart()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Single);
    }

    public void StartPVP()
    {
        BoardController temp = GameObject.FindGameObjectWithTag("GameController").GetComponent<BoardController>();
        temp.menuCanvas.gameObject.SetActive(false);
        temp.hasGameStarted = true;
        temp.playerTurnText.text = "Player 1's Turn";
        temp.playerWinText.text = "Player 1 Wins!";
        temp.PCTurnText.text = "Player 2's Turn";
        temp.PCWinText.text = "Player 2 Wins!";
        temp.didPlayerStart = true;
        temp.isPlayerTurn = true;
        temp.isPVP = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

}
