using Atomic.Contexts;
using Atomic.Elements;

namespace Modules.Extensions
{
    public interface IContextCondition : IFunction<IContext, bool>
    {
    }
}