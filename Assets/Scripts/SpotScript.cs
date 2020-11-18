using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpotScript : MonoBehaviour
{
    public int line;
    public int column;

    public Image img;
    public Sprite spriteX;
    public Sprite spriteO;
    public Sprite spriteDead;

    public BoardController _boardController;

    public bool changed;
    public bool winHappened;

    void Awake()
    {
        changed = false;
        winHappened = false;
    }

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {

        if(_boardController.board[line,column] != 0 && !changed)
        {

            if (_boardController.isCircleNext)
            {
                img.sprite = spriteO;
                _boardController.isCircleNext = false;
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
                transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                changed = true;
            }

            else
            {
                img.sprite = spriteX;
                _boardController.isCircleNext = true;
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
                transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                changed = true;
            }
            _boardController.board[line, column] = 1;
            if(_boardController.currentTurn < 10)
            {
                _boardController.PCTurnText.gameObject.SetActive(false);
                _boardController.playerTurnText.gameObject.SetActive(true);
            }

            StartCoroutine(PlayAudioAndAnim());
        }

        if (_boardController.isGameOver && !winHappened)
        {
            int winner = _boardController.CheckWin();
            winHappened = true;

            //print(line + " " + column + "    " +  (_boardController.board[line, column] + "    " + winner));

            if ((_boardController.board[line, column] == 1 && winner == -1) || (_boardController.board[line, column] == -1 && winner == 1))
            {
                img.sprite = spriteDead;
                //StartCoroutine(FadeUntilX(0.2f,2f));
            }
        }
    }
    public void OnButtonClick()
    {
        if((_boardController.isPlayerTurn && !_boardController.isPVP) || _boardController.isPVP)
        {
            if (_boardController.board[line, column] == 0 && _boardController.isGameOver == false)
            {
                changed = true;
                _boardController.currentTurn++;

                if (_boardController.isPlayerTurn)
                {
                    _boardController.board[line, column] = -1;
                    _boardController.isPlayerTurn = false;
                }
                else
                {
                    _boardController.board[line,column] = 1;
                    _boardController.isPlayerTurn = true;
                }
                if (_boardController.isCircleNext)
                {
                    img.sprite = spriteO;
                    _boardController.isCircleNext = false;
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
                    transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                }
                else
                {
                    img.sprite = spriteX;
                    _boardController.isCircleNext = true;
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
                    transform.localScale = new Vector3(0.8f, 0.8f, 1f);
                }

                _boardController.CheckWin();

                NextTurn();

            }
            if (!_boardController.isGameOver) StartCoroutine(PlayAudioAndAnim());
            changed = true;
        }

    }

    public void NextTurn()
    {
        if (_boardController.isGameOver == false)
        {
            if (_boardController.isPlayerTurn)
            {
                if(!_boardController.IsPCTurn && _boardController.currentTurn < 10)
                {
                    _boardController.PCTurnText.gameObject.SetActive(false);
                    _boardController.playerTurnText.gameObject.SetActive(true);
                }
            }
            else
            {
                if (!_boardController.IsPCTurn && _boardController.currentTurn < 10)
                {
                    _boardController.playerTurnText.gameObject.SetActive(false);
                    _boardController.PCTurnText.gameObject.SetActive(true);
                }
                if (!_boardController.isPVP) StartCoroutine(_boardController.CallAI());
            }
        }
    }

    public IEnumerator FadeUntilX(float minimum, float duration)
    {
        float  i = 1;
        while(i > minimum)
        {
            i -= Time.deltaTime / duration;
            img.color = new Color(i, i, i, 1f);
            yield return null;
        }
        yield return null;
    }

    public IEnumerator PlayAudioAndAnim()
    {
        GameObject.FindGameObjectWithTag("AudioManager").GetComponent<SoundScript>().PlayAudio("Play");

        Vector3 toSubtract = new Vector3(0.01f, 0.01f, 0.01f);
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        while(transform.localScale.x > 1)
        {
            transform.localScale -= toSubtract;
            yield return null;
        }

        yield return null;
    }
}
