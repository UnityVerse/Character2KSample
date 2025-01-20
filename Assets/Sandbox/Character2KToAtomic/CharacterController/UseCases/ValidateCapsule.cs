using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Call this when the capsule's values change.
        //      updateCapsuleCollider: Update the capsule collider's values (e.g. center, height, radius)?
        //      currentPosition: position of the character
        //      checkForPenetration: Check for collision, and then de-penetrate if there's collision?
        //      updateGrounded: Update the grounded state? This uses a cast, so only set it to true if you need it.
        public static void ValidateCapsule(
            this IEntity entity,
            bool updateCapsuleCollider,
            ref Vector3 currentPosition,
            bool checkForPenetration = false,
            bool updateGrounded = false
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();

            data.slopeLimit = Mathf.Clamp(data.slopeLimit, 0.0f, Character2KData.k_MaxSlopeLimit);
            data.skinWidth = Mathf.Clamp(data.skinWidth, Character2KData.k_MinSkinWidth, float.MaxValue);
            float oldHeight = data.height;
            data.height = entity.ValidateHeight(data.height);

            if (data.m_CapsuleCollider != null)
            {
                if (updateCapsuleCollider)
                {
                    // Copy settings to the capsule collider
                    data.m_CapsuleCollider.center = data.center;
                    data.m_CapsuleCollider.radius = data.radius;
                    data.m_CapsuleCollider.height = data.height;
                }
                else if (!Mathf.Approximately(data.height, oldHeight))
                {
                    // Height changed
                    data.m_CapsuleCollider.height = data.height;
                }
            }

            if (checkForPenetration)
            {
                entity.Depenetrate(ref currentPosition);
            }

            if (updateGrounded)
            {
                entity.UpdateGrounded(CollisionFlags.None);
            }
        }
    }
}