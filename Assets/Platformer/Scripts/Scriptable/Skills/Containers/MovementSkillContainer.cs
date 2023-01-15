using Platformer.Scriptable.Skills.Configurations;
using Platformer.SkillSystem.Skills;
using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Containers/Movement skill container")]
    public class MovementSkillContainer : SkillContainer<CharacterMovementSkill>
    {
        [SerializeField]
        private MovementSkillConfiguration[] _skills;

        private MovementSkillConfiguration FindSkill(string id) =>
            Array.Find(_skills, x => x.SkillId == id);

        public override CharacterMovementSkill CreateSkill(string skillId)
        {
            return new CharacterMovementSkill(skillId, FindSkill(skillId).GetSkills());
        }
    }
}