using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Initialize the capsule collider and the rigidbody
        public static void InitCapsuleColliderAndRigidbody(this IEntity entity)
        {
            Character2KData data = entity.GetValue<Character2KData>(SandboxEntityAPI.Character2KData);
            
            
            GameObject go = data.transform.gameObject;
            data.m_CapsuleCollider = go.GetComponent<CapsuleCollider>();

            // Copy settings to the capsule collider
            data.m_CapsuleCollider.center = data.center;
            data.m_CapsuleCollider.radius = data.radius;
            data.m_CapsuleCollider.height = data.height;

            // Ensure that the rigidbody is kinematic and does not use gravity
            Rigidbody rigidbody = go.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;

            data.defaultHeight = data.height;
            data.m_DefaultCenter = data.center;
        }
    }
}