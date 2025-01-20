using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable UnassignedField.Local

namespace AudioEngine
{
    [CreateAssetMenu(
        fileName = "AudioEvent",
        menuName = "AudioEngine/Scene/New AudioEvent"
    )]
    internal class AudioEvent : AudioEventBase
    {
        [Title("Main")]
        [SerializeField]
        private float duration;

        [SerializeField]
        private bool loop;

        [SuffixLabel("2D — 3D")]
        [SerializeField, Range(0, 1)]
        private float spartialBlend;

        [SerializeField]
        private AudioMixerGroup output;

        [Title("Audio")]
        [SerializeReference]
        private IClipProvider clip = new ClipSingle();

        [SerializeReference, Space]
        private IFloatProvider pitch = new FloatSingle();

        [SerializeReference, Space]
        private IFloatProvider volume = new FloatSingle();

        [SerializeReference, Space]
        private IFloatProvider reverbZoneMix = new FloatSingle(1);

        [FoldoutGroup("3D")]
        [SerializeField, Space]
        private AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

        [FoldoutGroup("3D")]
        [SerializeField, ShowIf(nameof(rolloffMode), AudioRolloffMode.Custom)]
        private AnimationCurve rolloffCurve;

        [FoldoutGroup("3D")]
        [SerializeReference, Space]
        private IFloatProvider minDistance = new FloatSingle(1);

        [FoldoutGroup("3D")]
        [SerializeReference]
        private IFloatProvider maxDistance = new FloatSingle(500);

        [FoldoutGroup("3D")]
        [SerializeReference]
        private IFloatProvider dopplerLevel = new FloatSingle(0.8f);

        [FoldoutGroup("3D")]
        [SerializeReference]
        private IFloatProvider spread = new FloatSingle(0);

        [Title("Logic")]
        [SerializeReference, Space]
        private IAudioSourceFilter[] filters;

        [SerializeField]
        private ControllerInfo[] controllers;

        [SerializeField, Space]
        private ActionInfo[] actions;

        private AudioSource _audioSource;
        private Transform _audioSourceTransform;

        private float _currentTime;
        private float _progress;

        protected internal override Vector3 Position
        {
            get { return this.position; }
            set
            {
                this.position = value;
                if (_audioSourceTransform != null) _audioSourceTransform.position = value;
            }
        }

        protected internal override Quaternion Rotation
        {
            get { return this.rotation; }
            set
            {
                this.rotation = value;
                if (_audioSourceTransform != null) _audioSourceTransform.rotation = value;
            }
        }

        protected internal override void OnStart()
        {
            _audioSource = audioSystem.TakeAudioSource();

            _audioSource.name = this.identifier;
            _audioSource.outputAudioMixerGroup = this.output;
            _audioSource.loop = this.loop;
            _audioSource.spatialBlend = this.spartialBlend;

            _audioSourceTransform = _audioSource.transform;
            _audioSourceTransform.position = this.position;
            _audioSourceTransform.rotation = this.rotation;

            this.ResetState();
            this.ApplyFilters();

            _audioSource.Play();
        }

        protected internal override void OnStop()
        {
            this.DiscardFilters();
            audioSystem.ReleaseAudioSource(_audioSource);
            _audioSource = null;
        }

        protected internal override void OnUpdate(float deltaTime)
        {
            this.UpdateTime(deltaTime);
            this.UpdateLogic(deltaTime);
        }

        protected internal override void OnPause()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
        }

        protected internal override void OnResume()
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.UnPause();
            }
        }

        private void ResetState()
        {
            _audioSource.volume = this.volume?.Value ?? 1;
            _audioSource.pitch = this.pitch?.Value ?? 1;
            _audioSource.clip = this.clip?.Value;
            _audioSource.reverbZoneMix = reverbZoneMix?.Value ?? 1;

            _audioSource.dopplerLevel = this.dopplerLevel.Value;
            _audioSource.spread = this.spread.Value;
            _audioSource.minDistance = this.minDistance.Value;
            _audioSource.maxDistance = this.maxDistance.Value;
            _audioSource.rolloffMode = this.rolloffMode;
            if (this.rolloffMode == AudioRolloffMode.Custom)
                _audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, this.rolloffCurve);

            _currentTime = 0;
            _progress = 0;

            this.ResetActions();
            this.ResetControllers();
        }

        private void ResetControllers()
        {
            for (int i = 0, count = this.controllers.Length; i < count; i++)
            {
                ControllerInfo controller = this.controllers[i];
                controller.value.Reset();;
            }
        }

        private void ResetActions()
        {
            for (int i = 0, count = this.actions.Length; i < count; i++)
            {
                ActionInfo action = this.actions[i];
                action.passed = false;
            }
        }

        private void UpdateAudioTransform()
        {
            _audioSourceTransform.position = this.position;
            _audioSourceTransform.rotation = this.rotation;
        }

        private void UpdateTime(float deltaTime)
        {
            _currentTime += deltaTime;
            _progress = _currentTime / this.duration;

            if (_progress < 1)
            {
                return;
            }

            if (this.loop)
            {
                this.ResetState();
            }
            else
            {
                this.Complete();
            }
        }

        private void UpdateLogic(float deltaTime)
        {
            AudioFrameArgs frame = new AudioFrameArgs
            {
                currentTime = _currentTime,
                duration = duration,
                progress = _progress,
                deltaTime = deltaTime
            };

            //Process actions:
            for (int i = 0, count = this.actions.Length; i < count; i++)
            {
                ActionInfo action = this.actions[i];

                if (action.off)
                {
                    continue;
                }

                if (!action.passed && _currentTime >= action.time)
                {
                    action.value.Invoke(this, _audioSource, audioSystem, frame);
                    action.passed = true;
                }
            }

            //Process controllers:
            for (int i = 0, count = this.controllers.Length; i < count; i++)
            {
                ControllerInfo controller = this.controllers[i];

                if (controller.off)
                {
                    continue;
                }

                if (controller.fullTime || _currentTime >= controller.startTime && _currentTime <= controller.endTime)
                {
                    controller.value.Update(this, _audioSource, audioSystem, frame);
                }
            }
        }

        private void ApplyFilters()
        {
            for (int i = 0, count = this.filters.Length; i < count; i++)
            {
                IAudioSourceFilter filter = this.filters[i];
                filter.Apply(_audioSource);
            }
        }

        private void DiscardFilters()
        {
            for (int i = 0, count = this.filters.Length; i < count; i++)
            {
                IAudioSourceFilter filter = this.filters[i];
                filter.Discard(_audioSource);
            }
        }

        [Serializable]
        private sealed class ControllerInfo
        {
            [SerializeField]
            public bool fullTime = true;

            [SerializeField, HideIf(nameof(fullTime))]
            public float startTime;

            [SerializeField, HideIf(nameof(fullTime))]
            public float endTime;

            [SerializeReference]
            public IAudioEventController value;

            [SerializeField, Space]
            public bool off;
        }

        [Serializable]
        private sealed class ActionInfo
        {
            [SerializeField]
            public float time;

            [SerializeReference]
            public IAudioEventAction value;

            [SerializeField, Space]
            public bool off;

            internal bool passed;
        }

#if UNITY_EDITOR
        [Title("Tools")]
        [Button("Assign Duration From Clip")]
        [GUIColor(0, 1, 0)]
        private void AssignDurationFromClip()
        {
            this.duration = this.clip?.MaxLength ?? 0;
            AssetDatabase.SaveAssets();
        }
#endif
    }
}