using System;
using Unity.Mathematics;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard")]
    [Serializable]
    public sealed class QuaternionInstaller : BlackboardInstaller<quaternion>
    {
        public override void Install(IBlackboard blackboard)
        {
            blackboard.SetQuaternion(this.key, this.value);
        }
    }
}