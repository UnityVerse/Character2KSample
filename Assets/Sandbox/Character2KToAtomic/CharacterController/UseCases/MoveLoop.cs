using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Movement loop. Keep moving until completely blocked by obstacles, or we reached the desired position/distance.
        //      moveVector: The move vector.
        //      tryToStickToGround: Try to stick to the ground?
        //      slideWhenMovingDown: Slide against obstacles when moving down? (e.g. we don't want to slide when applying gravity while the character is grounded)
        //      doNotStepOffset: Do not try to perform the step offset?
        public static void MoveLoop(
            this IEntity entity,
            Vector3 moveVector,
            bool tryToStickToGround,
            bool slideWhenMovingDown,
            bool doNotStepOffset
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            data.m_MoveVectors.Clear();
            data.m_NextMoveVectorIndex = 0;

            // Split the move vector into horizontal and vertical components.
            entity.SplitMoveVector(moveVector, slideWhenMovingDown, doNotStepOffset);
            MoveVector remainingMoveVector = data.m_MoveVectors[data.m_NextMoveVectorIndex];
            data.m_NextMoveVectorIndex++;

            bool didTryToStickToGround = false;
            data.m_StuckInfo.OnMoveLoop();
            Vector3 virtualPosition = data.transform.position;

            // The loop
            for (int i = 0; i < Character2KData.k_MaxMoveIterations; i++)
            {
                Vector3 refMoveVector = remainingMoveVector.moveVector;
                
                bool collided = entity.MoveMajorStep(
                    ref refMoveVector,
                    remainingMoveVector.canSlide,
                    didTryToStickToGround,
                    ref virtualPosition
                );

                remainingMoveVector.moveVector = refMoveVector;

                // Character stuck?
                if (data.m_StuckInfo.UpdateStuck(virtualPosition, remainingMoveVector.moveVector, moveVector))
                {
                    // Stop current move loop vector
                    remainingMoveVector = new MoveVector(Vector3.zero);
                }
                else if (!data.isLocalHuman && collided)
                {
                    // Only slide once for non-human controlled characters
                    remainingMoveVector.canSlide = false;
                }

                // Not collided OR vector used up (i.e. vector is zero)?
                if (!collided || remainingMoveVector.moveVector.sqrMagnitude.IsEqualToZero())
                {
                    // Are there remaining movement vectors?
                    if (data.m_NextMoveVectorIndex < data.m_MoveVectors.Count)
                    {
                        remainingMoveVector = data.m_MoveVectors[data.m_NextMoveVectorIndex];
                        data.m_NextMoveVectorIndex++;
                    }
                    else
                    {
                        if (!tryToStickToGround || didTryToStickToGround)
                        {
                            break;
                        }

                        // Try to stick to the ground
                        didTryToStickToGround = true;
                        if (!CanStickToGround(moveVector, out remainingMoveVector))
                        {
                            break;
                        }
                    }
                }

#if UNITY_EDITOR
                if (i == Character2KData.k_MaxMoveIterations - 1)
                {
                    Debug.Log(data.transform.name + " reached MaxMoveInterations(" +
                              Character2KData.k_MaxMoveIterations + "): remainingVector=" +
                              remainingMoveVector + " moveVector=" + moveVector + " hitCount=" +
                              data.m_StuckInfo.hitCount);
                }
#endif
            }

            data.transform.position = virtualPosition;
        }
    }
}