using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearableShell : InteractableBase
{
    public ShellManager shellManager; 
    public float minSize;
    public float maxSize;

    public struct ShellData
    {
        public float minSize;
        public float maxSize;
    };

    public ShellData shellData;

    // Start is called before the first frame update
    private void Start()
    {
        if (maxSize == 0) maxSize = 100f;

        shellData.minSize = minSize;
        shellData.maxSize = maxSize;
        shellManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShellManager>();
       // SetInteract(true); 
    }

    public override void Action(InteractionManager manager)
    {
        print("Shell has been picked up");
        shellManager.EquipShell(transform.parent.gameObject); 

    }
    public ShellData GetShellData()
    {
        return shellData;
    }

}
