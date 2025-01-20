using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static void ProgressStepCycle(this IEntity entity, Vector3 inputDir, float speed)
        {
            var characterData = (Character2KData) entity.GetCharacter2KData();
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            if (characterData.velocity.sqrMagnitude > 0 && (inputDir.x != 0 || inputDir.y != 0))
            {
                movementData.stepCycle += (characterData.velocity.magnitude + speed * 
                    (movementData.state == MoveState.WALKING ? 1 : movementData.runStepLength)) * Time.fixedDeltaTime;
            }

            if (movementData.stepCycle > movementData.nextStep)
            {
                movementData.nextStep = movementData.stepCycle + movementData.runStepInterval;
                entity.PlayFootStepAudio();
            }
        }
    }
}