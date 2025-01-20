using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static bool EventLanded(this IEntity entity)
        {
            var characterData = (Character2KData) entity.GetCharacter2KData();
            return characterData.isGrounded;
        }
    }
}