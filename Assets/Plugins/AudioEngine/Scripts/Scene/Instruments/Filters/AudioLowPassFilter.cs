using System;
using UnityEngine;

namespace AudioEngine
{
    [Serializable]
    internal sealed class AudioLowPassFilter : IAudioSourceFilter
    {
        [SerializeReference] private IFloatProvider cutoffFrequency = new FloatSingle(5007.7f);
        [SerializeReference] private IFloatProvider lowpassResonanceQ = new FloatSingle(1);

        private UnityEngine.AudioLowPassFilter _filter;

        public void Apply(AudioSource source)
        {
            _filter = source.gameObject.AddComponent<UnityEngine.AudioLowPassFilter>();

            _filter.cutoffFrequency = this.cutoffFrequency.Value;
            _filter.lowpassResonanceQ = this.lowpassResonanceQ.Value;
        }

        public void Discard(AudioSource source)
        {
            if (_filter != null)
            {
                GameObject.Destroy(_filter);
            }
        }
    }
}