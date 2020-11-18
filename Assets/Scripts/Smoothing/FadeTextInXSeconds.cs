using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Script com o objetivo de aplicar fade out em um texto ao longo de um segundo. 
public class FadeTextInXSeconds : MonoBehaviour
{
    public Text toFade;
    public float fadeDuration;
    private float i;

    private void OnEnable()
    {
        i = 1;
        toFade = GetComponent<Text>();
    }


    private void Update()
    {
        if (i >= 0)
        {
            toFade.color = new Color(255, 255, 255, i);
            i -= Time.deltaTime / fadeDuration;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


}