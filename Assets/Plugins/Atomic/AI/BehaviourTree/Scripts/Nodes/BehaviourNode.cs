using Sirenix.OdinInspector;

namespace Atomic.AI
{
    public abstract class BehaviourNode<TSource>
    {
        public enum State
        {
            RUNNING = 0,
            SUCCESS = 1,
            FAILURE = 2
        }

        public string Name => this.name;
        public bool IsRunning => this.isRunning;

        [ShowInInspector, ReadOnly, HideInEditorMode]
        private readonly string name;
        
        [ShowInInspector, ReadOnly, HideInEditorMode]
        private bool isRunning;

        protected BehaviourNode(string name)
        {
            this.name = name;
        }

        public State Run(TSource source, float deltaTime)
        {
            if (!this.isRunning)
            {
                this.isRunning = true;
                this.OnEnable(source);
            }

            State result = this.OnUpdate(source, deltaTime);

            if (result != State.RUNNING)
            {
                this.isRunning = false;
                this.OnDisable(source);
            }

            return result;
        }

        public void Abort(TSource source)
        {
            if (this.isRunning)
            {
                this.isRunning = false;
                this.OnAbort(source);
                this.OnDisable(source);
            }
        }

        protected abstract State OnUpdate(TSource source, float deltaTime);
        
        protected virtual void OnEnable(TSource source)
        {
        }

        protected virtual void OnDisable(TSource source)
        {
        }

        protected virtual void OnAbort(TSource source)
        {
        }

        public virtual BehaviourNode<TSource> FindChild(string name)
        {
            return null;
        }
    }
}