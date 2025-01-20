using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Determine if the character is grounded.
        //      movedCollisionFlags: Moved collision flags of the current move. Set to None if not moving.
        //      doDownCast: Do a down cast? We want to avoid this when the character is jumping upwards.
        public static void UpdateGrounded(
            this IEntity entity,
            CollisionFlags movedCollisionFlags,
            bool doDownCast = true
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            if ((movedCollisionFlags & CollisionFlags.CollidedBelow) != 0)
            {
                data.isGrounded = true;
            }
            else if (doDownCast)
            {
                data.isGrounded = entity.CheckCollisionBelow(
                    data.groundedTestDistance,
                    out RaycastHit hitInfo,
                    data.transform.position,
                    Vector3.zero,
                    true,
                    data.isLocalHuman,
                    data.isLocalHuman
                );
            }
            else
            {
                data.isGrounded = false;
            }
        }
    }
}