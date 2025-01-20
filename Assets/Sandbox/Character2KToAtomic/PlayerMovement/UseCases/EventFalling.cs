using Atomic.Entities;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static bool EventFalling(this IEntity entity)
        {
            // use minimum fall magnitude so walking down steps isn't detected as
            // falling! otherwise walking down steps would show the fall animation
            // and play the landing sound.
            return !entity.IsGroundedWithinTolerance();
        }
    }
}