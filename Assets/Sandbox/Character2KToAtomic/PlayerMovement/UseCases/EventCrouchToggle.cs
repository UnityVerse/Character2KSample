using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static bool EventCrouchToggle(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            return movementData.crouchKeyPressed;
        }
    }
}