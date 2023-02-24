namespace Platformer.SkillSystem.Skills
{
    public abstract class GenericStats 
    {
        private string _skillId;
        public string SkillId => _skillId;
        
        public GenericStats(string id) =>
            _skillId = id;
    }

    /// <summary>
    /// Stats modificator, that applied to character directly
    /// </summary>
	public class Stats<T> : GenericStats
    {
        protected T _data;
        public T Data => _data;

        public Stats(string id, T data) : base(id) =>
            _data = data;
    }
}