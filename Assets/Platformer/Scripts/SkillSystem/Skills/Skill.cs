namespace Platformer.SkillSystem.Skills
{
    public abstract class GenericSkill 
    {
        private string _skillId;
        public string SkillId => _skillId;
        
        public GenericSkill(string id) =>
            _skillId = id;
    }

    /// <summary>
    /// Skill modificator, that applied to character directly
    /// </summary>
	public class Skill<T> : GenericSkill
    {
        protected T _skills;
        public T SkillData => _skills;

        public Skill(string id, T data) : base(id) =>
            _skills = data;
    }
}