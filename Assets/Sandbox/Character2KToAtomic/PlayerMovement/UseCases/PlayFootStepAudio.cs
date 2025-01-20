using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static void PlayFootStepAudio(this IEntity entity)
        {
            var characterData = (Character2KData) entity.GetCharacter2KData();
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();
            
            if (!characterData.isGrounded) return;

            // any configured sounds?
            if (movementData.footstepSounds == null || movementData.footstepSounds.Length == 0) return;

            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, movementData.footstepSounds.Length);
            movementData.feetAudio.clip = movementData.footstepSounds[n];
            movementData.feetAudio.PlayOneShot(movementData.feetAudio.clip);

            // move picked sound to index 0 so it's not picked next time
            movementData.footstepSounds[n] = movementData.footstepSounds[0];
            movementData.footstepSounds[0] = movementData.feetAudio.clip;
        }
    }
}