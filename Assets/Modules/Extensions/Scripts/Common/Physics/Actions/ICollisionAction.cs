using UnityEngine;

namespace Atomic.Elements
{
    public interface ICollisionAction : IAction<Collision>
    {
    }
    
    public interface ICollisionAction2D : IAction<Collision2D>
    {
    }
}