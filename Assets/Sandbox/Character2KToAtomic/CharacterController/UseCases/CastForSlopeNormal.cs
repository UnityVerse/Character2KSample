using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // do different casts downwards to find the best slope normal
        public static bool CastForSlopeNormal(
            this IEntity entity,
            out Vector3 slopeNormal
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // sphere down to hit slope
            if (!entity.SmallSphereCast(
                    Vector3.down,
                    data.skinWidth + Character2KData.k_SlideDownSlopeTestDistance,
                    out RaycastHit hitInfoSphere,
                    Vector3.zero,
                    true,
                    data.transform.position))
            {
                // no slope found, not sliding anymore
                slopeNormal = Vector3.zero;
                return false;
            }

            Vector3 rayOrigin = Helpers
                .GetBottomSphereWorldPosition(data.transform.position, data.transformedCenter, data.scaledRadius,
                    data.scaledHeight);
            Vector3 rayDirection = hitInfoSphere.point - rayOrigin;

            // there is a slope below us.
            // let's raycast again for a more accurate normal than spherecast/capsulecast
            if (Physics.Raycast(rayOrigin,
                    rayDirection,
                    out var hitInfoRay,
                    rayDirection.magnitude * Character2KData.k_RaycastScaleDistance,
                    data.collisionLayerMask,
                    data.triggerQuery
                ) && hitInfoRay.collider == hitInfoSphere.collider)
            {
                // raycast hit something, so we have a more accurate normal now
                slopeNormal = hitInfoRay.normal;
            }
            else
            {
                // raycast hit nothing. let's keep the first normal.
                slopeNormal = hitInfoSphere.normal;
            }

            return true;
        }
    }
}