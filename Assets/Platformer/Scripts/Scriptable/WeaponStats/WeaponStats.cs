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
		private bool _hasStrengthAttack;
		[SerializeField]
		private float _strengthAttackChargeTime;
		[SerializeField]
		private float _strengthAttackDamageMultipler;
		[SerializeField]
		private float _pushForce;
		[SerializeField]
		private bool _isKamikazeAttack;

		public float Damage => _damage;
		public float AttackRadius => _attackRadius;
		public float ReloadTime => _reloadTime;
		public bool HasStrengthAttack => _hasStrengthAttack;
		public float StrengthAttackChargeTime => _strengthAttackChargeTime;
		public float StrengthAttackDamageMultipler => _strengthAttackDamageMultipler;
		public float PushForce => _pushForce;
		public bool IsKamikazeAttack => _isKamikazeAttack;
	}
}