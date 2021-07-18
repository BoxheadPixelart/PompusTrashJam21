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

    public KeyCode QuitShellKey;
    
    public GameObject PlayerHolder;
    public Transform playerMountingPoint;
    [Tooltip("The Object the Shell will be parented to - to pull it into the level")]
    public GameObject ParentForShell;


    private KinematicCharacterController.Crab.CrabCharacterController characterController;

    private CrabSizeManager _sizeManager;
    private float _crabSize;

    private GameObject currentShell;
    private Transform shellMountingPoint;
    private Collider shellCollider;
    private WearableShell shellClass;
    private WearableShell.ShellData shellData;
    private WearableShell.ShellData nullShellData;
    private Rigidbody shellRigidbody;

    private bool amIThePlayer = false;


    #region OnShellChangeDelegate event methods

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnShellChangeDelegate(bool wearingShell,WearableShell.ShellData shellData);
    public event OnShellChangeDelegate OnShellChange;

    public void AddShellChangeListener(OnShellChangeDelegate __del)
    {
        OnShellChange += __del;

    }

    public void RemoveShellChangeListener(OnShellChangeDelegate __del)
    {
        OnShellChange -= __del;
    }


    #endregion



    // Start is called before the first frame update
    void Start()
    {
        _sizeManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<CrabSizeManager>();
        _sizeManager.AddSizeChangeListener(_SizeUpdate);


        Debug.Log("Current shell status is " + ShellStatus().ToString());
        characterController = PlayerHolder.GetComponentInChildren<KinematicCharacterController.Crab.CrabCharacterController>();

        if (ParentForShell == null) ParentForShell = playerMountingPoint.parent.gameObject;

        if(PlayerHolder.CompareTag("Player"))
        {
            amIThePlayer = true;

            if (QuitShellKey == KeyCode.None) QuitShellKey = KeyCode.Q;
        }



        

    }


    private void Update()
    {
        bool haveShell = ShellStatus();

        if(haveShell)
        {
            shellMountingPoint.position = playerMountingPoint.position;
            shellMountingPoint.rotation = playerMountingPoint.rotation;

            if (amIThePlayer)
            {
                if(Input.GetKeyDown(QuitShellKey))
                {
                    UnequipShell();

                }

            }

        }


    }


    public bool ShellStatus()
    {
        return (currentShell != null);
    }

    // Add a SizeUpdate here and toss off the shell if it's too small
    //UnequipShell();

    private void _SizeUpdate(float __size)
    {
        _crabSize = __size;

        if(ShellStatus())
        {
            if (__size > shellData.maxSize) UnequipShell();

        }

    }



    public void EquipShell(GameObject __shell)
    {
        WearableShell tempShellClass = __shell.transform.GetChild(0).GetComponent<WearableShell>();
        WearableShell.ShellData tempShellData = tempShellClass.GetShellData();

        if (Mathf.Clamp(_crabSize,tempShellData.minSize,tempShellData.maxSize) != _crabSize)
        {
            __shell.GetComponentInChildren<Rigidbody>().AddRelativeForce(-PlayerHolder.transform.right * 10, ForceMode.Impulse);
            

            return;
        }


        if(ShellStatus())
        {
            UnequipShell();
        }

        // Get all the associated data for the shell
        currentShell = __shell;
        shellMountingPoint = __shell.transform;
        shellClass = tempShellClass;
        shellData = tempShellData;
        shellCollider = __shell.transform.GetChild(0).GetComponent<Collider>();

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

        __shellMP.SetParent(ParentForShell.transform, true);
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

