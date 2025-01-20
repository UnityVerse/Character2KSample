using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Update the collision flags and info.
        //      direction: The direction moved.
        //      hitInfo: The hit info of the collision.
        //      currentPosition: position of the character
        public static void UpdateCollisionInfo(
            this IEntity entity,
            Vector3 direction,
            RaycastHit? hitInfo,
            Vector3 currentPosition
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            if (direction.x.NotEqualToZero() || direction.z.NotEqualToZero())
            {
                data.collisionFlags |= CollisionFlags.Sides;
            }

            if (direction.y > 0.0f)
            {
                data.collisionFlags |= CollisionFlags.CollidedAbove;
            }
            else if (direction.y < 0.0f)
            {
                data.collisionFlags |= CollisionFlags.CollidedBelow;
            }

            data.m_StuckInfo.hitCount++;

            if (hitInfo != null)
            {
                Collider collider = hitInfo.Value.collider;

                // We only care about the first collision with a collider
                if (!data.m_CollisionInfoDictionary.ContainsKey(collider))
                {
                    Vector3 moved = currentPosition - data.m_StartPosition;
                    CollisionInfo newCollisionInfo = new CollisionInfo(null, hitInfo.Value, direction, moved.magnitude);
                    data.m_CollisionInfoDictionary.Add(collider, newCollisionInfo);
                }
            }
        }
    }
}