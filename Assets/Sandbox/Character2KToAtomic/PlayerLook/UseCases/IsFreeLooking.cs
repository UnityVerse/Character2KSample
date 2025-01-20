using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerLookUseCases
    {
        // free look mode //////////////////////////////////////////////////////////
        public static bool IsFreeLooking(this IEntity entity)
        {
            var lookData = (PlayerLookData) entity.GetPlayerLookData();
            lookData.camera.transform.SetParent(lookData.freeLookParent, false);

            return lookData.camera != null && // camera isn't initialized while loading players in charselection
                   lookData.camera.transform.parent == lookData.freeLookParent;
        }
    }
}