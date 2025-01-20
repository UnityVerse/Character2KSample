using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Check if any colliders overlap the capsule.
        //      includeSkinWidth: Include the skin width in the test?
        //      offsetPosition: Offset position, if we want to test somewhere relative to the capsule's position.
        public static bool CheckCapsule(
            this IEntity entity,
            Vector3 currentPosition,
            bool includeSkinWidth = true,
            Vector3? offsetPosition = null
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            Vector3 offset = offsetPosition ?? Vector3.zero;
            float tempSkinWidth = includeSkinWidth ? data.skinWidth : 0;
            return Physics.CheckCapsule(
                Helpers.GetTopSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius,
                    data.scaledHeight) +
                offset,
                Helpers.GetBottomSphereWorldPosition(currentPosition, data.transformedCenter, data.scaledRadius,
                    data.scaledHeight) +
                offset,
                data.scaledRadius + tempSkinWidth,
                data.collisionLayerMask,
                data.triggerQuery
            );
        }
    }
}