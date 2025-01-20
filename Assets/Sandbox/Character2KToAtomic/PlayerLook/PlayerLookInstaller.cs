using Atomic.Entities;
using UnityEngine;

namespace Sandbox
{
    public sealed class PlayerLookInstaller : SceneEntityInstallerBase
    {
        [EntityValue]
        [SerializeField]
        private int dataKey;
        
        [SerializeField]
        private PlayerLookData data;
        
        public override void Install(IEntity entity)
        {
            data.camera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
            
            entity.AddValue(this.dataKey, this.data);
            entity.WhenInit(() =>
            {
                // set camera parent to player
                data.camera!.transform.SetParent(transform, false);

                // look into player forward direction, which was loaded from the db
                data.camera.transform.rotation = transform.rotation;

                // set camera to head position
                data.camera.transform.position = data.headPosition;

                // remember original camera position
                data.originalCameraPosition = data.camera.transform.localPosition;
            });
            
            
            entity.WhenUpdate(_ => entity.PlayerLookUpdate());
            entity.WhenLateUpdate(_ => entity.PlayerLookLateUpdate());
        }
    }
}