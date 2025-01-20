namespace Atomic.AI
{
    public interface IBehaviourNodeAsset<T>
    {
        string Name { get; }

        BehaviourNode<T> Create();
    }
}