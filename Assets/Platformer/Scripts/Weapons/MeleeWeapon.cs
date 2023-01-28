using Platformer.Scriptable.LevelElements.Traps;
using System;
using UnityEngine;

namespace Platformer.Weapons
{
	public class MeleeWeapon : Weapon
	{
        [SerializeField]
        protected DamageStats _damageStats;
        
        protected bool _chargingAttack;
        protected Coroutine _chargeAttackCoroutine;

        public DamageStats Stats => _damageStats;

        public event EventHandler HitEnded;

        protected virtual void InvokeHitEnded() => 
            HitEnded?.Invoke(this, EventArgs.Empty);

        public virtual void MakeHit() { }
        public virtual void StopHit() { }
    }
}