using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AudioEngine
{
    [Serializable]
    internal sealed class AudioChorusFilter : IAudioSourceFilter
    {
        [SerializeReference] private IFloatProvider dryMix = new FloatSingle(0.5f);
        [SerializeReference] private IFloatProvider delay = new FloatSingle(40);
        [SerializeReference] private IFloatProvider rate = new FloatSingle(0.8f);
        [SerializeReference] private IFloatProvider depth = new FloatSingle(0.03f);

        [Space] 
        [SerializeReference] private IFloatProvider wetMix1 = new FloatSingle(0.5f);
        [SerializeReference] private IFloatProvider wetMix2 = new FloatSingle(0.5f);
        [SerializeReference] private IFloatProvider wetMix3 = new FloatSingle(0.5f);

        private UnityEngine.AudioChorusFilter _filter;
        
        public void Apply(AudioSource source)
        {
            _filter = source.gameObject.AddComponent<UnityEngine.AudioChorusFilter>();
            
            _filter.dryMix = this.dryMix?.Value ?? 0.5f;
            _filter.delay = this.delay?.Value ?? 40;
            _filter.rate = this.rate?.Value ?? 0.8f;
            _filter.depth = this.depth?.Value ?? 0.03f;

            _filter.wetMix1 = wetMix1?.Value ?? 0.5f;
            _filter.wetMix2 = wetMix2?.Value ?? 0.5f;
            _filter.wetMix3 = wetMix3?.Value ?? 0.5f;
        }

        public void Discard(AudioSource source)
        {
            if (_filter != null)
            {
                Object.Destroy(_filter);
                _filter = null;
            }
        }
    }
}