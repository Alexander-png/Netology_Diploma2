using Platformer.CharacterSystem.Movement.Base;
using UnityEngine;

namespace Platformer.CharacterSystem.Input
{
	public class PlayerInputHandler : MonoBehaviour
	{
		[SerializeField]
		private CharacterMovement _movementController;

		protected CharacterMovement MovementController => _movementController;

		[ContextMenu("Find Movement controller")]
		private void FindController()
		{
			_movementController = FindObjectOfType<CharacterMovement>();
		}
	}
}