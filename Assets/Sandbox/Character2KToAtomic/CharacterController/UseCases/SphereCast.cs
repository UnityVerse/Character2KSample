using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Do a sphere cast, excludes the skin width. Sphere position is at the top or bottom of the capsule.
        //      direction: Direction to cast.
        //      distance: Distance to cast.
        //      smallRadiusHitInfo: Hit info.
        //      offsetPosition: Offset position, if we want to test somewhere relative to the capsule's position.
        //      useBottomSphere: Use the sphere at the bottom of the capsule? If false then use the top sphere.
        //      currentPosition: position of the character
        public static bool SmallSphereCast(
            this IEntity entity,
            Vector3 direction,
            float distance,
            out RaycastHit smallRadiusHitInfo,
            Vector3 offsetPosition,
            bool useBottomSphere,
            Vector3 currentPosition
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // Cast further than the distance we need, to try to take into account small edge cases (e.g. Casts fail
            // when moving almost parallel to an obstacle for small distances).
            float extraDistance = data.scaledRadius;

            Vector3 spherePosition = useBottomSphere
                ? Helpers.GetBottomSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius,
                      data.scaledHeight) +
                  offsetPosition
                : Helpers.GetTopSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius,
                      data.scaledHeight) +
                  offsetPosition;

            if (Physics.SphereCast(spherePosition,
                    data.scaledRadius,
                    direction,
                    out smallRadiusHitInfo,
                    distance + extraDistance,
                    data.collisionLayerMask,
                    data.triggerQuery))
            {
                return smallRadiusHitInfo.distance <= distance;
            }

            return false;
        }

        // Do a sphere cast, including the skin width. Sphere position is at the top or bottom of the capsule.
        //      direction: Direction to cast.
        //      distance: Distance to cast.
        //      currentPosition: position of the character
        //      bigRadiusHitInfo: Hit info.
        //      offsetPosition: Offset position, if we want to test somewhere relative to the capsule's position.
        //      useBottomSphere: Use the sphere at the bottom of the capsule? If false then use the top sphere.
        public static bool BigSphereCast(
            this IEntity entity,
            Vector3 direction, 
            float distance,
            Vector3 currentPosition,
            out RaycastHit bigRadiusHitInfo,
            Vector3 offsetPosition,
            bool useBottomSphere)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            
            // Cast further than the distance we need, to try to take into account small edge cases (e.g. Casts fail
            // when moving almost parallel to an obstacle for small distances).
            float extraDistance = data.scaledRadius + data.skinWidth;

            Vector3 spherePosition = useBottomSphere
                ? Helpers.GetBottomSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius, data.scaledHeight) +
                  offsetPosition
                : Helpers.GetTopSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius, data.scaledHeight) +
                  offsetPosition;
            if (Physics.SphereCast(spherePosition,
                    data.scaledRadius + data.skinWidth,
                    direction,
                    out bigRadiusHitInfo,
                    distance + extraDistance,
                    data.collisionLayerMask,
                    data.triggerQuery))
            {
                return bigRadiusHitInfo.distance <= distance;
            }

            return false;
        }
    }
}