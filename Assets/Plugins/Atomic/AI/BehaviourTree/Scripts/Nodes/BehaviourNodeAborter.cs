using System;
using Sirenix.OdinInspector;

namespace Atomic.AI
{
    public class BehaviourNodeAborter<TSource> : BehaviourNode<TSource>
    {
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly BehaviourNode<TSource> node;
        private readonly Predicate<TSource>[] conditions;
        private readonly Action<TSource> action;
        private readonly bool[] states;

        public BehaviourNodeAborter(
            string name,
            BehaviourNode<TSource> node,
            Predicate<TSource>[] conditions,
            Action<TSource> action
        ) : base(name)
        {
            this.node = node;
            this.conditions = conditions;
            this.action = action;
            this.states = new bool[this.conditions.Length];
        }

        protected override State OnUpdate(TSource source, float deltaTime)
        {
            this.TryAbort(source);
            return this.node.Run(source, deltaTime);
        }

        private void TryAbort(TSource source)
        {
            for (int i = 0, count = this.conditions.Length; i < count; i++)
            {
                bool prevState = this.states[i];
                bool newState = this.conditions[i].Invoke(source);

                if (newState != prevState)
                {
                    this.states[i] = newState;
                    this.node.Abort(source);
                    this.action.Invoke(source);
                    return;
                }
            }
        }

        public override BehaviourNode<TSource> FindChild(string name)
        {
            if (name.Equals(this.node.Name))
            {
                return this.node;
            }

            return this.node.FindChild(name);
        }
    }
}