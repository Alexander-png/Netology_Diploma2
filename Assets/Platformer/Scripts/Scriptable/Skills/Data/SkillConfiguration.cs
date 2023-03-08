using UnityEngine;

namespace Platformer.Scriptable.Skills.Data
{
	public abstract class SkillConfiguration<T> : ScriptableObject
	{
		[SerializeField]
		private string _skillId;
        [SerializeField]
		private bool _isProprotion;

		public string SkillId => _skillId;
		public bool IsProprotion => _isProprotion;
		public abstract T GetData();
	}
}