using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public List<Transform> nearbyInteracts = new List<Transform>();
    public Transform playerRoot; 
    public KeyCode InteractKey;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(InteractKey))
        {
            print("E pressed"); 
            InteractableBase interact = GetClosestEnemy(nearbyInteracts, playerRoot).gameObject.GetComponent<InteractableBase>();
            print(interact);
            interact.InteractAction(); 
        }
        
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
