using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcController : MonoBehaviour
{
    public List<ItemBase> items = new List<ItemBase>();
    public ItemData goalItem;
    public GameObject dialoguePrefab;
    public DialogueData WinSentence;
    public DialogueData FailSentence; 
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
        ItemBase item = other.gameObject.GetComponent<ItemBase>();
        items.Add(item);
        CheckItem(item);
    }
    public void CheckItem(ItemBase incoming)
    {
        if (incoming.itemData.name == goalItem.name)
        {
            AcceptItem(incoming); 
        }  
        else
        {
            RejectItem(incoming); 
        }
    }
    public void Speak(DialogueData data) 
    {
        GameObject popup = Instantiate(dialoguePrefab, transform.position, Quaternion.identity); 
        DialoguePopUpBehaviour dialogue =  popup.GetComponent<DialoguePopUpBehaviour>();
        dialogue.SetData(data); 
    }
      
    private void OnTriggerExit(Collider other)
    {
        ItemBase item = other.gameObject.GetComponent<ItemBase>();
        items.Remove(item);
    }
    private void AcceptItem(ItemBase incoming)
    {
        print("Yes Please");
        if (incoming.isPickedUp)
        {
            incoming.holder.GetComponentInChildren<InteractionManager>().canInteract = true;
            incoming.Drop();
            Vector3 heading = (transform.transform.position - incoming.Rb.position);
            incoming.SetVelocity(((heading) * 5));
        }
        else
        {
            Vector3 heading = (transform.transform.position - incoming.Rb.position);
            incoming.SetVelocity(((heading) * 5));
        }
        Destroy(incoming.gameObject, 0.25f);
        Speak(WinSentence); 
    }
    private void RejectItem(ItemBase incoming)
    {
        print("GOD NO");
        if (incoming.isPickedUp)
        {
            incoming.holder.GetComponentInChildren<InteractionManager>().canInteract = true; 
            incoming.Drop();
            Vector3 heading = (transform.transform.position - incoming.Rb.position) ;
            Vector3 hOffset = new Vector3(Random.Range(-1, 1), 0, 0);
            incoming.SetVelocity(((-heading + hOffset) * 5) + Vector3.up);
        } else
        {
            Vector3 heading = (transform.transform.position - incoming.Rb.position);
            Vector3 hOffset = new Vector3(Random.Range(-1, 1), 0, 0);
            incoming.SetVelocity(((-heading + hOffset) * 5) + Vector3.up);
        }
        Speak(FailSentence);

    }
}
