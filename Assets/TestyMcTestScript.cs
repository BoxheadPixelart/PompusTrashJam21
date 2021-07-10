using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestyMcTestScript : MonoBehaviour
{
    private string _myName = "TestMcTestObject: ";

    #region Testing the crab size functions

    private CrabSizeManager _CrabSizeManager;
    
    private void SizeChangeReporter(int __diff)
    {
        Debug.Log("SIZE CHANGE REPORTER EVENT: Crab sized changed to " + _CrabSizeManager.GetCrabSize().ToString() + ", a change of " + __diff.ToString());

    }



    // Start is called before the first frame update
    void Start()
    {
        GameObject __gm = GameObject.FindGameObjectWithTag("GameController");

        _CrabSizeManager = __gm.GetComponentInChildren<CrabSizeManager>();

        if (_CrabSizeManager == null) Debug.Log(this.name.ToString() + " couldn't find CrabSizeManager.");
        //else Debug.Log(_myName + "Found the crab size manager.");

        // Add my listening method HOPEFULLY OMG
        _CrabSizeManager.AddSizeChangeListener(SizeChangeReporter);


    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.I)) Debug.Log(_myName + _CrabSizeManager.GetCrabXP().ToString() + " Experience");
        if (Input.GetKeyDown(KeyCode.K)) Debug.Log(_myName + _CrabSizeManager.GetCrabSize().ToString() + " is current size");

        bool __pressedJ = Input.GetKeyDown(KeyCode.J);
        bool __pressedL = Input.GetKeyDown(KeyCode.L);

        if (__pressedJ || __pressedL) 
        {
            string __text = " 25 XP to crab.";
            int __amt = 25;

            if (__pressedJ)
            {
                __text = "Subtracted" + __text;
                __amt *= -1;
            }
            else __text = "Added " + __text;

            _CrabSizeManager.AddXP(__amt);

            Debug.Log(_myName + __text);

            Debug.Log("UPDATED: " + _CrabSizeManager.GetCrabXP().ToString() + " Experience and current level is: " + _CrabSizeManager.GetCrabSize().ToString());

        }
        

    }

    #endregion


}
