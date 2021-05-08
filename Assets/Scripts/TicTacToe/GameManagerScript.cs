using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private static bool objectExists;

    private void OnEnable()
    {
        if (!objectExists)
        {
            //DontDestroyOnLoad(this);
            objectExists = true;
        }

        else
        {
            Destroy(GameObject.FindGameObjectWithTag("Transition"));
            Destroy(GameObject.FindGameObjectWithTag("Title"));
            Destroy(this);
        }

    }
}
