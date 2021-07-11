using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestyMcTestScript : MonoBehaviour
{
    private string _myName = "TestMcTestObject: ";

    public GameObject Player;
    public GameObject Shell;

    private bool mounted = false;
    private ShellManager shellManager;

    private void Start()
    {
        shellManager = Player.GetComponentInChildren<ShellManager>();

        Debug.Log("Press Q to wear / take off the shell.  Still need to figure out how to scale the mounting point position without scaling the shell you're wearing.");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (!mounted) shellManager.EquipShell(Shell);
            else shellManager.UnequipShell();

            mounted = !mounted;



        }
    }



}
