using Atomic.Elements;
using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public sealed class PlayerMovementInstaller : SceneEntityInstallerBase
    {
        [EntityValue]
        [SerializeField]
        private int movementDataKey;

        [SerializeField]
        private PlayerMovementData movementData;

        [SerializeField]
        private TriggerEventReceiver triggerEventReceiver;
        
        public override void Install(IEntity entity)
        {
            entity.AddTriggerEventReceiver(this.triggerEventReceiver);
            this.triggerEventReceiver.OnEntered += entity.OnTriggerEnter;
            this.triggerEventReceiver.OnExited += entity.OnTriggerExit;

            entity.AddValue(this.movementDataKey, this.movementData);
            entity.WhenInit(() => this.movementData.camera = Camera.main);
            entity.WhenUpdate(_ => entity.PlayerMovementUpdate());
            entity.WhenFixedUpdate(_ => entity.PlayerMovementFixedUpdate());
        }
    }
}