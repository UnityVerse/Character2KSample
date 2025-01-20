using System.Collections.Generic;
using Atomic.Entities;
using Controller2k;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Send hit messages.
        public static void BroadcastCollisionEvent(this IEntity entity)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            if (data.collision == null || data.m_CollisionInfoDictionary is not {Count: > 0})
            {
                return;
            }

            foreach (KeyValuePair<Collider, CollisionInfo> kvp in data.m_CollisionInfoDictionary)
            {
                data.collision(kvp.Value);
            }
        }   
    }
}