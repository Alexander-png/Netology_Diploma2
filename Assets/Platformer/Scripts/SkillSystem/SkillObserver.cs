using Platformer.CharacterSystem.Attacking;
using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Movement.Base;
using Platformer.EditorExtentions;
using Platformer.Scriptable.Skills.Containers;
using Platformer.Scriptable.Skills.Data;
using Platformer.SkillSystem.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.SkillSystem
{
	public class SkillObserver : MonoBehaviour
	{
        [SerializeField]
        private SkillContainer[] _skillContainers;

        [SerializeField]
        private bool _distinctSkillsOnly = true;

        private Entity _entity;
        private CharacterMovement _movementController;
        private Attacker _attacker;
        private List<GenericSkill> _appliedSkills = new List<GenericSkill>();

        private void Start()
        {
            _movementController = gameObject.GetComponent<CharacterMovement>();
            _entity = gameObject.GetComponent<Entity>();
            _attacker = gameObject.GetComponentInChildren<Attacker>();
        }

        private GenericSkill FindAppliedSkill(string id) =>
            _appliedSkills.Find(s => s.SkillId == id);

        public void AddSkill(string skillId, bool distinctForNow = false)
        {
            if (_distinctSkillsOnly || distinctForNow)
            {
                if (FindAppliedSkill(skillId) != null)
                {
                    return;
                }
            }
            GenericSkill skill = null;
            foreach (var container in _skillContainers)
            {
                container.TryCreateSkill(skillId, out skill);
                if (skill != null)
                {
                    AddSkillToEntity(skill);
                    break;
                }
            }
            if (skill == null)
            {
                GameLogger.AddMessage($"Skill with id {skill} not found in containers.", GameLogger.LogType.Warning);
            }
        }

        public void AddSkill(string[] skillIds, bool distinctForNow = false)
        {
            foreach (var id in skillIds)
            {
                AddSkill(id, distinctForNow);
            }
        }

		public void RemoveSkill(string skillId)
        {
            GenericSkill skillToRemove = FindAppliedSkill(skillId);
            if (skillToRemove != null)
            {
                RemoveSkillFromEntity(skillToRemove);
            }
        }

        public bool CheckSkillAdded(string skillId) =>
            FindAppliedSkill(skillId) != null;

        private void AddSkillToEntity(GenericSkill skill)
        {
            if (skill is Skill<CharacterSkillData> stats)
            {
                _entity.AddSkill(stats.Data);
            }
            else if (skill is Skill<MovementSkillData> moves)
            {
                _movementController.AddSkill(moves.Data);
            }
            else if (skill is Skill<CombatSkillData> combat)
            {
                _attacker.AddSkill(combat.Data);
            }
            else
            {
                GameLogger.AddMessage($"Unknown skill type: {skill.GetType()}", GameLogger.LogType.Error);
            }
            _appliedSkills.Add(skill);
        }

        private void RemoveSkillFromEntity(GenericSkill skill)
        {
            if (skill is Skill<CharacterSkillData> stats)
            {
                _entity.RemoveSkill(stats.Data);
            }
            else if (skill is Skill<MovementSkillData> moves)
            {
                _movementController.RemoveSkill(moves.Data);
            }
            else if (skill is Skill<CombatSkillData> combat)
            {
                _attacker.RemoveSkill(combat.Data);
            }
            else
            {
                GameLogger.AddMessage($"Unknown skill type: {skill.GetType()}", GameLogger.LogType.Error);
            }
            _appliedSkills.Remove(skill);
        }
    }
}