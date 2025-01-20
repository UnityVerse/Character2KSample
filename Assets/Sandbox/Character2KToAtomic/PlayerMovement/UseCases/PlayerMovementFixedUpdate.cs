using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        // CharacterController movement is physics based and requires FixedUpdate.
        // (using Update causes strange movement speeds in builds otherwise)
        public static void PlayerMovementFixedUpdate(this IEntity entity)
        {
            var characterData = (Character2KData) entity.GetCharacter2KData();
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // get input and desired direction based on camera and ground
            Vector2 inputDir = GetInputDirection();
            Vector3 desiredDir = entity.GetDesiredDirection(inputDir);
            Debug.DrawLine(movementData.transform.position, movementData.transform.position + desiredDir, Color.blue);
            Debug.DrawLine(movementData.transform.position, movementData.transform.position + desiredDir, Color.cyan);

            // update state machine
            if (movementData.state == MoveState.IDLE) movementData.state = entity.UpdateIDLE(inputDir, desiredDir);
            else if (movementData.state == MoveState.WALKING) movementData.state = entity.UpdateWALKINGandRUNNING(inputDir, desiredDir);
            else if (movementData.state == MoveState.RUNNING) movementData.state = entity.UpdateWALKINGandRUNNING(inputDir, desiredDir);
            else if (movementData.state == MoveState.CROUCHING) movementData.state = entity.UpdateCROUCHING(inputDir, desiredDir);
            else if (movementData.state == MoveState.CRAWLING) movementData.state = entity.UpdateCRAWLING(inputDir, desiredDir);
            else if (movementData.state == MoveState.AIRBORNE) movementData.state = entity.UpdateAIRBORNE(inputDir, desiredDir);
            else if (movementData.state == MoveState.CLIMBING) movementData.state = entity.UpdateCLIMBING(inputDir, desiredDir);
            else if (movementData.state == MoveState.SWIMMING) movementData.state = entity.UpdateSWIMMING(inputDir, desiredDir);
            else Debug.LogError("Unhandled Movement State: " + movementData.state);

            // cache this move's state to detect landing etc. next time
            if (!characterData.isGrounded) movementData.lastFall = characterData.velocity;

            // move depending on latest moveDir changes
            Debug.DrawLine(movementData.transform.position, movementData.transform.position + movementData.moveDir * Time.fixedDeltaTime, Color.magenta);
            entity.Move(movementData.moveDir * Time.fixedDeltaTime); // note: returns CollisionFlags if needed

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            if (movementData.animator != null)
            {
                float runCycle = Mathf.Repeat(
                    movementData.animator.GetCurrentAnimatorStateInfo(0).normalizedTime + movementData.runCycleLegOffset,
                    1);
                movementData.jumpLeg = runCycle < 0.5f ? 1 : -1; // * move.z;
            }

            // reset keys no matter what
            movementData.jumpKeyPressed = false;
            movementData.crawlKeyPressed = false;
            movementData.crouchKeyPressed = false;
        }
    }
}