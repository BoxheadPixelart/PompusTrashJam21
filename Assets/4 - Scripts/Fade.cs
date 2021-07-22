using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    // Start is called before the first frame update
    public Image fade;
    float alpha;
    public Scene endgameScene;
   

    // Update is called once per frame
    void Update()
    {
        Color fadeColor = new Color();
        fadeColor.r = Color.white.r;
        fadeColor.g = Color.white.g;
        fadeColor.b = Color.white.b;
        fadeColor.a = alpha;
        fade.color = fadeColor;
    }
   
    public void Fader()
    {
        StartCoroutine(FadeOverTime());
    }
    IEnumerator FadeOverTime()
    {
        print("Fade Started"); 
        while (alpha < 1)
        {
            yield return new WaitForSeconds(0.01f);
            alpha += 0.005f;
        }
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(7);
    }
}
