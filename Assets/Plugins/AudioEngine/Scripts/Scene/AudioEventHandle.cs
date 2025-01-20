using System;
using UnityEngine;

namespace AudioEngine
{
    public readonly struct AudioEventHandle
    {
        private readonly AudioEventBase audioEvent;
        private readonly AudioSystem audioSystem;
        
        public string Id
        {
            get { return this.audioEvent.identifier; }
        }

        public bool IsPlaying
        {
            get { return this.audioSystem.IsPlayingEvent(this.audioEvent); }
        }

        internal AudioEventHandle(AudioEventBase audioEvent, AudioSystem audioSystem)
        {
            this.audioEvent = audioEvent;
            this.audioSystem = audioSystem;
        }
        
        public void SetPosition(Vector3 position)
        {
            if (this.audioEvent.spawned)
            {
                this.audioEvent.Position = position;
            }
        }

        public void SetRotation(Quaternion quaternion)
        {
            if (this.audioEvent.spawned)
            {
                this.audioEvent.Rotation = quaternion;
            }
        }
        
        public void SetArgs(AudioArgs args)
        {
            
        }
        
        public void Play()
        {
            if (this.audioEvent.spawned)
            {
                this.audioSystem.StartEvent(this.audioEvent);
            }
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            if (this.audioEvent.spawned)
            {
                this.audioSystem.StopEvent(this.audioEvent);
            }
        }

        public void Stop(float fadeoutTime, AnimationCurve curve)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (this.audioEvent.spawned)
            {
                this.audioSystem.DisposeEvent(this.audioEvent);
            }
        }
    }
}