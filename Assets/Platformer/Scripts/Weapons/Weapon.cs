using Platformer.CharacterSystem.Base;
using UnityEngine;

namespace Platformer.Weapons
{
	public abstract class Weapon : MonoBehaviour
	{
		[SerializeField]
		protected Entity _owner;

		public void SetDamageToOwner(float damage)
		{
			(_owner as IDamagable).SetDamage(damage, Vector3.zero, true);
		}
	}
}