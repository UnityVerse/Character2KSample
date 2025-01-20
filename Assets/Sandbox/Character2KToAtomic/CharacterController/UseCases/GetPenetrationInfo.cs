using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Get direction and distance to move out of the obstacle.
        //      getDistance: Get distance to move out of the obstacle.
        //      getDirection: Get direction to move out of the obstacle.
        //      currentPosition: position of the character
        //      includeSkinWidth: Include the skin width in the test?
        //      offsetPosition: Offset position, if we want to test somewhere relative to the capsule's position.
        //      hitInfo: The hit info.
        public static bool GetPenetrationInfo(
            this IEntity entity,
            out float getDistance,
            out Vector3 getDirection,
            Vector3 currentPosition,
            bool includeSkinWidth = true,
            Vector3? offsetPosition = null,
            RaycastHit? hitInfo = null
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            getDistance = 0.0f;
            getDirection = Vector3.zero;

            Vector3 offset = offsetPosition ?? Vector3.zero;
            float tempSkinWidth = includeSkinWidth ? data.skinWidth : 0.0f;
            int overlapCount = Physics.OverlapCapsuleNonAlloc(
                Helpers.GetTopSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius, data.scaledHeight) +
                offset,
                Helpers.GetBottomSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius, data.scaledHeight) +
                offset,
                data.scaledRadius + tempSkinWidth,
                data.m_PenetrationInfoColliders,
                data.collisionLayerMask,
                data.triggerQuery);
            if (overlapCount <= 0 || data.m_PenetrationInfoColliders.Length <= 0)
            {
                return false;
            }

            bool result = false;
            Vector3 localPos = Vector3.zero;
            for (int i = 0; i < overlapCount; i++)
            {
                Collider col = data.m_PenetrationInfoColliders[i];
                if (col == null)
                {
                    break;
                }

                Transform colliderTransform = col.transform;
                if (entity.ComputePenetration(offset,
                        col, colliderTransform.position, colliderTransform.rotation,
                        out var direction, out var distance, includeSkinWidth, currentPosition))
                {
                    localPos += direction * (distance + Character2KData.k_CollisionOffset);
                    result = true;
                }
                else if (hitInfo != null && hitInfo.Value.collider == col)
                {
                    // We can use the hit normal to push away from the collider, because CapsuleCast generally returns a normal
                    // that pushes away from the collider.
                    localPos += hitInfo.Value.normal * Character2KData.k_CollisionOffset;
                    result = true;
                }
            }

            if (result)
            {
                getDistance = localPos.magnitude;
                getDirection = localPos.normalized;
            }

            return result;
        }
    }
}