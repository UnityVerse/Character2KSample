using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.BehaviourSet", "IUpdateBehaviour")]
    public interface IUpdateAIBehaviour : IAIBehaviour
    {
        void OnUpdate(IBlackboard blackboard, float deltaTime);
    }
}