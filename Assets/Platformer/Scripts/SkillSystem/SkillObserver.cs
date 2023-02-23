using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Movement.Base;
using Platformer.GameCore;
using Platformer.Scriptable.Skills.Containers;
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

        [SerializeField, Space(15)]
        private bool _distinctSkillsOnly = true;

        private CharacterMovement _movementController;        
        private Entity _character;
        private List<GenericSkill> _appliedSkills = new List<GenericSkill>();

        private void Start()
        {
            _movementController = gameObject.GetComponent<CharacterMovement>();
            _character = gameObject.GetComponent<Entity>();
            AddSkill(SaveSystem.GetRewardList());
        }

        private GenericSkill FindAppliedSkill(string id) =>
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
            GenericSkill skillToRemove = FindAppliedSkill(skillId);
            if (skillToRemove != null)
            {
                RemoveSkillFromEntity(skillToRemove);
                _appliedSkills.Remove(skillToRemove);
            }
        }

        public bool CheckSkillAdded(string skillId) =>
            FindAppliedSkill(skillId) != null;

        private void AddSkillToEntity(GenericSkill skill)
        {
            if (skill is Skill<CharacterStatsData> stats)
            {
                throw new System.NotImplementedException();
            }
            else if (skill is Skill<MovementStatsData> moves)
            {
                _movementController.AddStats(moves.SkillData);
            }
        }

        private void RemoveSkillFromEntity(GenericSkill skill)
        {
            if (skill is Skill<CharacterStatsData> stats)
            {
                throw new System.NotImplementedException();
            }
            else if (skill is Skill<MovementStatsData> moves)
            {
                _movementController.RemoveStats(moves.SkillData);
            }
        }
    }
}