using Platformer.CharacterSystem.Attacking;
using Platformer.Weapons;
using UnityEngine;

namespace Platformer.PlayerSystem
{
	public class PlayerMeleeAttacker : MeleeAttacker
	{
        private PlayerInputListener _inputListener;

        protected override void Start()
        {
            base.Start();
            _inputListener = transform.parent.GetComponentInChildren<PlayerInputListener>();
            CurrentWeapon = transform.parent.GetComponentInChildren<MeleeWeapon>();
        }

        private void Update() =>
            UpdateHitColliderPosition();

        private void UpdateHitColliderPosition()
        {
            Vector3 mousePosition = _inputListener.GetMousePositionInWorld();
            Vector3 relativePosition = mousePosition - transform.parent.position;
            Ray ray = new Ray(transform.parent.position, relativePosition);
            transform.position = ray.GetPoint(CurrentWeapon.Stats.AttackRadius);
        }
    }
}