using Platformer.Scriptable.Skills.Data;
using Platformer.Scriptable.WeaponStats;
using System;
using System.Collections;
using UnityEngine;

namespace Platformer.Weapons
{
	public class MeleeWeapon : Weapon
	{
        [SerializeField]
        protected WeaponStats _stats;

        // TODO: IHMO: modifying weapon reload time from attacker stats is not good solution.
        private float _reloadTimeOffset;
        protected bool _chargingAttack;
        protected Coroutine _chargeAttackCoroutine;

        protected bool _attacking;
        private bool _reloadingAttack;

        public WeaponStats Stats => _stats;
        public float ReloadTime => Stats.ReloadTime + _reloadTimeOffset;

        public virtual bool CanNotAttack() => _attacking || _reloadingAttack;

        public event EventHandler HitEnded;

        protected virtual void OnEnable() =>
            HitEnded += OnHitEnded;

        protected virtual void OnDisable() =>
            HitEnded -= OnHitEnded;

        protected virtual void OnHitEnded(object sender, EventArgs e) =>
            StartCoroutine(ReloadMainAttack());

        protected virtual void InvokeHitEnded() => 
            HitEnded?.Invoke(this, EventArgs.Empty);

        public virtual void MakeHit()
        {
            if (CanNotAttack())
            {
                return;
            }
            _attacking = true;
        }

        public virtual void MakeAlternativeHit() { }
        public virtual void StopHit() { }

        public virtual void OnSkillAddedToAttacker(CombatSkillData toAdd) =>
            _reloadTimeOffset += toAdd.ReloadTime; 

        public virtual void OnSkillRemovedFromAttacker(CombatSkillData toRemove) =>
            _reloadTimeOffset -= toRemove.ReloadTime;

        protected IEnumerator ReloadMainAttack()
        {
            _reloadingAttack = true;
            yield return new WaitForSeconds(ReloadTime);
            _reloadingAttack = false;
        }
    }
}