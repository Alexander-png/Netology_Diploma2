using Platformer.PlayerSystem;
using Platformer.Scriptable.Projectiles;
using System.Collections;
using UnityEngine;

namespace Platformer.Projectiles
{
	public class NonPhysicProjectile : Projectile
	{
        [SerializeField]
        private ProjectileStats _stats;
        [SerializeField]
        private SphereCollider _trigger;

        private float _currentSpeed;

        private void Start()
        {
            _trigger.radius = _stats.TriggerDistance;
            StartCoroutine(ExplosionCoroutine());
        }

        private void Update() =>
            UpdatePosition();

        private void UpdatePosition()
        {
            Vector3 position = transform.position;
            position += transform.forward * _currentSpeed * Time.deltaTime;
            transform.position = position;
        }

        public override void SetSpeed(float value) =>
            _currentSpeed = value;

        private void Explose() =>
            Destroy(gameObject);

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                StopAllCoroutines();
                player.SetDamage(_stats.Damage, (player.transform.position - transform.position) * _stats.ImpactForce);
            }
            Explose();
        }

        private IEnumerator ExplosionCoroutine()
        {
            yield return new WaitForSeconds(_stats.BlastTimeout);
            Explose();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_stats != null)
            {
                Color c = Color.red;
                c.a = 0.1f;
                Gizmos.color = c;

                Gizmos.DrawSphere(transform.position, _stats.BlastRange);

                c = Color.cyan;
                c.a = 0.1f;
                Gizmos.color = c;
                Gizmos.DrawSphere(transform.position, _stats.TriggerDistance);
            }
            else
            {
                EditorExtentions.GameLogger.AddMessage("No stats setted", EditorExtentions.GameLogger.LogType.Warning);
            }
        }
#endif
    }
}