using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Auto-slide down steep slopes.
        public static void UpdateSlideDownSlopes(this IEntity entity, float deltaTime)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // only if sliding feature enabled, and if on ground
            if (!data.slideDownSlopes || !data.isGrounded)
            {
                data.slidingState = SlidingState.NONE;
                return;
            }

            // sliding mechanics are complex. we need a state machine to keep
            // it simple, understandable and modifiable.
            // (previously it used complex if/else cases, which were hard to
            //  understand and hard to modify/debug)
            if      (data.slidingState == SlidingState.NONE)     data.slidingState = entity.UpdateSlidingNONE();
            else if (data.slidingState == SlidingState.STARTING) data.slidingState = entity.UpdateSlidingSTARTING();
            else if (data.slidingState == SlidingState.SLIDING)  data.slidingState = entity.UpdateSlidingSLIDING();
            else if (data.slidingState == SlidingState.STOPPING) data.slidingState = entity.UpdateSlidingSTOPPING();
            else Debug.LogError("Unhandled sliding state: " + data.slidingState);
        }
    }
}