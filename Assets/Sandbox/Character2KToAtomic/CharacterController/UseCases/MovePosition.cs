using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Move the capsule position.
        //      moveVector: Move vector.
        //      collideDirection: Direction we encountered collision. Null if no collision.
        //      hitInfo: Hit info of the collision. Null if no collision.
        //      currentPosition: position of the character
        
        public static void MovePosition(
            this IEntity entity,
            Vector3 moveVector,
            Vector3? collideDirection,
            RaycastHit? hitInfo,
            ref Vector3 currentPosition
        )
        {
            if (moveVector.sqrMagnitude.NotEqualToZero())
            {
                currentPosition += moveVector;
            }

            if (collideDirection != null && hitInfo != null)
            {
                entity.UpdateCollisionInfo(collideDirection.Value, hitInfo.Value, currentPosition);
            }
        }
    }
}