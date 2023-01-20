using Platformer.PlayerSystem;
using Platformer.Scriptable.Projectiles;
using System.Collections;
using UnityEngine;

namespace Platformer.Projectiles
{
	public class Grenade : MonoBehaviour
	{
		[SerializeField]
		private ExplosiveProjectileStats _stats;
        [SerializeField]
        private SphereCollider _trigger;

		private void Start()
        {
            _trigger.radius = _stats.TriggerDistance;
            StartCoroutine(ExplosionCoroutine());
        }

        private void Explosion()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                StopAllCoroutines();
                player.SetDamage(_stats.Damage, (player.transform.position - transform.position) * _stats.ImpactForce);
                Explosion();
            }
        }

        private IEnumerator ExplosionCoroutine()
        {
            yield return new WaitForSeconds(_stats.BlastTimeout);
            Explosion();
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
                EditorExtentions.GameLogger.AddMessage("Please set _stats", EditorExtentions.GameLogger.LogType.Warning);
            }   
        }
#endif
    }
}