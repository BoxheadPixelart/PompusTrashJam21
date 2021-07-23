using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class SunDamage : MonoBehaviour
{

    public GameObject PlayerObject;
    public GameObject CenterOfMesh;
    public  ShellManager shellManager;

    public GameObject DirectionalLightSunReference;

    public float DistanceToRaycast;

    public float SunDPS;
    public float SunWithShellDPS;
    private float sunDPSDelta;
    private float sunWithShellDPSDelta;

    public float RestoreHealthInShadeDPS;
    private float RestoreHealthInShadeDPSDelta;

    public Health HealthScript;
    private float _health;
    private float _max_health;

    public int CheckForSunlightEveryXSteps = 1;
    private int sunlightCheckCounter = -1;
    public bool inSunlight;
    private bool prev_in_sunlight = false;
    public TextMeshProUGUI text; 

    public GameObject gameController;

    public bool RenderSunLightCheckPoints = false;
    public List<Transform> sunLightCheckPoints;

    public LayerMask IgnoreTheseLayers;
    private LayerMask acceptedLayersBitMask;

    private Vector3 angleToSun;

    public AudioSource sizzle; 
   
    #region OnSunChangeDelegate event methods

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnSunChangeDelegate(bool inSun, float direction);
    public event OnSunChangeDelegate OnSunChange;

    public void AddSunChangeListener(OnSunChangeDelegate __del)
    {
        OnSunChange += __del;
    }

    public void RemoveSunChangeListener(OnSunChangeDelegate __del)
    {
        OnSunChange -= __del;
    }


    #endregion






    // Start is called before the first frame update
    void Awake()
    {
        print("Start has occured"); 
       // gameController = GameObject.FindGameObjectWithTag("GameController");

        sunDPSDelta = SunDPS / 60f;
        sunWithShellDPSDelta = SunWithShellDPS / 60f;

        RestoreHealthInShadeDPSDelta = RestoreHealthInShadeDPS / 60f;


        //     shellManager = gameController.GetComponent<ShellManager>();

        //  HealthScript = gameController.GetComponent<Health>();
        print(HealthScript);
        print(shellManager); 
        //HealthScript.AddHealthChangeListener(OnHealthChange);

        _health = HealthScript.GetHealth();
        print(HealthScript + " is the health Ref");
        // We assume the player is starting at max health. This is sloppy.  Do not care
        _max_health = _health;



        // --- Creating the inverted bitmask for the raycast
       // LayerMask acceptedLayersBitMask = 0;
        
       // acceptedLayersBitMask = ~IgnoreTheseLayers;

      //  Debug.Log("Ignored Bitmask inverted: " + LayerMask.LayerToName(acceptedLayersBitMask) + " (" + (int)acceptedLayersBitMask + ")");

        foreach(Transform sclp in sunLightCheckPoints)
        {
           sclp.GetComponent<Renderer>().enabled = RenderSunLightCheckPoints;
           sclp.GetComponent<MeshRenderer>().enabled = RenderSunLightCheckPoints;
        }
        print("Start has ended");
    }

    
    void FixedUpdate()
    {
        _health = HealthScript.health; 
        if (inSunlight) // if we're in sunlight, check if we still are right away
        {
            
            inSunlight = AreWeInSunlight();
        }

        else // ok, not in sunlight currently, we're only going to check every StepsX to see if we're in sunlight - a little coyote time and it's easier on the procs
        {
            sunlightCheckCounter = (sunlightCheckCounter + 1) % CheckForSunlightEveryXSteps;

            if(sunlightCheckCounter == 0)
            {
                //Debug.Log("Not in sunlight, checking if things have changed");
               // PrintLocal("Not in sunlight, checking if things have changed");
                inSunlight = AreWeInSunlight();

            }
            

        }
      //  PrintLocal("In Sunlight is: "+ inSunlight);
        // if we're in sunlight, take damage
        if (inSunlight)
        {
          
            if (shellManager.ShellStatus())
            {
                print("TAKING DAMAGE SHELLED");
         
                HealthScript.SubtractHealth(sunWithShellDPSDelta);
            }
            else
            {
                print(" 1 TAKING DAMAGE NOT SHELLED");
                print("2 " + sizzle);
                print("3 " + HealthScript);
                print("4 " + sunDPSDelta);
                HealthScript.SubtractHealth(sunDPSDelta);
                print("5 " + sizzle.isPlaying);
                if (sizzle.isPlaying != true)
                {
                    print("6 ");
                    print(sizzle);
                    sizzle.Play();
                }
            
            }
        }
        else if(_health < _max_health)
        {
           
            HealthScript.AddHealth(RestoreHealthInShadeDPSDelta);

        }

    }


    private bool AreWeInSunlight()
    {

        acceptedLayersBitMask = ~IgnoreTheseLayers;

        bool areWe = false;

        /* Use this if we want to calculate based on the position fo the reference object
        angleToSun = SunReference.transform.position - CenterOfMesh.transform.position;

        angleToSun = angleToSun.normalized;
        */

        //Quaternion tempAngle = Quaternion.Inverse(DirectionalLightSunReference.transform.rotation);

        //angleToSun = tempAngle.eulerAngles.normalized;
     
        angleToSun = DirectionalLightSunReference.transform.forward * -1;

        //Debug.Log("ANgle to sun is: " + angleToSun);

        areWe = _CheckSunlightPoint(CenterOfMesh.transform);

        
        if (!areWe)
        {
            foreach (Transform slcp in sunLightCheckPoints)
            {
                areWe = _CheckSunlightPoint(slcp);

                if (areWe) break;
            }
        }


        if (areWe != prev_in_sunlight)
        {
            prev_in_sunlight = areWe;

            if (OnSunChange != null)
            {
                OnSunChange(areWe, 0);

            }
        }


        //Debug.Log("SUN DAMAGE: Are we in sunlight? " + areWe);
        return areWe;

    }


    private bool _CheckSunlightPoint(Transform slcp)
    {
      //  PrintLocal("We have Checked for Sun");
        Vector3 worldCoords = transform.TransformPoint(slcp.localPosition);

        Vector3 rayDraw = angleToSun * DistanceToRaycast * 50;

        //  Debug.DrawRay(worldCoords, rayDraw,Color.white, 0.5f, false);
        //return Physics.Raycast(slcp.position, angleToSun, DistanceToRaycast, acceptedLayersBitMask, QueryTriggerInteraction.Ignore);
      //  PrintLocal("THE SUN STATE IS: " + Physics.Raycast(worldCoords, angleToSun, DistanceToRaycast, acceptedLayersBitMask, QueryTriggerInteraction.Ignore).ToString()); ;;
        return !Physics.Raycast(worldCoords, angleToSun, DistanceToRaycast,acceptedLayersBitMask,QueryTriggerInteraction.Ignore);

    }


    public float GetAngleToSun()
    {
        //float ats = angleToSun - CenterOfMesh.transform.forward;


        float dotProd = Vector3.Dot(CenterOfMesh.transform.forward, angleToSun);

        float result = Mathf.Acos(dotProd) * Mathf.Rad2Deg;

        return result;

    }

    private void OnHealthChange(float health, float healthPercent)
    {
      print("SUN DAMAGE IS LISTIEN");  

        _health = health;

    }

    public bool InSunlight()
    {

        return inSunlight;
    }



}
