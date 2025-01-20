using System;

namespace Atomic.AI
{
    [Serializable]
    public abstract class BehaviorNodeAsset_Decorator<TSource> : BehaviourNodeAsset<TSource>
    {
        protected abstract IBehaviourNodeAsset<TSource> Node { get; }
        protected abstract Action<TSource> EnableAction { get; }
        protected abstract Action<TSource> DisableAction { get; }
        protected abstract Action<TSource> UpdateAction { get; }
        protected abstract Action<TSource> AbortAction { get; }

        protected override BehaviourNode<TSource> Create(string name)
        {
            return new BehaviourNodeDecorator<TSource>(
                this.Node.Create(),
                this.EnableAction,
                this.DisableAction,
                this.UpdateAction,
                this.AbortAction
            );
        }
    }
}