using System;
using Sirenix.OdinInspector;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard")]
    [Serializable, InlineProperty]
    public sealed class BoolInstaller : BlackboardInstaller<bool>
    {
        public override void Install(IBlackboard blackboard)
        {
            blackboard.SetBool(this.key, this.value);
        }
    }
}