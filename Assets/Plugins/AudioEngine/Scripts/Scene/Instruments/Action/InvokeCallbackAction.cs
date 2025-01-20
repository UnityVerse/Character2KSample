using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class InvokeCallbackAction : IAudioEventAction
    {
        [SerializeField]
        private AudioCallbackKey callback;
        
        public void Invoke(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            system.InvokeCallback(this.callback);
        }
    }
}