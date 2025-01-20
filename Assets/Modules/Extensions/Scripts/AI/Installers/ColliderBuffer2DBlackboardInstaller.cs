using System;
using Atomic.AI;
using UnityEngine;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class ColliderBuffer2DBlackboardInstaller : IBlackboardInstaller
    {
        [BlackboardKey]
        [SerializeField]
        private int key;
        
        [Range(0, 256)]
        [SerializeField]
        private int bufferSize;
        
        public void Install(IBlackboard blackboard)
        {
            blackboard.SetStruct(this.key, new BufferData<Collider2D>
            {
                values = new Collider2D[this.bufferSize],
                size = 0
            });
        }
    }
}