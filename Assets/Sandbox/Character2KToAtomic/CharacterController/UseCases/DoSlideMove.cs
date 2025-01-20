using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // helper function to do the actual sliding move
        // returns true if we did slide. false otherwise.
        public static bool DoSlideMove(this IEntity entity, Vector3 slopeNormal, float slidingTimeElapsed)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // calculate slide velocity Y based on parameters
            float velocityY = CalculateSlideVerticalVelocity(slopeNormal, slidingTimeElapsed, data.slideGravityMultiplier, data.slideMaxSpeed);

            // multiply with deltaTime so it's frame rate independent
            velocityY *= Time.deltaTime;

            // Push slightly away from the slope
            // => not multiplied by deltaTime because we stay away 'pushDistance'
            //    from the slope surface at all times
            Vector3 push = new Vector3(slopeNormal.x, 0, slopeNormal.z).normalized * Character2KData.k_PushAwayFromSlopeDistance;
            Vector3 moveVector = new Vector3(push.x, velocityY, push.z);

            // Preserve collision flags and velocity. Because user expects them to only be set when manually calling Move/SimpleMove.
            CollisionFlags oldCollisionFlags = data.collisionFlags;
            Vector3 oldVelocity = data.velocity;

            // move along the slope
            bool didSlide = true;
            entity.MoveInternal(moveVector, true, true, true);
            if ((data.collisionFlags & CollisionFlags.CollidedSides) != 0)
            {
                // Stop sliding when hit something on the side
                didSlide = false;
            }

            // restore collision flags and velocity
            data.collisionFlags = oldCollisionFlags;
            data.velocity = oldVelocity;

            // return result
            return didSlide;
        }
    }
}