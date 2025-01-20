using System;
using System.Collections.Generic;
using Controller2k;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sandbox
{
    [Serializable]
    public sealed class Character2KData
    {
        // Fired on collision with colliders in the world
        public Action<CollisionInfo> collision;

        [Header("MAIN TRANSFORM")]
        [SerializeField]
        public Transform transform;

        [Header("Player Root")]
        [FormerlySerializedAs("m_PlayerRootTransform")]
        [Tooltip("The root bone in the avatar.")]
        public Transform playerRootTransform;

        [FormerlySerializedAs("m_RootTransformOffset")]
        [Tooltip("The root transform will be positioned at this offset.")]
        public Vector3 rootTransformOffset = Vector3.zero;

        [Header("Collision")]
        [FormerlySerializedAs("m_SlopeLimit")]
        [Tooltip("Limits the collider to only climb slopes that are less steep (in degrees) than the indicated value.")]
        public float slopeLimit = 45.0f;

        [FormerlySerializedAs("m_StepOffset")]
        [Tooltip("The character will step up a stair only if it is closer to the ground than the indicated value. " +
                 "This should not be greater than the Character Controller’s height or it will generate an error. " +
                 "Generally this should be kept as small as possible.")]
        public float stepOffset = 0.3f;

        [FormerlySerializedAs("m_SkinWidth")]
        [Tooltip(
            "Two colliders can penetrate each other as deep as their Skin Width. Larger Skin Widths reduce jitter. " +
            "Low Skin Width can cause the character to get stuck. A good setting is to make this value 10% of the Radius.")]
        public float skinWidth = 0.08f;

        [FormerlySerializedAs("m_GroundedTestDistance")]
        [Tooltip(
            "Distance to test beneath the character when doing the grounded test. Increase if controller.isGrounded doesn't give the correct results or switches between true/false a lot.")]
        public float groundedTestDistance = 0.002f; // 0.001f isn't enough for big BoxColliders like uSurvival's Floor, even though it would work for MeshColliders.

        [FormerlySerializedAs("m_Center")]
        [Tooltip("This will offset the Capsule Collider in world space, and won’t affect how the Character pivots. " +
                 "Ideally, x and z should be zero to avoid rotating into another collider.")]
        public Vector3 center;

        [FormerlySerializedAs("m_Radius")]
        [Tooltip("Length of the Capsule Collider’s radius. This is essentially the width of the collider.")]
        public float radius = 0.5f;

        [FormerlySerializedAs("m_Height")]
        [Tooltip("The Character’s Capsule Collider height. It should be at least double the radius.")]
        public float height = 2.0f;

        [FormerlySerializedAs("m_CollisionLayerMask")]
        [Tooltip("Layers to test against for collisions.")]
        public LayerMask collisionLayerMask = ~0; // ~0 sets it to Everything

        [FormerlySerializedAs("m_IsLocalHuman")]
        [Tooltip(
            "Is the character controlled by a local human? If true then more calculations are done for more accurate movement.")]
        public bool isLocalHuman = true;

        [FormerlySerializedAs("m_SlideAlongCeiling")]
        [Tooltip("Can character slide vertically when touching the ceiling? (For example, if ceiling is sloped.)")]
        public bool slideAlongCeiling = true;

        [FormerlySerializedAs("m_SlowAgainstWalls")]
        [Tooltip("Should the character slow down against walls?")]
        public bool slowAgainstWalls = false;

        [FormerlySerializedAs("m_MinSlowAgainstWallsAngle")]
        [Range(0.0f, 90.0f), Tooltip("The minimal angle from which the character will start slowing down on walls.")]
        public float minSlowAgainstWallsAngle = 10.0f;

        [FormerlySerializedAs("m_TriggerQuery")]
        [Tooltip("The desired interaction that cast calls should make against triggers")]
        public QueryTriggerInteraction triggerQuery = QueryTriggerInteraction.Ignore;

        [Header("Sliding")]
        [FormerlySerializedAs("m_SlideDownSlopes")]
        [Tooltip("Should the character slide down slopes when their angle is more than the slope limit?")]
        public bool slideDownSlopes = true;

        [FormerlySerializedAs("m_SlideMaxSpeed")]
        [Tooltip("The maximum speed that the character can slide downwards")]
        public float slideMaxSpeed = 10.0f;

        [FormerlySerializedAs("m_SlideGravityScale")]
        [Tooltip("Gravity multiplier to apply when sliding down slopes.")]
        public float slideGravityMultiplier = 1.0f;

        [FormerlySerializedAs("m_SlideStartTime")]
        [Tooltip(
            "The time (in seconds) after initiating a slide classified as a slide start. Used to disable jumping.")]
        public float slideStartDelay = 0.1f;

        [Tooltip(
            "Slight delay (in seconds) before we stop sliding down slopes. To handle cases where sliding test fails for a few frames.")]
        public float slideStopDelay = 0.1f;

        // Max slope limit.
        public const float k_MaxSlopeLimit = 90.0f;

        // Max slope angle on which character can slide down automatically.
        public const float k_MaxSlideAngle = 90.0f;

        // Distance to test for ground when sliding down slopes.
        public const float k_SlideDownSlopeTestDistance = 1.0f;

        // Distance to push away from slopes when sliding down them.
        public const float k_PushAwayFromSlopeDistance = 0.001f;

        // Minimum distance to use when checking ahead for steep slopes, when checking if it's safe to do the step offset.
        public const float k_MinCheckSteepSlopeAheadDistance = 0.2f;

        // Min skin width.
        public const float k_MinSkinWidth = 0.0001f;

        // The maximum move iterations. Mainly used as a fail safe to prevent an infinite loop.
        public const int k_MaxMoveIterations = 20;

        // Stick to the ground if it is less than this distance from the character.
        public const float k_MaxStickToGroundDownDistance = 1.0f;

        // Min distance to test for the ground when sticking to the ground.
        public const float k_MinStickToGroundDownDistance = 0.01f;

        // Max colliders to use in the overlap methods.
        public const int k_MaxOverlapColliders = 10;

        // Offset to use when moving to a collision point, to try to prevent overlapping the colliders
        public const float k_CollisionOffset = 0.001f;

        // Minimum distance to move. This minimizes small penetrations and inaccurate casts (e.g. into the floor)
        public const float k_MinMoveDistance = 0.0001f;

        // Minimum step offset height to move (if character has a step offset).
        public const float k_MinStepOffsetHeight = k_MinMoveDistance;

        // Small value to test if the movement vector is small.
        public const float k_SmallMoveVector = 1e-6f;

        // If angle between raycast and capsule/sphere cast normal is less than this then use the raycast normal, which is more accurate.
        public const float k_MaxAngleToUseRaycastNormal = 5.0f;

        // Scale the capsule/sphere hit distance when doing the additional raycast to get a more accurate normal
        public const float k_RaycastScaleDistance = 2.0f;

        // Slope check ahead is clamped by the distance moved multiplied by this scale.
        public const float k_SlopeCheckDistanceMultiplier = 5.0f;

        // The capsule collider.
        public CapsuleCollider m_CapsuleCollider;

        // The position at the start of the movement.
        public Vector3 m_StartPosition;

        // Movement vectors used in the move loop.
        public List<MoveVector> m_MoveVectors = new List<MoveVector>();

        // Next index in the moveVectors list.
        public int m_NextMoveVectorIndex;

        // Surface normal of the last collision while moving down.
        public Vector3? m_DownCollisionNormal;

        // Stuck info.
        public StuckInfo m_StuckInfo = new StuckInfo();

        // The collision info when hitting colliders.
        public Dictionary<Collider, CollisionInfo>
            m_CollisionInfoDictionary = new Dictionary<Collider, CollisionInfo>();

        // Collider array used for UnityEngine.Physics.OverlapCapsuleNonAlloc in GetPenetrationInfo
        public readonly Collider[] m_PenetrationInfoColliders = new Collider[k_MaxOverlapColliders];

        // Velocity of the last movement. It's the new position minus the old position.
        // (should only be set internally, but needs to be readable for animations)
        public Vector3 velocity { get; set; }

        // Default center of the capsule (e.g. for resetting it).
        public Vector3 m_DefaultCenter;

        // Used to offset movement raycast when determining if a slope is travesable.
        public float m_SlopeMovementOffset;

        // current sliding state
        // * NONE means we aren't sliding
        // * STARTING means we are standing on a slide, going go start soon
        // * SLIDING means we are sliding down right now
        // * STOPPING means we were standing on a slide, not anymore, just
        //   waiting a little longer so we don't immediately stop sliding when
        //   sliding over an tiny even surface
        public SlidingState slidingState { get; set; }

        // We need to know how long a character has been sliding down a steep
        // slope in order to calculate sliding speed (it gets faster the longer
        // we have been sliding).
        // => remembering the start time is easier than adding += deltaTime
        //    everywhere.
        public float slidingStartedTime;

        // we need to know when the physical sliding finished, so we can remain
        // in sliding state a little longer before we truly quit.
        // this is necessary so we don't immediately stop sliding if we slide
        // over a tiny flat surface.
        public float slidingStoppedTime;

        // The capsule center with scaling and rotation applied.
        public Vector3 transformedCenter
        {
            get { return transform.TransformVector(center); }
        }

        // The capsule height with the relevant scaling applied (e.g. if object scale is not 1,1,1)
        public float scaledHeight
        {
            get { return height * transform.lossyScale.y; }
        }

        // Is the character on the ground? This is updated during Move or SetPosition.
        public bool isGrounded { get; set; }

        // Collision flags from the last move.
        public CollisionFlags collisionFlags { get; set; }

        // Default height of the capsule (e.g. for resetting it).
        public float defaultHeight { get; set; }

        // The capsule radius with the relevant scaling applied (e.g. if object scale is not 1,1,1)
        public float scaledRadius
        {
            get
            {
                Vector3 scale = transform.lossyScale;
                float maxScale = Mathf.Max(Mathf.Max(scale.x, scale.y), scale.z);
                return radius * maxScale;
            }
        }

        // vis2k: add old character controller compatibility
        public Bounds bounds => m_CapsuleCollider.bounds;

        public readonly Collider[] m_OverlapCapsuleColliders = new Collider[k_MaxOverlapColliders];
    }
}