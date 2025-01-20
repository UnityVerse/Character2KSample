using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerLookUseCases
    {
        public static void PlayerLookUpdate(this IEntity entity)
        {
            var lookData = (PlayerLookData) entity.GetPlayerLookData();
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // escape unlocks cursor
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            // mouse click locks cursor
            else if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // only while alive and  while cursor is locked, otherwise we are in a UI
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // calculate horizontal and vertical rotation steps
                float xExtra = Input.GetAxis("Mouse X") * lookData.XSensitivity;
                float yExtra = Input.GetAxis("Mouse Y") * lookData.YSensitivity;

                // use mouse to rotate character
                // (but use camera freelook parent while climbing so player isn't rotated
                //  while climbing)
                // (no free look in first person)
                if (movementData.state == MoveState.CLIMBING ||
                    Input.GetKey(lookData.freeLookKey) && lookData.distance > 0)
                {
                    // set to freelook parent already?
                    if (lookData.camera.transform.parent != lookData.freeLookParent)
                        entity.InitializeFreeLook();

                    // rotate freelooktarget for horizontal, rotate camera for vertical
                    lookData.freeLookParent.Rotate(new Vector3(0, xExtra, 0));
                    lookData.camera.transform.Rotate(new Vector3(-yExtra, 0, 0));
                }
                else
                {
                    // set to player parent already?
                    if (lookData.camera.transform.parent != lookData.transform)
                        entity.InitializeForcedLook();

                    // rotate character for horizontal, rotate camera for vertical
                    lookData.transform.Rotate(new Vector3(0, xExtra, 0));
                    lookData.camera.transform.Rotate(new Vector3(-yExtra, 0, 0));
                }
            }
        }
    }
}