using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSTManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip IntroTrack;
    public AudioClip Track2;
    public AudioClip Track3;
    public AudioSource layer1; 

    public void PlayIntro()
    {
        layer1.clip = IntroTrack; 
        layer1.Play();
    }
    public void Play2()
    {
        layer1.clip = Track2;
        layer1.Play();
    }
    public void Play3()
    {
        layer1.clip = Track3;
        layer1.Play();
    }
}
