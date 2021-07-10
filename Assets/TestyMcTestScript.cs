using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestyMcTestScript : MonoBehaviour
{

    public GameObject objectToScale;
    public float maximumScaleSizeAddition = 2f;

    private string _myName = "TestMcTestObject: ";

    #region Testing the crab size functions

    private CrabSizeManager _CrabSizeManager;
    
    private void SizeChangeReporter(float __incomingSize)
    {
        float __size = 1f + Mathf.Min(0.01f * __incomingSize * maximumScaleSizeAddition,maximumScaleSizeAddition);

        Vector3 __vec3Size = new Vector3(__size, __size, __size);

        objectToScale.transform.localScale = __vec3Size;

        Debug.Log("SIZE CHANGE REPORTER EVENT: Crab sized changed to " + __incomingSize.ToString() + ", test object scale is now " + __size.ToString());

    }



    // Start is called before the first frame update
    void Start()
    {
        GameObject __gm = GameObject.FindGameObjectWithTag("GameController");

        _CrabSizeManager = __gm.GetComponentInChildren<CrabSizeManager>();

        if (_CrabSizeManager == null) Debug.Log(this.name.ToString() + " couldn't find CrabSizeManager.");
        //else Debug.Log(_myName + "Found the crab size manager.");

        // Add my listening method HOPEFULLY OMG
        _CrabSizeManager.AddSizeChangeListener(SizeChangeReporter);


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        _CrabSizeManager.AddSize();
        

    }

    #endregion


}
