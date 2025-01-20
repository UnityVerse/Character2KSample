using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static MoveState UpdateSWIMMING(this IEntity entity, Vector2 inputDir, Vector3 desiredDir)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            var characterData = (Character2KData) entity.GetCharacter2KData();

            // ladder under / above water?
            if (entity.EventLadderEnter())
            {
                // rescale capsule if possible
                if (entity.TrySetHeight(characterData.defaultHeight * 1f, true, true, false))
                {
                    entity.EnterLadder();
                    return MoveState.CLIMBING;
                }
            }

            // not under water anymore?
            else if (!entity.EventUnderWater())
            {
                // rescale capsule if possible
                if (entity.TrySetHeight(characterData.defaultHeight * 1f, true, true, false))
                {
                    return MoveState.IDLE;
                }
            }

            // QE key rotation
            entity.RotateWithKeys();

            // move with acceleration (feels better)
            movementData.horizontalSpeed = AccelerateSpeed(
                inputDir,
                movementData.horizontalSpeed,
                movementData.swimSpeed,
                inputDir != Vector2.zero ? movementData.swimAcceleration : movementData.swimDeceleration
            );

            movementData.moveDir.x = desiredDir.x * movementData.horizontalSpeed;
            movementData.moveDir.z = desiredDir.z * movementData.horizontalSpeed;

            // gravitate toward surface
            if (movementData.waterCollider != null)
            {
                float surface = movementData.waterCollider.bounds.max.y;
                float surfaceDirection = surface - characterData.bounds.min.y - movementData.swimSurfaceOffset;
                movementData.moveDir.y = surfaceDirection * movementData.swimSpeed;
            }
            else
            {
                movementData.moveDir.y = 0;
            }

            return MoveState.SWIMMING;
        }
    }
}