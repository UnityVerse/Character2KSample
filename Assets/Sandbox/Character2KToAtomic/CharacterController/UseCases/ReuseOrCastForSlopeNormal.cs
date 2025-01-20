using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // get best slope normal from either:
        // * reusing last move's down collision normal IF sliding already
        // * or casting for slope normal if NOT sliding yet
        public static bool ReuseOrCastForSlopeNormal(this IEntity entity, out Vector3 slopeNormal)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // if we are about to slide, or currently sliding, then we already
            // moved down and collided with the slope's surface. in that case,
            // simply reuse the normal instead of casting again.
            // IMPORTANT: we are NOT on a slope surface if state == Stopped!
            bool onSlopeSurface = data.slidingState is SlidingState.STARTING or SlidingState.SLIDING;
           
            if (onSlopeSurface && data.m_DownCollisionNormal != null)
            {
                slopeNormal = data.m_DownCollisionNormal.Value;
                return true;
            }
            
            // otherwise sphere/raycast to find a really good slope normal

            if (entity.CastForSlopeNormal(out slopeNormal))
            {
                return true;
            }

            // no slope found
            return false;
        }
    }
}