using Platformer.Scriptable.EntityConfig;
using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Movement skill container")]
    public class MovementSkillContainer : SkillContainer<Stats<MovementSkillData>>
    {
        [SerializeField]
        private MovementSkillConfiguration[] _skills;

        private MovementSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override Stats<MovementSkillData> CreateSkill(string skillId) =>
            new Stats<MovementSkillData>(skillId, FindSkill(skillId).GetData());
    }
}