using Platformer.CharacterSystem.StatsData;
using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Stats skill container")]
    public class StatsSkillContainer : SkillContainer<Skill<CharacterStatsData>>
    {
        [SerializeField]
        private StatsSkillConfiguration[] _skills;

        private StatsSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override Skill<CharacterStatsData> CreateSkill(string skillId) =>
            new Skill<CharacterStatsData>(skillId, FindSkill(skillId).GetData());
    }
}