using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour
{
    public bool canInteract;
    // Start is called before the first frame update
    public void InteractAction(InteractionManager manager)
    {
        print("Interact Action has been Called");
        print("Can Interract is: " + canInteract );
        if (canInteract)
        {

            SetInteract(false); 
            print("Can Interract is: " + canInteract);
            Action(manager);
            StartCoroutine(ResetDelay()); 
            print(gameObject.name + " has been interacted with.");
        }
    }
    virtual public void Action(InteractionManager manager)
    {

        print("This is the base interactable action"); 
        //this will be overriden by things that enherit from this class
    }
    IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(1);
        canInteract = true; 

    }
    public void SetInteract(bool choice)
    {
        canInteract = choice;
    }
}
