using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*      CRAB Size Manager 
 *      CrabSizeManager has five public functions.
 *      
 *      
 *      int GetCrabSize()
 *      Returns int from 0 to crabSizeMaximum representing size of crab. (0 is starting size)
 *      
 *      float GetCrabXP()
 *      Returns a percentage to the nearest hundredth (0 to 0.99) of progress toward the next crab size. If we're at max size, it returns 1f.
 * 
 *      AddXP()
 *      AddXP(int xpAmt)
 *      Adds xpAmt of XP towards the next size, or the crabXPDefaultIncrease (if no argument supplied).
 *      
 *      
 *      AddSizeChangeListener(Method)
 *      RemoveSizeChangeListener(Method)
 *      
 *      Adds/removes a listener for size changes.  
 *      
 *      Size changes return one argument (int sizeDifference), which represents the amount of change from previous size to the next.  This will most likely be 1 (went up) or -1 (went down).
 *      
 *      
 * 
 *      Paste in declarations for crab size manager
 *      --------------------------------------------------

        private CrabSizeManager _CrabSizeManager;
  
*       Paste into Update function
*       -----------------------------------------------------------------------

        GameObject __gm = GameObject.FindGameObjectWithTag("GameController");

        _CrabSizeManager = __gm.GetComponentInChildren<CrabSizeManager>();

        if (_CrabSizeManager == null) Debug.Log(this.name.ToString() + " couldn't find CrabSizeManager.");
        
        _CrabSizeManager.AddSizeChangeListener(Method);
 
 */


public class CrabSizeManager : MonoBehaviour
{
    // --- Configurable variables 
    [Tooltip("Max size of crab (Starts at 0)")]
    public int crabSizeMaximum = 2;
    [Tooltip("Max XP before leveling up to next size")]
    public int crabXPMaximum = 100;
    [Tooltip("Default XP awarded")]
    public int crabXPDefaultIncrease = 25;
    [Tooltip("Does crab keep XP gained over maximum")]
    public bool extraXPRollsOverToNextLevel = true;
    [Tooltip("Can the crab lose a size level if they lose enough XP")]
    public bool canCrabLoseSizeLevel = false;
    [Tooltip("A buffer zone of XP that can be lost before losing a size level")]
    public int CrabCanBeThisFarIntoNegativeBeforeLosingLevel = 10;
    // actual size (level?) and current XP 
    private int _crabSize = 0;
    private int _sizeXP = 0;

    #region OnSizeChangeDelegate event methods

    // --- the Delegate, for alerting other classes that we've changed XP levels
    public delegate void OnSizeChangeDelegate(int sizeDifference);
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


    #region public accessors / setters for XP and size

    public int GetCrabSize()
    {
        return _crabSize;
    }

public float GetCrabXP() // Returns a percentage to the nearest hundredth (0 to 0.99) of progress toward the next crab size (returns 1f when we are at max size)
    {
        if (_crabSize == crabSizeMaximum) return 1f;
        else
        { 
            float __per = _sizeXP / crabXPMaximum;
            __per = Mathf.Round(__per * 100) / 100;
            return __per;
        }
    }


public void AddXP() // Add default XP amount to the current XP 
    {
        _addXP(crabXPDefaultIncrease);
    }


public void AddXP(int xpAmt)
    {
        _addXP(xpAmt);

        //Debug.Log("CRAB SIZE MANAGER: AddXP( " + xpAmt.ToString() + " sent to internal method.");
    }


    #endregion

    private void _addXP(int __xpAmt)
    {
        // --- if we've gone in the red, start us at back at zero before adding new XP, to be faaaaaair

        //Debug.Log("CRAB SIZE MANAGER: Internal AddXP: received __xpAmt of " + __xpAmt.ToString());

        _sizeXP = Mathf.Max(0, _sizeXP);

        //Debug.Log("CRAB SIZE MANAGER: Internal AddXP set sizeXP to " + _sizeXP.ToString());

        _sizeXP += __xpAmt;

        //Debug.Log("CRAB SIZE MANAGER: Internal AddXP added _amt to sizexp, new total is: " + _sizeXP.ToString());

        _checkForSizeChange();
    }



private void _checkForSizeChange() // Checks if we've exceeded the XP limit in either direction
    {

        int __oldSize = _crabSize;

        if (_sizeXP >= crabXPMaximum)
        {
            _crabSize = Mathf.Min(_crabSize + 1,crabSizeMaximum);

            if (extraXPRollsOverToNextLevel) _sizeXP = crabXPMaximum - _sizeXP;
            else _sizeXP = 0;
        }
        else if(_sizeXP < -CrabCanBeThisFarIntoNegativeBeforeLosingLevel && canCrabLoseSizeLevel)
        {
            _crabSize = Mathf.Max(0, _crabSize - 1);

            if (_crabSize != __oldSize) _sizeXP = Mathf.Max(crabXPMaximum + _sizeXP,-CrabCanBeThisFarIntoNegativeBeforeLosingLevel); // if we're here, _sizeXP is less than 0, so we're remembering how much we lost and applying the difference to the previous level
            else _sizeXP = -CrabCanBeThisFarIntoNegativeBeforeLosingLevel; // if new crabSize is same as old, we're at 0, and we're going to clamp the _sizeXP loss to our negative buffer amount
            
        }

    if(__oldSize != _crabSize) // ok, we changed sizes for real, LET 'EM KNOW
        {
            if(OnSizeChange != null) // if this actually has any listeners in it, send the difference in size change
            {
                OnSizeChange(_crabSize - __oldSize);

            }
        }

    }



}
