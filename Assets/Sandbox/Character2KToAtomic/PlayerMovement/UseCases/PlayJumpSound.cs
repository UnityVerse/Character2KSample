using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static void PlayJumpSound(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            movementData.feetAudio.clip = movementData.jumpSound;
            movementData.feetAudio.Play();
        }
    }
}