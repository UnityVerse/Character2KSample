using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class VolumeParameterController : IAudioEventController
    {
        [SerializeField]
        private AudioParameterKey volumeKey;

        public void Update(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            if (system.TryGetFloat(this.volumeKey, out float volume))
            {
                source.volume = Mathf.Clamp01(volume);
            }
        }

        public void Reset()
        {
            
        }
    }
}