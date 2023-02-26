using Platformer.CharacterSystem.Base;
using Platformer.Scriptable.Skills.Data;
using System;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public abstract class Attacker : MonoBehaviour
	{
		public event EventHandler<EntityEventTypes> EventInvoked;

		protected void InvokeAttackerEvent(EntityEventTypes e) =>
			EventInvoked?.Invoke(this, e);

		public abstract void OnMainAttackPressed();
		public virtual void OnSecondAttackPressed() { }
		public virtual void OnStrongAttackPressed() { }
		public virtual void OnAttackReleased() { }

		public virtual float GetAttackChargeTime() => 0f;
		public virtual void AddSkill(CombatSkillData toAdd) { }
		public virtual void RemoveSkill(CombatSkillData toRemove) { }

        public virtual void OnEventProcessed(EntityEventTypes e) { }
    }
}