using Platformer.Weapons;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class DistantAttacker : Attacker
	{
		[SerializeField]
		protected DistantWeapon _currentWeapon;

		public Vector3 GetProjectileSpawnPointPosition() =>
			_currentWeapon.ProjectileSpawnPosition;

		public float GetShootDistance() =>
			_currentWeapon.ShootDistance;

        public override void StartAttack() =>
            _currentWeapon.Shoot();

        public override void EndAttack()
        {
            
        }
    }
}