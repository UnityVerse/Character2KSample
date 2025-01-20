using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Set the position of the character.
        //   position: Position to set.
        //   updateGrounded: Update the grounded state? This uses a cast, so only set it to true if you need it.
        public static void SetPosition(this IEntity entity, Vector3 position, bool updateGrounded)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            data.transform.position = position;

            if (updateGrounded)
            {
                entity.UpdateGrounded(CollisionFlags.None);
            }
        }
    }
}