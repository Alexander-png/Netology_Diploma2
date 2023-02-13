using UnityEngine;

namespace Platformer.Scriptable.WeaponStats
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/WeaponStats")]
	public class WeaponStats : ScriptableObject
	{
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _attackRadius;
		[SerializeField]
		private float _reloadTime;
		[SerializeField]
		private float _attackDuration;
		[SerializeField]
		private bool _hasStrongAttack;
		[SerializeField]
		private float _strongAttackChargeTime;
		[SerializeField]
		private float _strongAttackDamageMultipler;
		[SerializeField]
		private float _strongAttackRadiusMultipler;
		[SerializeField]
		private float _pushForce;
		[SerializeField]
		private bool _isKamikazeAttack;

		public float Damage => _damage;
		public float AttackRadius => _attackRadius;
		public float ReloadTime => _reloadTime;
		public float AttackDuration => _attackDuration;
		public bool HasStrongAttack => _hasStrongAttack;
		public float StrongAttackChargeTime => _strongAttackChargeTime;
		public float StrongAttackDamageMultipler => _strongAttackDamageMultipler;
		public float StrongAttackRadiusMultipler => _strongAttackRadiusMultipler;
		public float PushForce => _pushForce;
		public bool IsKamikazeAttack => _isKamikazeAttack;
	}
}