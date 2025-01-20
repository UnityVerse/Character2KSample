using Atomic.Entities;
using Controller2k;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        // movement state machine //////////////////////////////////////////////////
        public static bool EventJumpRequested(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            var characterData = (Character2KData) entity.GetCharacter2KData();

            // only while grounded, so jump key while jumping doesn't start a new
            // jump immediately after landing
            // => and not while sliding, otherwise we could climb slides by jumping
            // => not even while SlidingState.Starting, so we aren't able to avoid
            //    sliding by bunny hopping.
            // => grounded check uses min fall tolerance so we can actually still
            //    jump when walking down steps.
            return entity.IsGroundedWithinTolerance() &&
                   characterData.slidingState == SlidingState.NONE &&
                   movementData.jumpKeyPressed;
        }
    }
}