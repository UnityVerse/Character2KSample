using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard")]
    [Serializable]
    public sealed class Float2Installer : BlackboardInstaller<Vector2>
    {
        public override void Install(IBlackboard blackboard)
        {
            blackboard.SetFloat2(this.key, this.value);
        }
    }
}