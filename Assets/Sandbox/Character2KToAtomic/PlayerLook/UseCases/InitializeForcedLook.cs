using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerLookUseCases
    {
        public static void InitializeForcedLook(this IEntity entity)
        {
            var lookData = (PlayerLookData) entity.GetPlayerLookData();
            lookData.camera.transform.SetParent(lookData.transform, false);
        }
    }
}