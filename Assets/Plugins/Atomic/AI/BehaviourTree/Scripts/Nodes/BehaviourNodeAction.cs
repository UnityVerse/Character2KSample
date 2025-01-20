using System;

namespace Atomic.AI
{
    public class BehaviourNodeAction<TSource> : BehaviourNode<TSource>
    {
        public readonly Action<TSource> action;

        public BehaviourNodeAction(string name, Action<TSource> action) : base(name)
        {
            this.action = action;
        }

        protected override State OnUpdate(TSource source, float deltaTime)
        {
            this.action.Invoke(source);
            return State.SUCCESS;
        }
    }
}