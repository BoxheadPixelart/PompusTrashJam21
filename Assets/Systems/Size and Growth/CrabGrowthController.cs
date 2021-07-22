using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;

[System.Serializable]
public class SizeVariables
{
    public float characterScale = 1f;

    public float rigidbodyMass = 1f;
    public float capsuleRadius = 0.5f;
    public float crouchedHeight = 1f;
    public float capsuleHeight = 2f;

    public float capsuleYOffset = 1f;
    public float cameraFOV = 75;
    public float cameraDefaultDistance = 6;
    public float cameraMinDistance = 0;
    public float cameraMaxDistance = 10;
    public float characterStableMovementSpeed = 15;
    public float characterStableMovementSharpness = 20;
    public float characterMaxStableSlopeAngle = 60;
    public float characterMaxStepHeight = 1;
    public float characterOrentationSharpness = 8;
    public float characterJumpUpSpeed = 10;
    public float characterAirMoveSpeed = 15;
    public float characterAirAccelerationSpeed = 15;



}

/// <summary>
/// Component that manages actual scaling of Crab.
/// </summary>
/// 

public class CrabGrowthController : MonoBehaviour
{
    public GameObject ParentObjectToPhysicallyScale;
    public GameObject PlayerCameraObject;
    private KinematicCharacterController.Crab.CrabCharacterCamera _cameraScript;
    private KinematicCharacterController.Crab.CrabCharacterController _charController;
    private Camera _camera;
    private CapsuleCollider _capsuleCollider;
    private Rigidbody _rigidBody;

    public ShellManager _ShellManager;
    private WearableShell.ShellData _shellData;



    [SerializeField]
    public CrabSizeData startingSize;

    [SerializeField]
    public CrabSizeData endingSize;

    [SerializeField]
    private float size = 0f;


    // Shell variables
    // these will be sent to us by the shell, but we are using defaults for now
    private bool _wearingShell = false;

    private float _shellSizeMinimum = 0f;
    private float _shellSizeMaximum = 100f;

    private float _shellSpeedDebuffAtMinimum = 0.6f; // below minimum size, debuff multiplier is 0
    private float _shellSpeedDebuffAtMaximum = 0.7f;


    private PostProcessingController _postProcessingController;

    // ---- Connect to the crabSizeManager
    private CrabSizeManager _CrabSizeManager;


    // --- Connect to character motor
    private KinematicCharacterMotor _CharMotor;
  

    // Start is called before the first frame update
    void Start()
    {
        // Prepare our Crab Size Manager reference
        GameObject __gm = GameObject.FindGameObjectWithTag("GameController");
        _CrabSizeManager = __gm.GetComponentInChildren<CrabSizeManager>();
        if (_CrabSizeManager == null) Debug.Log(this.name.ToString() + " couldn't find CrabSizeManager.");

        _CrabSizeManager.AddSizeChangeListener(SizeUpdate);

        // Connect to the post processing controller
        _postProcessingController = __gm.GetComponent<PostProcessingController>();

        // Prepare our KinematicCharacterMotor
        _CharMotor = this.GetComponent<KinematicCharacterMotor>();

        _ShellManager.AddShellChangeListener(ShellUpdate);

        _cameraScript = PlayerCameraObject.GetComponent<KinematicCharacterController.Crab.CrabCharacterCamera>();
        _camera = PlayerCameraObject.GetComponent<Camera>();
        _capsuleCollider = this.GetComponent<CapsuleCollider>();
        _rigidBody = this.GetComponent<Rigidbody>();
        _charController = this.GetComponent<KinematicCharacterController.Crab.CrabCharacterController>();
    }


    private void FixedUpdate()
    {
        if (!_CrabSizeManager.DEBUGDisableGrowth)
        {
            _CrabSizeManager.AddSize();
        }
    }


    private float ShellDebuffMultiplier(float __size)
    {
        if (!_wearingShell) return 1f;

        if (__size < _shellData.minSize) return 0.05f;

        if (__size > _shellData.maxSize)
        {
            return 1f;
        }
        //C L A M P 
        //  float __multi = 1f;
        float _lerpVal = (__size - _shellSizeMinimum) / (_shellSizeMaximum - _shellSizeMinimum); //Mathf.InverseLerp()
        //float __multi = Mathf.Lerp(_shellSpeedDebuffAtMinimum, _shellSpeedDebuffAtMaximum, _lerpVal);
        return Mathf.Lerp(_shellSpeedDebuffAtMinimum, _shellSpeedDebuffAtMaximum, _lerpVal);
    }

    private void ShellUpdate(bool __wearingShell, WearableShell.ShellData shellData)
    {
        _wearingShell = __wearingShell;

        _shellData = shellData;


        // We store "size" so we can call a size update manually if need be, when we switch shells
       
            SizeUpdate(size);
       
       
    }

