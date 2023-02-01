using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Enemies;
using Platformer.Weapons;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class MeleeAttacker : Attacker
	{
        [SerializeField]
        protected MeleeWeapon _currentWeapon;
        [SerializeField]
        private Collider _damageTrigger;

        // todo: move to configuraion
        [SerializeField]
        private float _attackReloadTime = 1f;

        private bool _attacking;
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

        private void Start()
        {
            _damageTrigger.enabled = false;
            // Refreshing event subscription
            CurrentWeapon = CurrentWeapon;
        }

        private void OnDisable() =>
            CurrentWeapon = null;

        private void OnDestroy() =>
            CurrentWeapon = null;

        private bool CanNotAttack() => _attacking || _reloadingAttack || _currentWeapon == null;

        public override void StartAttack()
        {
            if (CanNotAttack())
            {
                return;
            }
            StartAttackInternal();
        }

        protected virtual void StartAttackInternal()
        {
            _attacking = true;
            _currentWeapon?.MakeHit();
            _damageTrigger.enabled = true;
        }

        public override void EndAttack()
        {
            _attacking = false;
            _currentWeapon?.StopHit();
            _damageTrigger.enabled = false;
        }

        private void OnHitEnded(object sender, System.EventArgs e)
        {
            EndAttack();
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
            CurrentWeapon.Stats.AttackChargeTime;

        private IEnumerator ReloadAttack()
        {
            _reloadingAttack = true;
            yield return new WaitForSeconds(_attackReloadTime);
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