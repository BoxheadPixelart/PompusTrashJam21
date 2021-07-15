using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WearableShell : MonoBehaviour
{

    public float minSize;
    public float maxSize;

    public struct ShellData
    {
        public float minSize;
        public float maxSize;
    };

    public ShellData shellData;

    // Start is called before the first frame update
    void Start()
    {
        if (maxSize == 0) maxSize = 100f;

        shellData.minSize = minSize;
        shellData.maxSize = maxSize;

    }


    public ShellData GetShellData()
    {
        return shellData;
    }

}
