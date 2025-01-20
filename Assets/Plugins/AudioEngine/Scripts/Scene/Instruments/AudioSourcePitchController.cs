using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class AudioSourcePitchController : IAudioEventController
    {
        [SerializeField]
        private AudioParameterKey pitch;
        
        public void Update(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            if (system.TryGetFloat(this.pitch, out float pitch))
            {
                source.pitch = pitch;
            }
        }

        public void Reset()
        {
        }
    }
}