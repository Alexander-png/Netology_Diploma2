using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Movement.Base;
using Platformer.Scriptable.Skills.Data;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Movement
{
	public class GroundCharacterMovement : CharacterMovement
	{
        private bool _dashCharged = true;
        private bool _chargingDash = false;
        private float _dashDirection;

        private bool _jumpedFromGround;
        private int _jumpsLeft;

        private bool DashPressed { get; set; }
        
        public bool CanJump => _jumpsLeft > 0;
        public float JumpForce => MovementStats.GetJumpForce(_jumpsLeft);
        public float MaxJumpForce => MovementStats.MaxJumpForce;
        public int JumpCountInRow => MovementStats.Jumps.Count;

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
                DashPressed = DashInput >= 0.01f && CheckCanDash();
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
                Dash();
                Jump();
            }
        }

        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            OnGround = !InAir;
            if (OnGround)
            {
                InvokeEntityEvent(EntityEventTypes.Landing);
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
            IsDashing = false;
            _dashCharged = true;
            _chargingDash = false;
            _jumpedFromGround = false;
            ResetJumpState();
            StopAllCoroutines();
        }

        private void Move()
        {
            Vector2 velocity = Velocity;
            if (!DashPressed && !IsDashing)
            {
                velocity.x += Acceleration * HorizontalInput * Time.deltaTime;
                velocity.x = Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed);
            }
            Velocity = velocity;
        }

        private void Dash()
        {
            if (!InAir && !_dashCharged && !_chargingDash)
            {
                StartCoroutine(RechargeDash(DashRechargeTime));
                return;
            }

            Vector2 velocity = Velocity;

            if (DashPressed && !IsDashing && _dashCharged)
            {
                _dashCharged = false;
                StartCoroutine(DashMove(DashDuration));
                _dashDirection = Mathf.Sign(HorizontalInput);
                DashPressed = false;
            }
            if (IsDashing)
            {
                velocity = CalclulateDashDirection() * DashForce;
            }
            Velocity = velocity;
        }

        protected override Vector2 CalclulateDashDirection() =>
            new Vector2(_dashDirection, 0f);

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

                if (InAir)
                {
                    StartCoroutine(RechargeDash(DashRechargeTime));
                }

                IsJumping = false;
                Velocity = velocity;
            }
        }

        private void ResetJumpState()
        {
            _jumpsLeft = JumpCountInRow;
            _jumpedFromGround = false;
        }

        public override void AddSkill(MovementSkillData stats)
        {
            base.AddSkill(stats);
            ResetJumpState();
        }

        public override void RemoveSkill(MovementSkillData stats)
        {
            base.RemoveSkill(stats);
            ResetJumpState();
        }

        public override void SetVerticalInput(float input) =>
            IsJumping = input >= 0.01f;

        public override void TriggerDash(float input) =>
            DashPressed = input >= 0.01f && CheckCanDash();

        private bool CheckCanDash()
        {
            if (HorizontalInput == 0)
            {
                return false;
            }
            return DashForce != 0 && DashDuration != 0;
        }

        protected virtual void OnDashStarted() =>
            InvokeEntityEvent(EntityEventTypes.DashStarted);

        protected virtual void OnDashEnded() =>
            InvokeEntityEvent(EntityEventTypes.DashEnded);

        private IEnumerator DashMove(float time)
        {
            IsDashing = true;
            OnDashStarted();
            yield return new WaitForSeconds(time);
            OnDashEnded();
            IsDashing = false;
        }

        private IEnumerator RechargeDash(float time)
        {
            if (_dashCharged || _chargingDash)
            {
                yield break;
            }

            _chargingDash = true;
            yield return new WaitForSeconds(time);
            _dashCharged = true;
            _chargingDash = false;
        }
    }
}