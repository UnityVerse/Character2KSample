using System;
using Atomic.AI;
using UnityEngine;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class ColliderSensor : IUpdateAIBehaviour
    {
        [BlackboardKey]
        [SerializeField]
        private int bufferKey;

        [Space]
        [SerializeReference]
        private IColliderBufferBlackboardAction castFunction = default;

        public void OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            ref var buffer = ref blackboard.GetStruct<BufferData<Collider>>(this.bufferKey);
            this.castFunction.Invoke(blackboard, ref buffer);
        }
    }
}