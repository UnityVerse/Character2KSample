using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

namespace Modules.Extensions
{
    public sealed class PerlinShakeCameraPlaybleBehaviour : PlayableBehaviour
    {
        internal CinemachineBasicMultiChannelPerlin noiseComponent;
        
        internal float frequiency;
        internal float amplitude;
        internal NoiseSettings profile;

        private float _prevFrequiency;
        private float _prevAmplitude;
        private NoiseSettings _prevProfile;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (this.noiseComponent != null)
            {
                _prevProfile = this.noiseComponent.m_NoiseProfile; 
                _prevFrequiency = this.noiseComponent.m_FrequencyGain;
                _prevAmplitude = this.noiseComponent.m_AmplitudeGain;

                this.noiseComponent.m_NoiseProfile = this.profile;
            }
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (this.noiseComponent != null)
            {
                this.noiseComponent.m_FrequencyGain = _prevFrequiency;
                this.noiseComponent.m_AmplitudeGain = _prevAmplitude;
                this.noiseComponent.m_NoiseProfile = _prevProfile;
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (this.noiseComponent != null)
            {
                float weight = info.weight;
                float frequiency = this.frequiency * weight;
                float amplitude = this.amplitude * weight;

                this.noiseComponent.m_FrequencyGain = frequiency;
                this.noiseComponent.m_AmplitudeGain = amplitude;
            }
        }
    }

    public sealed class PerlinShakeCameraPlayableAsset : PlayableAsset
    {
        [SerializeField]
        private ExposedReference<CinemachineVirtualCamera> camera;

        [SerializeField]
        private float frequiency;

        [SerializeField]
        private float amplitude;

        [SerializeField]
        private NoiseSettings profile;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<PerlinShakeCameraPlaybleBehaviour>.Create(graph);
            PerlinShakeCameraPlaybleBehaviour behaviour = playable.GetBehaviour();
            behaviour.profile = this.profile;
            behaviour.amplitude = this.amplitude;
            behaviour.frequiency = this.frequiency;

            CinemachineVirtualCamera camera = this.camera.Resolve(graph.GetResolver());
            if (camera != null)
            {
                behaviour.noiseComponent = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }
            
            return playable;
        }
    }
}