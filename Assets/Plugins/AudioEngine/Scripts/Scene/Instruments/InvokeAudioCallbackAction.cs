using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class InvokeAudioCallbackAction : IAudioEventAction
    {
        [SerializeField]
        private AudioCallbackKey callbackId;
        
        public void Invoke(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            system.InvokeCallback(this.callbackId);
        }
    }
}