using Platformer.CharacterSystem.StatsData;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Data
{
	[CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Combat skill")]
	public class CombatSkillConfiguration : SkillConfiguration<CombatStatsData>
	{
		[SerializeField]
		private float _maxHealth;
		[SerializeField]
		private float _damageImmuneTime;

        public override CombatStatsData GetData() => new CombatStatsData()
        {
            //MaxHealth = _maxHealth,
            //DamageImmuneTime = _damageImmuneTime,
        };
    }
}