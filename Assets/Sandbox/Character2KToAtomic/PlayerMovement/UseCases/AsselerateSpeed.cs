using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        // acceleration can be different when accelerating/decelerating
        public static float AccelerateSpeed(Vector2 inputDir, float currentSpeed, float targetSpeed, float acceleration)
        {
            // desired speed is between 'speed' and '0'
            float desiredSpeed = inputDir.magnitude * targetSpeed;

            // accelerate speed
            return Mathf.MoveTowards(currentSpeed, desiredSpeed, acceleration * Time.fixedDeltaTime);
        }
    }
}