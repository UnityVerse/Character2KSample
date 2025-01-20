using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Reset the capsule's center to the default value.
        //   checkForPenetration: Check for collision, and then de-penetrate if there's collision?
        //   updateGrounded: Update the grounded state? This uses a cast, so only set it to true if you need it.
        public static bool TryResetCenter(this IEntity entity, bool checkForPenetration, bool updateGrounded)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            return entity.TrySetCenter(data.m_DefaultCenter, checkForPenetration, updateGrounded);
        }
    }
}