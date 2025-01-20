using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Blackboard")]
    public interface IBlackboardInstaller
    {
        void Install(IBlackboard blackboard);
    }
}