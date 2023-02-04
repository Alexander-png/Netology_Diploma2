using Platformer.Scriptable.LevelElements.Traps;
using Platformer.Scriptable.WeaponStats;
using System;
using UnityEngine;

namespace Platformer.Weapons
{
	public class MeleeWeapon : Weapon
	{
        [SerializeField]
        protected WeaponStats _stats;
        
        protected bool _chargingAttack;
        protected Coroutine _chargeAttackCoroutine;

        public WeaponStats Stats => _stats;

        public event EventHandler HitEnded;

        protected virtual void InvokeHitEnded() => 
            HitEnded?.Invoke(this, EventArgs.Empty);

        public virtual void MakeHit() { }
        public virtual void StopHit() { }
    }
}