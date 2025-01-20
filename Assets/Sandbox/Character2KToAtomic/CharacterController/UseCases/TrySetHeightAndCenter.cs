using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Try to set the capsule's height and center. Originally, this would
        // keep trying every Update until it's safe to resize. this required a
        // LOT of magic. It now either resizes immediately or it returns false
        // if there is no space.
        //   newHeight: The new height.
        //   newCenter: The new center.
        //   checkForPenetration: Check for collision, and then de-penetrate if there's collision?
        //   updateGrounded: Update the grounded state? This uses a cast, so only set it to true if you need it.
        public static bool TrySetHeightAndCenter(
            this IEntity entity,
            float newHeight,
            Vector3 newCenter,
            bool checkForPenetration,
            bool updateGrounded
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            // only resize if we will succeed. otherwise don't.
            if (checkForPenetration &&
                !entity.CanSetHeightAndCenter(newHeight, newCenter))
                return false;

            float oldHeight = data.height;
            Vector3 oldCenter = data.center;
            Vector3 oldPosition = data.transform.position;
            Vector3 virtualPosition = oldPosition;

            // set center, set height
            // if either one fails, we don't return immediately.
            // we want to do the penetration test no matter what,
            // and restore data no matter what if it fails.
            bool result = entity.TrySetCenter(newCenter, false, false) &&
                          entity.TrySetHeight(newHeight, false, false, false);

            if (checkForPenetration)
            {
                if (entity.Depenetrate(ref virtualPosition))
                {
                    // Inside colliders?
                    if (entity.CheckCapsule(virtualPosition))
                    {
                        // Restore data
                        // NOTE: even though we simulate the resize first, we
                        // might still end up inside a collider after resizing
                        // because the simulation isn't 100% precise. so we
                        // sometimes still get here when it 'almost' succeeeds
                        data.height = oldHeight;
                        data.center = oldCenter;
                        data.transform.position = oldPosition;
                        entity.ValidateCapsule(true, ref virtualPosition);

                        // return false later
                        result = false;
                    }
                }
            }

            if (updateGrounded)
            {
                entity.UpdateGrounded(CollisionFlags.None);
            }

            data.transform.position = virtualPosition;
            return result;
        }
    }
}