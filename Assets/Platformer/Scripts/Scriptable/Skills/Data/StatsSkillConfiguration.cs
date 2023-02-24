using UnityEngine;

namespace Platformer.Scriptable.Skills.Data
{
	//public string Name => _name;
	//public float MaxHealth => _maxHealth;
	//public float DamageImmuneTime => _damageImmuneTime;
	//public bool Immortal => _immortal;
	//public bool Interactable => _interactable;
	public struct CharacterSkillData
	{
		public float MaxHealth;
		public float DamageImmuneTime;

		public static CharacterSkillData operator +(CharacterSkillData first, CharacterSkillData second)
		{
			CharacterSkillData result = new CharacterSkillData();
			result.MaxHealth = first.MaxHealth + second.MaxHealth;
			result.DamageImmuneTime = first.DamageImmuneTime + second.DamageImmuneTime;
			return result;
		}

		public static CharacterSkillData operator -(CharacterSkillData first, CharacterSkillData second)
		{
			CharacterSkillData result = new CharacterSkillData();
			result.MaxHealth = first.MaxHealth - second.MaxHealth;
			result.DamageImmuneTime = first.DamageImmuneTime - second.DamageImmuneTime;
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
        };
    }
}