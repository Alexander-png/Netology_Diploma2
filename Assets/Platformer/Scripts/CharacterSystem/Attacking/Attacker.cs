using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public abstract class Attacker : MonoBehaviour
	{
		public abstract void OnMainAttackPressed();
		public virtual void OnSecondAttackPressed() { }
		public virtual void OnStrongAttackPressed() { }
		public virtual void OnAttackReleased() { }

		public virtual float GetAttackChargeTime() => 0f;
	}
}