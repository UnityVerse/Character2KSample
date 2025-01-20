using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay
{
    [Serializable]
    public sealed class ProductionSlot<T> where T : class
    {
        public enum State
        {
            IDLE = 0,
            PLAYING = 1,
            PAUSED = 2,
            FINISHED = 3
        }
        
        public event Action<T> OnStarted;
        public event Action<T> OnFinished;
        public event Action<T> OnPaused;
        public event Action<T> OnResumed;
        public event Action<T> OnCanceled;
        public event Action<T> OnCompleted; 

        public event Action<State> OnStateChanged;

        public event Action<float> OnCurrentTimeChanged;
        public event Action<float> OnProgressChanged;

        private readonly float multiplier;
        
        private float currentTime;
        private State currentState;

        private T _target;
        private float _targetDuration;

        [ShowInInspector, ReadOnly]
        public State CurrentState => currentState;

        public bool IsIdle => this.currentState == State.IDLE;
        public bool IsPlaying => this.currentState == State.PLAYING;
        public bool IsFinished => this.currentState == State.FINISHED;
        public bool IsPaused => this.currentState == State.PAUSED;

        [ShowInInspector, ReadOnly]
        public float CurrentTime => this.currentTime;

        [ShowInInspector, ReadOnly]
        public float CurrentProgress => this.GetProgress();

        [ShowInInspector, ReadOnly]
        public T Target => _target;

        [ShowInInspector, ReadOnly]
        public float TargetDuration => _targetDuration;

        [ShowInInspector, ReadOnly]
        public float Multiplier => this.multiplier;

        public ProductionSlot(float multiplier = 1)
        {
            this.multiplier = multiplier;
        }

        [Button]
        public void ForceStart(T target, float duration)
        {
            this.Cancel();
            this.Start(target, duration);
        }

        [Button]
        public bool Start(T target, float duration)
        {
            if (target == null)
            {
                throw new Exception("Target is null!");
            }

            if (duration <= 0)
            {
                throw new Exception("Duration must be more than zero"!);
            }
            
            if (this.currentState != State.IDLE)
            {
                return false;
            }

            _target = target;
            _targetDuration = duration;

            this.currentState = State.PLAYING;
            this.OnStateChanged?.Invoke(State.PLAYING);
            this.OnStarted?.Invoke(target);
            return true;
        }

        [Button]
        public void Tick(float deltaTime)
        {
            if (this.currentState != State.PLAYING)
            {
                return;
            }

            this.currentTime = Mathf.Min(this.currentTime + deltaTime * this.multiplier, _targetDuration);
            this.OnCurrentTimeChanged?.Invoke(this.currentTime);

            float progress = this.currentTime / _targetDuration;
            this.OnProgressChanged?.Invoke(progress);

            if (progress >= 1)
            {
                this.Finish();
            }
        }

        [Button]
        public bool Pause()
        {
            if (this.currentState != State.PLAYING)
            {
                return false;
            }

            this.currentState = State.PAUSED;
            this.OnStateChanged?.Invoke(State.PAUSED);
            this.OnPaused?.Invoke(_target);
            return true;
        }

        [Button]
        public bool Resume()
        {
            if (this.currentState != State.PAUSED)
            {
                return false;
            }

            this.currentState = State.PLAYING;
            this.OnStateChanged?.Invoke(State.PLAYING);
            this.OnResumed?.Invoke(_target);
            return true;
        }

        [Button]
        public bool Cancel()
        {
            if (this.currentState == State.IDLE)
            {
                return false;
            }

            T target = _target;

            _target = null;
            _targetDuration = 0;

            this.currentTime = 0;
            this.currentState = State.IDLE;
            this.OnStateChanged?.Invoke(State.IDLE);
            this.OnCanceled?.Invoke(target);
            return true;
        }

        private void Finish()
        {
            T target = _target;
            this.currentState = State.FINISHED;
            this.OnFinished?.Invoke(target);
        }

        public bool Complete()
        {
            if (this.currentState != State.FINISHED)
            {
                return false;
            }

            T target = _target;

            _targetDuration = 0;
            _target = null;
            this.currentTime = 0;

            this.currentState = State.IDLE;
            this.OnStateChanged?.Invoke(State.IDLE);
            this.OnCompleted?.Invoke(target);
            return true;
        }

        public float GetProgress()
        {
            return this.currentState switch
            {
                State.PLAYING or State.PAUSED => this.currentTime / _targetDuration,
                State.FINISHED => 1,
                _ => 0
            };
        }
    }
}