using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    public bool canInteract; 
    // Start is called before the first frame update

    public void InteractAction()
    {
        if (canInteract)
        {
            canInteract = false; 
            Action();
            ResetDelay(); 
            print(gameObject.name + " has been interacted with.");
        }
    }
    public void Action()
    {
        //this will be overriden by things that enherit from this class
    }
    IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(1);
        canInteract = true; 

    }
}
