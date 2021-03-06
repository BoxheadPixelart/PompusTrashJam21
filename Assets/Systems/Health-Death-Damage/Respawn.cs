using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*          Hi there!
 *          
 *          - Respawn has a bunch of checks to make sure respawn points still exist when you die.  If they don't exist - how do we handle htat?
 *          
 *          
 *          - AddRespawnListener(Method)
 *                  OnRespawn passes GameObject PlayerRootObject (Player Hold V2)
 *          - RemoveRespawnListener(Method)
 *          
 *          - Use SetRespawnPoint(Transform) to set a new transform point
 * 
 * 
 * 
 */


public class Respawn : DeathManager
{
    public GameObject PlayerRootObject;
    private KinematicCharacterController.KinematicCharacterMotor PlayerMotorObject;
    public Transform DefaultRespawnPoint;
    private CrabSizeManager crabSizeManager;

    //[Tooltip("If the player hits the same Respawn save point two times, save the bigger size.")]
    public bool SaveSizeNoMatterWhat = true;

    private Transform RespawnPoint;
    private float respawnSize;
    private GameObject respawnShell;


    private ShellManager shellManager;

    public Health healthScript;

    
    public GameObject DefaultEmptyObject;

    private Transform empty;
    private Transform origRespawnPoint;

    GameObject go;




    #region OnRespawnDelegate event methods

    // --- the Delegate, for alerting other classes that we've respawned
    public delegate void OnRespawnDelegate(GameObject PlayerRootObject);
    public event OnRespawnDelegate OnRespawn;

    public void AddRespawnListener(OnRespawnDelegate __del)
    {
        OnRespawn += __del;

    }

    public void RemoveRespawnListener(OnRespawnDelegate __del)
    {
        OnRespawn -= __del;
    }

    public delegate void OnSetRespawnPointDelegate(GameObject PlayerRootObject);
    public event OnSetRespawnPointDelegate OnSetRespawnPoint;

    public void AddSetRespawnPointListener(OnSetRespawnPointDelegate __del)
    {
        OnSetRespawnPoint += __del;

    }

    public void RemoveSetRespawnPointListener(OnSetRespawnPointDelegate __del)
    {
        OnSetRespawnPoint -= __del;
    }




    #endregion




    // Start is called before the first frame update
    void Start()
    {
        GameObject __gm = GameObject.FindGameObjectWithTag("GameController");

        shellManager = __gm.GetComponent<ShellManager>();
        
        crabSizeManager = __gm.GetComponent<CrabSizeManager>();

        if(PlayerRootObject == null)
        {
            PlayerRootObject = GameObject.FindGameObjectWithTag("Player");
        }

        PlayerMotorObject = PlayerRootObject.GetComponentInChildren<KinematicCharacterController.KinematicCharacterMotor>();

        


        healthScript.AddDeathListener(_OnDeath);
        
        if(DefaultRespawnPoint == null)
        {
            GameObject go = Instantiate(DefaultEmptyObject, PlayerMotorObject.transform.position, PlayerMotorObject.transform.rotation);
            DefaultRespawnPoint = go.transform;
        }

        origRespawnPoint = DefaultRespawnPoint;
        RespawnPoint = DefaultRespawnPoint;

        respawnSize = crabSizeManager.GetCrabSize();
        respawnShell = null;

    }


    public void SetRespawnPoint(Transform respn) // Sets the player's respawn point to a new transform
    {
        RespawnPoint = respn;

        bool saveSize = SaveSizeNoMatterWhat || (!SaveSizeNoMatterWhat && respn != RespawnPoint);

        if(saveSize)
        {
            respawnSize = crabSizeManager.GetCrabSize();

            if (shellManager.ShellStatus())
            {
                respawnShell = shellManager.GetShellID();
            }

            else respawnShell = null;

        }



        // Let everyone know we've set our respawn point
        if (OnSetRespawnPoint != null)
        {

            OnSetRespawnPoint(PlayerRootObject);
        }


    }

    
    private void _OnDeath(GameObject plyr)
    {
        Transform respn;

        if (RespawnPoint != null) respn = RespawnPoint;
        else respn = origRespawnPoint;


        //PlayerMotorObject.transform.position = respn.transform.position;
        //PlayerMotorObject.transform.rotation = respn.transform.rotation;

        if(shellManager.ShellStatus())
        {
            if(shellManager.GetShellID() != respawnShell)
            {
                shellManager.UnequipShell();
            }
        }

        PlayerMotorObject.SetPositionAndRotation(respn.position, respn.rotation, true);

        if(OnRespawn != null)
        {
            OnRespawn(PlayerRootObject);
        }

        Debug.Log("Respawned (" + System.DateTime.Now + ")");

        healthScript.ResetHealth();
        crabSizeManager.SetSize(respawnSize);

        if (respawnShell != null)
        {
            shellManager.EquipShell(respawnShell);

        }



    }




}
