using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace AudioEngine
{
    [DefaultExecutionOrder(-1000)]
    [AddComponentMenu("Audio/Audio System", -100)]
    public sealed class AudioSystem : MonoBehaviour, IAudioSystem
    {
        public static AudioSystem Instance { get; private set; }

        [SerializeField]
        private bool initOnAwake = true;

        [Header("Setup")]
        [SerializeField, HideInPlayMode]
        private AudioBank[] initialBanks;

        [FoldoutGroup("Parameters")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly Dictionary<string, bool> boolParameters = new();

        [FoldoutGroup("Parameters")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly Dictionary<string, int> intParameters = new();

        [FoldoutGroup("Parameters")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly Dictionary<string, float> floatParameters = new();

        [FoldoutGroup("Parameters")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly Dictionary<string, Action> triggers = new();

        [FoldoutGroup("Parameters")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly Dictionary<string, List<Action>> callbacks = new();

        [FoldoutGroup("Events")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly Dictionary<AudioEventBase, string> _createdEvents = new();
        private readonly List<KeyValuePair<AudioEventBase, string>> _createdEventsCache = new();

        [FoldoutGroup("Events")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly HashSet<AudioEventBase> _playingEvents = new();
        private readonly List<AudioEventBase> _playingEventsCache = new();

        [FoldoutGroup("Events")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly Dictionary<string, float> _thresholdEvents = new();
        private readonly List<KeyValuePair<string, float>> _thresholdEventsCache = new();

        [FoldoutGroup("Events")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly Dictionary<AudioEventBase, Transform> _eventPivots = new();

        [FoldoutGroup("Pools")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private AudioSourcePool audioSourcePool;

        [FoldoutGroup("Pools")]
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private AudioEventPool audioEventPool;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PlayEvent(string eventId, float threshold = 0, AudioArgs args = null)
        {
            return this.PlayEvent(eventId, Vector3.zero, Quaternion.identity, threshold, args);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PlayEvent(
            string eventId,
            Vector3 position,
            float threshold = 0,
            AudioArgs args = null
        )
        {
            return this.PlayEvent(eventId, position, Quaternion.identity, threshold, args);
        }

        [Title("Methods")]
        [Button, GUIColor(1f, 0.83f, 0f), HideInEditorMode]
        public bool PlayEvent(
            string eventId,
            Vector3 position,
            Quaternion rotation,
            float threshold = 0,
            AudioArgs args = null
        )
        {
            if (_thresholdEvents.ContainsKey(eventId))
            {
                return false;
            }

            if (!this.audioEventPool.TryGet(eventId, out AudioEventBase audioEvent))
            {
                return false;
            }

            audioEvent.Position = position;
            audioEvent.Rotation = rotation;
            audioEvent.SetCallback(this.DisposeEvent);

            _createdEvents.Add(audioEvent, eventId);

            if (threshold > 0)
            {
                _thresholdEvents.Add(eventId, threshold);
            }

            this.StartEvent(audioEvent);

            return true;
        }

        [Button, GUIColor(1f, 0.83f, 0f), HideInEditorMode]
        public bool PlayEvent(string eventId, Transform pivot, float threshold = 0, AudioArgs args = null)
        {
            if (_thresholdEvents.ContainsKey(eventId))
            {
                return false;
            }

            if (!this.audioEventPool.TryGet(eventId, out AudioEventBase audioEvent))
            {
                return false;
            }

            audioEvent.Position = pivot.position;
            audioEvent.Rotation = pivot.rotation;
            audioEvent.SetCallback(this.DisposeEvent);

            _createdEvents.Add(audioEvent, eventId);
            _eventPivots.Add(audioEvent, pivot);

            if (threshold > 0)
            {
                _thresholdEvents.Add(eventId, threshold);
            }

            this.StartEvent(audioEvent);
            return true;
        }

        public AudioEventHandle CreateEvent(string eventId)
        {
            return this.CreateEvent(eventId, Vector3.zero, Quaternion.identity);
        }

        public AudioEventHandle CreateEvent(string eventId, Vector3 position, Quaternion rotation)
        {
            AudioEventBase audioEvent = this.audioEventPool.Get(eventId);
            audioEvent.Position = position;
            audioEvent.Rotation = rotation;
            audioEvent.SetCallback(null);

            _createdEvents.Add(audioEvent, eventId);

            return new AudioEventHandle(audioEvent, this);
        }

        public bool TryCreateEvent(string eventId, out AudioEventHandle result)
        {
            return this.TryCreateEvent(eventId, Vector3.zero, Quaternion.identity, out result);
        }

        public bool TryCreateEvent(string eventId, Vector3 position, Quaternion rotation, out AudioEventHandle result)
        {
            if (!this.audioEventPool.TryGet(eventId, out AudioEventBase audioEvent))
            {
                result = default;
                return false;
            }

            audioEvent.Position = position;
            audioEvent.Rotation = rotation;
            audioEvent.SetCallback(null);

            _createdEvents.Add(audioEvent, eventId);

            result = new AudioEventHandle(audioEvent, this);
            return true;
        }

        public bool DisposeEvent(string eventId)
        {
            _createdEventsCache.Clear();
            _createdEventsCache.AddRange(_createdEvents);

            for (int i = 0, count = _createdEventsCache.Count; i < count; i++)
            {
                (AudioEventBase audioEvent, string fullId) = _createdEventsCache[i];
                if (fullId == eventId)
                {
                    this.DisposeEvent(audioEvent);
                    return true;
                }
            }

            return false;
        }

        public bool DisposeEvent(string eventId, float fadeoutTime)
        {
            throw new NotImplementedException();
        }

        public bool DisposeEvent(string eventId, AnimationCurve curve, float fadeoutTime)
        {
            throw new NotImplementedException();
        }

        public void DisposeEvents(string eventId)
        {
            _createdEventsCache.Clear();
            _createdEventsCache.AddRange(_createdEvents);

            for (int i = 0, count = _createdEventsCache.Count; i < count; i++)
            {
                (AudioEventBase audioEvent, string fullId) = _createdEventsCache[i];
                if (fullId == eventId)
                {
                    this.DisposeEvent(audioEvent);
                }
            }
        }

        public void DisposeEvents(string eventId, float fadeoutTime)
        {
            throw new NotImplementedException();
        }

        public bool DisposeEvents(string eventId, AnimationCurve curve, float fadeoutTime)
        {
            throw new NotImplementedException();
        }

        internal void DisposeEvent(AudioEventBase audioEvent)
        {
            if (!_createdEvents.Remove(audioEvent))
            {
                return;
            }

            if (_playingEvents.Remove(audioEvent))
            {
                audioEvent.OnStop();
            }

            _eventPivots.Remove(audioEvent);

            this.audioEventPool.Release(audioEvent);
        }

        internal void StartEvent(AudioEventBase audioEvent)
        {
            if (_playingEvents.Add(audioEvent))
            {
                audioEvent.OnStart();
            }
        }

        internal bool IsCreatedEvent(AudioEventBase audioEvent)
        {
            return _createdEvents.ContainsKey(audioEvent);
        }

        internal bool IsPlayingEvent(AudioEventBase audioEvent)
        {
            return _playingEvents.Contains(audioEvent);
        }

        internal void StopEvent(AudioEventBase audioEvent)
        {
            if (_playingEvents.Remove(audioEvent))
            {
                audioEvent.OnStop();
            }
        }

        public bool FindEvent(string eventId, out AudioEventHandle result)
        {
            foreach ((AudioEventBase audioEventBase, string fullKey) in _createdEvents)
            {
                if (fullKey == eventId)
                {
                    result = new AudioEventHandle(audioEventBase, this);
                    return true;
                }
            }

            result = default;
            return false;
        }

        public int FindEventsNonAlloc(string eventId, AudioEventHandle[] results)
        {
            int count = 0;

            foreach ((AudioEventBase audioEventBase, string fullKey) in _createdEvents)
            {
                if (fullKey == eventId)
                {
                    results[count++] = new AudioEventHandle(audioEventBase, this);
                }
            }

            return count;
        }

        public bool TryGetBool(string paramId, out bool result)
        {
            return this.boolParameters.TryGetValue(paramId, out result);
        }

        public bool TryGetInt(string paramId, out int result)
        {
            return this.intParameters.TryGetValue(paramId, out result);
        }

        public bool TryGetFloat(string paramId, out float result)
        {
            return this.floatParameters.TryGetValue(paramId, out result);
        }

        public bool GetBool(string paramId)
        {
            return this.boolParameters[paramId];
        }

        public int GetInt(string paramId)
        {
            return this.intParameters[paramId];
        }

        public float GetFloat(string paramId)
        {
            return this.floatParameters[paramId];
        }

        [Button, GUIColor(1f, 0.83f, 0f), HideInEditorMode]
        public void SetBool(string paramId, bool value)
        {
            this.boolParameters[paramId] = value;
        }

        [Button, GUIColor(1f, 0.83f, 0f), HideInEditorMode]
        public void SetInt(string paramId, int value)
        {
            this.intParameters[paramId] = value;
        }

        [Button, GUIColor(1f, 0.83f, 0f), HideInEditorMode]
        public void SetFloat(string paramId, float value)
        {
            this.floatParameters[paramId] = value;
        }

        public void SetTrigger(string triggerId, Action trigger)
        {
            this.triggers[triggerId] = trigger;
        }

        public void ResetTrigger(string triggerId)
        {
            this.triggers.Remove(triggerId);
        }

        public void ListenCallback(string callbackId, Action callback)
        {
            if (!this.callbacks.TryGetValue(callbackId, out List<Action> callbacks))
            {
                callbacks = new List<Action>(1);
                this.callbacks.Add(callbackId, callbacks);
            }

            callbacks.Add(callback);
        }

        public void UnlistenCallback(string callbackId, Action callback)
        {
            if (this.callbacks.TryGetValue(callbackId, out List<Action> callbacks))
            {
                callbacks.Remove(callback);
            }
        }

        internal void InvokeTrigger(string triggerId)
        {
            if (this.triggers.Remove(triggerId, out Action trigger))
            {
                trigger.Invoke();
            }
        }

        internal void InvokeCallback(string callbackId)
        {
            if (!this.callbacks.TryGetValue(callbackId, out List<Action> callbacks))
            {
                return;
            }

            for (int i = 0, count = callbacks.Count; i < count; i++)
            {
                Action callback = callbacks[i];
                callback.Invoke();
            }
        }

        public bool LoadBank(AudioBank audioBank)
        {
            if (audioBank == null)
            {
                return false;
            }

            string bankIdentifier = audioBank.identifier;

            foreach (EventParameter eventParameter in audioBank.eventParameters)
            {
                this.audioEventPool.RegisterPrefab($"{bankIdentifier}.{eventParameter.identifier}",
                    eventParameter.value);
            }

            foreach (BoolParameter boolParameter in audioBank.boolParameters)
            {
                this.boolParameters[$"{bankIdentifier}.{boolParameter.identifier}"] = boolParameter.value;
            }

            foreach (IntParameter intParameter in audioBank.intParameters)
            {
                this.intParameters[$"{bankIdentifier}.{intParameter.identifier}"] = intParameter.value;
            }

            foreach (FloatParameter floatParameter in audioBank.floatParameters)
            {
                this.floatParameters[$"{bankIdentifier}.{floatParameter.identifier}"] = floatParameter.value;
            }

            return true;
        }

        public bool UnloadBank(AudioBank audioBank)
        {
            if (audioBank == null)
            {
                return false;
            }

            string bankIdentifier = audioBank.identifier;

            foreach (EventParameter eventParameter in audioBank.eventParameters)
            {
                this.audioEventPool.UnregisterPrefab($"{bankIdentifier}.{eventParameter.identifier}");
            }

            foreach (BoolParameter boolParameter in audioBank.boolParameters)
            {
                this.boolParameters.Remove($"{bankIdentifier}.{boolParameter.identifier}");
            }

            foreach (IntParameter intParameter in audioBank.intParameters)
            {
                this.intParameters.Remove($"{bankIdentifier}.{intParameter.identifier}");
            }

            foreach (FloatParameter floatParameter in audioBank.floatParameters)
            {
                this.floatParameters.Remove($"{bankIdentifier}.{floatParameter.identifier}");
            }

            return true;
        }

        public void Initialize()
        {
            this.audioSourcePool = new AudioSourcePool(this.transform);
            this.audioEventPool = new AudioEventPool(this);
            this.LoadInitialBanks();
            Instance = this;
        }

        [Button, GUIColor(1f, 0.83f, 0f), HideInEditorMode]
        public void Pause()
        {
            this.enabled = false;

            foreach (AudioEventBase playingEvent in _playingEvents)
            {
                playingEvent.OnPause();
            }
        }

        [Button, GUIColor(1f, 0.83f, 0f), HideInEditorMode]
        public void Resume()
        {
            this.enabled = true;

            foreach (AudioEventBase playingEvent in _playingEvents)
            {
                playingEvent.OnResume();
            }
        }

        internal AudioSource TakeAudioSource()
        {
            return this.audioSourcePool.Take();
        }

        internal void ReleaseAudioSource(AudioSource audioSource)
        {
            this.audioSourcePool.Release(audioSource);
        }

        private void Awake()
        {
            if (this.initOnAwake)
            {
                this.Initialize();
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            this.UpdatePivotEvents();
            this.UpdatePlayingEvents(deltaTime);
            this.UpdateThresholdEvents(deltaTime);
        }

        private void OnDestroy()
        {
            this.audioSourcePool.Dispose();
            this.audioEventPool.Dispose();
            Instance = null;
        }

        private void LoadInitialBanks()
        {
            foreach (AudioBank bank in this.initialBanks)
            {
                if (bank != null) this.LoadBank(bank);
            }
        }

        private void UpdatePlayingEvents(float deltaTime)
        {
            _playingEventsCache.Clear();
            _playingEventsCache.AddRange(_playingEvents);

            for (int i = 0, count = _playingEventsCache.Count; i < count; i++)
            {
                AudioEventBase audioEvent = _playingEventsCache[i];
                audioEvent.OnUpdate(deltaTime);
            }
        }

        private void UpdateThresholdEvents(float deltaTime)
        {
            _thresholdEventsCache.Clear();
            _thresholdEventsCache.AddRange(_thresholdEvents);

            for (int i = 0, count = _thresholdEventsCache.Count; i < count; i++)
            {
                (string eventId, float threshold) = _thresholdEventsCache[i];
                threshold -= deltaTime;

                if (threshold <= 0)
                {
                    _thresholdEvents.Remove(eventId);
                }
                else
                {
                    _thresholdEvents[eventId] = threshold;
                }
            }
        }

        private void UpdatePivotEvents()
        {
            foreach ((AudioEventBase audioEvent, Transform pivot) in _eventPivots)
            {
                audioEvent.Position = pivot.position;
                audioEvent.Rotation = pivot.rotation;
            }
        }
    }
}