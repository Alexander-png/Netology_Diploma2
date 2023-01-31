using Platformer.CharacterSystem.Movement.Base;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Movement
{
	public class GroundCharacterMovement : CharacterMovement
	{
        private bool _dashCharged = true;
        private bool _inDash;
        private float _dashDirection;
        private bool _jumpedFromGround;
        private int _jumpsLeft;

        private bool IsJumping { get; set; }
        private bool CanDash { get; set; }

        public bool CanJump => _jumpsLeft > 0;
        public float JumpForce => MovementStats.GetJumpForce(_jumpsLeft);
        public float MaxJumpForce => MovementStats.MaxJumpForce;
        public int JumpCountInRow => MovementStats.JumpCountInRow;

        public override float VerticalInput
        {
            get => base.VerticalInput;
            set
            {
                base.VerticalInput = value;
                IsJumping = VerticalInput >= 0.01f;
            }
        }

        public override float DashInput 
        {
            get => base.DashInput;
            set
            {
                base.DashInput = value;
                CanDash = DashInput >= 0.01f && CheckCanDash();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            ResetJumpState();
        }

        private void FixedUpdate()
        {
            if (MovementEnabled)
            {
                Move();
                Jump();
            }
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            OnGround = !InAir;
            if (OnGround)
            {
                ResetJumpState();
            }
        }

        protected override void OnCollisionExit(Collision collision)
        {
            base.OnCollisionExit(collision);
            if (InAir && !_jumpedFromGround)
            {
                _jumpsLeft -= 1;
            }
        }

        protected override void ResetState()
        {
            base.ResetState();
            _inDash = false;
            _dashCharged = true;
            _jumpedFromGround = false;
            ResetJumpState();
            StopAllCoroutines();
        }

        private void Move()
        {
            Vector2 velocity = Velocity;
            if (!CanDash && !_inDash)
            {
                velocity.x += Acceleration * HorizontalInput * Time.deltaTime;
                velocity.x = Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed);
            }
            else
            {
                if (CanDash && !_inDash && _dashCharged)
                {
                    StartCoroutine(DashMove(DashDuration));
                    _dashDirection = Mathf.Sign(HorizontalInput);
                    CanDash = false;
                }
                if (_inDash)
                {
                    velocity.x = DashForce * _dashDirection;
                }
            }
            Velocity = velocity;
        }

        private void Jump()
        {
            if (CanJump && IsJumping)
            {
                Vector2 velocity = Velocity;
                if (!InAir)
                {
                    _jumpedFromGround = true;
                }
                
                if (velocity.y < 0)
                {
                    velocity.y = 0;
                }
                velocity.y = Mathf.Clamp(velocity.y + JumpForce, 0, MaxJumpForce);
                _jumpsLeft -= 1;

                IsJumping = false;
                Velocity = velocity;
            }
        }

        private void ResetJumpState()
        {
            _jumpsLeft = JumpCountInRow;
            _jumpedFromGround = false;
        }

        public override void AddStats(MovementStatsInfo stats)
        {
            base.AddStats(stats);
            ResetJumpState();
        }

        public override void RemoveStats(MovementStatsInfo stats)
        {
            base.RemoveStats(stats);
            ResetJumpState();
        }

        public override void SetVerticalInput(float input) =>
            IsJumping = input >= 0.01f;

        public override void SetDashInput(float input) =>
            CanDash = input >= 0.01f && CheckCanDash();

        private bool CheckCanDash()
        {
            if (HorizontalInput == 0)
            {
                return false;
            }
            return DashForce != 0 && DashDuration != 0;
        }

        private IEnumerator DashMove(float time)
        {
            _inDash = true;
            yield return new WaitForSeconds(time);
            _inDash = false;
            StartCoroutine(RechargeDash(DashRechargeTime));
        }

        private IEnumerator RechargeDash(float time)
        {
            _dashCharged = false;
            yield return new WaitForSeconds(time);
            _dashCharged = true;
        }
    }
}