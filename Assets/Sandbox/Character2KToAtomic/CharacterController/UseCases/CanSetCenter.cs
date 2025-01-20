using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // vis2k: add missing CanSetCenter function
        public static bool CanSetCenter(this IEntity entity, Vector3 newCenter)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            return entity.CanSetHeightAndCenter(data.height, newCenter);
        }
    }
}