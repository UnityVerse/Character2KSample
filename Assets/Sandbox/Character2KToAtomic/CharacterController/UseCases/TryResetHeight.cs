using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Reset the capsule's height to the default value.
        //   preserveFootPosition: Adjust the capsule's center to preserve the foot position?
        //   checkForPenetration: Check for collision, and then de-penetrate if there's collision?
        //   updateGrounded: Update the grounded state? This uses a cast, so only set it to true if you need it.
        // Returns the reset height.
        public static bool TryResetHeight(this IEntity entity, bool preserveFootPosition, bool checkForPenetration, bool updateGrounded)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            return entity.TrySetHeight(data.defaultHeight, preserveFootPosition, checkForPenetration, updateGrounded);
        }
    }
}