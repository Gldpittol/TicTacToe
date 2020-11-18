using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandAndShrink : MonoBehaviour
{
    public float maxSize;
    public float minSize;

    public float speed;

    private void Start()
    {
        StartCoroutine(Expand());
    }

    public IEnumerator Expand()
    {
        Vector3 toIncrease = new Vector3(speed, speed,speed);
        while(transform.localScale.x < maxSize)
        {
            transform.localScale += toIncrease * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Shrink());
        yield return null;
    }

    public IEnumerator Shrink()
    {
        Vector3 toDecrease = new Vector3(speed, speed,speed);
        while (transform.localScale.x > minSize)
        {
            transform.localScale -= toDecrease * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Expand());
        yield return null;

    }

}
