using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Combat skill container")]
    public class CombatSkillContainer : SkillContainer
    {
        [SerializeField]
        private CombatSkillConfiguration[] _skills;

        private CombatSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override GenericSkill CreateSkill(string skillId) =>
            new Skill<CombatSkillData>(skillId, FindSkill(skillId).GetData());
    }
}