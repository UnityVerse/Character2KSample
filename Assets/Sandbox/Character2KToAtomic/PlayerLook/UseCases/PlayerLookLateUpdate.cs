using Atomic.Entities;
using Game.Gameplay;
using UnityEngine;

namespace Sandbox
{
    public static partial class PlayerLookUseCases
    {
        // Update camera position after everything else was updated
        public static void PlayerLookLateUpdate(this IEntity entity)
        {
            var lookData = (PlayerLookData) entity.GetPlayerLookData();
            var movementData = (PlayerMovementData) entity.GetPlayerMovementData();

            // clamp camera rotation automatically. this way we can rotate it to
            // whatever we like in Update, and LateUpdate will correct it.
            lookData.camera.transform.localRotation =
                Utils.ClampRotationAroundXAxis(lookData.camera.transform.localRotation, lookData.MinimumX, lookData.MaximumX);

            // zoom after rotating, otherwise it won't be smooth and would overwrite
            // each other.

            // zoom should only happen if not in a UI right now
            if (!Utils.IsCursorOverUserInterface())
            {
                float step = Utils.GetZoomUniversal() * lookData.zoomSpeed;
                lookData.distance = Mathf.Clamp(lookData.distance - step, lookData.minDistance, lookData.maxDistance);
            }

            // calculate target and zoomed position
            if (lookData.distance == 0) // first person
            {
                // we use the current head bone position as origin here
                // -> gets rid of the idle->run head change effect that was odd
                // -> gets rid of upper body culling issues when looking downwards
                Vector3 headLocal = lookData.transform.InverseTransformPoint(lookData.headPosition);
                Vector3 origin = Vector3.zero;
                Vector3 offset = Vector3.zero;

                if (movementData.state == MoveState.CROUCHING)
                {
                    origin = headLocal * lookData.crouchOriginMultiplier;
                    offset = lookData.firstPersonOffsetCrouching;
                }
                else if (movementData.state == MoveState.CRAWLING)
                {
                    origin = headLocal * lookData.crawlOriginMultiplier;
                    offset = lookData.firstPersonOffsetCrawling;
                }
                else if (movementData.state == MoveState.SWIMMING)
                {
                    origin = headLocal;
                    offset = lookData.firstPersonOffsetSwimming;
                }
                else
                {
                    origin = headLocal;
                    offset = lookData.firstPersonOffsetStanding;
                }

                // set final position
                Vector3 target = lookData.transform.TransformPoint(origin + offset);
                lookData.camera.transform.position = target;
            }
            else // third person
            {
                Vector3 origin = Vector3.zero;
                Vector3 offsetBase = Vector3.zero;
                Vector3 offsetMult = Vector3.zero;

                if (movementData.state == MoveState.CROUCHING)
                {
                    origin = lookData.originalCameraPosition * lookData.crouchOriginMultiplier;
                    offsetBase = lookData.thirdPersonOffsetCrouching;
                    offsetMult = lookData.thirdPersonOffsetCrouchingMultiplier;
                }
                else if (movementData.state == MoveState.CRAWLING)
                {
                    origin = lookData.originalCameraPosition * lookData.crawlOriginMultiplier;
                    offsetBase = lookData.thirdPersonOffsetCrawling;
                    offsetMult = lookData.thirdPersonOffsetCrawlingMultiplier;
                }
                else if (movementData.state == MoveState.SWIMMING)
                {
                    origin = lookData.originalCameraPosition * lookData.swimOriginMultiplier;
                    offsetBase = lookData.thirdPersonOffsetSwimming;
                    offsetMult = lookData.thirdPersonOffsetSwimmingMultiplier;
                }
                else
                {
                    origin = lookData.originalCameraPosition;
                    offsetBase = lookData.thirdPersonOffsetStanding;
                    offsetMult = lookData.thirdPersonOffsetStandingMultiplier;
                }

                Vector3 target = lookData.transform.TransformPoint(origin + offsetBase + offsetMult * lookData.distance);
                Vector3 newPosition = target - lookData.camera.transform.rotation * Vector3.forward * lookData.distance;

                // avoid view blocking (only third person, pointless in first person)
                // -> always based on original distance and only overwrite if necessary
                //    so that we dont have to zoom out again after view block disappears
                // -> we cast exactly from cam to target, which is the crosshair position.
                //    if anything is inbetween then view blocking changes the distance.
                //    this works perfectly.
                float finalDistance = lookData.distance;
                Debug.DrawLine(target, lookData.camera.transform.position, Color.white);
                
                if (Physics.Linecast(target, newPosition, out var hit, lookData.viewBlockingLayers))
                {
                    // calculate a better distance (with some space between it)
                    finalDistance = Vector3.Distance(target, hit.point) - 0.1f;
                    Debug.DrawLine(target, hit.point, Color.red);
                }
                else Debug.DrawLine(target, newPosition, Color.green);

                // set final position
                lookData.camera.transform.position = target - lookData.camera.transform.rotation * Vector3.forward * finalDistance;
            }
        }
    }
}