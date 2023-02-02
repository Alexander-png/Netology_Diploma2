using Platformer.CharacterSystem.Movement;
using UnityEngine;

namespace Platformer.PlayerSystem
{
	public class PlayerGroundMovement : GroundCharacterMovement
	{
		[SerializeField]
		private PlayerInputListener _inputListener;

        protected override Vector2 CalclulateDashDirection()
        {
            Vector3 mousePos = _inputListener.GetMousePositionInWorld();
            Vector3 relativeMousePos = mousePos - transform.position;
            return relativeMousePos.normalized;
        }

        protected override void OnDashStarted() =>
            Physics.IgnoreLayerCollision(6, 7, true);

        protected override void OnDashEnded() =>
            Physics.IgnoreLayerCollision(6, 7, false);
    }
}