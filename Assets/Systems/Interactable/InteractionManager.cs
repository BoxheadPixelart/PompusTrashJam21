using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public List<GameObject> nearbyInteracts = new List<GameObject>(); 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        nearbyInteracts.Add(other.gameObject); 
    }
    private void OnTriggerExit(Collider other)
    {
        nearbyInteracts.Remove(other.gameObject); 
    }
}
