using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOnClick : MonoBehaviour
{

    public Image img;
    public Text txt;

    public float duration;
    public float minimum;

    private bool fadeInFinished;

    private void Start()
    {
        img = GetComponent<Image>();
        txt = GetComponentInChildren<Text>();
        StartCoroutine(StartFadeIn());
    }

    public void OnClick()
    {
        if(fadeInFinished)
        StartCoroutine(StartFadeOut());
    }

    public IEnumerator StartFadeIn()
    {
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime / duration;
            img.color = new Color(1, 1, 1, i);
            txt.color = new Color(1, 1, 1, i);
            yield return null;
        }

        fadeInFinished = true;
        yield return null;
    }


    public IEnumerator StartFadeOut()
    {
        GameObject.FindGameObjectWithTag("Transition").GetComponent<FadeOutAfterXSeconds>().canFade = true;
        float i = 1;
        while (i > minimum)
        {
            i -= Time.deltaTime / duration;
            img.color = new Color(1, 1, 1, i);
            txt.color = new Color(1, 1, 1, i);
            yield return null;
        }

        Destroy(txt.gameObject);
        Destroy(this.gameObject);
        yield return null;
    }
}
