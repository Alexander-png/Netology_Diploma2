using Platformer.CharacterSystem.StatsData;
using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Combat skill container")]
    public class CombatSkillContainer : SkillContainer<Skill<CombatStatsData>>
    {
        [SerializeField]
        private CombatSkillConfiguration[] _skills;

        private CombatSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override Skill<CombatStatsData> CreateSkill(string skillId) =>
            new Skill<CombatStatsData>(skillId, FindSkill(skillId).GetData());
    }
}