using System;
using Atomic.AI;
using Atomic.Entities;

namespace Modules.Extensions
{
    [Serializable]
    public sealed class SceneObjectBlackboardInstaller : BlackboardInstaller<SceneEntity>
    {
        public override void Install(IBlackboard blackboard)
        {
            blackboard.SetObject(this.key, this.value);
        }
    }
}