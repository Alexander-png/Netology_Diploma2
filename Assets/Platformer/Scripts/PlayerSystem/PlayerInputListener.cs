using Platformer.CharacterSystem.Attacking;
using Platformer.CharacterSystem.Movement.Base;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer.PlayerSystem
{
	public class PlayerInputListener : MonoBehaviour
	{
        [SerializeField]
        private CharacterMovement _playerMovement;
        [SerializeField]
        private Attacker _attacker;
        [SerializeField]
        private Interactor _interactor;

        public Vector2 MousePositionOnScreen { get; private set; }

        private void OnRun(InputValue input) =>
            _playerMovement.SetHorizontalInput(input.Get<float>());

        private void OnDash(InputValue input) =>
            _playerMovement.SetDashInput(input.Get<float>());

        private void OnJump(InputValue input) =>
            _playerMovement.SetVerticalInput(input.Get<float>());

        private void OnAttack(InputValue input) =>
            _attacker.StartAttack();

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