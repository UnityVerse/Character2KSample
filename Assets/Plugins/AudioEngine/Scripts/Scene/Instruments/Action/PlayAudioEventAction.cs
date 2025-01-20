using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class PlayAudioEventAction : IAudioEventAction
    {
        [SerializeField]
        private AudioEventKey eventId;

        [SerializeField]
        private float threshold;

        public void Invoke(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            system.PlayEvent(this.eventId, @event.Position, @event.Rotation, this.threshold);
        }
    }
}