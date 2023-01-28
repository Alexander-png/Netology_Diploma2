using Platformer.Weapons;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class DistantAttacker : Attacker
	{
		[SerializeField]
		protected DistantWeapon _currentWeapon;

		public void Shoot() => _currentWeapon.Shoot();

		public Vector3 GetProjectileSpawnPointPosition() =>
			_currentWeapon.ProjectileSpawnPosition;

		public float GetShootDistance() =>
			_currentWeapon.ShootDistance;
	}
}