using Platformer.CharacterSystem.StatsData;
using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Movement skill container")]
    public class MovementSkillContainer : SkillContainer<Skill<MovementStatsData>>
    {
        [SerializeField]
        private MovementSkillConfiguration[] _skills;

        private MovementSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override Skill<MovementStatsData> CreateSkill(string skillId) =>
            new Skill<MovementStatsData>(skillId, FindSkill(skillId).GetData());
    }
}