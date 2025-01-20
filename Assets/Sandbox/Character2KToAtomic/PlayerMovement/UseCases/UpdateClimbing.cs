using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static MoveState UpdateCLIMBING(
            this IEntity entity,
            Vector2 inputDir,
            Vector3 desiredDir
        )
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            var characterData = (Character2KData) entity.GetCharacter2KData();
            
            // finished climbing?
            if (entity.EventLadderExit())
            {
                // player rotation was adjusted to ladder rotation before.
                // let's reset it, but also keep look forward
                movementData.transform.rotation = Quaternion.Euler(0, movementData.transform.rotation.eulerAngles.y, 0);
                movementData.ladderCollider = null;
                return MoveState.IDLE;
            }

            // interpret forward/backward movement as upward/downward
            // note: NO ACCELERATION, otherwise we would climb really fast when
            //       sprinting towards a ladder. and the actual climb feels way too
            //       unresponsive when accelerating.
            movementData.moveDir.x = inputDir.x * movementData.climbSpeed;
            movementData.moveDir.y = inputDir.y * movementData.climbSpeed;
            movementData.moveDir.z = 0;

            // make the direction relative to ladder rotation. so when pressing right
            // we always climb to the right of the ladder, no matter how it's rotated
            movementData.moveDir = movementData.ladderCollider.transform.rotation * movementData.moveDir;
            Debug.DrawLine(movementData.transform.position, movementData.transform.position + movementData.moveDir, Color.yellow, 0.1f, false);

            return MoveState.CLIMBING;
        }
    }
}