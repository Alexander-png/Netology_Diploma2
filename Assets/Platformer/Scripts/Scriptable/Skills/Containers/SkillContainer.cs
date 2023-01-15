using Platformer.SkillSystem.Skills;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
	public abstract class SkillContainer<T> : ScriptableObject where T : Skill
    {
        public abstract T CreateSkill(string skillId);
    }
}