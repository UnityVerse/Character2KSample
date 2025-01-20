namespace Modules.App
{
    //TODO: Написать тесты!
    public sealed class TimeScaleTree
    {
        private ITimeScaleNode root;

        public TimeScaleTree(ITimeScaleNode root = null)
        {
            this.root = root;
        }

        public void SetRoot(ITimeScaleNode node)
        {
            this.root = node;
        }

        public ITimeScaleNode FindNode(string name)
        {
            if (this.root.Name == name)
            {
                return this.root;
            }

            return this.root.FindChild(name);
        }

        public T FindNode<T>(string name) where T : class, ITimeScaleNode
        {
            if (this.root.Name == name)
            {
                return this.root as T;
            }

            return this.root.FindChild<T>(name);
        }

        public float EvaluateScale()
        {
            if (this.root == null)
            {
                return 1;
            }

            return this.root.EvaluateScale();
        }
    }
}