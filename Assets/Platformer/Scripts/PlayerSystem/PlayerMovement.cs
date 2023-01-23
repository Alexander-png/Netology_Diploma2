using Platformer.CharacterSystem.Movement;

namespace Platformer.PlayerSystem
{
	public class PlayerMovement : GroundCharacterMovement
    {
        public void OnRunInput(float input) =>
            MoveInput = input;

        public void OnDashInput(float input) =>
            IsDashing = input >= 0.01f && CheckCanDash();

        public void OnJumpInput(float input) =>
            IsJumping = input >= 0.01f;

        private bool CheckCanDash()
        {
            if (MoveInput == 0)
            {
                return false;
            }
            return DashForce != 0 && DashDuration != 0;
        }
    }
}