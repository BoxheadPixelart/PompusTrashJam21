using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using KinematicCharacterController.Crab;

namespace KinematicCharacterController.Crab
{
    public class Teleporter : MonoBehaviour
    {
        public Teleporter TeleportTo;

        public UnityAction<CrabCharacterController> OnCharacterTeleport;

        public bool isBeingTeleportedTo { get; set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!isBeingTeleportedTo)
            {
                //ExampleCharacterController cc = other.GetComponent<ExampleCharacterController>();
                CrabCharacterController cc = other.GetComponent<CrabCharacterController>();
                if (cc)
                {
                    cc.Motor.SetPositionAndRotation(TeleportTo.transform.position, TeleportTo.transform.rotation);

                    if (OnCharacterTeleport != null)
                    {
                        OnCharacterTeleport(cc);
                    }
                    TeleportTo.isBeingTeleportedTo = true;
                }
            }

            isBeingTeleportedTo = false;
        }
    }
}