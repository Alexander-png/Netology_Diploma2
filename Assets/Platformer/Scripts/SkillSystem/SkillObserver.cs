using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Movement.Base;
using Platformer.GameCore;
using Platformer.SkillSystem.Skills;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Platformer.SkillSystem
{
	public class SkillObserver : MonoBehaviour
	{
        [Inject]
        private GameSystem _gameSystem;

        [SerializeField]
        private bool _distinctSkillsOnly = true;

        private CharacterMovement _movementController;        
        private Entity _character;
        private List<Skill> _appliedSkills = new List<Skill>();

        private void Start()
        {
            _movementController = gameObject.GetComponent<CharacterMovement>();
            _character = gameObject.GetComponent<Entity>();
        }

        private Skill FindSkill(string id) => _appliedSkills.Find(s => s.SkillId == id);

        public void AddSkill(string skillId)
        {
            var skill = _gameSystem.PlayerMovementSkillContainer.CreateSkill(skillId);
            if (_distinctSkillsOnly)
            {
                if (FindSkill(skill.SkillId) != null)
                {
                    return;
                }
            }
            AddSkillToEntity(skill);
            _appliedSkills.Add(skill);
        }

		public void RemoveSkill(Skill skill)
        {
            Skill skillToRemove = FindSkill(skill.SkillId);
            if (skillToRemove != null)
            {
                RemoveSkillFromEntity(skillToRemove);
                _appliedSkills.Remove(skillToRemove);
            }
        }

        public bool CheckSkillAdded(string skillId) =>
            FindSkill(skillId) != null;

        private void AddSkillToEntity(Skill skill)
        {
            if (skill is CharacterStatsSkill stats)
            {
                throw new System.NotImplementedException();
            }
            else if (skill is CharacterMovementSkill moves)
            {
                _movementController.AddStats(moves.GetData());
            }
        }

        private void RemoveSkillFromEntity(Skill skill)
        {
            if (skill is CharacterStatsSkill stats)
            {
                throw new System.NotImplementedException();
            }
            else if (skill is CharacterMovementSkill moves)
            {
                _movementController.RemoveStats(moves.GetData());
            }
        }
    }
}