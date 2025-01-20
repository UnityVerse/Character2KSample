using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.BehaviourSet", "IStopBehaviour")]
    public interface IStopAIBehaviour : IAIBehaviour
    {
        void OnStop(IBlackboard blackboard);
    }
}