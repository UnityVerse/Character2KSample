using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // sliding FSM state update
        public static SlidingState UpdateSlidingSLIDING(this IEntity entity)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // find slope normal by reusing or casting for a new one
            // AND check if valid angle
            if (entity.ReuseOrCastForSlopeNormal(out Vector3 slopeNormal) &&
                IsSlideableAngle(Vector3.Angle(Vector3.up, slopeNormal), data.slopeLimit))
            {
                // Pro tip: Here you can also use the friction of the physics material of the slope, to adjust the slide speed.

                // find out how long we have been sliding for.
                // speed gets faster the longer we have been sliding.
                float slidingTimeElapsed = Time.time - data.slidingStartedTime;

                // do the slide move
                // if we slided, then keep sliding
                if (entity.DoSlideMove(slopeNormal, slidingTimeElapsed))
                {
                    return SlidingState.SLIDING;
                }
                
                // if we collided on the side,  we transition to stopping
                data.slidingStoppedTime = Time.time;
                return SlidingState.STOPPING;
            }

            // if none is found, then we transition to stopping
            //Debug.LogWarning("Sliding->Stopping");
            data.slidingStoppedTime = Time.time;
            return SlidingState.STOPPING;
        }
    }
}