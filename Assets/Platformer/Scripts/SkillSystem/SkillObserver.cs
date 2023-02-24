using Platformer.CharacterSystem.Attacking;
using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Movement.Base;
using Platformer.EditorExtentions;
using Platformer.GameCore;
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
        private MovementSkillContainer _movementSkillContainer;
        [SerializeField]
        private StatsSkillContainer _statsSkillContainer;
        [SerializeField]
        private CombatSkillContainer _combatSkillContainer;

        [SerializeField, Space(15)]
        private bool _distinctSkillsOnly = true;

        private Entity _entity;
        private CharacterMovement _movementController;
        private Attacker _attacker;
        private List<GenericStats> _appliedSkills = new List<GenericStats>();

        private void Start()
        {
            _movementController = gameObject.GetComponent<CharacterMovement>();
            _entity = gameObject.GetComponent<Entity>();
            _attacker = gameObject.GetComponentInChildren<Attacker>();
            AddSkill(SaveSystem.GetRewardList());
        }

        private GenericStats FindAppliedSkill(string id) =>
            _appliedSkills.Find(s => s.SkillId == id);

        public void AddSkill(string skillId)
        {
            var skill = _movementSkillContainer.CreateSkill(skillId);
            if (_distinctSkillsOnly)
            {
                if (FindAppliedSkill(skill.SkillId) != null)
                {
                    return;
                }
            }
            AddSkillToEntity(skill);
            _appliedSkills.Add(skill);
        }

        public void AddSkill(string[] skillIds)
        {
            foreach (var id in skillIds)
            {
                AddSkill(id);
            }
        }

		public void RemoveSkill(string skillId)
        {
            GenericStats skillToRemove = FindAppliedSkill(skillId);
            if (skillToRemove != null)
            {
                RemoveSkillFromEntity(skillToRemove);
                _appliedSkills.Remove(skillToRemove);
            }
        }

        public bool CheckSkillAdded(string skillId) =>
            FindAppliedSkill(skillId) != null;

        private void AddSkillToEntity(GenericStats skill)
        {
            if (skill is Stats<CharacterSkillData> stats)
            {
                _entity.AddSkill(stats.Data);
            }
            else if (skill is Stats<MovementSkillData> moves)
            {
                _movementController.AddSkill(moves.Data);
            }
            else if (skill is Stats<CombatSkillData> combat)
            {
                _attacker.AddSkill(combat.Data);
            }
            else
            {
                GameLogger.AddMessage($"Unknown skill type: {skill.GetType()}", GameLogger.LogType.Error);
            }
        }

        private void RemoveSkillFromEntity(GenericStats skill)
        {
            if (skill is Stats<CharacterSkillData> stats)
            {
                _entity.RemoveSkill(stats.Data);
            }
            else if (skill is Stats<MovementSkillData> moves)
            {
                _movementController.RemoveSkill(moves.Data);
            }
            else if (skill is Stats<CombatSkillData> combat)
            {
                _attacker.RemoveSkill(combat.Data);
            }
            else
            {
                GameLogger.AddMessage($"Unknown skill type: {skill.GetType()}", GameLogger.LogType.Error);
            }
        }
    }
}