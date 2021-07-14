using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour
{
    /*  shell manager does a couple of things:
     *  Stores whether or not we're wearing a shell (boolean, and a function also)
     *  Manages the OnShellChange(bool) event, which notifies whether we have a shell (true) or don't (false)
     *  Puts a shell on or off, when supplied with EquipShell() and UnequipShell()
        
    */

    public GameObject PlayerHolder;
    public Transform playerMountingPoint;


    private KinematicCharacterController.Crab.CrabCharacterController characterController;

    private GameObject currentShell;
    private Transform shellMountingPoint;
    private Collider shellCollider;
    private WearableShell shellClass;
    private WearableShell.ShellData shellData;
    private WearableShell.ShellData nullShellData;
    private Rigidbody shellRigidbody;


    #region OnShellChangeDelegate event methods

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnShellChangeDelegate(bool wearingShell,WearableShell.ShellData shellData);
    public event OnShellChangeDelegate OnShellChange;

    public void AddShellChangeListener(OnShellChangeDelegate __del)
    {
        OnShellChange += __del;

    }

    public void RemoveSizeChangeListener(OnShellChangeDelegate __del)
    {
        OnShellChange -= __del;
    }


    #endregion



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Current shell status is " + ShellStatus().ToString());
        characterController = PlayerHolder.GetComponentInChildren<KinematicCharacterController.Crab.CrabCharacterController>();

    }


    public bool ShellStatus()
    {
        return (currentShell != null);
    }


   
    public void EquipShell(GameObject __shell)
    {
        if(ShellStatus())
        {
            UnequipShell();
        }

        // Get all the associated data for the shell
        currentShell = __shell;
        shellMountingPoint = __shell.transform;
        shellClass = __shell.GetComponent<WearableShell>();
        shellData = shellClass.GetShellData();
        shellCollider = __shell.GetComponentInChildren<Collider>();

        // register the collider with the character controller so we don't go ZOOMING away
        characterController.RegisterCollider(shellCollider);

        shellRigidbody = currentShell.GetComponentInChildren<Rigidbody>();
        
        shellRigidbody.isKinematic = true;

        // Physically bring the shell to us
        MountShell(shellMountingPoint);

        // Notify everyone of the new shell
        OnShellChange(true, shellData);

    }

    public void UnequipShell()
    {

        // Run the "pop off the shell" function
        UnmountShell();

        shellRigidbody.isKinematic = false;

        shellMountingPoint = null;
        shellClass = null;
        currentShell = null;
        shellRigidbody = null;

        characterController.DeregisterCollider(shellCollider);

        // Notify everyone of the null shell
        OnShellChange(false, nullShellData);

    }

    private void MountShell(Transform __shellMP)
    {

        __shellMP.SetParent(playerMountingPoint, true);
        __shellMP.transform.localPosition = new Vector3(0f, 0f, 0f);
        __shellMP.transform.localRotation = new Quaternion(0f, 0f, 0f,0f);


    }

    private void UnmountShell()
    {
        if(shellMountingPoint != null)
        {
            shellMountingPoint.SetParent(null);

        }

    }

}

