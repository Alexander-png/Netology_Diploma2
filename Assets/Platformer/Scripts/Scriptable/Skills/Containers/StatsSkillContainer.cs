using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Stats skill container")]
    public class StatsSkillContainer : SkillContainer
    {
        [SerializeField]
        private StatsSkillConfiguration[] _skills;

        private StatsSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override GenericSkill CreateSkill(string skillId) =>
            new Skill<CharacterSkillData>(skillId, FindSkill(skillId).GetData());
    }
}