using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        // rotate with QE keys
        public static void RotateWithKeys(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            float horizontal2 = Input.GetAxis("Horizontal2");
            movementData.transform.Rotate(Vector3.up * horizontal2 * movementData.rotationSpeed * Time.fixedDeltaTime);
        }
    }
}