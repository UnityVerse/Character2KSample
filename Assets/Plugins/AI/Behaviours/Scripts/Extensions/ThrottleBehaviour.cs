using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Atomic.AI
{
    [Serializable]
    public sealed class ThrottleBehaviour : IStartAIBehaviour, IStopAIBehaviour, IUpdateAIBehaviour
    {
        [SerializeField]
        private float minScanPeriod = 0.2f;

        [SerializeField]
        private float maxScanPeriod = 0.3f;

        private float currentTime;

        [SerializeReference]
        private IUpdateAIBehaviour behaviour = default;

        public void OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            if (this.currentTime > 0)
            {
                this.currentTime -= deltaTime;
            }
            else
            {
                this.behaviour?.OnUpdate(blackboard, deltaTime);
                this.currentTime = Random.Range(this.minScanPeriod, this.maxScanPeriod);
            }
        }

        public void OnStop(IBlackboard blackboard)
        {
            if (this.behaviour is IStopAIBehaviour behaviour)
            {
                behaviour.OnStop(blackboard);
            }
        }

        public void OnStart(IBlackboard blackboard)
        {
            if (this.behaviour is IStartAIBehaviour behaviour)
            {
                behaviour.OnStart(blackboard);
            }
        }
    }
}