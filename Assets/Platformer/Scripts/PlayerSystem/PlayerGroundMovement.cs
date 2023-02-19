using Platformer.CharacterSystem.Movement;
using UnityEngine;

namespace Platformer.PlayerSystem
{
	public class PlayerGroundMovement : GroundCharacterMovement
	{
		private PlayerInputListener _inputListener;

        protected override void Start()
        {
            base.Start();
            _inputListener = gameObject.GetComponent<PlayerInputListener>();
        }

        protected override Vector2 CalclulateDashDirection()
        {
            var vector = _inputListener.GetRelativeMousePosition(transform.position).normalized;
            vector.y /= MovementStats.VerticalDashDelimeter;
            return vector;
        }

        protected override void OnDashStarted()
        {
            base.OnDashStarted();
            Physics.IgnoreLayerCollision(6, 7, true);
        }

        protected override void OnDashEnded()
        {
            base.OnDashEnded();
            Physics.IgnoreLayerCollision(6, 7, false);
        }
    }
}