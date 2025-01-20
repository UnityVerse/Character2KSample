using System;
using UnityEngine.Scripting.APIUpdating;
using Object = UnityEngine.Object;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard")]
    [Serializable]
    public sealed class UnityObjectInstaller : BlackboardInstaller<Object>
    {
        public override void Install(IBlackboard blackboard)
        {
            blackboard.SetObject(this.key, this.value);
        }
    }
}