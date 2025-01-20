using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // vis2k: add missing CanSetHeightAndCenter function
        public static bool CanSetHeightAndCenter(this IEntity entity, float newHeight, Vector3 newCenter)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // vis2k fix:
            // IMPORTANT: adjust height BEFORE ever calculating the center.
            //            previously it was adjusted AFTER calculating the center.
            //            so the center would NOT EXACTLY be the center anymore
            //            if the height was adjusted.
            //            => causing all future center calculations to be wrong.
            //            => causing center.y to increase every time
            //            => causing the character to float in the air over time
            //            see also: https://github.com/vis2k/uMMORPG/issues/36
            newHeight = entity.ValidateHeight(newHeight);

            // debug draw
            Debug.DrawLine(
                Helpers.GetTopSphereWorldPositionSimulated(data.transform, newCenter, data.scaledRadius, newHeight),
                Helpers.GetBottomSphereWorldPositionSimulated(data.transform, newCenter, data.scaledRadius, newHeight),
                Color.yellow,
                3f
            );

            // check the overlap capsule
            int hits = Physics.OverlapCapsuleNonAlloc(
                Helpers.GetTopSphereWorldPositionSimulated(data.transform, newCenter, data.scaledRadius, newHeight),
                Helpers.GetBottomSphereWorldPositionSimulated(data.transform, newCenter, data.scaledRadius, newHeight),
                data.radius,
                data.m_OverlapCapsuleColliders,
                data.collisionLayerMask,
                data.triggerQuery
            );

            for (int i = 0; i < hits; ++i)
            {
                // a collider that is not self?
                if (data.m_OverlapCapsuleColliders[i] != data.m_CapsuleCollider)
                {
                    return false;
                }
            }

            // no overlaps
            return true;
        }
    }
}