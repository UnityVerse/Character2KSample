using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Split the move vector into horizontal and vertical components. The results are added to the moveVectors list.
        //      moveVector: The move vector.
        //      slideWhenMovingDown: Slide against obstacles when moving down? (e.g. we don't want to slide when applying gravity while the character is grounded)
        //      doNotStepOffset: Do not try to perform the step offset?
        public static void SplitMoveVector(
            this IEntity entity,
            Vector3 moveVector,
            bool slideWhenMovingDown,
            bool doNotStepOffset
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            Vector3 horizontal = new Vector3(moveVector.x, 0.0f, moveVector.z);
            Vector3 vertical = new Vector3(0.0f, moveVector.y, 0.0f);
            
            bool horizontalIsAlmostZero = Helpers.IsMoveVectorAlmostZero(horizontal, Character2KData.k_SmallMoveVector);
            
            float tempStepOffset = data.stepOffset;
            bool doStepOffset = data.isGrounded &&
                                !doNotStepOffset &&
                                !Mathf.Approximately(tempStepOffset, 0.0f) &&
                                !horizontalIsAlmostZero;

            // Note: Vector is split in this order: up, horizontal, down

            if (vertical.y > 0.0f)
            {
                // Up
                if (horizontal.x.NotEqualToZero() || horizontal.z.NotEqualToZero())
                {
                    // Move up then horizontal
                    entity.AddMoveVector(vertical, data.slideAlongCeiling);
                    entity.AddMoveVector(horizontal);
                }
                else
                {
                    // Move up
                    entity.AddMoveVector(vertical, data.slideAlongCeiling);
                }
            }
            else if (vertical.y < 0.0f)
            {
                // Down
                if (horizontal.x.NotEqualToZero() || horizontal.z.NotEqualToZero())
                {
                    if (doStepOffset && entity.CanStepOffset(horizontal))
                    {
                        // Move up, horizontal then down
                        entity.AddMoveVector(Vector3.up * tempStepOffset, false);
                        entity.AddMoveVector(horizontal);
                        if (slideWhenMovingDown)
                        {
                            entity.AddMoveVector(vertical);
                            entity.AddMoveVector(Vector3.down * tempStepOffset);
                        }
                        else
                        {
                            entity.AddMoveVector(vertical + Vector3.down * tempStepOffset);
                        }
                    }
                    else
                    {
                        // Move horizontal then down
                        entity.AddMoveVector(horizontal);
                        entity.AddMoveVector(vertical, slideWhenMovingDown);
                    }
                }
                else
                {
                    // Move down
                    entity.AddMoveVector(vertical, slideWhenMovingDown);
                }
            }
            else
            {
                // Horizontal
                if (doStepOffset && entity.CanStepOffset(horizontal))
                {
                    // Move up, horizontal then down
                    entity.AddMoveVector(Vector3.up * tempStepOffset, false);
                    entity.AddMoveVector(horizontal);
                    entity.AddMoveVector(Vector3.down * tempStepOffset);
                }
                else
                {
                    // Move horizontal
                    entity.AddMoveVector(horizontal);
                }
            }
        }
    }
}