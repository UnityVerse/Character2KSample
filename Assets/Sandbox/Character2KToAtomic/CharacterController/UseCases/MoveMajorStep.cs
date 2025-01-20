using Atomic.Entities;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // A single movement major step. Returns true when there is collision.
        //      moveVector: The move vector.
        //      canSlide: Can slide against obstacles?
        //      tryGrounding: Try grounding the player?
        //      currentPosition: position of the character
        public static bool MoveMajorStep(
            this IEntity entity,
            ref Vector3 moveVector,
            bool canSlide,
            bool tryGrounding,
            ref Vector3 currentPosition
        )
        {
            Vector3 direction = moveVector.normalized;
            float distance = moveVector.magnitude;

            if (!entity.CapsuleCast(
                    direction,
                    distance,
                    currentPosition,
                    out bool smallRadiusHit,
                    out bool bigRadiusHit,
                    out RaycastHit smallRadiusHitInfo,
                    out RaycastHit bigRadiusHitInfo,
                    Vector3.zero))
            {
                // No collision, so move to the position
                entity.MovePosition(moveVector, null, null, ref currentPosition);

                // Check for penetration
                if (entity.GetPenetrationInfo(
                        out float penetrationDistance,
                        out Vector3 penetrationDirection,
                        currentPosition))
                {
                    // Push away from obstacles
                    entity.MovePosition(penetrationDirection * penetrationDistance, null, null, ref currentPosition);
                }

                // Stop current move loop vector
                moveVector = Vector3.zero;

                return false;
            }

            // Did the big radius not hit an obstacle?
            if (!bigRadiusHit)
            {
                // The small radius hit an obstacle, so character is inside an obstacle
                entity.MoveAwayFromObstacle(
                    ref moveVector,
                    ref smallRadiusHitInfo,
                    direction, distance,
                    canSlide,
                    tryGrounding,
                    true,
                    ref currentPosition);

                return true;
            }

            // Use the nearest collision point (e.g. to handle cases where 2 or more colliders' edges meet)
            if (smallRadiusHit && smallRadiusHitInfo.distance < bigRadiusHitInfo.distance)
            {
                entity.MoveAwayFromObstacle(
                    ref moveVector,
                    ref smallRadiusHitInfo,
                    direction,
                    distance,
                    canSlide,
                    tryGrounding,
                    true,
                    ref currentPosition
                );
                return true;
            }

            entity.MoveAwayFromObstacle(
                ref moveVector,
                ref bigRadiusHitInfo,
                direction,
                distance,
                canSlide,
                tryGrounding,
                false,
                ref currentPosition
            );

            return true;
        }
    }
}