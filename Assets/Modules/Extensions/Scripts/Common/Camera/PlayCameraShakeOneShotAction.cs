using System;
using Atomic.Elements;
using Cinemachine;
using UnityEngine;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class PlayCameraShakeOneShotAction : IAction
    {
        [SerializeField]
        private CinemachineVirtualCamera camera;

        [SerializeField]
        private CinemachineCameraShaker.Shake shake;

        [SerializeField]
        private float duration;
    
        public void Invoke()
        {
            this.camera
                .GetCinemachineComponent<CinemachineCameraShaker>()
                .ShakeOneShot(this.shake, this.duration);
        }
    }
}