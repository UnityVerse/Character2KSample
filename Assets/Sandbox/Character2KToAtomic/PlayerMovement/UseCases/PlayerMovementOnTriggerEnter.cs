using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static void OnTriggerEnter(this IEntity entity, Collider co)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // touching ladder? then set ladder collider
            if (co.CompareTag("Ladder"))
                movementData.ladderCollider = co;
            
            // touching water? then set water collider
            else if (co.CompareTag("Water"))
                movementData.waterCollider = co;
        }
    }
}