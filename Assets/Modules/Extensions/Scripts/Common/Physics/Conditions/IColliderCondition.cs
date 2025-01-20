using Atomic.Elements;
using UnityEngine;

namespace Modules.Extensions
{
    public interface IColliderCondition : IFunction<Collider, bool>
    {
    }
}