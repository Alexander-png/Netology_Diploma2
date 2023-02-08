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
        
        protected bool _chargingAttack;
        protected Coroutine _chargeAttackCoroutine;

        protected bool _attacking;
        private bool _reloadingAttack;

        public WeaponStats Stats => _stats;

        public virtual bool CanNotAttack() => _attacking || _reloadingAttack;

        public event EventHandler HitEnded;

        protected override void Start()
        {
            base.Start();
        }

        protected virtual void OnEnable() =>
            HitEnded += OnHitEnded;

        protected virtual void OnDisable() =>
            HitEnded -= OnHitEnded;

        protected virtual void OnHitEnded(object sender, EventArgs e) =>
            StartCoroutine(ReloadAttack());

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

        public virtual void StopHit() 
        {

        }

        protected IEnumerator ReloadAttack()
        {
            _reloadingAttack = true;
            yield return new WaitForSeconds(Stats.ReloadTime);
            _reloadingAttack = false;
        }
    }
}