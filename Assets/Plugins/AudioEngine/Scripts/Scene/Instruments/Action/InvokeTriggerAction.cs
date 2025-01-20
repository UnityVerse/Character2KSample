using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class InvokeTriggerAction : IAudioEventAction
    {
        [SerializeField]
        private string triggerId;
        
        public void Invoke(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            system.InvokeTrigger(this.triggerId);
        }
    }
}