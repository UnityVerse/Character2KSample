using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Modules.Extensions
{
    public sealed class CinemachineCameraShaker : CinemachineBasicMultiChannelPerlin
    {
        [Space]
        [SerializeField]
        private NoiseSettings shakeSettings;

        [SerializeField]
        private float maxShakeFrequiency;
        
        [SerializeField]
        private float maxShakeAmplitude;

        [Space]
        [SerializeField]
        private NoiseSettings wobbleSettings;

        [SerializeField]
        private float wobbleFrequency = 0.67f;

        [SerializeField]
        private float wobbleAmplitude = 0.67f;

        private readonly List<Shake> persistenShakes = new();
        private readonly Dictionary<Shake, float> temporaryShakes = new();
        
        private readonly List<KeyValuePair<Shake, float>> _temporaryShakeCache = new();

        public override bool IsValid => this.enabled;
        public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Noise;

        public bool AddShake(Shake shake)
        {
            if (this.persistenShakes.Contains(shake))
            {
                return false;
            }
            
            this.persistenShakes.Add(shake);
            return true;
        }

        public bool RemoveShake(Shake shake)
        {
            return this.persistenShakes.Remove(shake);
        }

        public void ShakeOneShot(Shake shake, float duration)
        {
            this.temporaryShakes[shake] = duration;
        }
        
        //Update
        public override void MutateCameraState(ref CameraState curState, float deltaTime)
        {
            float resultAmplitude;
            float resultFrequency;
            NoiseSettings noiseSettings;


            if (this.persistenShakes.Count == 0 && this.temporaryShakes.Count == 0)
            {
                resultAmplitude = this.wobbleAmplitude;
                resultFrequency = this.wobbleFrequency;
                noiseSettings = this.wobbleSettings;
            }
            else
            {
                resultAmplitude = 0.0f;
                resultFrequency = 0.0f;
                
                for (int i = 0, count = this.persistenShakes.Count; i < count; i++)
                {
                    Shake shake = this.persistenShakes[i];
                    resultAmplitude += shake.amplitude;
                    resultFrequency += shake.frequency;
                }
            
                _temporaryShakeCache.Clear();
                _temporaryShakeCache.AddRange(this.temporaryShakes);

                for (int i = 0, count = _temporaryShakeCache.Count; i < count; i++)
                {
                    (Shake shake, float time) = _temporaryShakeCache[i];
                    time -= deltaTime;

                    if (time > 0)
                    {
                        this.temporaryShakes[shake] = time;
                    }
                    else
                    {
                        this.temporaryShakes.Remove(shake);
                    }
                
                    resultAmplitude += shake.amplitude;
                    resultFrequency += shake.frequency;
                }
                
                resultAmplitude = Mathf.Min(resultAmplitude, this.maxShakeAmplitude);
                resultFrequency = Mathf.Min(resultFrequency, this.maxShakeFrequiency);
                noiseSettings = this.shakeSettings;
            }

            m_AmplitudeGain = resultAmplitude;
            m_FrequencyGain = resultFrequency;
            m_NoiseProfile = noiseSettings;
            
            base.MutateCameraState(ref curState, deltaTime);
        }

        public IReadOnlyList<Shake> GetPersistenShakes()
        {
            return this.persistenShakes;
        }

        public IReadOnlyDictionary<Shake, float> GetTemporaryShakes()
        {
            return this.temporaryShakes;
        }

        [Serializable]
        public sealed class Shake
        {
            public float amplitude;
            public float frequency;
        }
    }
}