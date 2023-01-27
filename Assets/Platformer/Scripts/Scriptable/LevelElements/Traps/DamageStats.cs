using UnityEngine;

namespace Platformer.Scriptable.LevelElements.Traps
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Damage stats")]
	public class DamageStats : ScriptableObject
	{
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _pushForce;
		[SerializeField]
		private bool _isFatalDamage;
		[SerializeField]
		private bool _isKamikazeAttack;
		[SerializeField]
		private float _attackChargeTime;

		public float Damage => IsFatalDamage ? float.MaxValue : _damage;
		public float PushForce => _pushForce;
		public bool IsFatalDamage => _isFatalDamage;
		public bool IsKamikazeAttack => _isKamikazeAttack;
		public float AttackChargeTime => _attackChargeTime;
	}
}