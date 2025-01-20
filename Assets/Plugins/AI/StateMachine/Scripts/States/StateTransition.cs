using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

// ReSharper disable UnassignedField.Local

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.StateMachine")]
    [Serializable]
    public sealed class StateTransition
    {
        [HorizontalGroup]
        [ValueDropdown(nameof(DrawStateNames))]
        [LabelText("From")]
        [SerializeField]
        internal int sourceState;

        [HorizontalGroup]
        [ValueDropdown(nameof(DrawStateNames))]
        [LabelText("    To")]
        [SerializeField]
        internal int targetState;

        [SerializeReference, Space]
        private IBlackboardCondition[] conditions;

        [SerializeReference, Space]
        private IBlackboardAction[] actions;

        public StateTransition()
        {
        }

        public StateTransition(
            int sourceState,
            int targetState,
            IBlackboardCondition[] conditions = null,
            IBlackboardAction[] actions = null
        )
        {
            this.sourceState = sourceState;
            this.targetState = targetState;
            this.conditions = conditions;
            this.actions = actions;
        }

        internal bool Check(IBlackboard blackboard)
        {
            if (this.conditions == null)
            {
                return true;
            }

            for (int i = 0, count = this.conditions.Length; i < count; i++)
            {
                IBlackboardCondition condition = this.conditions[i];
                if (condition == null)
                {
                    continue;
                }

                if (!condition.Invoke(blackboard))
                {
                    return false;
                }
            }

            return true;
        }

        internal void Perform(IBlackboard blackboard)
        {
            int count = this.actions.Length;
            if (count == 0)
            {
                return;
            }

            for (int i = 0; i < count; i++)
            {
                var action = this.actions[i];
                action.Invoke(blackboard);
            }
        }

        internal ValueDropdownList<int> editor_stateNamesDebug;

        internal ValueDropdownList<int> DrawStateNames()
        {
            return editor_stateNamesDebug;
        }
    }
}