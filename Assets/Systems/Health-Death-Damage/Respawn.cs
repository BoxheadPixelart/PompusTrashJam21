using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : DeathManager
{
    public GameObject PlayerRootObject;

    public Health healthScript;

    public Transform DefaultRespawnPoint;
    private Transform empty;


    // Start is called before the first frame update
    void Start()
    {
        healthScript.AddDeathListener(_OnDeath);
        if(DefaultRespawnPoint == null)
        {

            GameObject respwn = Instantiate(Object EmptyObject);

        }

    }

    
    private void _OnDeath(GameObject plyr)
    {
        


    }




}
