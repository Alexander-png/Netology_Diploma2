using Platformer.CharacterSystem.Movement.Base;
using Platformer.LevelEnvironment.Elements.Common;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Movement
{
	public class GroundCharacterMovement : CharacterMovement
	{
        private bool _dashCharged = true;
        private bool _inDash;
        private float _dashDirection;

        protected override void Awake()
        {
            base.Awake();
            ResetJumpCounter();
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
            base.OnCollisionExit(collision);

            OnGround = !InAir;

            //if (collision.gameObject.TryGetComponent(out Platform plat))
            //{
            //    if (CurrentCollisionNormal != Vector3.zero)
            //    {
            //        if (newNormal.x != 0 && CurrentCollisionNormal.y != 0)
            //        {
            //            OnWall = true;
            //            OnGround = false;
            //            CurrentCollisionNormal = newNormal;
            //        }
            //    }
            //    else
            //    {
            //        CurrentCollisionNormal = newNormal;
            //        OnWall = plat.Climbable ? Mathf.Abs(CurrentCollisionNormal.x) > 0.9 : false;
            //        OnGround = !OnWall;
            //    }

            //    if ((OnGround || OnWall) && plat.Climbable)
            //    {
            //        ResetJumpCounter();
            //    }
            //}
        }

        protected override void ResetState()
        {
            HorizontalInput = 0;
            _inDash = false;
            _dashCharged = true;
            StopAllCoroutines();
        }

        private void Move()
        {
            Vector2 velocity = Velocity;
            if (!IsDashing && !_inDash)
            {
                velocity.x += Acceleration * HorizontalInput * Time.deltaTime;
                velocity.x = Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed);
            }
            else
            {
                if (IsDashing && !_inDash && _dashCharged)
                {
                    StartCoroutine(DashMove(DashDuration));
                    _dashDirection = Mathf.Sign(HorizontalInput);
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
                IsJumping = false;
                Velocity = velocity;
            }
        }

        private void ResetJumpCounter() =>
            JumpsLeft = JumpCountInRow;

        public override void AddStats(MovementStatsInfo stats)
        {
            base.AddStats(stats);
            ResetJumpCounter();
        }

        public override void RemoveStats(MovementStatsInfo stats)
        {
            base.RemoveStats(stats);
            ResetJumpCounter();
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