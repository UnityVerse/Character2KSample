using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static MoveState UpdateCRAWLING(this IEntity entity, Vector2 inputDir, Vector3 desiredDir)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            var characterData = (Character2KData) entity.GetCharacter2KData();

            // QE key rotation
            entity.RotateWithKeys();

            // move with acceleration (feels better)
            movementData.horizontalSpeed = AccelerateSpeed(inputDir, movementData.horizontalSpeed, movementData.crawlSpeed,
                inputDir != Vector2.zero ? movementData.crawlAcceleration : movementData.crawlDeceleration);
            
            movementData.moveDir.x = desiredDir.x * movementData.horizontalSpeed;
            movementData.moveDir.y = entity.ApplyGravity(movementData.moveDir.y);
            movementData.moveDir.z = desiredDir.z * movementData.horizontalSpeed;

            if (entity.EventFalling())
            {
                // rescale capsule if possible
                if (entity.TrySetHeight(characterData.defaultHeight * 1f, true, true, false))
                {
                    movementData.sprintingBeforeAirborne = false;
                    return MoveState.AIRBORNE;
                }
            }
            else if (entity.EventJumpRequested())
            {
                // stop crawling when pressing jump key. this feels better than
                // jumping from the crawling state.

                // rescale capsule if possible
                if (entity.TrySetHeight(characterData.defaultHeight * 1f, true, true, false))
                {
                    return MoveState.IDLE;
                }
            }
            else if (entity.EventCrouchToggle())
            {
                // rescale capsule if possible
                if (entity.TrySetHeight(characterData.defaultHeight * 0.5f, true, true, false))
                {
                    // limit speed to crouch speed so we don't decelerate from run speed
                    // to crouch speed (hence crouching too fast for a short time)
                    movementData.horizontalSpeed = Mathf.Min(movementData.horizontalSpeed, movementData.crouchSpeed);
                    return MoveState.CROUCHING;
                }
            }
            else if (entity.EventCrawlToggle())
            {
                // rescale capsule if possible
                if (entity.TrySetHeight(characterData.defaultHeight * 1f, true, true, false))
                {
                    return MoveState.IDLE;
                }
            }
            else if (entity.EventLadderEnter())
            {
                // rescale capsule if possible
                if (entity.TrySetHeight(characterData.defaultHeight * 1f, true, true, false))
                {
                    entity.EnterLadder();
                    return MoveState.CLIMBING;
                }
            }
            else if (entity.EventUnderWater())
            {
                // rescale capsule
                if (entity.TrySetHeight(characterData.defaultHeight * 0.25f, true, true, false))
                {
                    return MoveState.SWIMMING;
                }
            }

            entity.ProgressStepCycle(inputDir, movementData.crawlSpeed);
            return MoveState.CRAWLING;
        }
    }
}