using Platformer.EditorExtentions;
using Platformer.GameCore.Helpers;
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
        [SerializeField, ReadOnly]
		private Transform _projectileSpawnPoint;
		private Transform _projectilePool;
		public Vector3 ProjectileSpawnPosition => _projectileSpawnPoint.localPosition;
		public float ShootDistance => _stats.ShootDistance;

		private bool _reloading;

        protected override void Start()
        {
			base.Start();
			_projectilePool = FindObjectOfType<ProjectilePool>().transform;
			if (_projectileSpawnPoint == null)
            {
				_projectileSpawnPoint = GetComponentInChildren<ProjectileSpawnPoint>().transform;
            }
		}

		public void Shoot()
		{
			if (_reloading)
			{
				return;
			}

			var projectile = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
			projectile.transform.parent = _projectilePool;
			projectile.SetSpeed(_stats.StartProjectileSpeed);
			StartCoroutine(ReloadCoroutine());
		}

		private IEnumerator ReloadCoroutine()
		{
			_reloading = true;
			yield return new WaitForSeconds(_stats.ReloadTime);
			_reloading = false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
			_projectileSpawnPoint = GetComponentInChildren<ProjectileSpawnPoint>().transform;
		}
#endif
    }
}