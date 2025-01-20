using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Move the character. This function does not apply any gravity.
        //   moveVector: Move along this vector.
        // CollisionFlags is the summary of collisions that occurred during the Move.
        public static CollisionFlags Move(this IEntity entity, Vector3 moveVector)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            entity.MoveInternal(moveVector, true);
            return data.collisionFlags;
        }

        // Moves the characters.
        //      moveVector: Move vector
        //      slideWhenMovingDown: Slide against obstacles when moving down? (e.g. we don't want to slide when applying gravity while the character is grounded)
        //      forceTryStickToGround: Force try to stick to ground? Only used if character is grounded before moving.
        //      doNotStepOffset: Do not try to perform the step offset?
        private static void MoveInternal(
            this IEntity entity,
            Vector3 moveVector,
            bool slideWhenMovingDown,
            bool forceTryStickToGround = false,
            bool doNotStepOffset = false)
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            bool wasGrounded = data.isGrounded;
            Vector3 moveVectorNoY = new Vector3(moveVector.x, 0, moveVector.z);
            bool tryToStickToGround = wasGrounded && (forceTryStickToGround ||
                                                      (moveVector.y <= 0.0f &&
                                                       moveVectorNoY.sqrMagnitude.NotEqualToZero()));

            data.m_StartPosition = data.transform.position;

            data.collisionFlags = CollisionFlags.None;
            data.m_CollisionInfoDictionary.Clear();
            data.m_DownCollisionNormal = null;

            // Stop sliding down slopes when character jumps
            if (moveVector.y > 0.0f && data.slidingState != SlidingState.NONE)
            {
                // this hasn't really been tested. let's show a log message for
                // now.
                Debug.Log("CharacterController2k: a jump stopped sliding: " + data.slidingState);
                data.slidingState = SlidingState.NONE;
            }

            // Do the move loop
            entity.MoveLoop(moveVector, tryToStickToGround, slideWhenMovingDown, doNotStepOffset);

            bool doDownCast = tryToStickToGround || moveVector.y <= 0.0f;
            entity.UpdateGrounded(data.collisionFlags, doDownCast);

            // vis2k: fix velocity
            // set velocity, which is direction * speed. we don't have speed,
            // but we do have elapsed time
            data.velocity = (data.transform.position - data.m_StartPosition) / Time.deltaTime;

            entity.BroadcastCollisionEvent();
        }
    }
}