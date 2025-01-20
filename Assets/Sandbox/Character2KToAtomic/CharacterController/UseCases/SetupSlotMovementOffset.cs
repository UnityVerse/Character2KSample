using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        public static void SetupSlotMovementOffset(this IEntity entity)
        {
            var data = (Character2KData) entity.GetCharacter2KData();
            data.m_SlopeMovementOffset =  data.stepOffset / Mathf.Tan(data.slopeLimit * Mathf.Deg2Rad);
        }
    }
}