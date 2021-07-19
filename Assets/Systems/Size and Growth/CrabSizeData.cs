using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrabSizeData", menuName ="Crab Size Data",order = 51)]
public class CrabSizeData : ScriptableObject
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
