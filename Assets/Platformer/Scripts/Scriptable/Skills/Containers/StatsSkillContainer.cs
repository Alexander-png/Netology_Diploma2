using Platformer.Scriptable.EntityConfig;
using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Stats skill container")]
    public class StatsSkillContainer : SkillContainer<Stats<CharacterSkillData>>
    {
        [SerializeField]
        private StatsSkillConfiguration[] _skills;

        private StatsSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override Stats<CharacterSkillData> CreateSkill(string skillId) =>
            new Stats<CharacterSkillData>(skillId, FindSkill(skillId).GetData());
    }
}