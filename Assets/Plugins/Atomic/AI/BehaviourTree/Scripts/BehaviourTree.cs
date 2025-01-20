using Sirenix.OdinInspector;

namespace Atomic.AI
{
    public sealed class BehaviourTree<TSource>
    {
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly BehaviourNode<TSource> root;
        
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly TSource source;

        public BehaviourTree(BehaviourNode<TSource> root, TSource source)
        {
            this.root = root;
            this.source = source;
        }

        public void Update(float deltaTime)
        {
            this.root.Run(this.source, deltaTime);
        }

        public void Abort()
        {
            this.root.Abort(this.source);
        }

        public BehaviourNode<TSource> FindChild(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (name.Equals(this.root.Name))
            {
                return this.root;
            }

            return this.root.FindChild(name);
        }
    }
}