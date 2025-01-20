using System;
using Atomic.AI;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Modules.Extensions
{
    [MovedFrom(true, null, null, "NoObjectAICondition")]
    [Serializable]
    public sealed class ObjectAbsentBlackboardCondition : IBlackboardCondition
    {
        [BlackboardKey]
        [SerializeField]
        private int objectKey;
        
        public bool Invoke(IBlackboard blackboard)
        {
            return !blackboard.HasObject(this.objectKey);
        }
    }
}