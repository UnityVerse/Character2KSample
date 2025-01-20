namespace Modules.App
{
    public interface ITimeScaleNode
    {
        string Name { get; }
        
        float EvaluateScale();

        T FindChild<T>(string name) where T : class, ITimeScaleNode;
        ITimeScaleNode FindChild(string name);
    }
}