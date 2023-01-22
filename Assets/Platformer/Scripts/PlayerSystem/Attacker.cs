using Platformer.CharacterSystem.Base;
using Platformer.CharacterSystem.Enemies;
using Platformer.EditorExtentions;
using Platformer.Weapons;
using System.Collections;
using UnityEngine;

namespace Platformer.PlayerSystem
{
	public class Attacker : MonoBehaviour
	{
        [SerializeField]
        private Transform _ownerTransform;
        [SerializeField]
        private PlayerWeapon _currentWeapon;
        [SerializeField]
        private BoxCollider _damageTrigger;

        private bool _attacking;
        private bool _reloadingAttack;

        private PlayerWeapon CurrentWeapon
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

        public void OnAttackInput()
        {
            if (CanNotAttack())
            {
                return;
            }
            _attacking = true;
            _currentWeapon.MakeHit();
            _damageTrigger.enabled = true;
        }

        private void OnHitEnded(object sender, System.EventArgs e)
        {
            _attacking = false;
            _damageTrigger.enabled = false;
            StartCoroutine(ReloadAttack());
        }

        private IDamagable GetEnemyComponent(Collider other)
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

        private void OnTriggerEnter(Collider other)
        {
            var enemy = GetEnemyComponent(other);
            if (enemy != null)
            {
                if (_currentWeapon != null)
                {
                    enemy.SetDamage(_currentWeapon.Stats.Damage, (_ownerTransform.position - transform.position) * _currentWeapon.Stats.PushForce);
                }
            }
        }

        private IEnumerator ReloadAttack()
        {
            _reloadingAttack = true;
            yield return new WaitForSeconds(1f);
            _reloadingAttack = false;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (TryGetComponent(out BoxCollider collider))
            {
                Color c = Color.red;
                c.a = 0.3f;
                Gizmos.color = c;
                Gizmos.DrawCube(collider.bounds.center, collider.bounds.size);
            }
            else
            {
                GameLogger.AddMessage("No hit zone found", GameLogger.LogType.Error);
            }
        }
#endif
    }
}