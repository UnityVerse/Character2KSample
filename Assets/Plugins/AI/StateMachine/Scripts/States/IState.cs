using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.StateMachine", null)] 
    public interface IState : IStartAIBehaviour, IUpdateAIBehaviour, IStopAIBehaviour
    {
        public string Name { get; }

        void OnEnter(IBlackboard blackboard);
        void OnExit(IBlackboard blackboard);

        void IStartAIBehaviour.OnStart(IBlackboard blackboard) => this.OnEnter(blackboard);
        void IStopAIBehaviour.OnStop(IBlackboard blackboard) => this.OnExit(blackboard);
    }
}