using UnityEngine;

namespace Platformer.Scriptable.LevelElements.Traps
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Damage stats")]
	public class TrapDamageStats : ScriptableObject
	{
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _pushForce;
		[SerializeField]
		private bool _isFatalDamage;

		public float Damage => IsFatalDamage ? float.MaxValue : _damage;
		public float PushForce => _pushForce;
		public bool IsFatalDamage => _isFatalDamage;
	}
}