using System;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard")]
    [Serializable]
    public sealed class IntInstaller : BlackboardInstaller<int>
    {
        public override void Install(IBlackboard blackboard)
        {
            blackboard.SetInt(this.key, this.value);
        }
    }
}