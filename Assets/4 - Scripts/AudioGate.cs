using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGate : MonoBehaviour
{
    public OSTManager ost;
    public bool isTrack1;
    public bool isTrack2;
    public bool isTrack3;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

     
        if (!ost.layer1.isPlaying)
        {
            if (isTrack1)
            {
                ost.PlayIntro();
            }
            if (isTrack2)
            {

            }
            if (isTrack3)
            {

            }
        }
        }
    }
}
