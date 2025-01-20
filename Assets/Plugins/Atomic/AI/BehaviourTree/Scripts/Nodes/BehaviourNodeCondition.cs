using System;

namespace Atomic.AI
{
    public class BehaviourNodeCondition<TSource> : BehaviourNode<TSource>
    {
        private readonly Predicate<TSource> condition;

        public BehaviourNodeCondition(string name, Predicate<TSource> condition) : base(name)
        {
            this.condition = condition;
        }

        protected override State OnUpdate(TSource source, float deltaTime)
        {
            return this.condition.Invoke(source) ? State.SUCCESS : State.FAILURE;
        }
    }
}