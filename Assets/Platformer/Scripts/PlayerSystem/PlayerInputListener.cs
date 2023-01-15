using Platformer.CharacterSystem.Interactors;
using Platformer.CharacterSystem.Movement;
using Platformer.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer.PlayerSystem
{
	public class PlayerInputListener : MonoBehaviour
	{
        [SerializeField]
        private PlayerMovement _playerMovement;
        [SerializeField]
        private PlayerWeapon _playerWeapon;
        [SerializeField]
        private Interactor _interactor;

        private void OnRun(InputValue input) =>
            _playerMovement.OnRunPerformed(input);

        private void OnDash(InputValue input) =>
            _playerMovement.OnDashPerformed(input);

        private void OnJump(InputValue input) =>
            _playerMovement.OnJumpPerformed(input);

        private void OnAttack(InputValue input) =>
            _playerWeapon.OnAttackPerformed(input);

        private void OnInteract(InputValue input) => 
            _interactor.OnInteractPerformed(input);
    }
}