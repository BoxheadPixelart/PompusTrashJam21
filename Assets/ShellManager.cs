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

    public GameObject Player;
    public GameObject ShellMountingPoint;
    private GameObject currentShell;

    #region OnShellChangeDelegate event methods

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnShellChangeDelegate(bool wearingShell);
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


    }

    public void UnequipShell()
    {

    }

}

