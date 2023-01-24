using Platformer.CharacterSystem.Movement.Base;
using UnityEngine;

namespace Platformer.CharacterSystem.Movement
{
	public class FlightCharacterMovement : CharacterMovement
	{
        // TODO: move to config
        [SerializeField]
        private float _drag = 10f;

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
            if (Mathf.Abs(HorizontalInput) > 0.0001f)
            {
                velocity.x += Acceleration * HorizontalInput * Time.deltaTime;
                velocity.x = Mathf.Clamp(velocity.x, -MaxSpeed, MaxSpeed);
            }
            else
            {
                velocity.x = Mathf.SmoothStep(velocity.x, 0, _drag * Time.deltaTime);
            }
            if (Mathf.Abs(VerticalInput) > 0.0001f)
            {
                velocity.y += Acceleration * VerticalInput * Time.deltaTime;
                velocity.y = Mathf.Clamp(velocity.y, -MaxSpeed, MaxSpeed);
            }
            else
            {
                velocity.y = Mathf.SmoothStep(velocity.y, 0, _drag * Time.deltaTime);
            }   
            Velocity = velocity;
        }
    }
}