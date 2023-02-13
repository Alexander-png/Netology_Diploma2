using Platformer.CharacterSystem.Base;
using Platformer.PlayerSystem;
using UnityEngine;

namespace Platformer.CharacterSystem.Attacking
{
	public class FirstBossAttacker : MeleeAttacker
	{
        public override void OnMainAttackPressed()
        {
            StartMainAttack();
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
                    enemy.SetDamage(CurrentWeapon.Stats.Damage, (transform.position - other.transform.position) * CurrentWeapon.Stats.PushForce);
                }
            }
            if (CurrentWeapon.Stats.IsKamikazeAttack)
            {
                CurrentWeapon.SetDamageToOwner(float.MaxValue);
            }
        }
    }
}