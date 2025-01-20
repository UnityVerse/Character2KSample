using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static void OnTriggerExit(this IEntity entity, Collider co)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // not touching water anymore? then clear water collider
            if (co.CompareTag("Water"))
                movementData.waterCollider = null;
        }
    }
}