using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPool : MonoBehaviour
{
    public Health hp;
    public bool isWet;
    float timer; 
   
    // Start is called before the first frame update
    void Start()
    {
        hp = GameObject.FindGameObjectWithTag("GameController").GetComponent<Health>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isWet)
        {
            timer += Time.deltaTime; 
            if (timer >= .05f)
            {
                timer = 0;
                hp.AddHealth(10); 
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isWet = true;
        }
   
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isWet = false;
        }
    }
}
