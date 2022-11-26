using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        [Header("Audio Clips")]
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        [Header("Movement Settings")]
        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;

        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;

        /// <summary>
        /// The maximum time the player has to accelerate, while holding down the jump button
        /// </summary>
        public float maxJumpAccelerationTime = 0.5f;

        /// Minimum time between jumps
        public float jumpCooldown = 0.2f;

        [Header("Wall Jump")]
        /// Friction for after wall jumps
        public float wallJumpTakeOffAcceleration = 10f;

        /// Friction for after wall jumps
        public float airFriction = 0.2f;

        /// Timer to keep track of the time the player has left to accelerate
        private float jumpAccelerationTimer;

        /// Timer to keep player from spamming a wall jump
        private float jumpTimer;

        // Terrain layer for raycasting
        public LayerMask terrainLayer;

        [Header("Internal")]
        public JumpState jumpState = JumpState.Grounded;
        /*internal new*/ public Collider2D collider2d;
        /*internal new*/ public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        private bool stopJump;
        private JumpType jump = JumpType.None;
        Vector2 move;
        Vector2 acceleration;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;
        private int direction => spriteRenderer.flipX ? -1 : 1;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");
                if ((jumpState == JumpState.Grounded || onWallForward() || onWallBackward()) && Input.GetButtonDown("Jump") && (jumpTimer <= 0))
                {
                    jumpState = JumpState.PrepareToJump;
                    jumpAccelerationTimer = maxJumpAccelerationTime;
                    jumpTimer = jumpCooldown;
                }
                else if (Input.GetButtonUp("Jump") || jumpAccelerationTimer <= 0)
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
                else if (jumpState != JumpState.Grounded) 
                {
                    jumpAccelerationTimer -= Time.deltaTime;
                }
                jumpTimer -= Time.deltaTime;
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            ReduceAcceleration();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = JumpType.None;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    stopJump = false;
                    if (IsGrounded)
                        jump = JumpType.Normal;
                    else if (onWallForward())
                        jump = JumpType.WallForward;
                    else if (onWallBackward())
                        jump = JumpType.WallBackward;
                    else 
                        jump = JumpType.Normal;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        public void ReduceAcceleration() {
            acceleration = new Vector2(
                Mathf.Max(0, Mathf.Abs(acceleration.x) - (airFriction + move.x) * Time.deltaTime) * Mathf.Sign(acceleration.x), 
                Mathf.Max(0, Mathf.Abs(acceleration.y) - airFriction * Time.deltaTime) * Mathf.Sign(acceleration.y)
            );
        }

        protected override void ComputeVelocity()
        {
            if (jump == JumpType.Normal && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = JumpType.None;
            }
            else if (jump == JumpType.WallForward)
            {
                acceleration.x = -direction * wallJumpTakeOffAcceleration * model.jumpModifier;
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = JumpType.None;
            }
            else if (jump == JumpType.WallBackward)
            {
                acceleration.x = direction * wallJumpTakeOffAcceleration / 2 * model.jumpModifier;
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = JumpType.None;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            FlipSprite();

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            if (IsGrounded) 
                acceleration = Vector2.zero;
            targetVelocity = move * maxSpeed + acceleration;
        }

        private void FlipSprite() {
            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;
        }

        public bool onWallForward() {
            RaycastHit2D raycastHit = Physics2D.BoxCast(Bounds.center, Bounds.size, 0, new Vector2(direction, 0), 0.1f, terrainLayer);
            return raycastHit.collider != null;
        }

        public bool onWallBackward() {
            RaycastHit2D raycastHit = Physics2D.BoxCast(Bounds.center, Bounds.size, 0, new Vector2(-direction, 0), 0.1f, terrainLayer);
            return raycastHit.collider != null;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        public enum JumpType
        {
            None,
            Normal,
            WallForward,
            WallBackward
        }
    }
}