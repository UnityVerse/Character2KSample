using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard")]
    [Serializable]
    public sealed class TagInstaller : IBlackboardInstaller
    {
        [BlackboardKey]
        [SerializeField]
        private int tag;
        
        public void Install(IBlackboard blackboard)
        {
            blackboard.SetTag(this.tag);
        }
    }
}