    private void SizeUpdate(float __size)
    {
        size = __size;

        float _shellDebuffMultiplier = ShellDebuffMultiplier(__size);



        __size = Mathf.Clamp(__size * 0.01f, 0f, 1f); // __size can now be used as a 0-1 lerp

        // Mesh object scale change
        float __scaleSize = startingSize.characterScale + ((endingSize.characterScale - startingSize.characterScale) * __size);
        Vector3 __newScale = new Vector3(__scaleSize, __scaleSize, __scaleSize);
        ParentObjectToPhysicallyScale.transform.localScale = __newScale;



        // Rigidbody mass change
        float __rigidbodyMass = Mathf.Lerp(startingSize.rigidbodyMass, endingSize.rigidbodyMass, __size);

        _rigidBody.mass = __rigidbodyMass;



        #region  // Capsule resizing / Character Motor
        //SetCapsuleDimensions(float radius, float height, float yOffset)
        float __radius = Mathf.Lerp(startingSize.capsuleRadius, endingSize.capsuleRadius, __size);
        float __height = Mathf.Lerp(startingSize.capsuleHeight, endingSize.capsuleHeight, __size);
        float __crouchHeight = Mathf.Lerp(startingSize.crouchedHeight, endingSize.crouchedHeight, __size);
        float __yOffset = Mathf.Lerp(startingSize.capsuleYOffset, endingSize.capsuleYOffset, __size);
        float __maxStableSlopeAngle = Mathf.Lerp(startingSize.characterMaxStableSlopeAngle, endingSize.characterMaxStableSlopeAngle, __size);
        float __maxStepHeight = Mathf.Lerp(startingSize.characterMaxStepHeight, endingSize.characterMaxStepHeight, __size);



        _CharMotor.SetCapsuleDimensions(__radius, __height, __yOffset);
        _CharMotor.MaxStableSlopeAngle = __maxStableSlopeAngle;
        _CharMotor.MaxStepHeight = __maxStepHeight;
        _CharMotor.SimulatedCharacterMass = __rigidbodyMass;

        #endregion




        #region // Camera settings

        float __fov = Mathf.Lerp(startingSize.cameraFOV, endingSize.cameraFOV, __size);
        float __defaultDistance = Mathf.Lerp(startingSize.cameraDefaultDistance, endingSize.cameraDefaultDistance, __size);
        float __minDistance = Mathf.Lerp(startingSize.cameraMinDistance, endingSize.cameraMinDistance, __size);
        float __maxDistance = Mathf.Lerp(startingSize.cameraMaxDistance, endingSize.cameraMaxDistance, __size);

        _camera.fieldOfView = __fov;

        _cameraScript.DefaultDistance = __defaultDistance;
        _cameraScript.MinDistance = __minDistance;
        _cameraScript.MaxDistance = __maxDistance;

        #endregion


        #region // Char Controller
        float __stableMovement = Mathf.Lerp(startingSize.characterStableMovementSpeed, endingSize.characterStableMovementSpeed, __size) * _shellDebuffMultiplier;
        float __stableMovementSharpness = Mathf.Lerp(startingSize.characterStableMovementSharpness, endingSize.characterStableMovementSharpness, __size);
        float __orientationSharpness = Mathf.Lerp(startingSize.characterOrentationSharpness, endingSize.characterOrentationSharpness, __size);
        float __airMoveSpeed = Mathf.Lerp(startingSize.characterAirMoveSpeed, endingSize.characterAirMoveSpeed, __size) * _shellDebuffMultiplier;
        float __airAccelerationSpeed = Mathf.Lerp(startingSize.characterAirAccelerationSpeed, endingSize.characterAirAccelerationSpeed, __size) * _shellDebuffMultiplier;
        float __jumpUpSpeed = Mathf.Lerp(startingSize.characterJumpUpSpeed, endingSize.characterJumpUpSpeed, __size) * _shellDebuffMultiplier;



        _charController.MaxStableMoveSpeed = __stableMovement;
        _charController.StableMovementSharpness = __stableMovementSharpness;
        _charController.OrientationSharpness = __orientationSharpness;
        _charController.MaxAirMoveSpeed = __airMoveSpeed;
        _charController.AirAccelerationSpeed = __airAccelerationSpeed;



        #endregion


        #region Post Processing controls

        float __cameraDOFFocalDistance = Mathf.Lerp(startingSize.cameraDOFFocalDistance, endingSize.cameraDOFFocalDistance, __size);
        float __cameraDOFFocalLength = Mathf.Lerp(startingSize.cameraDOFFocalLength, endingSize.cameraDOFFocalLength, __size);
        float __cameraDOFAperture = Mathf.Lerp(startingSize.cameraDOFAperture, endingSize.cameraDOFAperture, __size);

        _postProcessingController.SetCameraDOF(__cameraDOFFocalDistance, __cameraDOFFocalLength, __cameraDOFAperture);
        
        #endregion



        /*
         * All variables used, for reference

            public float rigidbodyMass = 1;
            
            public float capsuleRadius = 0.5f;
            private float crouchedHeight = 1f;
            public float capsuleHeight
            public float capsuleYOffset = 1f;
            
            public float cameraFOV = 75;
            public float cameraDefaultDistance = 6;
            public float cameraMinDistance = 0;
            public float cameraMaxDistance = 10;

            public float characterStableMovementSpeed = 15;
            public float characterStableMovementSharpness = 20;
            public float characterMaxStableSlopeAngle = 60;
            public float characterMaxStepHeight = 1;
            public float characterOrentationSharpness = 8;
            public float characterAirMoveSpeed = 15;
            public float characterAirAccelerationSpeed = 15;

         * 
         * */



    }




}
