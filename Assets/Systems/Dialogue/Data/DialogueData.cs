using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DialogueData", menuName = "Dialogue Data", order = 51)]
public class DialogueData : ScriptableObject
{
    public WordData[] sentence; 
}
