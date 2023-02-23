using Platformer.CharacterSystem.Movement.Base;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Configurations
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Character skill")]
	public class StatsSkillConfiguration : SkillConfiguration<CharacterStatsData>
	{
		[SerializeField]
		private float _maxHealth;
		[SerializeField]
		private float _damageImmuneTime;

        public override CharacterStatsData GetData() => new CharacterStatsData()
        {
            MaxHealth = _maxHealth,
            DamageImmuneTime = _damageImmuneTime,
        };
    }
}