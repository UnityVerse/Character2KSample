using System;
using Unity.Mathematics;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard")]
    [Serializable]
    public sealed class Float3Installer : BlackboardInstaller<float3>
    {
        public override void Install(IBlackboard blackboard)
        {
            blackboard.SetFloat3(this.key, this.value);
        }
    }
}