using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static void PlayerMovementUpdate(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            if (!movementData.jumpKeyPressed) movementData.jumpKeyPressed = Input.GetButtonDown("Jump");
            if (!movementData.crawlKeyPressed) movementData.crawlKeyPressed = Input.GetKeyDown(movementData.crawlKey);
            if (!movementData.crouchKeyPressed) movementData.crouchKeyPressed = Input.GetKeyDown(movementData.crouchKey);
        }
    }
}