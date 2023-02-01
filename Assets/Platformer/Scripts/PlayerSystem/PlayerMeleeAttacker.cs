using Platformer.CharacterSystem.Attacking;
using UnityEngine;

namespace Platformer.PlayerSystem
{
	public class PlayerMeleeAttacker : MeleeAttacker
	{
        [SerializeField]
        private float _attackRadius;

        [SerializeField]
        private PlayerInputListener _inputListener;

        private void Update() =>
            UpdateHitColliderPosition();

        private void UpdateHitColliderPosition()
        {
            Vector3 mousePosition = _inputListener.GetMousePositionInWorld();
            Vector3 relativePosition = mousePosition - transform.parent.position;
            Ray ray = new Ray(transform.parent.position, relativePosition);
            transform.position = ray.GetPoint(_attackRadius);
        }
    }
}