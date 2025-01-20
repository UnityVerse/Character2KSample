using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class VolumeCurveController : IAudioEventController
    {
        [SerializeField]
        private AnimationCurve curve;

        public void Update(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            source.volume = this.curve.Evaluate(args.progress);
        }

        public void Reset()
        {
            
        }
    }
}