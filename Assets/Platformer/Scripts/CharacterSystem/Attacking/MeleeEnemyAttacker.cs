using Platformer.CharacterSystem.Base;
using Platformer.PlayerSystem;
using System.Collections;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class MeleeEnemyAttacker : MeleeAttacker
	{
        private bool _pressingMainAttack;
        private Coroutine _attackerCoroutine;

        public override void OnMainAttackPressed()
        {
            _pressingMainAttack = true;
            _attackerCoroutine = StartCoroutine(MainAttackCoroutine());
        }

        public override void OnAttackReleased()
        {
            _pressingMainAttack = false;
            base.OnAttackReleased();
        }

        private IEnumerator MainAttackCoroutine()
        {
            while (_pressingMainAttack)
            {
                _damageTrigger.enabled = true;
                yield return new WaitForSeconds(CurrentWeapon.Stats.AttackDuration); 
                _damageTrigger.enabled = false;
                yield return new WaitForSeconds(CurrentWeapon.Stats.ReloadTime);
            }
            _attackerCoroutine = null;
        }

        protected override IDamagable GetEnemyComponent(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                return player;
            }
            return null;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            var enemy = GetEnemyComponent(other);
            if (enemy != null)
            {
                if (CurrentWeapon != null)
                {
                    if (enemy.CanBeDamaged)
                    {
                        InvokeAttackerEvent(EntityEventTypes.Attack);
                        enemy.SetDamage(CurrentWeapon.Stats.Damage, (other.transform.position - transform.position) * CurrentWeapon.Stats.PushForce);
                    }
                }
            }
            if (CurrentWeapon.Stats.IsKamikazeAttack)
            {
                CurrentWeapon.SetDamageToOwner(float.MaxValue);
            }
        }
    }
}