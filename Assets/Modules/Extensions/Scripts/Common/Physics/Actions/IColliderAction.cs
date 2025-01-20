using UnityEngine;

namespace Atomic.Elements
{
    public interface IColliderAction : IAction<Collider>
    {
    }
    
    public interface IColliderAction2D : IAction<Collider2D>
    {
    }
}