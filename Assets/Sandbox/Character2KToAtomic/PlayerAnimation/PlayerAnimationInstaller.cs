using Atomic.Entities;
using UnityEngine;

namespace Sandbox
{
    public sealed class PlayerAnimationInstaller : SceneEntityInstallerBase
    {
        [EntityValue]
        [SerializeField]
        private int dataKey;
        
        [SerializeField]
        private PlayerAnimationData data;
        
        public override void Install(IEntity entity)
        {
            entity.AddValue(this.dataKey, this.data);

            entity.WhenInit(() => this.data.lastForward = transform.forward);
            entity.WhenLateUpdate(_ => entity.PlayerAnimationLateUpdate());
        }
    }
}