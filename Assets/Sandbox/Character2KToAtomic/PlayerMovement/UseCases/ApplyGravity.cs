using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        // helper function to apply gravity based on previous Y direction
        public static float ApplyGravity(this IEntity entity, float moveDirY)
        {
            var characterData = (Character2KData) entity.GetCharacter2KData();
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            
            // apply full gravity while falling
            if (!characterData.isGrounded)
                // gravity needs to be * Time.fixedDeltaTime even though we multiply
                // the final controller.Move * Time.fixedDeltaTime too, because the
                // unit is 9.81m/s²
                return moveDirY + Physics.gravity.y * movementData.gravityMultiplier * Time.fixedDeltaTime;
            
            // if grounded then apply no force. the new OpenCharacterController
            // doesn't need a ground stick force. it would only make the character
            // slide on all uneven surfaces.
            return 0;
        }
    }
}