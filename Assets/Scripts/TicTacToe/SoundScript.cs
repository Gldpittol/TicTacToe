using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    public static SoundScript instance;

    public AudioSource audSource;
    public AudioClip playClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audSource = GetComponent<AudioSource>();
    }
    public void PlayAudio(string name)
    {
        switch(name)
        {
            case "Play":
                audSource.PlayOneShot(playClip, 0.5f);
                break;
            case "Win":
                audSource.PlayOneShot(winClip, 0.5f);
                break;
            case "Lose":
                audSource.PlayOneShot(loseClip, 0.5f);
                break;
        }
    }
}
