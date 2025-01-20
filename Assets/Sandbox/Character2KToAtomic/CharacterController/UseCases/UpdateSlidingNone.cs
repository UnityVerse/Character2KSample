using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // sliding FSM state update
        public static SlidingState UpdateSlidingNONE(this IEntity entity)
        {
            // find slope normal by reusing or casting for a new one
            // AND check if valid angle
            
            var data = (Character2KData) entity.GetCharacter2KData();

            if (entity.ReuseOrCastForSlopeNormal(out Vector3 slopeNormal) &&
                IsSlideableAngle(Vector3.Angle(Vector3.up, slopeNormal), data.slopeLimit))
            {
                // we are definitely on a slide.
                // sometimes, we are running over tiny slides, but we
                // shouldn't immediately start sliding every time.
                // only after a 'slideStartTime'
                // (originally slideStartTime was completely ignored.
                //  this fixes it.)
                //Debug.Log("Considering sliding for slope with angle: " + Vector3.Angle(Vector3.up, slopeNormal));
                data.slidingStartedTime = Time.time;
                return SlidingState.STARTING;
            }
           
            // if none is found, then we just aren't sliding
            return SlidingState.NONE;
        }
    }
}