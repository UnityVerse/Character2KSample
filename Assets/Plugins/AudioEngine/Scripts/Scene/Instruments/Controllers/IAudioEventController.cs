using UnityEngine;

namespace AudioEngine
{
    internal interface IAudioEventController
    {
        void Update(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args);
        void Reset();
    }
}