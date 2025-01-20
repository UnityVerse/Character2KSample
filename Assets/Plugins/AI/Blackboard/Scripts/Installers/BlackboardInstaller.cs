using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard", "ValueInstaller")] 
    [Serializable, InlineProperty]
    public abstract class BlackboardInstaller<T> : IBlackboardInstaller
    {
        [SerializeField, BlackboardKey]
        protected int key;

        [SerializeField]
        protected T value;

        public abstract void Install(IBlackboard blackboard);
    }
}