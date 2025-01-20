using System;
using Atomic.AI;
using UnityEngine;

namespace Modules.Extensions
{
    
    [Serializable]
    public sealed class ColliderBufferBlackboardInstaller : IBlackboardInstaller
    {
        [BlackboardKey]
        [SerializeField]
        private int key;
        
        [Range(0, 256)]
        [SerializeField]
        private int bufferSize;
        
        public void Install(IBlackboard blackboard)
        {
            blackboard.SetStruct(this.key, new BufferData<Collider>
            {
                values = new Collider[this.bufferSize],
                size = 0
            });
        }
    }
}