using Platformer.CharacterSystem.Movement.Base;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Movement
{
	public class FlightCharacterMovement : CharacterMovement
	{
        // TODO: move to config
        [SerializeField]
        private float _drag = 10f;
        private bool _inDash;

        private bool CanDash { get; set; }

        public override float DashInput
        {
            get => base.DashInput;
            set
            {
                base.DashInput = value;
                CanDash = DashInput >= 0.01f && CheckCanDash();
            }
        }

        private void Start()
        {
            Body.useGravity = false;
        }

        private void FixedUpdate()
        {
            if (MovementEnabled)
            {
                Move();
            }
        }

        private void Move()
        {
            Vector2 velocity = Velocity;
            if (!CanDash && !_inDash)
            {
                velocity.x = CalculateVelocity(velocity.x, HorizontalInput);
                velocity.y = CalculateVelocity(velocity.y, VerticalInput);
            }
            else
            {
                if (CanDash && !_inDash)
                {
                    StartCoroutine(DashMove(DashDuration));
                    CanDash = false;
                }
                if (_inDash)
                {
                    velocity.x = DashForce * HorizontalInput;
                    velocity.y = DashForce * VerticalInput;
                }
            }
            Velocity = velocity;
        }

        private float CalculateVelocity(float velocity, float input)
        {
            if (Mathf.Abs(input) > 0.0001f)
            {
                velocity += Acceleration * input * Time.deltaTime;
                velocity = Mathf.Clamp(velocity, -MaxSpeed, MaxSpeed);
            }
            else
            {
                velocity = Mathf.SmoothStep(velocity, 0, _drag * Time.deltaTime);
            }
            return velocity;
        }

        protected override void ResetState()
        {
            base.ResetState();
            _inDash = false;
        }

        private IEnumerator DashMove(float time)
        {
            _inDash = true;
            yield return new WaitForSeconds(time);
            _inDash = false;
        }

        private bool CheckCanDash()
        {
            if (HorizontalInput == 0f && VerticalInput != 0f)
            {
                return false;
            }
            return DashForce != 0 && DashDuration != 0;
        }
    }
}