using Controller2k;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Test if character can stick to the ground, and set the down vector if so.
        //      moveVector: The original movement vector.
        //      getDownVector: Get the down vector.
        public static bool CanStickToGround(Vector3 moveVector, out MoveVector getDownVector)
        {
            Vector3 moveVectorNoY = new Vector3(moveVector.x, 0.0f, moveVector.z);
            float downDistance = Mathf.Max(moveVectorNoY.magnitude, Character2KData.k_MinStickToGroundDownDistance);
            if (moveVector.y < 0.0f)
            {
                downDistance = Mathf.Max(downDistance, Mathf.Abs(moveVector.y));
            }

            if (downDistance <= Character2KData.k_MaxStickToGroundDownDistance)
            {
                getDownVector = new MoveVector(Vector3.down * downDistance, false);
                return true;
            }

            getDownVector = new MoveVector(Vector3.zero);
            return false;
        }
    }
}