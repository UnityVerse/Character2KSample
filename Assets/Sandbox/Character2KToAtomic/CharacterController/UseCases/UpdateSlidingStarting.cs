using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // sliding FSM state update
        public static SlidingState UpdateSlidingSTARTING(this IEntity entity)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // find slope normal by reusing or casting for a new one
            // AND check if valid angle
            if (entity.ReuseOrCastForSlopeNormal(out Vector3 slopeNormal) &&
                IsSlideableAngle(Vector3.Angle(Vector3.up, slopeNormal), data.slopeLimit))
            {
                // we are still on a slope that will cause sliding.
                // but wait until start time has elapsed.
                if (Time.time >= data.slidingStartedTime + data.slideStartDelay)
                {
                    // actually start sliding in the next frame
                    //Debug.LogWarning("Starting sliding for slope with angle: " + Vector3.Angle(Vector3.up, slopeNormal) + " after on it for " + slideStartDelay + " seconds");
                    return SlidingState.SLIDING;
                }

                // otherwise wait a little longer
                return SlidingState.STARTING;
            }

            // if none is found, then we briefly walked over a slope, but not
            // long enough to actually start sliding
            return SlidingState.NONE;
        }
    }
}