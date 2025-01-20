using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerAnimationUseCases
    {
        public static void PlayerAnimationLateUpdate(this IEntity entity)
        {
            var animationData = (PlayerAnimationData) entity.GetPlayerAnimationData();
            var characterData = (Character2KData) entity.GetCharacter2KData();
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // local velocity (based on rotation) for animations
            Vector3 localVelocity = animationData.transform.InverseTransformDirection(characterData.velocity);

            // Turn value so that mouse-rotating the character plays some animation
            // instead of only raw rotating the model.
            float turnAngle = AnimationDeltaUnclamped(animationData.lastForward, animationData.transform.forward);
            animationData.lastForward = animationData.transform.forward;

            // apply animation parameters to all animators.
            // there might be multiple if we use skinned mesh equipment.
            foreach (Animator animator in animationData.gameObject.GetComponentsInChildren<Animator>())
            {
                animator.SetBool("DEAD", false);
                animator.SetFloat("DirX", localVelocity.x, animationData.animationDirectionDampening,
                    Time.deltaTime); // smooth idle<->run transitions
                animator.SetFloat("DirY", localVelocity.y, animationData.animationDirectionDampening,
                    Time.deltaTime); // smooth idle<->run transitions
                animator.SetFloat("DirZ", localVelocity.z, animationData.animationDirectionDampening,
                    Time.deltaTime); // smooth idle<->run transitions
                animator.SetFloat("LastFallY", movementData.lastFall.y);
                animator.SetFloat("Turn", turnAngle, animationData.animationTurnDampening,
                    Time.deltaTime); // smooth turn
                animator.SetBool("CROUCHING", movementData.state == MoveState.CROUCHING);
                animator.SetBool("CRAWLING", movementData.state == MoveState.CRAWLING);
                animator.SetBool("CLIMBING", movementData.state == MoveState.CLIMBING);
                animator.SetBool("SWIMMING", movementData.state == MoveState.SWIMMING);

                // smoothest way to do climbing-idle is to stop right where we were
                if (movementData.state == MoveState.CLIMBING)
                    animator.speed = localVelocity.y == 0 ? 0 : 1;
                else
                    animator.speed = 1;

                // grounded detection works best via .state
                // -> check AIRBORNE state instead of controller.isGrounded to have some
                //    minimum fall tolerance so we don't play the AIRBORNE animation
                //    while walking down steps etc.
                animator.SetBool("OnGround", movementData.state != MoveState.AIRBORNE);
                if (characterData.isGrounded) animator.SetFloat("JumpLeg", movementData.jumpLeg);

                // upper body layer
                animator.SetBool("UPPERBODY_HANDS", true);
            }
        }
    }
}