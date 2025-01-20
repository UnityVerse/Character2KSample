using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Set the capsule's height (local). Minimum limit is double the capsule radius size.
        // Call CanSetHeight if you want to test if height can change, e.g. when changing from crouch to stand.
        //   newHeight: The new height.
        //   preserveFootPosition: Adjust the capsule's center to preserve the foot position?
        //   checkForPenetration: Check for collision, and then de-penetrate if there's collision?
        //   updateGrounded: Update the grounded state? This uses a cast, so only set it to true if you need it.
        public static bool TrySetHeight(
            this IEntity entity,
            float newHeight,
            bool preserveFootPosition,
            bool checkForPenetration,
            bool updateGrounded
        )
        {
            // only resize if we will succeed. otherwise don't.
            if (checkForPenetration && !entity.CanSetHeight(newHeight, preserveFootPosition))
                return false;

            // vis2k fix:
            // IMPORTANT: adjust height BEFORE ever calculating the center.
            //            previously it was adjusted AFTER calculating the center.
            //            so the center would NOT EXACTLY be the center anymore
            //            if the height was adjusted.
            //            => causing all future center calculations to be wrong.
            //            => causing center.y to increase every time
            //            => causing the character to float in the air over time
            //            see also: https://github.com/vis2k/uMMORPG/issues/36
            newHeight = entity.ValidateHeight(newHeight);
            
            var data = (Character2KData) entity.GetCharacter2KData();


            Vector3 virtualPosition = data.transform.position;
            Vector3 newCenter = preserveFootPosition
                ? Helpers.CalculateCenterWithSameFootPosition(data.center, data.height, newHeight, data.skinWidth)
                : data.center;
            if (Mathf.Approximately(data.height, newHeight))
            {
                // Height remains the same. only set new center, which may have
                // changed because of preserveFootPosition
                return entity.TrySetCenter(newCenter, checkForPenetration, updateGrounded);
            }

            float oldHeight = data.height;
            Vector3 oldCenter = data.center;
            Vector3 oldPosition = data.transform.position;

            if (preserveFootPosition)
            {
                data.center = newCenter;
            }

            data.height = newHeight;
            entity.ValidateCapsule(true, ref virtualPosition);

            bool result = true;
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
                        if (preserveFootPosition)
                        {
                            data.center = oldCenter;
                        }

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