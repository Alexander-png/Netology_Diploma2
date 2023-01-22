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
            _playerMovement.OnRun(input);

        private void OnDash(InputValue input) =>
            _playerMovement.OnDash(input);

        private void OnJump(InputValue input) =>
            _playerMovement.OnJump(input);

        private void OnAttack(InputValue input) =>
            _attacker.OnAttack(input);

        private void OnInteract(InputValue input) => 
            _interactor.OnInteract(input);

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