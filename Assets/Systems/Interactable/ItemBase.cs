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
    private Transform holder;
    private Vector3 itemDir;
    private Vector3 gItemDir;
    private Vector3 itemDirVelo;
    public Vector3 debugRot;
    //
    bool isPickedUp;
    public Transform pickupTarget;
    public Vector3 velo;
    public Transform rotTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void Action()
    {
        print("Item has been picked up");
    }
    //
    public void MoveToHand()
    {
        Vector3 heading = (pickupTarget.position - rb.position);
        rb.velocity += (heading * (25) - rb.velocity) * 0.75f;
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
        gameObject.layer = LayerMask.NameToLayer("ItemHeld");
        pickupTarget = pT;
        isPickedUp = true;
        //stats = hold.stats;
        holder = hold;
    }

    public void Drop()
    {
        gameObject.layer = LayerMask.NameToLayer("Item");
        isPickedUp = false;
        pickupTarget = null;
        holder = null;
    }
    public void SetVelocity(Vector3 desiredVelocity)
    {
        rb.velocity = desiredVelocity;
    }
    public void AddVelocity(Vector3 desiredVelocity)
    {
        rb.velocity += desiredVelocity;
    }
    public void ResetAngularVelocity()
    {
        rb.angularVelocity = Vector3.zero;
    }
    //
}
