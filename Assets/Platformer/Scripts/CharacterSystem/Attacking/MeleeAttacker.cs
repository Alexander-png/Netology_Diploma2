using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Enemies;
using Platformer.Weapons;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class MeleeAttacker : Attacker
	{
        private MeleeWeapon _currentWeapon;
        protected Collider _damageTrigger;

        protected bool _attacking;
        private bool _reloadingAttack;

        protected MeleeWeapon CurrentWeapon
        {  
            get => _currentWeapon;
            set
            {
                if (_currentWeapon != null)
                {
                    _currentWeapon.HitEnded -= OnHitEnded;
                }
                _currentWeapon = value;
                if (_currentWeapon != null)
                {
                    _currentWeapon.HitEnded += OnHitEnded;
                }
            }
        }

        protected virtual void Start()
        {
            CurrentWeapon = GetComponentInChildren<MeleeWeapon>();
            _damageTrigger = GetComponent<Collider>();
            _damageTrigger.enabled = false;
        }

        private void OnDisable() =>
            CurrentWeapon = null;

        private void OnDestroy() =>
            CurrentWeapon = null;

        protected virtual bool CanNotAttack() => _attacking || _reloadingAttack || _currentWeapon == null;

        public override void OnAttackPressed()
        {
            if (CanNotAttack())
            {
                return;
            }
            StartAttackInternal();
        }

        public override void OnStrongAttackInput() { }

        protected virtual void StartAttackInternal()
        {
            _attacking = true;
            _currentWeapon?.MakeHit();
            _damageTrigger.enabled = true;
        }

        public override void OnAttackReleased()
        {
            _attacking = false;
            _currentWeapon?.StopHit();
            _damageTrigger.enabled = false;
        }

        protected virtual void OnHitEnded(object sender, System.EventArgs e)
        {
            OnAttackReleased();
            StartCoroutine(ReloadAttack());
        }

        protected virtual IDamagable GetEnemyComponent(Collider other)
        {
            // My game architechture went bad and this showed here.
            // TODO: resolve this problem.
            if (other.TryGetComponent(out MoveableEnemy enemy))
            {
                return enemy;
            }
            if (other.TryGetComponent(out StationaryEnemy enemy1))
            {
                return enemy1;
            }
            return null;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var enemy = GetEnemyComponent(other);
            if (enemy != null)
            {
                if (_currentWeapon != null)
                {
                    enemy.SetDamage(CurrentWeapon.Stats.Damage, (transform.position - other.transform.position) * CurrentWeapon.Stats.PushForce);
                }
            }
        }

        public override float GetAttackChargeTime() =>
            CurrentWeapon.Stats.StrongAttackChargeTime;

        protected IEnumerator ReloadAttack()
        {
            _reloadingAttack = true;
            yield return new WaitForSeconds(CurrentWeapon.Stats.ReloadTime);
            _reloadingAttack = false;
        }

#if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            Color c = Color.red;
            c.a = 0.3f;
            Gizmos.color = c;
            if (TryGetComponent(out BoxCollider box))
            {
                Gizmos.DrawCube(box.bounds.center, box.bounds.size);
            }
            else if (TryGetComponent(out SphereCollider sphere))
            {
                Gizmos.DrawSphere(transform.position, sphere.radius);
            }
        }
#endif
    }
}