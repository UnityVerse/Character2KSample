using System;
using Atomic.Elements;
using Cinemachine;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Modules.Extensions
{
    //Move to Extensions!
    [MovedFrom(true, "Game.Engine", "Engine", null)]
    [Serializable]
    public sealed class PlayCameraImpulseAction : IAction
    {
        [SerializeField]
        private CinemachineImpulseSource impulseSource;
        
        public void Invoke()
        {
            this.impulseSource.GenerateImpulse();
        }
    }
}