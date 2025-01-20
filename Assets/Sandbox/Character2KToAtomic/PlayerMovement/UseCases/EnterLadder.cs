using Atomic.Entities;
using Game.Gameplay;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        public static void EnterLadder(this IEntity entity)
        {
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // make player look directly at ladder forward. but we also initialize
            // freelook manually already to overwrite the initial rotation, so
            // that in the end, the camera keeps looking at the same angle even
            // though we did modify transform.forward.
            // note: even though we set the rotation perfectly here, there's
            //       still one frame where it seems to interpolate between the
            //       new and the old rotation, which causes 1 odd camera frame.
            //       this could be avoided by overwriting transform.forward once
            //       more in LateUpdate.
            entity.InitializeFreeLook();
            movementData.transform.forward = movementData.ladderCollider.transform.forward;
        }
    }
}