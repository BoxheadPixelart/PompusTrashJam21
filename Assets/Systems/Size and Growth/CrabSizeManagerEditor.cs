using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CrabSizeManager))]
public class CrabSizeManagerEditor : Editor
{
    float scale = 0.0f;
    CrabSizeManager myTarget; 
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public override void OnInspectorGUI()
    {
         myTarget = (CrabSizeManager)target; 
        DrawDefaultInspector();
        scale = EditorGUILayout.Slider(myTarget.GetCrabSize(), 0, 100);
        myTarget.SetSize(scale);
    }
    private void OnValidate()
    {
        myTarget.SetSize(scale);
    }
}
