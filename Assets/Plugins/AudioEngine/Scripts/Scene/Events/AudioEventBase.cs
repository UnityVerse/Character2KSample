using System;
using UnityEngine;

namespace AudioEngine
{
    //TODO: Удалять временные параметры события!
    internal abstract class AudioEventBase : ScriptableObject
    {
        internal string identifier;
        
        internal bool spawned;
        internal AudioSystem audioSystem;

        private protected Vector3 position;
        private protected Quaternion rotation;
        
        private Action<AudioEventBase> _completeCallback;
        
        protected internal virtual Vector3 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        protected internal virtual Quaternion Rotation
        {
            get { return this.rotation; }
            set { this.rotation = value; }
        }

        protected internal abstract void OnStart();
        protected internal abstract void OnUpdate(float deltaTime);
        protected internal abstract void OnStop();

        protected internal abstract void OnPause();
        protected internal abstract void OnResume();

        internal void Initialize(string identifier, AudioSystem audioSystem)
        {
            this.identifier = identifier;
            this.audioSystem = audioSystem;
        }

        internal void SetCallback(Action<AudioEventBase> callback)
        {
            _completeCallback = callback;
        }

        protected void Complete()
        {
            _completeCallback?.Invoke(this);
        }
    }
}