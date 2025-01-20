using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static void PlayLandingSound(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            movementData.feetAudio.clip = movementData.landSound;
            movementData.feetAudio.Play();
            movementData.nextStep = movementData.stepCycle + .5f;
        }
    }
}