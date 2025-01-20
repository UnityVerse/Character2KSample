using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerMovementUseCases
    {
        // input directions ////////////////////////////////////////////////////////
        public static Vector2 GetInputDirection()
        {
            // get input direction while alive and while not typing in chat
            // (otherwise 0 so we keep falling even if we die while jumping etc.)
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            return new Vector2(horizontal, vertical).normalized;
        }
    }
}