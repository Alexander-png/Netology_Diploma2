using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Movement skill container")]
    public class MovementSkillContainer : SkillContainer
    {
        [SerializeField]
        private MovementSkillConfiguration[] _skills;

        private MovementSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override GenericSkill CreateSkill(string skillId) =>
            new Skill<MovementSkillData>(skillId, FindSkill(skillId).GetData());
    }
}