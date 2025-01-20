using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class PitchCurveController : IAudioEventController
    {
        [SerializeField]
        private AnimationCurve curve;

        public void Update(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            source.pitch = this.curve.Evaluate(args.progress);
        }

        public void Reset()
        {
            
        }
    }
}