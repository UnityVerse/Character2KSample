using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static Vector3 GetDesiredDirection(this IEntity entity, Vector2 inputDir)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // always move along the camera forward as it is the direction that is being aimed at
            return movementData.transform.forward * inputDir.y + movementData.transform.right * inputDir.x;
        }
    }
}