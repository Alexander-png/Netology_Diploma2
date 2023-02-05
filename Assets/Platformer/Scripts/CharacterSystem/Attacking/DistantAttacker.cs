using Platformer.EditorExtentions;
using Platformer.Weapons;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class DistantAttacker : Attacker
	{
        [SerializeField, ReadOnly]
		protected DistantWeapon _currentWeapon;

        private void OnValidate() =>
            FindWeapon();

        private void Start() =>
            FindWeapon();

        private void FindWeapon()
        {
            if (_currentWeapon == null)
            {
                _currentWeapon = GetComponentInChildren<DistantWeapon>();
            }
        }

        public Vector3 GetProjectileSpawnPointPosition() =>
			_currentWeapon.ProjectileSpawnPosition;

		public float GetShootDistance() =>
			_currentWeapon.ShootDistance;

        public override void OnAttackPressed() =>
            _currentWeapon.Shoot();

        public override void OnStrongAttackInput() { }

        public override void OnAttackReleased() { }
    }
}