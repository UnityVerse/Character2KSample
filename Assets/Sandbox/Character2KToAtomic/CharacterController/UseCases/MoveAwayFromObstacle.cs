using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Called when a capsule cast detected an obstacle. Move away from the obstacle and slide against it if needed.
        //      moveVector: The movement vector.
        //      hitInfoCapsule: Hit info of the capsule cast collision.
        //      direction: Direction of the cast.
        //      distance: Distance of the cast.
        //      canSlide: Can slide against obstacles?
        //      tryGrounding: Try grounding the player?
        //      hitSmallCapsule: Did the collision occur with the small capsule (i.e. no skin width)?
        //      currentPosition: position of the character
        public static void MoveAwayFromObstacle(
            this IEntity entity,
            ref Vector3 moveVector,
            ref RaycastHit hitInfoCapsule,
            Vector3 direction,
            float distance,
            bool canSlide,
            bool tryGrounding,
            bool hitSmallCapsule,
            ref Vector3 currentPosition)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // IMPORTANT: This method must set moveVector.

            // When the small capsule hit then stop skinWidth away from obstacles
            float collisionOffset = hitSmallCapsule ? data.skinWidth : Character2KData.k_CollisionOffset;

            float hitDistance = Mathf.Max(hitInfoCapsule.distance - collisionOffset, 0.0f);
            // Note: remainingDistance is more accurate is we use hitDistance, but using hitInfoCapsule.distance gives a tiny
            // bit of dampening when sliding along obstacles
            float remainingDistance = Mathf.Max(distance - hitInfoCapsule.distance, 0.0f);

            // Move to the collision point
            entity.MovePosition(direction * hitDistance, direction, hitInfoCapsule, ref currentPosition);

            Vector3 hitNormal;
            Vector3 rayOrigin = currentPosition + data.transformedCenter;
            Vector3 rayDirection = hitInfoCapsule.point - rayOrigin;

            // Raycast returns a more accurate normal than SphereCast/CapsuleCast
            // Using angle <= k_MaxAngleToUseRaycastNormal gives a curve when collision is near an edge.
            if (Physics.Raycast(rayOrigin,
                    rayDirection,
                    out var hitInfoRay,
                    rayDirection.magnitude * Character2KData.k_RaycastScaleDistance,
                    data.collisionLayerMask,
                    data.triggerQuery) &&
                hitInfoRay.collider == hitInfoCapsule.collider &&
                Vector3.Angle(hitInfoCapsule.normal, hitInfoRay.normal) <= Character2KData.k_MaxAngleToUseRaycastNormal)
            {
                hitNormal = hitInfoRay.normal;
            }
            else
            {
                hitNormal = hitInfoCapsule.normal;
            }

            if (entity.GetPenetrationInfo(out var penetrationDistance, out var penetrationDirection, currentPosition, true, null,
                    hitInfoCapsule))
            {
                // Push away from the obstacle
                entity.MovePosition(penetrationDirection * penetrationDistance, null, null, ref currentPosition);
            }

            bool slopeIsSteep = false;
            if (tryGrounding || data.m_StuckInfo.isStuck)
            {
                // No further movement when grounding the character, or the character is stuck
                canSlide = false;
            }
            else if (moveVector.x.NotEqualToZero() || moveVector.z.NotEqualToZero())
            {
                // Test if character is trying to walk up a steep slope
                float slopeAngle = Vector3.Angle(Vector3.up, hitNormal);
                slopeIsSteep = slopeAngle > data.slopeLimit && slopeAngle < Character2KData.k_MaxSlopeLimit &&
                               Vector3.Dot(direction, hitNormal) < 0.0f;
            }

            // Set moveVector
            if (canSlide && remainingDistance > 0.0f)
            {
                Vector3 slideNormal = hitNormal;

                if (slopeIsSteep && slideNormal.y > 0.0f)
                {
                    // Do not move up the slope
                    slideNormal.y = 0.0f;
                    slideNormal.Normalize();
                }

                // Vector to slide along the obstacle
                Vector3 project = Vector3.Cross(direction, slideNormal);
                project = Vector3.Cross(slideNormal, project);

                if (slopeIsSteep && project.y > 0.0f)
                {
                    // Do not move up the slope
                    project.y = 0.0f;
                }

                project.Normalize();

                // Slide along the obstacle
                bool isWallSlowingDown = data.slowAgainstWalls && data.minSlowAgainstWallsAngle < 90.0f;

                if (isWallSlowingDown)
                {
                    // Factor used to perform a slow down against the walls.
                    float invRescaleFactor = 1 / Mathf.Cos(data.minSlowAgainstWallsAngle * Mathf.Deg2Rad);

                    // Cosine of angle between the movement direction and the tangent is equivalent to the sin of
                    // the angle between the movement direction and the normal, which is the sliding component of
                    // our movement.
                    float cosine = Vector3.Dot(project, direction);
                    float slowDownFactor = Mathf.Clamp01(cosine * invRescaleFactor);

                    moveVector = project * (remainingDistance * slowDownFactor);
                }
                else
                {
                    // No slow down, keep the same speed even against walls.
                    moveVector = project * remainingDistance;
                }
            }
            else
            {
                // Stop current move loop vector
                moveVector = Vector3.zero;
            }

            if (direction.y < 0.0f && Mathf.Approximately(direction.x, 0.0f) && Mathf.Approximately(direction.z, 0.0f))
            {
                // This is used by the sliding down slopes
                data.m_DownCollisionNormal = hitNormal;
            }
        }
    }
}