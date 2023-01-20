using UnityEngine;

namespace Platformer.Scriptable.Projectiles
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Projectile Stats")]
	public class ExplosiveProjectileStats : ScriptableObject
	{
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _impactForce;
		[SerializeField]
		private float _blastRange;
		[SerializeField]
		private float _blastTimeout;
		[SerializeField]
		private float _triggerDistance;

		public float Damage => _damage;
		public float ImpactForce => _impactForce;
		public float BlastRange => _blastRange;
		public float BlastTimeout => _blastTimeout;
		public float TriggerDistance => _triggerDistance;
	}
}