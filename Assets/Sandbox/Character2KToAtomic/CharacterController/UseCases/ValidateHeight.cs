using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Validate the capsule's height. (It must be at least double the radius size.)
        public static float ValidateHeight(
            this IEntity entity,
            float newHeight
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            return Mathf.Clamp(newHeight, data.radius * 2.0f, float.MaxValue);
        }
    }
}