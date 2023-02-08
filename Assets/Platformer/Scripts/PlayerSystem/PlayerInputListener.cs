using Platformer.CharacterSystem.Attacking;
using Platformer.CharacterSystem.Movement.Base;
using Platformer.EditorExtentions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer.PlayerSystem
{
	public class PlayerInputListener : MonoBehaviour
	{
        private CharacterMovement _playerMovement;        
        private MeleeAttacker _attacker;
        private Interactor _interactor;
        private Vector2 _mousePositionOnScreen;

        private void Start()
        {
            _playerMovement = gameObject.GetComponentInChildren<CharacterMovement>();
            _attacker = gameObject.GetComponentInChildren<MeleeAttacker>();
            _interactor = gameObject.GetComponent<Interactor>();
        }

        private void OnRun(InputValue input) =>
            _playerMovement.SetHorizontalInput(input.Get<float>());

        private void OnDash(InputValue input) =>
            _playerMovement.SetDashInput(input.Get<float>());

        private void OnJump(InputValue input) =>
            _playerMovement.SetVerticalInput(input.Get<float>());

        private void OnAttack(InputValue input)
        {
            float value = input.Get<float>();
            if (value > 0)
            {
                _attacker.OnMainAttackPressed();
            }
            else
            {
                _attacker.OnAttackReleased();
            }
        }

        private void OnStrongAttack(InputValue input) =>
            _attacker.OnStrongAttackPressed();
            //GameLogger.AddMessage("Strong attack");

        private void OnInteract(InputValue input) => 
            _interactor.OnInteractInput();

        private void OnMousePosition(InputValue input)
        {
            Vector2 newMousePos = input.Get<Vector2>();
            if (_mousePositionOnScreen != newMousePos)
            {
                _mousePositionOnScreen = newMousePos;
            }
        }

        public Vector3 GetMousePositionInWorld()
        {
            // tooked form here: https://forum.unity.com/threads/mouse-to-world-position-using-perspective-camera-when-there-is-nothing-to-hit.1199350/
            Plane plane = new Plane(Vector3.back, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(_mousePositionOnScreen);
            if (plane.Raycast(ray, out float enter))
            {
                return ray.GetPoint(enter);
            }
            return Vector3.zero;
        }
    }
}