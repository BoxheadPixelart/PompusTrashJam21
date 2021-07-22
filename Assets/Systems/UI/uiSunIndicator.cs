using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiSunIndicator : MonoBehaviour
{
    public GameObject SunDamage;
    public GameObject SunSafe;



    private SunDamage sunDamageManager;
    private RectTransform SunDamageRect;
    private RectTransform SunSafeRect;

    private PanelController SunDamagePanelCon;
    private PanelController SunSafePanelCon;
    public AudioSource sizzle; 

    private bool inSun;
    private float angleToSun2D;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        sunDamageManager = player.GetComponentInChildren<SunDamage>();

        SunDamageRect = SunDamage.GetComponent<RectTransform>();
        SunSafeRect = SunSafe.GetComponent<RectTransform>();

        SunDamagePanelCon = SunDamage.GetComponent<PanelController>();
        SunSafePanelCon = SunSafe.GetComponent<PanelController>();

        sunDamageManager.AddSunChangeListener(SunChange);

        SunDamagePanelCon.TweenOut();
        SunSafePanelCon.Tween();

    }

    // Update is called once per frame
    void Update()
    {
        if(inSun)
        {
            angleToSun2D = sunDamageManager.GetAngleToSun();

        }


    }

    void SunChange(bool sunStatus,float direction)
    {
        if (sunStatus == inSun) return;

        inSun = sunStatus;

        switch(sunStatus)
        {
            // replace these method calls with whatever animation method calls we have to do
            case true:
                {
                    //SunDamage.SetActive(true);
                    //SunSafe.SetActive(false);
                    SunDamagePanelCon.Tween();
                   //izzle.Stop();

                    SunSafePanelCon.TweenOut();

                    break;
                }
            case false:
                {
                    SunSafePanelCon.Tween();
                    SunDamagePanelCon.TweenOut();
                    if (sizzle.isPlaying)
                    {
                        sizzle.Stop();
                    }
                  //sizzle.Stop();
                    //SunDamage.SetActive(false);
                    //SunSafe.SetActive(true);

                    break;
                }

        }

    }


}
