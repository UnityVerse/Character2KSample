using System;
using Controller2k;
using UnityEngine;

namespace Sandbox
{
    [Serializable]
    public sealed class PlayerMovementData
    {
        public GameObject gameObject;
        public Transform transform;


        // components to be assigned in inspector
        [Header("Components")]
        public Animator animator;
        public AudioSource feetAudio;
        
        // the collider for the character controller. NOT the hips collider. this
        // one is NOT affected by animations and generally a better choice for state
        // machine logic.
        public CapsuleCollider controllerCollider;
        public Camera camera;

        [Header("State")]
        public MoveState state = MoveState.IDLE;
        [HideInInspector]
        public Vector3 moveDir;

        // it's useful to have both strafe movement (WASD) and rotations (QE)
        // => like in WoW, it more fun to play this way.
        [Header("Rotation")]
        public float rotationSpeed = 150;

        [Header("Walking")]
        public float walkSpeed = 5;
        public float walkAcceleration = 15; // set to maxint for instant speed
        public float walkDeceleration = 20; // feels best if higher than acceleration

        [Header("Running")]
        public float runSpeed = 8;
        [Range(0f, 1f)]
        public float runStepLength = 0.7f;
        public float runStepInterval = 3;
        public float
            runCycleLegOffset =
                0.2f; //specific to the character in sample assets, will need to be modified to work with others
        public KeyCode runKey = KeyCode.LeftShift;
        public float stepCycle;
        public float nextStep;

        [Header("Crouching")]
        public float crouchSpeed = 1.5f;
        public float crouchAcceleration = 5; // set to maxint for instant speed
        public float crouchDeceleration = 10; // feels best if higher than acceleration
        public KeyCode crouchKey = KeyCode.C;
        public bool crouchKeyPressed;

        [Header("Crawling")]
        public float crawlSpeed = 1;
        public float crawlAcceleration = 5; // set to maxint for instant speed
        public float crawlDeceleration = 10; // feels best if higher than acceleration
        public KeyCode crawlKey = KeyCode.V;
        public bool crawlKeyPressed;

        [Header("Swimming")]
        public float swimSpeed = 4;
        public float swimAcceleration = 15; // set to maxint for instant speed
        public float swimDeceleration = 20; // feels best if higher than acceleration
        public float swimSurfaceOffset = 0.25f;
        public Collider waterCollider;
        public bool inWater => waterCollider != null; // standing in water / touching it?
        bool underWater; // deep enough in water so we need to swim?
        [Range(0, 1)]
        public float underwaterThreshold = 0.9f; // percent of body that need to be underwater to start swimming
        public LayerMask
            canStandInWaterCheckLayers = Physics.DefaultRaycastLayers; // set this to everything except water layer

        [Header("Jumping")]
        public float jumpSpeed = 7;
        [HideInInspector]
        public float jumpLeg;
        public bool jumpKeyPressed;

        [Header("Falling")]
        public float airborneAcceleration = 15; // set to maxint for instant speed
        public float airborneDeceleration = 20; // feels best if higher than acceleration
        public float
            fallMinimumMagnitude = 9; // walking down steps shouldn't count as falling and play no falling sound.
        public float fallDamageMinimumMagnitude = 13;
        public float fallDamageMultiplier = 2;
        [HideInInspector]
        public Vector3 lastFall;
        public bool
            sprintingBeforeAirborne; // don't allow sprint key to accelerate while jumping. decision has to be made before that.

        [Header("Climbing")]
        public float climbSpeed = 3;
        public Collider ladderCollider;

        [Header("Mounted")]
        public float mountedRotationSpeed = 100;
        public float mountedAcceleration = 15; // set to maxint for instant speed
        public float mountedDeceleration = 20; // feels best if higher than acceleration

        [Header("Physics")]
        public float gravityMultiplier = 2;

        // we need to remember the last accelerated xz speed without gravity etc.
        // (using moveDir.xz.magnitude doesn't work well with mounted movement)
        public float horizontalSpeed;

        [Header("Sounds")]
        public AudioClip[] footstepSounds; // an array of footstep sounds that will be randomly selected from.
        public AudioClip jumpSound; // the sound played when character leaves the ground.
        public AudioClip landSound; // the sound played when character touches back on ground.
    }
}