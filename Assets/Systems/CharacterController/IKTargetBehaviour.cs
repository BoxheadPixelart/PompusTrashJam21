using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTargetBehaviour : MonoBehaviour
{
    public Transform[] targets;
    public Transform[] origins; 
    public LayerMask layer; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            StickToGround(i); 
        }
    }
    void StickToGround(int i)
    {
        if(Physics.Raycast(origins[i].position,Vector3.down,out RaycastHit hit,10f,layer))
        {
            targets[i].position = hit.point; 
        }
    }
}
