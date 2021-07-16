using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public List<Transform> nearbyInteracts = new List<Transform>();
    public Transform playerRoot;
    public Transform grabPoint; 
    public KeyCode InteractKey;
    public bool canInteract;
    InteractableBase interact; 
    // Start is called before the first frame update
    void Start()
    {
        canInteract = true; 
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(InteractKey))
       {
            print("E pressed");
            print(canInteract);
        
            if (canInteract) // this is ugly and you should feel bad, break this into methods later 
            {

                interact = GetClosestEnemy(nearbyInteracts, playerRoot).gameObject.GetComponent<InteractableBase>();
                print(interact);
                if (interact.GetType() == typeof(WearableShell))
                {
                    interact.InteractAction(this);

                    print("YOU HAVE FOUND A SHELL");
                }
                if (interact.GetType() == typeof(ItemBase))
                {
                    interact.InteractAction(this);
                    canInteract = false; 
                    print("YOU HAVE FOUND An ITEM");
                }
                // do one more for NPCS LATER
            } 
            else
            {
                print(interact);
                if (interact is null)
                {
                    print("OOF"); 
                } else
                {   
                    if (interact.GetType() == typeof(ItemBase))
                    {
                        DropItem(); 
                    }
                }
            }
       }
    }
    public void DropItem()
    {
        interact.InteractAction(this);
        canInteract = true; 
        print("Dropped");
    }
    //
    Transform GetClosestEnemy(List<Transform> enemies, Transform fromThis)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (Transform potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
    public void ClearHeld()
    {
        nearbyInteracts.Clear();
        canInteract = true; 
    }
    //
    private void OnTriggerEnter(Collider other)
    {
        nearbyInteracts.Add(other.transform); 
    }
    private void OnTriggerExit(Collider other)
    {
        nearbyInteracts.Remove(other.transform);
    }
}
