using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Get the foot world position.
        public static Vector3 GetFootWorldPosition(this IEntity entity)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            return entity.GetFootWorldPosition(data.transform.position);
        }
        
        // Get the foot world position.
        public static Vector3 GetFootWorldPosition(this IEntity entity, Vector3 position)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            return position + data.transformedCenter + Vector3.down * (data.scaledHeight / 2.0f + data.skinWidth);
        }
    }
}