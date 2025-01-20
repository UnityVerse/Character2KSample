using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static void ApplyFallDamage(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // measure only the Y direction. we don't want to take fall damage
            // if we jump forward into a wall because xz is high.
            float fallMagnitude = Mathf.Abs(movementData.lastFall.y);
            if (fallMagnitude >= movementData.fallDamageMinimumMagnitude)
            {
                int damage = Mathf.RoundToInt(fallMagnitude * movementData.fallDamageMultiplier);
                Debug.LogWarning("Fall Damage: " + damage);
            }
        }
    }
}