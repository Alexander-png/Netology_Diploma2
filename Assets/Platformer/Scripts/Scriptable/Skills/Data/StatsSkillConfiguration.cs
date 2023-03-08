using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Data
{
	public struct CharacterSkillData
	{
		public float MaxHealth;
		public float DamageImmuneTime;

		public bool IsProportion;

		public static CharacterSkillData operator +(CharacterSkillData first, CharacterSkillData second)
		{
			CharacterSkillData result = new CharacterSkillData();
			if (first.IsProportion == second.IsProportion)
			{
				result.MaxHealth = first.MaxHealth + second.MaxHealth;
				result.DamageImmuneTime = first.DamageImmuneTime + second.DamageImmuneTime;
			}
			else if (!first.IsProportion && second.IsProportion)
			{
				result.MaxHealth = first.MaxHealth + (first.MaxHealth * second.MaxHealth);
				result.DamageImmuneTime = first.DamageImmuneTime + (first.DamageImmuneTime * second.DamageImmuneTime);
			}
			else
			{
				throw new InvalidOperationException("Addition of absolute values to propotion is not supported.");
			}
			return result;
		}

		public static CharacterSkillData operator -(CharacterSkillData first, CharacterSkillData second)
		{
			CharacterSkillData result = new CharacterSkillData();
			if (first.IsProportion == second.IsProportion)
			{
				result.MaxHealth = first.MaxHealth - second.MaxHealth;
				result.DamageImmuneTime = first.DamageImmuneTime - second.DamageImmuneTime;
			}
			else if (!first.IsProportion && second.IsProportion)
			{
				result.MaxHealth = first.MaxHealth - (first.MaxHealth * second.MaxHealth);
				result.DamageImmuneTime = first.DamageImmuneTime - (first.DamageImmuneTime * second.DamageImmuneTime);
			}
			else
			{
				throw new InvalidOperationException("Substraction of absolute values to propotion is not supported.");
			}
			return result;
		}
	}

	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Character skill")]
	public class StatsSkillConfiguration : SkillConfiguration<CharacterSkillData>
	{
		[SerializeField]
		private float _maxHealth;
		[SerializeField]
		private float _damageImmuneTime;

        public override CharacterSkillData GetData() => new CharacterSkillData()
        {
            MaxHealth = _maxHealth,
            DamageImmuneTime = _damageImmuneTime,
			IsProportion = IsProprotion,
		};
    }
}