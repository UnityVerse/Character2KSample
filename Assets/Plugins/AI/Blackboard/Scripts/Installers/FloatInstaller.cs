using System;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard")]
    [Serializable]
    public sealed class FloatInstaller : BlackboardInstaller<float>
    {
        public override void Install(IBlackboard blackboard)
        {
            blackboard.SetFloat(this.key, this.value);
        }
    }
}