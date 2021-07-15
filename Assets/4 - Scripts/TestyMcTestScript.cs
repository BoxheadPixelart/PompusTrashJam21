using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestyMcTestScript : MonoBehaviour
{
    private string _myName = "TestMcTestObject: ";

    public GameObject PlayerRootObject;
    public GameObject GameManager;
    public CrabSizeManager crabSizeManager;
    public Health healthScript;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            healthScript.SubtractHealth(100);

        }
    }


}
