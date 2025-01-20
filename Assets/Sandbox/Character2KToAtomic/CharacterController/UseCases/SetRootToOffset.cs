using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        public static void SetRootToOffset(this IEntity entity)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            if (data.playerRootTransform != null)
            {
                data.playerRootTransform.localPosition = data.rootTransformOffset;
            }
        }
    }
}