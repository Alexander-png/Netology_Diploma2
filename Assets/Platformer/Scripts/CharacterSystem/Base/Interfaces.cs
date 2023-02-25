using Platformer.SkillSystem;
using System;
using UnityEngine;

namespace Platformer.CharacterSystem.Base
{
	public interface IPlayerInteractable
	{
	    
	}

	public interface IDamagable
    {
		public float MaxHealth { get; }
        public float CurrentHealth { get; }
		public bool CanBeDamaged { get; }
		public void SetDamage(float damage, Vector3 pushVector, bool forced = false);
		public void Heal(float value);

		public event EventHandler Died;
    }

	public interface ISkillObservable
    {
		public SkillObserver SkillObserver { get; }
    }
}