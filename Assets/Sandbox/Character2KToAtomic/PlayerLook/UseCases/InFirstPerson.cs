using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerLookUseCases
    {
        public static bool InFirstPerson(this IEntity entity)
        {
            var lookData = (PlayerLookData) entity.GetPlayerLookData();
            return lookData.distance == 0;
        }
    }
}