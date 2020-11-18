using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutAfterXSeconds : MonoBehaviour
{
    public float fadeDelay;
    public float duration;
    public float minimumFade;

    private Image img;

    public bool canFade;
    public bool fadeStarted;

    private void Start()
    {
        img = GetComponent<Image>();
    }
    private void Update()
    {
        if(canFade && !fadeStarted)
        {
            StartCoroutine(FadeUntilX(minimumFade, duration));
            fadeStarted = true;
        }
    }

    public IEnumerator FadeUntilX(float minimum, float duration)
    {
        yield return new WaitForSeconds(fadeDelay);
        float i = 1;
        while (i > minimum)
        {
            i -= Time.deltaTime / duration;
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
        Destroy(this.gameObject);
        yield return null;
    }
}
