using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static MoveState UpdateAIRBORNE(
            this IEntity entity,
            Vector2 inputDir,
            Vector3 desiredDir
        )
        {
            // QE key rotation
            entity.RotateWithKeys();

            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            var characterData = (Character2KData) entity.GetCharacter2KData();

            // max speed depends on what we did before jumping/falling
            float speed = movementData.sprintingBeforeAirborne ? movementData.runSpeed : movementData.walkSpeed;

            // move with acceleration (feels better)
            movementData.horizontalSpeed = AccelerateSpeed(inputDir, movementData.horizontalSpeed, speed,
                inputDir != Vector2.zero ? movementData.airborneAcceleration : movementData.airborneDeceleration);
            
            movementData.moveDir.x = desiredDir.x * movementData.horizontalSpeed;
            movementData.moveDir.y = entity.ApplyGravity(movementData.moveDir.y);
            movementData.moveDir.z = desiredDir.z * movementData.horizontalSpeed;

            if (entity.EventLanded())
            {
                // apply fall damage only in AIRBORNE->Landed.
                // (e.g. not if we run face forward into a wall with high velocity)
                entity.ApplyFallDamage();
                entity.PlayLandingSound();
                return MoveState.IDLE;
            }

            if (entity.EventLadderEnter())
            {
                entity.EnterLadder();
                return MoveState.CLIMBING;
            }

            if (entity.EventUnderWater())
            {
                // rescale capsule
                if (entity.TrySetHeight(characterData.defaultHeight * 0.25f, true, true, false))
                {
                    return MoveState.SWIMMING;
                }
            }

            return MoveState.AIRBORNE;
        }
    }
}