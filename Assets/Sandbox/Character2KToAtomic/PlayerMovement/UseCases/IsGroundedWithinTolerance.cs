using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        // helper property to check grounded with some tolerance. technically we
        // aren't grounded when walking down steps, but this way we factor in a
        // minimum fall magnitude. useful for more tolerant jumping etc.
        // (= while grounded or while velocity not smaller than min fall yet)
        public static bool IsGroundedWithinTolerance(this IEntity entity)
        {
            var characterData = (Character2KData) entity.GetCharacter2KData();
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            return characterData.isGrounded || characterData.velocity.y > -movementData.fallMinimumMagnitude;
        }
    }
}