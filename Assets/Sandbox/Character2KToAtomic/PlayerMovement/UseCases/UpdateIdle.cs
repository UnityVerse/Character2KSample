using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static MoveState UpdateIDLE(this IEntity entity, Vector2 inputDir, Vector3 desiredDir)
        {
            // QE key rotation
            entity.RotateWithKeys();

            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            var characterData = (Character2KData) entity.GetCharacter2KData();
            
            // decelerate from last move (e.g. from jump)
            // (moveDir.xz can be set to 0 to have an interruption when landing)
            movementData.horizontalSpeed = AccelerateSpeed(inputDir, movementData.horizontalSpeed, 0, movementData.walkDeceleration);
            
            movementData.moveDir.x = desiredDir.x * movementData.horizontalSpeed;
            movementData.moveDir.y = entity.ApplyGravity(movementData.moveDir.y);
            movementData.moveDir.z = desiredDir.z * movementData.horizontalSpeed;

            if (entity.EventFalling())
            {
                movementData.sprintingBeforeAirborne = false;
                return MoveState.AIRBORNE;
            }

            if (entity.EventJumpRequested())
            {
                // start the jump movement into Y dir, go to jumping
                // note: no endurance>0 check because it feels odd if we can't jump
                movementData.moveDir.y = movementData.jumpSpeed;
                movementData.sprintingBeforeAirborne = false;
                entity.PlayJumpSound();
                return MoveState.AIRBORNE;
            }

            if (entity.EventCrouchToggle())
            {
                // rescale capsule
                if (entity.TrySetHeight(characterData.defaultHeight * 0.5f, true, true, false))
                    return MoveState.CROUCHING;
            }
            else if (entity.EventCrawlToggle())
            {
                // rescale capsule
                if (entity.TrySetHeight(characterData.defaultHeight * 0.25f, true, true, false))
                    return MoveState.CRAWLING;
            }
            else if (entity.EventLadderEnter())
            {
                entity.EnterLadder();
                return MoveState.CLIMBING;
            }
            else if (entity.EventUnderWater())
            {
                // rescale capsule
                if (entity.TrySetHeight(characterData.defaultHeight * 0.25f, true, true, false))
                    return MoveState.SWIMMING;
            }
            else if (inputDir != Vector2.zero)
            {
                return MoveState.WALKING;
            }

            return MoveState.IDLE;
        }
    }
}