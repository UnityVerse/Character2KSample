using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static bool EventLadderExit(this IEntity entity)
        {
            // OnTriggerExit isn't good enough to detect ladder exits because we
            // shouldn't exit as soon as our head sticks out of the ladder collider.
            // only if we fully left it. so check this manually here:
            
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            var characterData = (Character2KData) entity.GetCharacter2KData();

            return movementData.ladderCollider != null &&
                   !movementData.ladderCollider.bounds.Intersects(characterData.bounds);
        }
    }
}