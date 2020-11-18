using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImageInXSeconds : MonoBehaviour
{
    public Image toFade;
    public float fadeDuration;
    private float i;

    private void OnEnable()
    {
        i = 0;
        toFade = GetComponent<Image>();
    }


    private void Update()
    {
        if (i < 1)
        {
            toFade.color = new Color(toFade.color.r, toFade.color.g, toFade.color.b, i);
            i += Time.deltaTime / fadeDuration;
        }
        else
        {
            i = 1;
        }
    }
}
