using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Enemies;
using Platformer.EditorExtentions;
using Platformer.Weapons;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class Attacker : MonoBehaviour
	{
        [SerializeField]
        protected Transform _ownerTransform;
        [SerializeField]
        private Weapon _currentWeapon;
        [SerializeField]
        private Collider _damageTrigger;

        [SerializeField]
        private float _attackReloadTime = 1f;

        private bool _attacking;
        private bool _reloadingAttack;

        protected Weapon CurrentWeapon
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

        public void SetWeapon(PlayerWeapon weapon) =>
            CurrentWeapon = weapon;

        private bool CanNotAttack() => _attacking || _reloadingAttack || _currentWeapon == null;

        public virtual void StartAttack()
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
            _currentWeapon.MakeHit();
            _damageTrigger.enabled = true;
        }

        public virtual void EndAttack()
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

        public float GetAttackChargeTime() =>
            CurrentWeapon.Stats.AttackChargeTime;

        private IEnumerator ReloadAttack()
        {
            _reloadingAttack = true;
            yield return new WaitForSeconds(_attackReloadTime);
            _reloadingAttack = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
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
            else
            {
                GameLogger.AddMessage("No hit zone found", GameLogger.LogType.Error);
            }
        }
#endif
    }
}