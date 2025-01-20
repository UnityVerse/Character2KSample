using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class AudioSourceVolumeController : IAudioEventController
    {
        [SerializeField]
        private AudioParameterKey volume;
        
        public void Update(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            if (system.TryGetFloat(this.volume, out float volume))
            {
                source.volume = volume;
            }
        }

        public void Reset()
        {
        }
    }
}