using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerLookUseCases
    {
        public static void InitializeFreeLook(this IEntity entity)
        {
            var lookData = (PlayerLookData) entity.GetPlayerLookData();
            lookData.camera.transform.SetParent(lookData.freeLookParent, false);
            lookData.freeLookParent.localRotation = Quaternion.identity; // initial rotation := where we look at right now
        }
    }
}