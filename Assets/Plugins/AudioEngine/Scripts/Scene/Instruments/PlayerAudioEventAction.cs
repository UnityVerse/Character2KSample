using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class PlayerAudioEventAction : IAudioEventAction
    {
        [SerializeField]
        private AudioEventKey thunderSFX;

        public void Invoke(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            system.PlayEvent(this.thunderSFX, @event.Position, @event.Rotation);
        }
    }
}