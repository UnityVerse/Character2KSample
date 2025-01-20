using Atomic.Entities;
using UnityEngine;

namespace Sandbox
{
    public sealed class Character2KEntityInstaller : SceneEntityInstallerBase
    {
        [EntityValue]
        [SerializeField]
        private int characterDataKey;

        [SerializeField]
        private Character2KData characterData;
        
        public override void Install(IEntity entity)
        {
            entity.AddValue(characterDataKey, this.characterData);
            
            entity.WhenInit(entity.InitCapsuleColliderAndRigidbody);
            entity.WhenInit(entity.SetRootToOffset);
            entity.WhenInit(entity.SetupSlotMovementOffset);
            
            entity.WhenUpdate(entity.UpdateSlideDownSlopes);
            entity.WhenLateUpdate(_ => entity.SetRootToOffset());
        }
    }
}