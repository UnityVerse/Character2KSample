using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Compute the minimal translation required to separate the character from the collider.
        //   positionOffset: Position offset to add to the capsule collider's position.
        //   collider: The collider to test.
        //   colliderPosition: Position of the collider.
        //   colliderRotation: Rotation of the collider.
        //   direction: Direction along which the translation required to separate the colliders apart is minimal.
        //   distance: The distance along direction that is required to separate the colliders apart.
        //   includeSkinWidth: Include the skin width in the test?
        //   currentPosition: Position of the character
        // True if found penetration.
        public static bool ComputePenetration(
            this IEntity entity,
            Vector3 positionOffset,
            Collider collider,
            Vector3 colliderPosition,
            Quaternion colliderRotation,
            out Vector3 direction,
            out float distance,
            bool includeSkinWidth,
            Vector3 currentPosition
        )
        {
            Character2KData data = entity.GetValue<Character2KData>(SandboxEntityAPI.Character2KData);

            if (collider == data.m_CapsuleCollider)
            {
                // Ignore self
                direction = Vector3.one;
                distance = 0;
                return false;
            }

            if (includeSkinWidth)
            {
                data.m_CapsuleCollider.radius = data.radius + data.skinWidth;
                data.m_CapsuleCollider.height = data.height + data.skinWidth * 2.0f;
            }

            // Note: Physics.ComputePenetration does not always return values when the colliders overlap.
            bool result = Physics.ComputePenetration(
                data.m_CapsuleCollider,
                currentPosition + positionOffset,
                Quaternion.identity,
                collider,
                colliderPosition,
                colliderRotation,
                out direction,
                out distance
            );
            
            if (includeSkinWidth)
            {
                data.m_CapsuleCollider.radius = data.radius;
                data.m_CapsuleCollider.height = data.height;
            }

            return result;
        }
    }
}