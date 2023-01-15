using Platformer.SkillSystem.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Configurations
{
	public abstract class SkillConfiguration : ScriptableObject
	{
		[SerializeField]
		private string _skillId;

		public string SkillId => _skillId;

		public abstract Dictionary<SkillTypes, object> GetSkills();
	}
}