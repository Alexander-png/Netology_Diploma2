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

        private void FixedUpdate()
        {
            if (MovementEnabled)
            {
                Move();
                Jump();
            }
        }

        protected override void ResetState()
        {
            MoveInput = 0;
            _inDash = false;
            _dashCharged = true;
            StopAllCoroutines();
        }

        private void Move()
        {
            Vector2 velocity = Velocity;
            if (!IsDashing && !_inDash)
            {
                velocity.x += Acceleration * MoveInput * Time.deltaTime;
                velocity.x = Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed);
            }
            else
            {
                if (IsDashing && !_inDash && _dashCharged)
                {
                    StartCoroutine(DashMove(DashDuration));
                    _dashDirection = Mathf.Sign(MoveInput);
                    IsDashing = false;
                }
                if (_inDash)
                {
                    velocity.x = DashForce * Mathf.Sign(_dashDirection);
                }
            }
            Velocity = velocity;
        }

        private void Jump()
        {
            if (CanJump && IsJumping)
            {
                Vector2 velocity = Velocity;

                if (OnGround || InAir)
                {
                    if (velocity.y < 0)
                    {
                        velocity.y = 0;
                    }
                    velocity.y = Mathf.Clamp(velocity.y + JumpForce, 0, MaxJumpForce);
                    JumpsLeft -= 1;
                }
                else if (OnWall)
                {
                    velocity.x += WallClimbRepulsion * -Mathf.Sign(MoveInput);
                    velocity.y += ClimbForce;
                    JumpsLeft -= 1;
                }

                IsJumping = false;
                Velocity = velocity;
            }
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