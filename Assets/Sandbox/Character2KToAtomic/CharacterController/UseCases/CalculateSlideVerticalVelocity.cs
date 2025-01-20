using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // calculate y (down) component of slide move
        public static float CalculateSlideVerticalVelocity(
            Vector3 slopeNormal,
            float slidingTime,
            float slideGravityMultiplier,
            float slideMaxSpeed
        )
        {
            // calculate slope angle
            float slopeAngle = Vector3.Angle(Vector3.up, slopeNormal);

            // Speed increases as slope angle increases
            float slideSpeedScale = Mathf.Clamp01(slopeAngle / Character2KData.k_MaxSlideAngle);

            // gravity depends on Physics.gravity and how steep the slide is
            float gravity = Mathf.Abs(Physics.gravity.y) * slideGravityMultiplier * slideSpeedScale;

            // Apply gravity and slide along the obstacle
            // -> multiplied by slidingTime so it gets faster the longer we slide
            return -Mathf.Clamp(gravity * slidingTime, 0, Mathf.Abs(slideMaxSpeed));
        }
    }
}