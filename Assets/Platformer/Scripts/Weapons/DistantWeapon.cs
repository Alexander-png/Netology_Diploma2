using Platformer.EditorExtentions;
using Platformer.GameCore.Helpers;
using Platformer.Projectiles;
using Platformer.Scriptable.EntityConfig;
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
		private ProjectileSpawnPoint _projectileSpawnPoint;

		private ProjectilePool _projectilePool;

		private Transform ProjectilePoolTransform => _projectilePool.transform;
		private Transform ProjectileSpawnPointTransform => _projectileSpawnPoint.transform;
		public Vector3 ProjectileSpawnPosition => ProjectileSpawnPointTransform.localPosition;

		public float ShootDistance => _stats.ShootDistance;

		private bool _reloading;

        protected override void Start()
        {
			base.Start();
			_projectilePool = FindObjectOfType<ProjectilePool>();
			if (_projectileSpawnPoint == null)
            {
				_projectileSpawnPoint = GetComponentInChildren<ProjectileSpawnPoint>();
            }
		}

		public void Shoot()
		{
			if (_reloading)
			{
				return;
			}

			var projectile = Instantiate(_projectilePrefab, ProjectileSpawnPointTransform.position, ProjectileSpawnPointTransform.rotation);
			projectile.transform.parent = ProjectilePoolTransform;
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
			_projectileSpawnPoint = GetComponentInChildren<ProjectileSpawnPoint>();
		}
#endif
    }
}