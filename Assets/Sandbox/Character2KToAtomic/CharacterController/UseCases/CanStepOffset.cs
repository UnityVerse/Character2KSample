using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Can the character perform a step offset?
        //      moveVector: Horizontal movement vector.
        public static bool CanStepOffset(this IEntity entity, Vector3 moveVector)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            float moveVectorMagnitude = moveVector.magnitude;
            Vector3 position = data.transform.position;

            // Only step up if there's an obstacle at the character's feet (e.g. do not step when only character's head collides)
            if (!entity.SmallSphereCast(moveVector, moveVectorMagnitude, out _, Vector3.zero, true, position) &&
                !entity.BigSphereCast(moveVector, moveVectorMagnitude, position, out _, Vector3.zero, true))
            {
                return false;
            }

            float upDistance = Mathf.Max(data.stepOffset, Character2KData.k_MinStepOffsetHeight);

            // We only step over obstacles if we can partially fit on it (i.e. fit the capsule's radius)
            Vector3 horizontal = moveVector * data.scaledRadius;
            
            float horizontalSize = horizontal.magnitude;
            horizontal.Normalize();

            // Any obstacles ahead (after we moved up)?
            Vector3 up = Vector3.up * upDistance;
            if (entity.SmallCapsuleCast(horizontal, data.skinWidth + horizontalSize, out _, up, position) ||
                entity.BigCapsuleCast(horizontal, horizontalSize, out _, up, position))
            {
                return false;
            }

            return !entity.CheckSteepSlopeAhead(moveVector);
        }
    }
}