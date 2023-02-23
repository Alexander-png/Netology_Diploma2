using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
	public abstract class SkillContainer<T> : ScriptableObject
    {
        public abstract T CreateSkill(string skillId);
    }
}