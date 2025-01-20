using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Returns true if there's a steep slope ahead.
        //      moveVector: The movement vector.
        //      alsoCheckForStepOffset: Do a second test where the step offset will move the player to?
        public static bool CheckSteepSlopeAhead(
            this IEntity entity,
            Vector3 moveVector,
            bool alsoCheckForStepOffset = true
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            Vector3 direction = moveVector.normalized;
            float distance = moveVector.magnitude;

            if (entity.CheckSteepSlopAhead(direction, distance, Vector3.zero))
            {
                return true;
            }

            // Only need to do the second test for human controlled character
            if (!alsoCheckForStepOffset || !data.isLocalHuman)
            {
                return false;
            }

            // Check above where the step offset will move the player to
            return entity.CheckSteepSlopAhead(direction,
                Mathf.Max(distance, Character2KData.k_MinCheckSteepSlopeAheadDistance),
                Vector3.up * data.stepOffset);
        }

        // Returns true if there's a steep slope ahead.
        public static bool CheckSteepSlopAhead(
            this IEntity entity,
            Vector3 direction,
            float distance,
            Vector3 offsetPosition
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            if (!entity.CapsuleCast(direction, distance, data.transform.position,
                    out var smallRadiusHit, out var bigRadiusHit,
                    out var smallRadiusHitInfo, out var bigRadiusHitInfo,
                    offsetPosition))
            {
                // No collision
                return false;
            }

            RaycastHit hitInfoCapsule =
                !bigRadiusHit || smallRadiusHit && smallRadiusHitInfo.distance < bigRadiusHitInfo.distance
                    ? smallRadiusHitInfo
                    : bigRadiusHitInfo;

            Vector3 rayOrigin = data.transform.position + data.transformedCenter + offsetPosition;

            float offset = Mathf.Clamp(data.m_SlopeMovementOffset, 0.0f, distance * Character2KData.k_SlopeCheckDistanceMultiplier);
            Vector3 rayDirection = hitInfoCapsule.point + direction * offset - rayOrigin;

            // Raycast returns a more accurate normal than SphereCast/CapsuleCast
            if (Physics.Raycast(rayOrigin,
                    rayDirection,
                    out var hitInfoRay,
                    rayDirection.magnitude * Character2KData.k_RaycastScaleDistance,
                    data.collisionLayerMask,
                    data.triggerQuery) &&
                hitInfoRay.collider == hitInfoCapsule.collider)
            {
                hitInfoCapsule = hitInfoRay;
            }
            else
            {
                return false;
            }

            float slopeAngle = Vector3.Angle(Vector3.up, hitInfoCapsule.normal);
            bool slopeIsSteep = slopeAngle > data.slopeLimit &&
                                slopeAngle < Character2KData.k_MaxSlopeLimit &&
                                Vector3.Dot(direction, hitInfoCapsule.normal) < 0.0f;

            return slopeIsSteep;
        }
    }
}