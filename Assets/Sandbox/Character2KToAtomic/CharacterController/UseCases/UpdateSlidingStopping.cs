using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // sliding FSM state update
        public static SlidingState UpdateSlidingSTOPPING(this IEntity entity)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // find slope normal by reusing or casting for a new one
            // AND check if valid angle
            if (entity.ReuseOrCastForSlopeNormal(out Vector3 slopeNormal) &&
                IsSlideableAngle(Vector3.Angle(Vector3.up, slopeNormal), data.slopeLimit))
            {
                // we found a new one even though we were about to stop.
                // in that case, continue sliding in the next frame.
                return SlidingState.SLIDING;
            }
            
            // not on a slope and enough time elapsed to stop?
            // this is necessary for two reason:
            // * so we don't immediately stop to slide when sliding over a
            //   tiny flat surface
            // * so we don't allow jumping immediately after sliding, which
            //   is useful in some games.

            if (Time.time >= data.slidingStoppedTime + data.slideStopDelay)
            {
                //Debug.LogWarning("Stopping sliding after not on a slope for " + slideStopDelay + " seconds");
                return SlidingState.NONE;
            }

            // not on a slope, but not enough time elapsed.
            // wait a little longer.
            return SlidingState.STOPPING;
        }
    }
}