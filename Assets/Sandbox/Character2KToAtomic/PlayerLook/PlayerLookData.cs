using System;
using UnityEngine;

namespace Sandbox
{
    [Serializable]
    public sealed class PlayerLookData
    {
        public GameObject gameObject;
        public Transform transform;
        
        [Header("Components")]
        public Camera camera;

        [Header("Camera")]
        public float XSensitivity = 2;
        public float YSensitivity = 2;
        public float MinimumX = -90;
        public float MaximumX = 90;

        // head position is useful for raycasting etc.
        public Transform firstPersonParent;
        public Vector3 headPosition => firstPersonParent.position;
        public Transform freeLookParent;

        public Vector3 originalCameraPosition;

        public KeyCode freeLookKey = KeyCode.LeftAlt;

        // the layer mask to use when trying to detect view blocking
        // (this way we dont zoom in all the way when standing in another entity)
        // (-> create a entity layer for them if needed)
        public LayerMask viewBlockingLayers;
        public float zoomSpeed = 0.5f;
        public float distance = 0;
        public float minDistance = 0;
        public float maxDistance = 7;

        [Header("Physical Interaction")]
        [Tooltip(
            "Layers to use for raycasting. Check Default, Walls, Player, Zombie, Doors, Interactables, Item, etc. Uncheck IgnoreRaycast, AggroArea, Water, UI, etc.")]
        public LayerMask raycastLayers = Physics.DefaultRaycastLayers;

        // camera offsets. Vector2 because we only want X (left/right) and Y (up/down)
        // to be modified. Z (forward/backward) should NEVER be modified because
        // then we could look through walls when tilting our head forward to look
        // downwards, etc. This can be avoided in the camera positioning logic, but
        // is way to complex and not worth it at all.
        [Header("Offsets - Standing")]
        public Vector2 firstPersonOffsetStanding = Vector2.zero;
        public Vector2 thirdPersonOffsetStanding = Vector2.up;
        public Vector2 thirdPersonOffsetStandingMultiplier = Vector2.zero;

        [Header("Offsets - Crouching")]
        public Vector2 firstPersonOffsetCrouching = Vector2.zero;
        public Vector2 thirdPersonOffsetCrouching = Vector2.up / 2;
        public Vector2 thirdPersonOffsetCrouchingMultiplier = Vector2.zero;
        // scale offset by distance so that 100% zoom in = first person and
        // zooming out puts camera target slightly above head for easier aiming
        public float crouchOriginMultiplier = 0.65f;

        [Header("Offsets - Crawling")]
        public Vector2 firstPersonOffsetCrawling = Vector2.zero;
        public Vector2 thirdPersonOffsetCrawling = Vector2.up;
        public Vector2 thirdPersonOffsetCrawlingMultiplier = Vector2.zero;
        // scale offset by distance so that 100% zoom in = first person and
        // zooming out puts camera target slightly above head for easier aiming
        public float crawlOriginMultiplier = 0.65f;

        [Header("Offsets - Swimming")]
        public Vector2 firstPersonOffsetSwimming = Vector2.zero;
        public Vector2 thirdPersonOffsetSwimming = Vector2.up;
        public Vector2 thirdPersonOffsetSwimmingMultiplier = Vector2.zero;
        // scale offset by distance so that 100% zoom in = first person and
        // zooming out puts camera target slightly above head for easier aiming
        public float swimOriginMultiplier = 0.65f;

        // look directions /////////////////////////////////////////////////////////
        // * for first person, all we need is the camera.forward
        //
        // * for third person, we need to raycast where the camera looks and then
        //   calculate the direction from the eyes.
        //   BUT for animations we actually only want camera.forward because it
        //   looks strange if we stand right in front of a wall, camera aiming above
        //   a player's head (because of head offset) and then the players arms
        //   aiming at that point above his head (on the wall) too.
        //     => he should always appear to aim into the far direction
        //     => he should always fire at the raycasted point
        //   in other words, if we want 1st and 3rd person WITH camera offsets, then
        //   we need both the FAR direction and the RAYCASTED direction
        //
        // * we also need to sync it over the network to animate other players.
        //   => we compress it as far as possible to save bandwidth. syncing it via
        //      rotation bytes X and Y uses 2 instead of 12 bytes per observer(!)
        //
        // * and we can't only calculate and store the values in Update because
        //   ShoulderLookAt needs them live in LateUpdate, Update is too far behind
        //   and would cause the arms to be lag behind a bit.
        //
        public Vector3 lookDirectionFar
        {
            get { return camera.transform.forward; }
        }

        //[SyncVar, HideInInspector] Vector3 syncedLookDirectionRaycasted; not needed atm, see lookPositionRaycasted comment
        public Vector3 lookDirectionRaycasted
        {
            get
            {
                // same for local and other players
                // (positionRaycasted uses camera || syncedDirectionRaycasted anyway)
                return (lookPositionRaycasted - headPosition).normalized;
            }
        }

        // the far position, directionFar projected into nirvana
        public Vector3 lookPositionFar
        {
            get
            {
                Vector3 position = camera.transform.position;
                return position + lookDirectionFar * 9999f;
            }
        }

        // the raycasted position is needed for lookDirectionRaycasted calculation
        // and for firing, so we might as well reuse it here
        public Vector3 lookPositionRaycasted
        {
            get
            {
                // raycast based on position and direction, project into nirvana if nothing hit
                // (not * infinity because might overflow depending on position)
                return Utils.RaycastWithout(camera.transform.position, camera.transform.forward, out var hit,
                    Mathf.Infinity, gameObject, raycastLayers)
                    ? hit.point
                    : lookPositionFar;
            }
        }
    }
}