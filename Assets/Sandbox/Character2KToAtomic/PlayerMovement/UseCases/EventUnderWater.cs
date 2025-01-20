using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static bool EventUnderWater(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // we can't really make it player position dependent, because he might
            // swim to the surface at which point it might be detected as standing
            // in water but not being under water, etc.
            if (movementData.inWater) // in water and valid water collider?
            {
                // raycasting from water to the bottom at the position of the player
                // seems like a very precise solution
                Vector3 origin = new Vector3(
                    movementData.transform.position.x,
                    movementData.waterCollider.bounds.max.y,
                    movementData.transform.position.z
                );

                float distance = movementData.controllerCollider.height * movementData.underwaterThreshold;
                Debug.DrawLine(origin, origin + Vector3.down * distance, Color.cyan);

                // we are underwater if the raycast doesn't hit anything
                return !Utils.RaycastWithout(
                    origin,
                    Vector3.down,
                    out _,
                    distance,
                    movementData.gameObject,
                    movementData.canStandInWaterCheckLayers
                );
            }

            return false;
        }
    }
}