using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        // helper function to get move or walk speed depending on key press & endurance
        public static float GetWalkOrRunSpeed(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            bool runRequested = Input.GetKey(movementData.runKey);
            return runRequested ? movementData.runSpeed : movementData.walkSpeed;
        }
    }
}