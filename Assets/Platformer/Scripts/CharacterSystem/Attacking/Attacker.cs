using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public abstract class Attacker : MonoBehaviour
	{
		public abstract void StartAttack();
		public abstract void EndAttack();

		public virtual float GetAttackChargeTime() => 0f;
	}
}