using System;
using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static MoveState UpdateWALKINGandRUNNING(this IEntity entity, Vector2 inputDir, Vector3 desiredDir)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            var characterData = (Character2KData) entity.GetCharacter2KData();

            // QE key rotation
            entity.RotateWithKeys();

            // walk or run?
            float speed = entity.GetWalkOrRunSpeed();

            // move with acceleration (feels better)
            movementData.horizontalSpeed = AccelerateSpeed(
                inputDir,
                movementData.horizontalSpeed,
                speed,
                inputDir != Vector2.zero ? movementData.walkAcceleration : movementData.walkDeceleration
            );

            movementData.moveDir.x = desiredDir.x * movementData.horizontalSpeed;
            movementData.moveDir.y = entity.ApplyGravity(movementData.moveDir.y);
            movementData.moveDir.z = desiredDir.z * movementData.horizontalSpeed;

            if (entity.EventFalling())
            {
                movementData.sprintingBeforeAirborne = Math.Abs(speed - movementData.runSpeed) < float.Epsilon;
                return MoveState.AIRBORNE;
            }

            if (entity.EventJumpRequested())
            {
                // start the jump movement into Y dir, go to jumping
                // note: no endurance>0 check because it feels odd if we can't jump
                movementData.moveDir.y = movementData.jumpSpeed;
                movementData.sprintingBeforeAirborne = Math.Abs(speed - movementData.runSpeed) < float.Epsilon;
                entity.PlayJumpSound();
                return MoveState.AIRBORNE;
            }

            if (entity.EventCrouchToggle())
            {
                // rescale capsule
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
                // rescale capsule
                if (entity.TrySetHeight(characterData.defaultHeight * 0.25f, true, true, false))
                {
                    // limit speed to crawl speed so we don't decelerate from run speed
                    // to crawl speed (hence crawling too fast for a short time)
                    movementData.horizontalSpeed = Mathf.Min(movementData.horizontalSpeed, movementData.crawlSpeed);
                    return MoveState.CRAWLING;
                }
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
            // go to idle after fully decelerating (y doesn't matter)
            else if (movementData.moveDir.x == 0 && movementData.moveDir.z == 0)
            {
                return MoveState.IDLE;
            }

            entity.ProgressStepCycle(inputDir, speed);
            return Math.Abs(speed - movementData.walkSpeed) < float.Epsilon ? MoveState.WALKING : MoveState.RUNNING;
        }
    }
}