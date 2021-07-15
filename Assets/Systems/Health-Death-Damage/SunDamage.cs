using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunDamage : MonoBehaviour
{
    public GameObject PlayerObject;
    public GameObject CenterOfMesh;
    private ShellManager shellManager;

    public GameObject DirectionalLightSunReference;

    public float DistanceToRaycast;

    public float SunDPS;
    public float SunWithShellDPS;
    private float sunDPSDelta;
    private float sunWithShellDPSDelta;

    public Health HealthScript;

    public int CheckForSunlightEveryXSteps = 1;
    private int sunlightCheckCounter = -1;
    public bool inSunlight;

    private GameObject gameController;

    public List<Transform> sunLightCheckPoints;

    public LayerMask IgnoreTheseLayers;
    private LayerMask acceptedLayersBitMask;

    private Vector3 angleToSun;
    

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");

        sunDPSDelta = SunDPS / 60f;
        sunWithShellDPSDelta = SunWithShellDPS / 60f;

        shellManager = gameController.GetComponent<ShellManager>();

        HealthScript = gameController.GetComponent<Health>();

        // --- Creating the inverted bitmask for the raycast
        LayerMask acceptedLayersBitMask = 0;
        /*
        Debug.Log("Default ignored Bitmask is: " + acceptedLayersBitMask);

        foreach(LayerMask ignoreLayer in IgnoreTheseLayers)
        {
            acceptedLayersBitMask = acceptedLayersBitMask | ignoreLayer;
            Debug.Log("Ignored Bitmask included " + ignoreLayer + " and is now " + acceptedLayersBitMask);
        }
        acceptedLayersBitMask = ~acceptedLayersBitMask;
         */

        //acceptedLayersBitMask = LayerMask.GetMask("Default");

        acceptedLayersBitMask = ~IgnoreTheseLayers;

        Debug.Log("Ignored Bitmask inverted: " + LayerMask.LayerToName(acceptedLayersBitMask) + " (" + (int)acceptedLayersBitMask + ")");

        foreach(Transform sclp in sunLightCheckPoints)
        {
            sclp.GetComponent<Renderer>().enabled = false;
            sclp.GetComponent<MeshRenderer>().enabled = false;
        }

    }

    
    void FixedUpdate()
    {

        if(inSunlight) // if we're in sunlight, check if we still are right away
        {
            
            inSunlight = AreWeInSunlight();
        }

        else // ok, not in sunlight currently, we're only going to check every StepsX to see if we're in sunlight - a little coyote time and it's easier on the procs
        {
            sunlightCheckCounter = (sunlightCheckCounter + 1) % CheckForSunlightEveryXSteps;

            if(sunlightCheckCounter == 0)
            {
                //Debug.Log("Not in sunlight, checking if things have changed");
                inSunlight = AreWeInSunlight();
            }
            

        }

        // if we're in sunlight, take damage
        if (inSunlight)
        {
            if (shellManager.ShellStatus()) HealthScript.SubtractHealth(sunWithShellDPSDelta);
            else HealthScript.SubtractHealth(sunDPSDelta);
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

        //Debug.Log("SUN DAMAGE: Are we in sunlight? " + areWe);
        return areWe;

    }


    private bool _CheckSunlightPoint(Transform slcp)
    {
        Vector3 worldCoords = transform.TransformPoint(slcp.localPosition);

        Vector3 rayDraw = angleToSun * DistanceToRaycast * 50;

        Debug.DrawRay(worldCoords, rayDraw,Color.white, 0.5f, false);
        //return Physics.Raycast(slcp.position, angleToSun, DistanceToRaycast, acceptedLayersBitMask, QueryTriggerInteraction.Ignore);

        return !Physics.Raycast(worldCoords, angleToSun, DistanceToRaycast,acceptedLayersBitMask,QueryTriggerInteraction.Ignore);

    }

}
