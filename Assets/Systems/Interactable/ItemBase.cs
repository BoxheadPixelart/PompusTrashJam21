using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : InteractableBase
{
    public bool IsPickedUp;
    //
    private MeshFilter meshFilter;
    private Rigidbody rb;
    private Collider collider;
    private Renderer renderer;
    //
    public Rigidbody Rb
    {
        get {
            if (rb is null)
            {
                rb = GetComponent<Rigidbody>();
            }
            return rb;
        }
        set
        {
            rb = value;
        }

    }
    private Transform holder;
    private Vector3 itemDir;
    private Vector3 gItemDir;
    private Vector3 itemDirVelo;
    public Vector3 debugRot;
    //
    bool isPickedUp;
    public Transform pickupTarget;

    void Start()
    {
        SetInteract(true);
        rb.GetComponent<Rigidbody>(); 
    }
    private void Update()
    {
   
        if (isPickedUp)
        {
            MoveToGrabPoint();
        }
        else
        {
            itemDir = Rb.rotation.eulerAngles;
        }
    }
    // Update is called once per frame
    public override void Action(InteractionManager manager)
    {
        if (isPickedUp != true)
        {
            pickupTarget = manager.grabPoint;
            Pickup(manager.grabPoint, manager.playerRoot);
            print("Item has been picked up");
        } else
        {
            Drop(); 
        }
      
    }
    //
    public void MoveToGrabPoint()
    {
        print(Rb); 
        Vector3 heading = (pickupTarget.position - Rb.position);
        rb.velocity = (heading * 25);
        gItemDir = holder.rotation.eulerAngles;
        SpringRotateToHand();
    }

    void SpringRotateToHand()
    {
        Vector3 look = (pickupTarget.position - holder.transform.position);
        Quaternion rot = Quaternion.LookRotation(look, Vector3.up);
        rb.rotation = rot;
    }
    public void Pickup(Transform pT, Transform hold)
    {
       // gameObject.layer = LayerMask.NameToLayer("ItemHeld");
        pickupTarget = pT;
        isPickedUp = true;
        //stats = hold.stats;
        holder = hold;
    }

    public void Drop()
    {
      //  gameObject.layer = LayerMask.NameToLayer("Item");
        isPickedUp = false;
        pickupTarget = null;
        holder = null;
    }
    public void SetVelocity(Vector3 desiredVelocity)
    {
        Rb.velocity = desiredVelocity;
    }
    public void AddVelocity(Vector3 desiredVelocity)
    {
        Rb.velocity += desiredVelocity;
    }
    public void ResetAngularVelocity()
    {
        Rb.angularVelocity = Vector3.zero;
    }
    //
}
