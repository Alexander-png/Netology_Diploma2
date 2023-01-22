using Platformer.CharacterSystem.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer.PlayerSystem
{
	public class PlayerInputListener : MonoBehaviour
	{
        [SerializeField]
        private PlayerMovement _playerMovement;
        [SerializeField]
        private Attacker _attacker;
        [SerializeField]
        private Interactor _interactor;

        public Vector2 MousePositionOnScreen { get; private set; }

        private void OnRun(InputValue input) =>
            _playerMovement.OnRunInput(input.Get<float>());

        private void OnDash(InputValue input) =>
            _playerMovement.OnDashInput(input.Get<float>());

        private void OnJump(InputValue input) =>
            _playerMovement.OnJumpInput(input.Get<float>());

        private void OnAttack(InputValue input) =>
            _attacker.OnAttackInput();

        private void OnInteract(InputValue input) => 
            _interactor.OnInteractInput();

        private void OnMousePosition(InputValue input)
        {
            Vector2 newMousePos = input.Get<Vector2>();
            if (MousePositionOnScreen != newMousePos)
            {
                MousePositionOnScreen = newMousePos;
            }
        }
    }
}