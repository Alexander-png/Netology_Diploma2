using UnityEngine;

namespace Platformer.Scriptable.Skills.Data
{
    // TODO: add proportions
    public struct CombatSkillData
    {
        public float Damage;
        public float ReloadTime;

        public static CombatSkillData operator +(CombatSkillData first, CombatSkillData second)
        {
            CombatSkillData result = new CombatSkillData();
            result.Damage = first.Damage + second.Damage;
            result.ReloadTime = first.ReloadTime + second.ReloadTime;
            return result;
        }

        public static CombatSkillData operator -(CombatSkillData first, CombatSkillData second)
        {
            CombatSkillData result = new CombatSkillData();
            result.Damage = first.Damage - second.Damage;
            result.ReloadTime = first.ReloadTime - second.ReloadTime;
            return result;
        }
    }

    [CreateAssetMenu(fileName = "NewObj", menuName = "ScriptableObjects/Skills/Combat skill")]
	public class CombatSkillConfiguration : SkillConfiguration<CombatSkillData>
	{
		[SerializeField]
		private float _damage;
		[SerializeField]
		private float _reloadTime;

        public override CombatSkillData GetData() => new CombatSkillData()
        {
            Damage = _damage,
            ReloadTime = _reloadTime
        };
    }
}