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
    /// Skill, that applied to character directly
    /// </summary>
	public class Skill<T> : GenericSkill
    {
        protected T _data;
        public T Data => _data;

        public Skill(string id, T data) : base(id) =>
            _data = data;
    }
}