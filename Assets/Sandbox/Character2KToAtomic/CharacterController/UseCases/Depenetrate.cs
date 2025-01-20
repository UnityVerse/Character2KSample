using Atomic.Entities;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Check for collision penetration, then try to de-penetrate if there is collision.
        public static bool Depenetrate(this IEntity entity, ref Vector3 currentPosition)
        {
            if (entity.GetPenetrationInfo(out var distance, out var direction, currentPosition))
            {
                entity.MovePosition(direction * distance, null, null, ref currentPosition);
                return true;
            }

            return false;
        }
    }
}