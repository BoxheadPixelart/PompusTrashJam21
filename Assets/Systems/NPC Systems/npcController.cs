using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcController : MonoBehaviour
{
    public List<ItemBase> items = new List<ItemBase>();
    public ItemData goalItem;
    public bool canTalk; 
    public GameObject dialoguePrefab;
    public DialogueData desireSentence; 
    public DialogueData WinSentence;
    public DialogueData FailSentence;
    public Transform dialoguePoint;
    public WearableShell shell;
    public GameObject shellPrefab;
    public Transform spawnPoint;
    public Vector3 launchVelo;
    
    private Respawn respawnManager;


    // Start is called before the first frame update
    void Start()
    {
        GameObject __gm = GameObject.FindGameObjectWithTag("GameController");
        respawnManager = __gm.GetComponent<Respawn>();

        shell.SetInteract(false);
        canTalk = true; 


    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (canTalk)
        {
            print(other.gameObject.tag); 
            if (other.gameObject.CompareTag("Player"))
            {
                print("Is Player"); 
                Speak(desireSentence);
                respawnManager.SetRespawnPoint(spawnPoint);
                print("Has Spoken");
            }
            else
            {
                ItemBase item = other.gameObject.GetComponent<ItemBase>();
                items.Add(item);
                CheckItem(item);
            }
        }
      
    }
    public void CheckItem(ItemBase incoming)
    {
        print(incoming); 
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
        GameObject popup = Instantiate(dialoguePrefab, dialoguePoint.position, Quaternion.identity); 
        DialoguePopUpBehaviour dialogue =  popup.GetComponent<DialoguePopUpBehaviour>();
        dialogue.SetData(data);
        StartCoroutine(CanTalkTimer()); 
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
            InteractionManager manager = incoming.holder.GetComponentInChildren<InteractionManager>();
            manager.canInteract = true;
            manager.nearbyInteracts.Clear(); 
            incoming.Drop();
            Vector3 heading = (transform.transform.position - incoming.Rb.position);
            incoming.SetVelocity(((heading) * 5));
        }
        else
        {
            Vector3 heading = (transform.transform.position - incoming.Rb.position);
            incoming.SetVelocity(((heading) * 5));
        }
        items.Remove(incoming);
        Destroy(incoming.gameObject, 0.25f);
        Speak(WinSentence);
        QuestComplete(); 
    }
    private void RejectItem(ItemBase incoming)
    {
        print("GOD NO");
        if (incoming.isPickedUp)
        {
            InteractionManager manager = incoming.holder.GetComponentInChildren<InteractionManager>();
            manager.canInteract = true;
            manager.nearbyInteracts.Clear();
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

    public void QuestComplete()
    {
       // shell.SetInteract(true);
        GameObject shell = Instantiate(shellPrefab, spawnPoint.position,Quaternion.identity);
        Rigidbody rb = shell.GetComponent<Rigidbody>(); 
        rb.velocity = launchVelo; 
    }
    IEnumerator CanTalkTimer()
    {
        canTalk = false;
        yield return new WaitForSeconds(1f);
        canTalk = true;
        print("Can Talk Again"); 
    }
}
