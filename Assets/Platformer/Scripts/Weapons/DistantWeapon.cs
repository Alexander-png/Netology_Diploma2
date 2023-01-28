using Platformer.Projectiles;
using Platformer.Scriptable.Characters;
using System.Collections;
using UnityEngine;

namespace Platformer.Weapons
{
	public class DistantWeapon : Weapon
	{
		[SerializeField]
		private ShootingStats _stats;
		[SerializeField]
		private Projectile _projectilePrefab;
		[SerializeField]
		private Transform _projectileSpawnPoint;
		[SerializeField]
		private Transform _projectilePool;

		public Vector3 ProjectileSpawnPosition => _projectileSpawnPoint.localPosition;
		public float ShootDistance => _stats.ShootDistance;

		private bool _reloading;

		public void Shoot()
		{
			if (_reloading)
			{
				return;
			}
			Rigidbody grenade = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation).GetComponent<Rigidbody>();
			grenade.transform.parent = _projectilePool;
			grenade.AddForce(grenade.transform.up * _stats.StartProjectileSpeed, ForceMode.Impulse);

			StartCoroutine(ReloadCoroutine());
		}

		private IEnumerator ReloadCoroutine()
		{
			_reloading = true;
			yield return new WaitForSeconds(_stats.ReloadTime);
			_reloading = false;
		}
	}
}