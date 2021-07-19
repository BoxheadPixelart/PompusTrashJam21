using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*      CRAB Size Manager 
 *
 *      
 *      PauseGrowthChange() / UnpauseGrowthChange() - obviously
 *      
 *      
 *      float GetCrabSize()
 *      Returns float from 0f to 100f representing size of crab from starting size to final size.
 *      
 *      float GetCrabSizeRelative()
 *      Returns float from 0f to 1f representing size of crab from starting size to final size.
 *      
 *      AddSize()
 *      AddSize(float sizeAmt)
 *      Adds xpAmt of XP towards the next size, or the default amount calculated based on growth over "crabSizeTimeToFullGrown" seconds.
 *      
 *      Will accept negative sizeAmt.
 *
 *
 *      SetSize(float)
 *      Sets crab size to float, between 0 and crabSizeMaximum
 *
 *
 *      AddSizeChangeListener(Method)
 *      RemoveSizeChangeListener(Method)
 *      
 *      Adds/removes a listener for size changes.  (reports every change of a 0.1 or more of size)
 *      
 *      Size changes return one argument (float newSize), which is the new size float.  
 *      
 *      
 * 
 *      Paste in declarations for crab size manager
 *      --------------------------------------------------

        private CrabSizeManager _CrabSizeManager;
  
*       Paste into Start function
*       -----------------------------------------------------------------------

        GameObject __gm = GameObject.FindGameObjectWithTag("GameController");

        _CrabSizeManager = __gm.GetComponentInChildren<CrabSizeManager>();

        if (_CrabSizeManager == null) Debug.Log(this.name.ToString() + " couldn't find CrabSizeManager.");
        
        _CrabSizeManager.AddSizeChangeListener(Method);
 
 */

/// <summary>
/// Component that manages the abstract variable "Size" that controls other behaviors.
/// </summary>
/// 

public class CrabSizeManager : MonoBehaviour
{
    // --- Configurable variables 
    [Tooltip("Time (in seconds) it takes crab to reach full size with steady growth")]
    public float crabSizeTimeToFullGrown = 300f;

    [Tooltip("OnSizeChangeDelegate will report every time growth total exceeds:")]
    public float reportIfGrowthIsGreaterThan = 0.1f;


    private readonly float crabSizeMaximum = 100f;

    //[Tooltip("Default Size increase - per second - with every AddSize() call")]
    private float crabSizeDefaultIncrease = 25f;

    // Float size tracker of crab, and the last time crab size was reported

        [SerializeField] private float _crabSize = 0;
    private float _crabSizeLastReported = 0;

    public bool DEBUGDisableGrowth;


    #region OnSizeChangeDelegate event methods

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnSizeChangeDelegate(float newSize);
    public event OnSizeChangeDelegate OnSizeChange;

    public void AddSizeChangeListener(OnSizeChangeDelegate __del)
    {
        OnSizeChange += __del;

    }

    public void RemoveSizeChangeListener(OnSizeChangeDelegate __del)
    {
        OnSizeChange -= __del;
    }


    #endregion


    #region public accessors / setters for Size

    public float GetCrabSize()
    {
        return _crabSize;
    }

    public float GetCrabSizeRelative()
    {
        return _crabSize / 100f;
    }



    public void PauseGrowthChange()
    {
        DEBUGDisableGrowth = true;

    }

    public void UnpauseGrowthChange()
    {
        DEBUGDisableGrowth = false;

    }




    public void AddSize() // Add default XP amount to the current XP 
    {
        _addSize(crabSizeDefaultIncrease);
    }


public void AddSize(float __sizeAmt)
    {
        if(!DEBUGDisableGrowth) _addSize(__sizeAmt);
    }


    #endregion

    private void Start()
    {
        crabSizeDefaultIncrease = (crabSizeMaximum/60)/crabSizeTimeToFullGrown;
    }



    private void _addSize(float __sizeAmt)
    {
        SetSize(__sizeAmt + _crabSize);

        
    }


    public void SetSize(float __sizeAmt)
    {
      //  if (!DEBUGDisableGrowth)
      //  {
            _crabSize = Mathf.Clamp(__sizeAmt, 0f, crabSizeMaximum);
            _checkForSizeChange();
      //  }
    }



private void _checkForSizeChange() // Checks if we've exceeded the XP limit in either direction
    {
        float __diff = Mathf.Abs(_crabSize - _crabSizeLastReported);

        if(__diff >= reportIfGrowthIsGreaterThan)
        {
            if (OnSizeChange != null) // if this actually has any listeners in it, send the difference in size change
            {

                OnSizeChange(_crabSize);
                _crabSizeLastReported = _crabSize;

            }

        }
 
    }



}
