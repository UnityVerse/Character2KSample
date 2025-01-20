using System;
using Sirenix.OdinInspector;

namespace Atomic.AI
{
    public class BehaviourNodeDecorator<TSource> : BehaviourNode<TSource>
    {
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly BehaviourNode<TSource> node;

        private readonly Action<TSource> enableAction;
        private readonly Action<TSource> disableAction;
        private readonly Action<TSource> updateAction;
        private readonly Action<TSource> abortAction;

        public BehaviourNodeDecorator(
            BehaviourNode<TSource> node,
            Action<TSource> enableAction,
            Action<TSource> disableAction,
            Action<TSource> updateAction,
            Action<TSource> abortAction
        ) : base(node.Name)
        {
            this.node = node;
            this.enableAction = enableAction;
            this.disableAction = disableAction;
            this.updateAction = updateAction;
            this.abortAction = abortAction;
        }

        protected override void OnEnable(TSource source)
        {
            this.enableAction.Invoke(source);
        }

        protected override void OnDisable(TSource source)
        {
            this.disableAction.Invoke(source);
        }

        protected override State OnUpdate(TSource source, float deltaTime)
        {
            this.updateAction.Invoke(source);
            return this.node.Run(source, deltaTime);
        }

        protected override void OnAbort(TSource source)
        {
            this.abortAction.Invoke(source);
            this.node.Abort(source);
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