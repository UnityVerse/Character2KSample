using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerAnimationUseCases
    {
        // animation ///////////////////////////////////////////////////////////////

        // Vector.Angle and Quaternion.FromToRotation and Quaternion.Angle all end
        // up clamping the .eulerAngles.y between 0 and 360, so the first overflow
        // angle from 360->0 would result in a negative value (even though we added
        // something to it), causing a rapid twitch between left and right turn
        // animations.
        //
        // the solution is to use the delta quaternion rotation.
        // when turning by 0.5, it is:
        //   0.5 when turning right (0 + angle)
        //   364.6 when turning left (360 - angle)
        // so if we assume that anything >180 is negative then that works great.
        public static float AnimationDeltaUnclamped(Vector3 lastForward, Vector3 currentForward)
        {
            Quaternion rotationDelta = Quaternion.FromToRotation(lastForward, currentForward);
            float turnAngle = rotationDelta.eulerAngles.y;
            return turnAngle >= 180 ? turnAngle - 360 : turnAngle;
        }
    }
}