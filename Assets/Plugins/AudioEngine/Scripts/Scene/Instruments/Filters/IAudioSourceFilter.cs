using UnityEngine;

namespace AudioEngine
{
    internal interface IAudioSourceFilter
    {
        void Apply(AudioSource source);
        void Discard(AudioSource source);
    }
}