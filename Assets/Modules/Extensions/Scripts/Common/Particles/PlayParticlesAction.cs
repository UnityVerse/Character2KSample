using System;
using Atomic.Elements;
using Modules.Extensions;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Modules.Extensions
{
    [MovedFrom(true, "Game.Engine", "Engine", null)] 
    [Serializable]
    public sealed class PlayParticlesAction : IAction
    {
        [SerializeField]
        private ParticleSystem[] particles;

        [SerializeField]
        private bool withChildren = true;
        
        public void Invoke()
        {
            this.particles.PlayAll(this.withChildren);
        }
    }
}