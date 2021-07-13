using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : InteractableBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Action()
    {
        print("Item has been picked up");
    }
}
