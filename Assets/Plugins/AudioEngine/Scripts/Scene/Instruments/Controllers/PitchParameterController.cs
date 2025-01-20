using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class PitchParameterController : IAudioEventController
    {
        [SerializeField]
        private AudioParameterKey pitchKey;
        
        public void Update(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            if (system.TryGetFloat(this.pitchKey, out float pitch))
            {
                source.pitch = pitch;
            }
        }

        public void Reset()
        {
            
        }
    }
}