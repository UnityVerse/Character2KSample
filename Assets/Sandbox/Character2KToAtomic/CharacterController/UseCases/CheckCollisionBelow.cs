using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class Character2KUseCases
    {
        // Check for collision below the character, using a ray or sphere cast.
        //   distance: Distance to check.
        //   hitInfo: Get the hit info.
        //   offsetPosition: Position offset. If we want to do a cast relative to the character's current position.
        //   useSphereCast: Use a sphere cast? If false then use a ray cast.
        //   useSecondSphereCast: The second cast includes the skin width. Ideally only needed for human controlled player, for more accuracy.
        //   adjustPositionSlightly: Adjust position slightly up, in case it's already inside an obstacle.
        //   currentPosition: Position of the character
        // True if collision occurred.
        public static bool CheckCollisionBelow(
            this IEntity entity,
            float distance,
            out RaycastHit hitInfo,
            Vector3 currentPosition,
            Vector3 offsetPosition,
            bool useSphereCast = false,
            bool useSecondSphereCast = false,
            bool adjustPositionSlightly = false
        )
        {
            var data = (Character2KData) entity.GetCharacter2KData();


            bool didCollide = false;
            float extraDistance = adjustPositionSlightly ? Character2KData.k_CollisionOffset : 0.0f;
            if (!useSphereCast)
            {
#if UNITY_EDITOR
                Vector3 start = entity.GetFootWorldPosition(currentPosition) + offsetPosition +
                                Vector3.up * extraDistance;

                Debug.DrawLine(start, start + Vector3.down * (distance + extraDistance), Color.red);
#endif
                if (Physics.Raycast(
                        entity.GetFootWorldPosition(currentPosition) + offsetPosition + Vector3.up * extraDistance,
                        Vector3.down,
                        out hitInfo,
                        distance + extraDistance,
                        data.collisionLayerMask,
                        data.triggerQuery))
                {
                    didCollide = true;
                    hitInfo.distance = Mathf.Max(0.0f, hitInfo.distance - extraDistance);
                }
            }
            else
            {
#if UNITY_EDITOR
                Debug.DrawRay(currentPosition, Vector3.down, Color.red); // Center

                Debug.DrawRay(currentPosition + new Vector3(data.scaledRadius, 0.0f), Vector3.down, Color.blue);
                Debug.DrawRay(currentPosition + new Vector3(-data.scaledRadius, 0.0f), Vector3.down, Color.blue);
                Debug.DrawRay(currentPosition + new Vector3(0.0f, 0.0f, data.scaledRadius), Vector3.down, Color.blue);
                Debug.DrawRay(currentPosition + new Vector3(0.0f, 0.0f, -data.scaledRadius), Vector3.down, Color.blue);
#endif
                if (entity.SmallSphereCast(
                        Vector3.down,
                        data.skinWidth + distance,
                        out hitInfo,
                        offsetPosition,
                        true, currentPosition))
                {
                    didCollide = true;
                    hitInfo.distance = Mathf.Max(0.0f, hitInfo.distance - data.skinWidth);
                }

                if (!didCollide && useSecondSphereCast)
                {
                    if (entity.BigSphereCast(
                            Vector3.down,
                            distance + extraDistance, currentPosition,
                            out hitInfo,
                            offsetPosition + Vector3.up * extraDistance,
                            true))
                    {
                        didCollide = true;
                        hitInfo.distance = Mathf.Max(0.0f, hitInfo.distance - extraDistance);
                    }
                }
            }

            return didCollide;
        }
    }
}