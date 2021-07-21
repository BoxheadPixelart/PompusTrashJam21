using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp_Distance_measure : MonoBehaviour
{
    public float Distance;
    public GameObject cubeToUse;


    
    // Update is called once per frame
    void Update()
    {
        Distance = Vector3.Distance(transform.position, cubeToUse.transform.position);
    }
}
