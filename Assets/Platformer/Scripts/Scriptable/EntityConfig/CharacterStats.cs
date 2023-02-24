using Platformer.Scriptable.Skills.Data;
using System;
using UnityEngine;

namespace Platformer.Scriptable.EntityConfig
{
	// TODO: replace default stats with default skills.
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Character stats")]
    [Obsolete("Must remove this class. Use Character skills instead")]
	public class CharacterStats : ScriptableObject
	{
		[SerializeField]
		private float _maxHealth;
		[SerializeField]
		private float _damageImmuneTime;

		public CharacterSkillData GetStats() => new CharacterSkillData()
		{
			MaxHealth = _maxHealth,
			DamageImmuneTime = _damageImmuneTime,
		};
    }
}