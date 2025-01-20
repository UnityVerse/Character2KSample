using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AudioEngine
{
    [Serializable]
    internal sealed class PlayAudioEventInRanomIntervalController : IAudioEventController
    {
        [SerializeField]
        private float startTime;
        
        [SerializeField]
        private float endTime;

        // [SerializeField]
        // private Interval[] intervals;
        //
        // [Serializable]
        // private struct Interval
        // {
        //     public float min;
        //     public float max;
        // }

        private float _actionTime;
        private bool _triggered;

        [SerializeField]
        private AudioEventKey eventKey;
        
        

        public void Reset()
        {
            // Debug.Log("UNITY TIME: " + Time.time);
            // int index = Random.Range(0, this.intervals.Length);
            // Interval interval = this.intervals[index];

            _actionTime = Random.Range(this.startTime, this.endTime);
            _triggered = false;
        }
        
        public void Update(AudioEvent @event, AudioSource source, AudioSystem system, AudioFrameArgs args)
        {
            if (!_triggered && args.currentTime >= _actionTime)
            {
                Debug.Log($"ACTION TIME {_actionTime} / UNITY {Time.time}");
                system.PlayEvent(this.eventKey, @event.Position, @event.Rotation);
                _triggered = true;
            }
        }
    }
}