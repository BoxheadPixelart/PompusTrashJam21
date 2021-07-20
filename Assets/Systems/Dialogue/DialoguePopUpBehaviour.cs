using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePopUpBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isOpen;

    private Vector3 scale;
    private Vector3 gScale;
    private Vector3 scaleVelo;
    //

    private Vector3 meshScale;
    private Vector3 gMeshScale;
    private Vector3 meshScaleVelo;
    private Vector3 baseMeshScale;
    private Vector3 meshScaleOffset; 
    [SerializeField] private DialogueData dialogueData; 
    [SerializeField] private KeyCode debugKey;

    [SerializeField] private KeyCode otherDebugKey;
    [SerializeField] private MeshFilter wordMesh;
    [SerializeField] private MeshFilter emotionMesh;
    [SerializeField] private Renderer render; 
    //
    private float speechTime;
    private int wordCount; 
    [SerializeField] private float speechInterval;
    private Vector3 rotOffset; 
    private bool isTalking; 
    //[SerializeField] private Vector3 openedSize; 
   


    void Start()
    {
       // CloseDialogue(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            OpenDialogue(); 
        }
        //
        if (Input.GetKeyDown(otherDebugKey))
        {
            CloseDialogue();
        }

        if (isTalking)
        {
           
            speechTime += Time.deltaTime;
          
            if (speechTime >= speechInterval)
            {
                if (wordCount < dialogueData.sentence.Length - 1)
                {
                    wordCount += 1; 
                    // BAP 
                    SetWord(dialogueData.sentence[wordCount]); 
                    speechTime = 0; 
                } 
                else 
                { 
                    wordCount = 0;
                    speechTime = 0;
                    isTalking = false;
                    CloseDialogue();
                }
            }
            if (dialogueData.sentence[wordCount].GetType() != typeof(VerbData))
            {
                wordMesh.transform.rotation = Quaternion.Euler(rotOffset.x, speechTime * 90, rotOffset.z);
            }
        } 
    }

    public void SetData(DialogueData data)
    {
        dialogueData = data;
        OpenDialogue(); 
    }
    private void FixedUpdate()
    {
        Vector3[] ScaleSpring = Numeric_Springing.Spring_Vector3(scale,scaleVelo,gScale,.8f,1.2f,3);
        scale = ScaleSpring[0];
        scaleVelo = ScaleSpring[1];
        scale += scaleVelo;
        transform.localScale = scale;
        //
        meshScaleOffset = meshScaleOffset / 1.05f;
        gMeshScale = baseMeshScale + meshScaleOffset;
        Vector3[] MeshScaleSpring = Numeric_Springing.Spring_Vector3(meshScale, meshScaleVelo, gMeshScale, .2f, 1f, 3);
        meshScale = MeshScaleSpring[0];
        meshScaleVelo = MeshScaleSpring[1];
        meshScale += meshScaleVelo;
      
   
        wordMesh.transform.localScale = meshScale;
        transform.LookAt(Camera.main.transform); 
    }

    void SetWord(WordData newWord)
    {
     //   float random = 1 + UnityEngine.Random.Range(-2, 2); 
        meshScaleOffset = new Vector3(.5f, 0, -.5f);
        wordMesh.mesh = newWord.mesh;
        render.material = newWord.mat; 
        rotOffset = newWord.offsetRotation;
        baseMeshScale = newWord.size; 
    }

    void OpenDialogue()
    {
        if (!isOpen)
        {
            gScale = Vector3.one;
            isOpen = true;
            StartTalking();
        }
    }
    void StartTalking()
    {
        wordCount = 0;
        speechTime = 0;
        SetWord(dialogueData.sentence[wordCount]);
        isTalking = true; 
    }
    void CloseDialogue()
    {
        if (isOpen)
        {
            gScale = Vector3.zero;
            isOpen = false;
        }
    }
}
