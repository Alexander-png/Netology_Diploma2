using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Combat skill container")]
    public class CombatSkillContainer : SkillContainer<Stats<CombatSkillData>>
    {
        [SerializeField]
        private CombatSkillConfiguration[] _skills;

        private CombatSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override Stats<CombatSkillData> CreateSkill(string skillId) =>
            new Stats<CombatSkillData>(skillId, FindSkill(skillId).GetData());
    }
}