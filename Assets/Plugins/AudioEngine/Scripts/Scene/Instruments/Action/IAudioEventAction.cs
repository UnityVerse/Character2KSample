using UnityEngine;

namespace AudioEngine
{
    internal interface IAudioEventAction
    {
        void Invoke(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args);
    }
}