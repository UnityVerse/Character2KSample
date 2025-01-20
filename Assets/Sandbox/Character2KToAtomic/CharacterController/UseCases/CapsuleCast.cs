using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Do two capsule casts. One excluding the capsule's skin width and one including the skin width.
        //      direction: Direction to cast
        //      distance: Distance to cast
        //      currentPosition: position of the character
        //      smallRadiusHit: Did hit, excluding the skin width?
        //      bigRadiusHit: Did hit, including the skin width?
        //      smallRadiusHitInfo: Hit info for cast excluding the skin width.
        //      bigRadiusHitInfo: Hit info for cast including the skin width.
        //      offsetPosition: Offset position, if we want to test somewhere relative to the capsule's position.
        public static bool CapsuleCast(
            this IEntity entity,
            Vector3 direction,
            float distance,
            Vector3 currentPosition,
            out bool smallRadiusHit,
            out bool bigRadiusHit,
            out RaycastHit smallRadiusHitInfo, out RaycastHit bigRadiusHitInfo,
            Vector3 offsetPosition)
        {
            // Exclude the skin width in the test
            smallRadiusHit = entity.SmallCapsuleCast(
                direction,
                distance,
                out smallRadiusHitInfo,
                offsetPosition,
                currentPosition
            );

            // Include the skin width in the test
            bigRadiusHit = entity.BigCapsuleCast(
                direction,
                distance,
                out bigRadiusHitInfo,
                offsetPosition,
                currentPosition
            );

            return smallRadiusHit || bigRadiusHit;
        }

        // Do a capsule cast, excluding the skin width.
        //      direction: Direction to cast.
        //      distance: Distance to cast.
        //      smallRadiusHitInfo: Hit info.
        //      offsetPosition: Offset position, if we want to test somewhere relative to the capsule's position.
        //      currentPosition: position of the character
        public static bool SmallCapsuleCast(
            this IEntity entity,
            Vector3 direction,
            float distance,
            out RaycastHit smallRadiusHitInfo,
            Vector3 offsetPosition,
            Vector3 currentPosition
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // Cast further than the distance we need, to try to take into account small edge cases (e.g. Casts fail
            // when moving almost parallel to an obstacle for small distances).
            float extraDistance = data.scaledRadius;

            if (Physics.CapsuleCast(
                    Helpers.GetTopSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius,
                        data.scaledHeight) + offsetPosition,
                    Helpers.GetBottomSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius,
                        data.scaledHeight) + offsetPosition,
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

        // Do a capsule cast, includes the skin width.
        //      direction: Direction to cast.
        //      distance: Distance to cast.
        //      bigRadiusHitInfo: Hit info.
        //      offsetPosition: Offset position, if we want to test somewhere relative to the capsule's position.
        //      currentPosition: position of the character
        public static bool BigCapsuleCast(
            this IEntity entity,
            Vector3 direction,
            float distance,
            out RaycastHit bigRadiusHitInfo,
            Vector3 offsetPosition,
            Vector3 currentPosition
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // Cast further than the distance we need, to try to take into account small edge cases (e.g. Casts fail
            // when moving almost parallel to an obstacle for small distances).
            float extraDistance = data.scaledRadius + data.skinWidth;

            if (Physics.CapsuleCast(
                    Helpers.GetTopSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius,
                        data.scaledHeight) + offsetPosition,
                    Helpers.GetBottomSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius,
                        data.scaledHeight) + offsetPosition,
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