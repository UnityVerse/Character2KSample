using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Elements")]
    [Serializable, InlineProperty, LabelWidth(1)]
    public sealed class NotBlackboardCondition : IBlackboardCondition
    {
        [SerializeReference, HideLabel]
        private IBlackboardCondition condition = default;
        
        public bool Invoke(IBlackboard blackboard)
        {
            if (this.condition == null)
            {
                return false;
            }
            
            return !this.condition.Invoke(blackboard);
        }
    }
}