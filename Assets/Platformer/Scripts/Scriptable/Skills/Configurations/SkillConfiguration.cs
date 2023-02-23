using UnityEngine;

namespace Platformer.Scriptable.Skills.Configurations
{
	public abstract class SkillConfiguration<T> : ScriptableObject
	{
		[SerializeField]
		private string _skillId;

		public string SkillId => _skillId;
		public abstract T GetData();
	}
}