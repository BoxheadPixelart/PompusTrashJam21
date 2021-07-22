using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fadeObj;
    public Fade fade; 



    private void OnTriggerEnter(Collider other)
    {
        print("TRIGGERED: " + other.name); 
        if (other.CompareTag("Player"))
        fadeObj.SetActive(true);
        fade.Fader(); 
    }
}
