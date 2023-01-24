using Platformer.PlayerSystem;
using Platformer.Scriptable.Characters;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Enemies
{
	public class WallShooter : StationaryEnemy
	{
		[SerializeField]
		private ShootingStats _shootStats;
		[SerializeField]
		private GameObject _grenadePrefab;
        [SerializeField]
        private Transform _projectileSpawnPoint;
        [SerializeField]
        private Transform _projectilePool;

        private bool _reloading;

        protected override void FixedUpdate() =>
            UpdateBehaviour();

        private void UpdateBehaviour()
        {
            // TODO: rework to attacker
            if (CheckPlayerNearby())
            {
                Shoot();
            }
            else
            {
            }
        }

        private bool CheckPlayerNearby()
        {
            Ray visual = GetViewRay();

            Physics.Raycast(visual, out RaycastHit hit, _shootStats.ShootDistance);
            return hit.transform?.TryGetComponent<Player>(out _) == true;
        }

        private void Shoot()
        {
            if (_reloading)
            {
                return;
            }
            Rigidbody grenade = Instantiate(_grenadePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation).GetComponent<Rigidbody>();
            grenade.transform.parent = _projectilePool;

            grenade.AddForce(grenade.transform.up * _shootStats.StartProjectileSpeed, ForceMode.Impulse);

            StartCoroutine(ReloadCoroutine());
        }

        private Ray GetCensorRay(Vector3 origin)
        {
            Vector3 startPoint = transform.TransformPoint(origin);
            Vector3 endPoint = transform.rotation * new Vector3(0, 1, 0);
            return new Ray(startPoint, endPoint);
        }

        private Ray GetViewRay() => GetCensorRay(_projectileSpawnPoint.localPosition);

        private IEnumerator ReloadCoroutine()
        {
            _reloading = true;
            yield return new WaitForSeconds(_shootStats.ReloadTime);
            _reloading = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            if (_shootStats != null && _projectileSpawnPoint != null)
            {
                Ray horz = GetViewRay();
                Gizmos.DrawRay(horz.origin, horz.direction * _shootStats.ShootDistance);
            }
            else
            {
                EditorExtentions.GameLogger.AddMessage("Please set shoot stats and projectile spawn point", EditorExtentions.GameLogger.LogType.Warning);
            }
        }
#endif
    }
}