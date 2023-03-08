using System;
using UnityEngine;

namespace Platformer.Scriptable.Skills.Data
{
    public struct CombatSkillData
    {
        public float Damage;
        public float ReloadTime;

        public bool IsProportion;

        public static CombatSkillData operator +(CombatSkillData first, CombatSkillData second)
        {
            CombatSkillData result = new CombatSkillData();
            if (first.IsProportion == second.IsProportion)
            {
                result.Damage = first.Damage + second.Damage;
                result.ReloadTime = first.ReloadTime + second.ReloadTime;
            }
            else if (!first.IsProportion && second.IsProportion)
            {
                result.Damage = first.Damage + (first.Damage * second.Damage);
                result.ReloadTime = first.ReloadTime + (first.ReloadTime * second.ReloadTime);
            }
            else
            {
                throw new InvalidOperationException("Addition of absolute values to propotion is not supported.");
            }
            return result;
        }

        public static CombatSkillData operator -(CombatSkillData first, CombatSkillData second)
        {
            CombatSkillData result = new CombatSkillData();
            if (first.IsProportion == second.IsProportion)
            {
                result.Damage = first.Damage - second.Damage;
                result.ReloadTime = first.ReloadTime - second.ReloadTime;
            }
            else if (!first.IsProportion && second.IsProportion)
            {
                result.Damage = first.Damage - (first.Damage * second.Damage);
                result.ReloadTime = first.ReloadTime - (first.ReloadTime * second.ReloadTime);
            }
            else
            {
                throw new InvalidOperationException("Substraction of absolute values from propotion is not supported.");
            }
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
            ReloadTime = _reloadTime,
            IsProportion = IsProprotion,
        };
    }
}