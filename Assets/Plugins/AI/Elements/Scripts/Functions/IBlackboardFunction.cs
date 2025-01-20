using UnityEngine.Scripting.APIUpdating;

namespace Atomic.AI
{
    [MovedFrom(true, "Modules.AI", "Modules.AI.Elements")]
    public interface IBlackboardFunction<out R>
    {
        R Invoke(IBlackboard blackboard);
    }

    public interface IBlackboardFunction<in T, out R>
    {
        R Invoke(IBlackboard blackboard, T arg);
    }

    public interface IBlackboardFunction<in T1, in T2, out R>
    {
        R Invoke(IBlackboard blackboard, T1 arg1, T2 arg2);
    }
}