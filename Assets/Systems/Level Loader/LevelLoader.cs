using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(4, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);

        SceneManager.LoadSceneAsync(6, LoadSceneMode.Additive);
       //ceneManager.LoadSceneAsync(7, LoadSceneMode.Additive);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
