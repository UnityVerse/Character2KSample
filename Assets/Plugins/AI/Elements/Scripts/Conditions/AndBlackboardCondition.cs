using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Elements")]
    [Serializable, InlineProperty, LabelWidth(1)]
    public sealed class AndBlackboardCondition : IBlackboardCondition
    {
        [SerializeReference, HideLabel]
        private IBlackboardCondition[] conditions;

        public AndBlackboardCondition()
        {
        }

        public AndBlackboardCondition(IBlackboardCondition[] conditions)
        {
            this.conditions = conditions;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Invoke(IBlackboard blackboard)
        {
            if (this.conditions == null)
            {
                return true;
            }
            
            for (int i = 0, count = this.conditions.Length; i < count; i++)
            {
                IBlackboardCondition condition = this.conditions[i];
                if (!condition.Invoke(blackboard))
                {
                    return false;
                }
            }

            return true;
        }
    }
}