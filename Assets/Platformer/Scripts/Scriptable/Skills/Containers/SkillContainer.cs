using Platformer.SkillSystem.Skills;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Containers
{
    public abstract class SkillContainer : ScriptableObject 
    {
        public abstract GenericSkill CreateSkill(string skillId);

        public bool TryCreateSkill(string skillId, out GenericSkill skill)
        {
            skill = default;
            try
            {
                skill = CreateSkill(skillId);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}