using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.BehaviourSet", "IStartBehaviour")]
    public interface IStartAIBehaviour : IAIBehaviour
    {
        void OnStart(IBlackboard blackboard);
    }
}