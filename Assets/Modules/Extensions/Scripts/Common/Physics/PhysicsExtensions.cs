using System.Collections.Generic;
using UnityEngine;

namespace Modules.Extensions
{
    public static class PhysicsExtensions
    {
        public static void EnableAll(this IReadOnlyList<Collider> colliders, bool enabled)
        {
            if (colliders == null)
            {
                return;
            }
            
            int count = colliders.Count;
        
            for (int i = 0; i < count; i++)
            {
                Collider collider = colliders[i];
                if (collider != null)
                {
                    collider.enabled = enabled;                    
                }
            }
        }
    }
}