using Platformer.EditorExtentions;
using Platformer.GameCore.Helpers;
using Platformer.Projectiles;
using Platformer.Scriptable.Characters;
using System.Collections;
using UnityEngine;

namespace Platformer.Weapons
{
	public class FirstBossWeapon : MeleeWeapon
	{
        [SerializeField]
        private ShootingStats _shootingStats;
        [SerializeField, ReadOnly]
        private ProjectileSpawnPoint[] _projectileSpawnPoints;
        [SerializeField]
        private Projectile _projectilePrefab;

        private ProjectilePool _projectilePool;
        private bool _reloadingShoot;
        private Transform ProjectilePoolTransform => _projectilePool.transform;

        private Transform GetProjectileSpawnPointTransform(int index) => 
            _projectileSpawnPoints[index].transform;

        protected override void Start()
        {
            base.Start();

            _projectilePool = FindObjectOfType<ProjectilePool>();
            if (_projectileSpawnPoints == null)
            {
                _projectileSpawnPoints = GetComponentsInChildren<ProjectileSpawnPoint>();
            }
        }

        public override void MakeAlternativeHit()
        {
            if (_reloadingShoot)
            {
                return;
            }

            for (int i = 0; i < _projectileSpawnPoints.Length; i++)
            {
                Transform tr = GetProjectileSpawnPointTransform(i);
                var projectile = Instantiate(_projectilePrefab, tr.position, tr.rotation);
                projectile.transform.parent = ProjectilePoolTransform;
                projectile.SetSpeed(_shootingStats.StartProjectileSpeed);
            }
            StartCoroutine(ReloadShooterCoroutine());
        }

        private IEnumerator ReloadShooterCoroutine()
        {
            _reloadingShoot = true;
            yield return new WaitForSeconds(_shootingStats.ReloadTime);
            _reloadingShoot = false;
        }

#if UNITY_EDITOR
        private Ray GetDirectionRay(Transform origin)
        {
            Vector3 startPoint = origin.position;
            Vector3 endPoint = origin.rotation * new Vector3(0, 1, 0);
            return new Ray(startPoint, endPoint);
        }

        private void OnValidate()
        {
            _projectileSpawnPoints = GetComponentsInChildren<ProjectileSpawnPoint>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            for (int i = 0; i < _projectileSpawnPoints.Length; i++)
            {
                Ray horz = GetDirectionRay(GetProjectileSpawnPointTransform(i));
                Gizmos.DrawRay(horz.origin, horz.direction * _shootingStats.StartProjectileSpeed);
            }
        }

        [ContextMenu("Refresh")]
        private void FindMenuItems()
        {
            OnValidate();
        }
#endif
    }
}