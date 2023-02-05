using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public abstract class Attacker : MonoBehaviour
	{
		public abstract void OnAttackPressed();
		public abstract void OnStrongAttackInput();
		public abstract void OnAttackReleased();

		public virtual float GetAttackChargeTime() => 0f;
	}
}