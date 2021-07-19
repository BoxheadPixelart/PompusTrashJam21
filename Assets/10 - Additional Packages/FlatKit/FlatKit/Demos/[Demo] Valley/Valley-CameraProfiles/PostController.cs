using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering; 
using UnityEngine.Rendering.Universal; 



public class PostController : MonoBehaviour
{
    public VolumeProfile profile;

    Bloom bL; 
    // Start is called before the first frame update
    void Start()
    {
        profile.TryGet<Bloom>(out Bloom bloom);
        bL = bloom;
    }

    // Update is called once per frame
    void Update()
    {
        bL.intensity.value += 0.001f; 
    }
}
