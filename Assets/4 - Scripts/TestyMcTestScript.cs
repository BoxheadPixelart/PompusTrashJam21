using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestyMcTestScript : MonoBehaviour
{
    // GameObject.FindGameObjectWithTag("GameController").GetComponent<ShellManager>();
    private string _myName = "TestMcTestObject: ";

    private GameObject PlayerRootObject;
    private GameObject GameManager;
    private CrabSizeManager crabSizeManager;
    private Health healthScript;

    private void Start()
    {
        PlayerRootObject = GameObject.FindGameObjectWithTag("Player");
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        crabSizeManager = GameManager.GetComponentInChildren<CrabSizeManager>();
        healthScript = GameManager.GetComponentInChildren<Health>();
    
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            healthScript.SubtractHealth(100);

        }
    }


}
